using CheckerServer.Data;
using CheckerServer.Models;
using CheckerServer.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CheckerServer.Controllers
{
    [Route("Orders")]
    [ApiController]
    public class OrderController : BasicDbController<Order>
    {
        public OrderController(CheckerDBContext context)
            : base(context, context.Orders) 
        {
            
        }

        protected override void updateItem(Order existingItem, Order updatedItem)
        {
            if (updatedItem.Table != -1)
            {
                existingItem.Table = updatedItem.Table;
            }

            existingItem.Status = updatedItem.Status;
            existingItem.OrderType = updatedItem.OrderType;
        }

        override internal async Task<ActionResult<IEnumerable<Order>>> get()
        {
            var res = await r_Set
                .Include("Items")
                .ToListAsync();

            return res;
        }

        override internal async Task<ActionResult<Order>> getSpecific(int id)
        {
            var res = await r_Set
                .Include("Items.Dish")
                .FirstOrDefaultAsync(d => d.ID == id);

            return res;
        }
    }
}
