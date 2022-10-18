using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CheckerServer.Data;
using CheckerServer.Models;
using CheckerServer.utils;

namespace CheckerServer.Hubs
{
    /***
     * Orders Hub
     * 
     * For use by Waiter:
     *      - get all orders
     *      - add order
     *      - pay for order
     *      - close order
     *      - pick up item from serving area
     *      
     * For use by Serving Areas View:
     *      - view all orders
     * 
     * ***/
    public class OrdersHub : Hub
    {
        private readonly CheckerDBContext _context;
        public IServiceProvider? Services { get; }

        public OrdersHub(IServiceProvider services, CheckerDBContext context)
        {
            Services = services;
            _context = context;
        }

        /*** gets all orders in system ***/
        private async Task getAllOrders()
        {
            List<Order> orders = await _context.Orders.Include("Items").ToListAsync();

            await Clients.Caller.SendAsync("ReceiveOrders", orders);
        }

        /*** 
         * Add Order
         * 
         * first add the order itself to db to get orderId, and after applying it to all items, add orderitems to db
         * ***/
        public async Task AddOrder(Order order)
        {
            List<OrderItem> items = order.Items;
            order.Items = null;
            int success = 0;
            try
            {
                await _context.Orders.AddAsync(order);
                success = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("DBError", ex.Message);
            }

            if (success > 0)
            {
                foreach (OrderItem item in items)
                {
                    item.OrderId = order.ID;
                    item.Dish = null;
                }

                try
                {
                    await _context.OrderItems.AddRangeAsync(items);
                    success = await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

                Order dbOrder = _context.Orders.Include("Items.Dish").FirstOrDefault(o => o.ID == order.ID);

                await Clients.All.SendAsync("ReceiveOrder", dbOrder);
            }
            else
            {
                await Clients.Caller.SendAsync("DBError", "Error in adding order");
            }
        }

        /***
         * Adds individual item to existing order
         * 
         * item must have correct order id
         * 
         * ***/
        public async Task AddOrderItem(OrderItem item)
        {
            Order? order = await _context.Orders.FirstOrDefaultAsync(o => o.ID == item.OrderId);
            int success = -1;
            if (order != null)
            {
                success = DBSetHelper.AddHelper<OrderItem>(_context, item, _context.OrderItems).Result.Value;
            }

            if (success < 0)
            {
                await Clients.Caller.SendAsync("DBError", "No Order with the id " + item.OrderId + " is registered");
            }
            else if (success == 0)
            {
                await Clients.Caller.SendAsync("DBError", "Could not add Order Item");
            }
            else
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

        /***
         * For Waiter : report order item picked up from serving area
         * 
         * ***/
        public async Task PickUpItemForServing(int itemId)
        {
            OrderItem item = await _context.OrderItems.Include("Dish").FirstOrDefaultAsync(oi => oi.ID == itemId);
            Line line = await _context.Lines.Include("ServingArea").FirstOrDefaultAsync(l => l.ID == item.Dish.LineId);
            if (item.Status == eItemStatus.WaitingToBeServed)
            {
                Boolean success = OrdersUtils.freeSpot(line.ServingArea, item);
                if (true)
                {
                    item.Status = eItemStatus.Served;
                    item.ServingAreaZone = -1;
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == line.RestaurantId);
                        await Clients.Group(rest.Name).SendAsync("ItemServed", item);
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("DBError", "could not save changes to item");
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("DBError", "unexpected error with serving area");
                }
            }
            else
            {
                await Clients.Caller.SendAsync("DBError", "the item is not waiting to be served");
            }
        }

        /***
         * For Waiter: closing an order
         * given a sum payes it (to fully or partially pay for the order),
         * after successful payment is made, waiter is immediately informed, and after, all waiters get update of payment. 
        * ***/
        public async Task PayForOrder(int orderId, float sum)
        {
            bool isOrder = await _context.Orders.AnyAsync(o => o.ID == orderId);
            if (isOrder)
            {
                Order order = await _context.Orders.FirstAsync(o => o.ID == orderId);
                if (sum >= order.RemainsToPay)
                {
                    sum = sum - order.RemainsToPay;
                    order.RemainsToPay = 0;
                    order.Status = eOrderStatus.Done;
                    await Clients.Caller.SendAsync("PaymentMadeFull", order, sum);
                    if (Services != null && await _context.SaveChangesAsync() > 0)
                    {
                        // yay - success
                        // now just update KitchenManager
                        RestaurantManager? manager = Services.GetService<RestaurantManager>();
                        if(manager != null)
                        {
                            await manager.getKitchenForRestaurant(order.RestaurantId).CloseOrder(orderId);
                        }   
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("DBError", "Error in updating DB, try again later");
                    }
                }
                else
                {
                    order.RemainsToPay -= sum;
                    await Clients.Caller.SendAsync("PartialPaymentMade", order, sum);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                await Clients.Caller.SendAsync("DBError", "No order with id " + orderId + " exists");
            }
        }


        public async Task GetAllOrderItemsForLine(Line line)
        {
            // get all orderItems that are registered as AtLine for this line
            List<OrderItem> items = _context.OrderItems
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

        /*** sends an ordered item to the kitchen ***/
        public async Task SendItemToKitchen(int itemId)
        {
            OrderItem actualItem = _context.OrderItems.FirstOrDefaultAsync(oi => oi.ID == itemId).Result;
            if (actualItem == null)
            {
                await Clients.Caller.SendAsync("SignalRError", "No such Order Item");
            }
            else
            {
                // check if item is capable of being sent to kitchen
                if (actualItem.Status != eItemStatus.Ordered)
                {
                    await Clients.Caller.SendAsync("SignalRError", "Couldn't send item to kitchen - it was already sent to kitchen");
                }
                else
                {
                    // send it to the kitchen!
                    actualItem.Status = eItemStatus.AtLine;
                    actualItem.LineStatus = eLineItemStatus.ToDo;
                    // saving it
                    int success = await _context.SaveChangesAsync();
                    if (success > 0)
                    {
                        // yay, succeeded
                        await Clients.Caller.SendAsync("ItemForKitchen", actualItem);
                    }
                    else
                    {
                        // oops, failure
                        await Clients.Caller.SendAsync("SignalRError", "Couldn't Alter status of item with ID " + itemId);
                    }
                }
            }
        }


        /**********************************************
         * 
         * 
         * 
         *          G   R   O   U   P   S   !
         * 
         * 
         * 
         **********************************************/
        /******************* IMPORTANT - THIS IS NOT RECONNECT SAFE
         * 
         * meaning that if a client disconnects from the hub, on reconnecting they MUST REREGISTER WITH THE GROUP
         * 
         * ***************************/

        /*** Register for group - to be included in updates ***/
        public async Task RegisterForGroup(int restId)
        {
            Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
            if (rest == null)
            {
                await Clients.Caller.SendAsync("DBError", "No such restaurant is registered");
            }
            else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, rest.Name);
                RestaurantManager manager = Services.GetService<RestaurantManager>();
                manager.getKitchenForRestaurant(restId).AddGroupMember();
                await Clients.Group(rest.Name).SendAsync("NewGroupMember", $"{Context.ConnectionId} has joined the group {rest.Name}.");
                await getAllOrders();
            }

        }

        /*** UnRegister from group ***/
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

                //s_Manager.RemoveGroupMember(restId);
                await Clients.Group(rest.Name).SendAsync("NewGroupMember", $"{Context.ConnectionId} has left the group {rest.Name}.");
            }
        }
    }
}
