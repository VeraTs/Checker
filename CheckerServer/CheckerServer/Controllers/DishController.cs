using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Controllers
{
    [Route("Dishes")]
    [ApiController]
    public class DishController : BasicDbController<Dish>
    {
        public DishController(CheckerDBContext context)
            : base(context, context.Dishes) 
        { }

        protected override void updateItem(Dish existingItem, Dish updatedItem)
        {
            if (updatedItem.LineId != 0)
            {
                existingItem.LineId = updatedItem.LineId;
            }

            if (!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }
            
            existingItem.Type = updatedItem.Type;
        }

        protected override async Task<ActionResult<int>> delete(Dish item)
        {
            ActionResult<int> success = await base.delete(item);
            if(success.Value > 0)
            {
                List<Statistic> dishStats = await r_DbContext.Statistics.Where(stat => stat.DishId == item.ID).ToListAsync();
                r_DbContext.Statistics.RemoveRange(dishStats);
                return await r_DbContext.SaveChangesAsync();
            }

            return success;
        }
    }
}
