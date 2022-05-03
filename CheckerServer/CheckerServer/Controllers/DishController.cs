using CheckerServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckerServer.utils;
using CheckerDTOs;

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

            if (string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }
            
            existingItem.Type = updatedItem.Type;
        }
    }
}
