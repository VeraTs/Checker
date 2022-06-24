using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;


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
    }
}
