using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CheckerServer.Data;
using CheckerServer.Models;
using CheckerServer.utils;

namespace CheckerServer.Hubs
{
    public class OrdersHub : Hub
    {
        private readonly CheckerDBContext _context;
        private readonly KitchenUtils r_KitchenUtils;

        public OrdersHub(CheckerDBContext context)
        {
            _context = context;
            r_KitchenUtils = new KitchenUtils(context);
            manageKitchen();
        }

        // gets all orders in system
        public async Task GetAllOrders()
        {
            List<Order> orders = await _context.Orders.ToListAsync();

            await Clients.Caller.SendAsync("ReceiveOrders", orders);
            
            /*foreach(Order order in orders)
            {
                await Clients.Caller.SendAsync("ReceiveOrder", order);
            }*/
        }

        public async Task AddOrder(Order order)
        {
            List<OrderItem> items = order.Items;
            await _context.Orders.AddAsync(order);
            int success = await _context.SaveChangesAsync();
            if(success > 0)
            {
                foreach(OrderItem item in items)
                {
                    item.OrderId = order.ID;
                }

                await _context.OrderItems.AddRangeAsync(items);
                success = await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveOrder", order);
            } else
            {
                await Clients.Caller.SendAsync("DBError", "Error in adding order");
            }
        }

        public async Task AddOrderItem(OrderItem item)  // orderITem should contain the order id propertly.
        {
            Order? order = await _context.Orders.FirstOrDefaultAsync(o => o.ID == item.OrderId);
            int success = -1;
            if (order != null)
            {
                success = DBSetHelper.AddHelper<OrderItem>(_context, item, _context.OrderItems).Result.Value;
            }

            if(success <0)
            {
                await Clients.Caller.SendAsync("DBError", "No Order with the id " + item.OrderId + " is registered");
            } else if(success == 0)
            {
                await Clients.Caller.SendAsync("DBError", "Could not add Order Item");
            }else
            {
                // successfull addition
                await Clients.Caller.SendAsync("DBSuccess", "Added Order Item successfuly.");
                // option A - will probably cause issues with circular references
                /*order = await _context.Orders.DistinctBy(o => o.ID == order.ID)
                    .Include("Items")
                    .FirstOrDefaultAsync();*/

                // option B
                order.Items.AddRange(_context.OrderItems.Where(oi => oi.OrderId == order.ID));

                await Clients.All.SendAsync("OrderUpdate", order); // send updated DB state to this instance
            }
        }

        // deprecated
        public async Task GetAllOrderItemsForLine(Line line)
        {
            // get all orderItems that are registered as AtLine for this line
            List<OrderItem> items  = _context.OrderItems
                .Where(oi => oi.Status == eItemStatus.AtLine)
                .Where(oi => oi.Dish.LineId == line.ID)
                .ToListAsync().Result;

            // create the three seperate lists according to eLineItemStatus
            List<OrderItem> lockedItems = items.Where(item => item.LineStatus == eLineItemStatus.Locked).ToList();
            List<OrderItem> toDoItems = items.Where(item => item.LineStatus == eLineItemStatus.ToDo).ToList();
            List<OrderItem> doingItems = items.Where(item => item.LineStatus == eLineItemStatus.Doing).ToList();

            // send the lists and the line back
            await Clients.Caller.SendAsync("ReceiveAllOrderItemsForLine", line, lockedItems, toDoItems, doingItems);
        }

        // actually release from locked state to ToDo line status
        public async Task SendItemToKitchen(int itemId)
        {
            OrderItem actualItem = _context.OrderItems.FirstOrDefaultAsync(oi => oi.ID == itemId).Result;
            if(actualItem == null)
            {
                await Clients.Caller.SendAsync("SignalRError", "No such Order Item");
            } else
            {
                // check if item is capable of being sent to kitchen
                if(actualItem.Status != eItemStatus.Ordered)
                {
                    await Clients.Caller.SendAsync("SignalRError", "Couldn't send item to kitchen - it was already sent to kitchen");
                } else
                {
                    // send it to the kitchen!
                    actualItem.Status = eItemStatus.AtLine;
                    actualItem.LineStatus = eLineItemStatus.ToDo;
                    // saving it
                    int success = await _context.SaveChangesAsync();
                    if(success > 0)
                    {
                        // yay, succeeded
                        await Clients.Caller.SendAsync("ItemForKitchen", actualItem);
                    } else
                    {
                        // oops, failure
                        await Clients.Caller.SendAsync("SignalRError", "Couldn't Alter status of item with ID " + itemId);
                    }
                }
            }
        }

        // not tested yet
        public async Task ChangeOrderItemStatus(OrderItem item, eItemStatus newStatus)
        {
            if (item == null)
            {
                await Clients.Caller.SendAsync("SignalRError", "Couldn't Alter status of null item");
            } else
            {
                OrderItem actualItem = _context.OrderItems.FirstOrDefaultAsync(oi => oi.ID == item.ID).Result;
                if (item == null)
                {
                    await Clients.Caller.SendAsync("SignalRError", "Couldn't Alter status of invalid item");
                }
            }
        }

        // amnage the kitchen utils
        private async Task manageKitchen()
        {

            // we're currently ignoring multiple restaurants, and assuming singal restaurant
            // for multiple restaurants, we will have multiple users, and in the SignalR case,
            // multiple Clients associated with the user, in a Group


            /**
             * How To manage a kitchen?
             *  1) Load everything (done in ctor!)
             *  2) while server operating:
             *      2.1) Check all open orders, and update the OrderITem lists for each Line
             *              \----- 'GetUpdatedLines' - will return a mapping restId -> {line | line in rest, with updated orderItem lists inside}
             *      2.2) For each line, send the Line and lists to each relevant restaurant
             *              \----- foreach(int key in keys) { await Clients.Group(...).SendAsync(event thing) }
             *      2.3) if there are new orders, sift them into the KitchenUtils
             *              \----- 'LoadNewOrders' with input of active orders from DB (actually, is input needed? KitchenUtils has context access)
             *      2.4) if there are closed orders, remove them from the KitchenUtils
             *              \----- 'ClearDeadOrders' with no input needed
             * 
             */

            // step 2.1
            // restId to Lines
            Dictionary<int, List<Line>> updatedLines = r_KitchenUtils.GetUpdatedLines();

            // step 2.2
            foreach (int restId in updatedLines.Keys)
            {
                Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
                if(rest != null)
                {
                    // not awaited since this pretains to different restaurants
                    Clients.Group(rest.Name).SendAsync("UpdatedLines", updatedLines[rest.ID]);
                }
            }

            // step 2.3
            int success = r_KitchenUtils.LoadNewOrders();
            if(success > 0)
            {
                // yay, things loaded and hoarded - continue with my life, this will happen again, obvs.
            } else
            {
                // should I report a signalR error? And if so - to whom?
                await Clients.All.SendAsync("SignalRError", "Error in uploading new orders to the management system");
            }

            // step 2.4
            r_KitchenUtils.ClearOrders();
        }

        // temporary group registration with rest name
        // in future will be with actual protection and privacy thing
        /******************* IMPORTANT - THIS IS NOT RECONNECT SAFE
         * 
         * meaning that if a client disconnects from the hub, on reconnecting they MUST REREGISTER WITH THE GROUP
         * 
         * ***************************/
        public async Task RegisterForGroup(int restId)
        {
            Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
            if(rest == null)
            {
                await Clients.Caller.SendAsync("DBError", "No such restaurant is registered");
            } else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, rest.Name);
                await Clients.Group(rest.Name).SendAsync("NewGroupMember", $"{Context.ConnectionId} has joined the group {rest.Name}.");
            }
            
        }

        public async Task UnRegisterForGroup(int restId)
        {
            Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
            if (rest == null)
            {
                await Clients.Caller.SendAsync("DBError", "No such restaurant is registered");
            }
            else
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, rest.Name);
                await Clients.Group(rest.Name).SendAsync("NewGroupMember", $"{Context.ConnectionId} has left the group {rest.Name}.");
            }
        }
    }
}
