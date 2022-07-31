using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CheckerServer.Controllers
{
    [ApiController]
    [Route("Menus")]
    public class MenuController : BasicDbController<RestMenu>
    {
        public MenuController(CheckerDBContext context)
            : base(context, context.RestMenus) 
        { }

        // add a more specified post I guess, to enter the dishes as well as the menu entity

        protected override void updateItem(RestMenu existingItem, RestMenu updatedItem)
        {
            if (!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }

            // changing the restaurant id is not allowed
        }

        override internal async Task<ActionResult<IEnumerable<RestMenu>>> get()
        {
            var res = await r_Set
                .Include(m => m.Dishes)
                .ToListAsync();

            return res;
        }

        override internal async Task<ActionResult<RestMenu>> getSpecific(int id)
        {
            var res = await r_Set
                .Include(m => m.Dishes)
                .FirstOrDefaultAsync(d => d.ID == id);

            return res;
        }
    }
}
