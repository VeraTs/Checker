using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CheckerServer.Data;
using CheckerDTOs;

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
        public async Task InitialOrders()
        {
            List<Order> orders = await _context.Orders.ToListAsync();
            foreach(Order order in orders)
            {
                await Clients.Caller.SendAsync("ReceiveOrder", order);
            }
        }

        public async Task AddOrder(Order order)
        {
            _context.Add(order);
            int success = await _context.SaveChangesAsync();
            if(success > 0)
            {
                await Clients.All.SendAsync("ReceiveOrder", order);
            }
        }
    }
}
