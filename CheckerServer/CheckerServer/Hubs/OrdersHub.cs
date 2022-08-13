﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CheckerServer.Data;
using CheckerServer.Models;
using CheckerServer.utils;

namespace CheckerServer.Hubs
{
    public class OrdersHub : Hub
    {
        private readonly CheckerDBContext _context;
        public IServiceProvider? Services { get; }

        public OrdersHub(IServiceProvider services, CheckerDBContext context)
        {
            Services = services;
            _context = context;
        }

        // gets all orders in system
        public async Task GetAllOrders()
        {
            List<Order> orders = await _context.Orders.Include("Items").ToListAsync();

            await Clients.Caller.SendAsync("ReceiveOrders", orders);

            /*foreach(Order order in orders)
            {
                await Clients.Caller.SendAsync("ReceiveOrder", order);
            }*/
        }

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

        public async Task AddOrderItem(OrderItem item)  // orderItem should contain the order id propertly.
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

        // for closing order by waiter: given a sum payes it (to fully or partially pay for the order),
        // after successful payment is made, waiter is immediately informed, and after, all waiters get update of payment.
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
                    await Clients.Caller.SendAsync("PaymentMadeFull", order, sum);
                    if (Services != null && await _context.SaveChangesAsync() > 0)
                    {
                        // yay - success
                        // now just update KitchenManager
                        KitchenManager manager = Services.GetService<KitchenManager>();
                        await manager.CloseOrder(orderId);
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
                   // await GetAllOrders();
            }
            else
            {
                await Clients.Caller.SendAsync("DBError", "No order with id " + orderId + " exists");
            }
        }

        // deprecated
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

        // actually release from locked state to ToDo line status
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

        // not tested yet
        public async Task ChangeOrderItemStatus(OrderItem item, eItemStatus newStatus)
        {
            if (item == null)
            {
                await Clients.Caller.SendAsync("SignalRError", "Couldn't Alter status of null item");
            }
            else
            {
                OrderItem actualItem = _context.OrderItems.FirstOrDefaultAsync(oi => oi.ID == item.ID).Result;
                if (item == null)
                {
                    await Clients.Caller.SendAsync("SignalRError", "Couldn't Alter status of invalid item");
                }
            }
        }
    }
}
