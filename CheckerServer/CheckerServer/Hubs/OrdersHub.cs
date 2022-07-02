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

        public OrdersHub(CheckerDBContext context)
        {
            _context = context;
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
    }
}
