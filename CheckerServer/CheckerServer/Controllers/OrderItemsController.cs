using CheckerServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckerServer.utils;
using CheckerDTOs;

namespace CheckerServer.Controllers
{
    [Route("OrderItems")]
    [ApiController]
    public class OrderItemsController : BasicDbController<OrderItem>
    {
        public OrderItemsController(CheckerDBContext context)
            : base(context, context.OrderItems)
        { }

        protected override void updateItem(OrderItem existingItem, OrderItem updatedItem)
        {
            // this is to be uncommented later
            /*if (updatedItem.ServingAreaZone != -1)
            {
                existingItem.ServingAreaZone = updatedItem.ServingAreaZone;
            }*/

            existingItem.LineStatus = updatedItem.LineStatus;

            existingItem.Status = updatedItem.Status;
        }

        override internal async Task<ActionResult<IEnumerable<OrderItem>>> get()
        {
            var res = await r_Set
                .Include(i => i.Dish)
                .ToListAsync();

            return res;
        }

        override internal async Task<ActionResult<OrderItem>> getSpecific(int id)
        {
            var res = await r_Set
                .Include(i => i.Dish)
                .FirstOrDefaultAsync(d => d.ID == id);

            return res;
        }
    }
}
