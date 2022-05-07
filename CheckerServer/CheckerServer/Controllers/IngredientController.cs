using CheckerServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckerServer.utils;
using CheckerDTOs;

namespace CheckerServer.Controllers
{
    [Route("Ingredients")]
    [ApiController]
    public class IngredientController : BasicDbController<Ingredient>
    {
        public IngredientController(CheckerDBContext context)
            : base(context, context.Ingredients)
        { }

        protected override void updateItem(Ingredient existingItem, Ingredient updatedItem)
        {
            if (!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }

            if (updatedItem.InStock != 0)
            {
                existingItem.InStock = updatedItem.InStock;
            }

            if (updatedItem.Measurement != null)
            {
                existingItem.Measurement = updatedItem.Measurement;
            }
        }
    }
}
