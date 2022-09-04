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

        protected async override Task<RestMenu?> createAuthenticatedUserItem(RestMenu item)
        {
            RestMenu? menu = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // add relevant rest id if none is there
                Restaurant? rest = await getUserRestaurant();
                if(rest!= null)
                {
                    if(item.RestaurantId == 0)
                    {
                        item.RestaurantId = rest.ID;
                    }

                    if(item.RestaurantId == rest.ID)
                    {
                        menu = item;
                    }
                }
            }

            return menu;
        }

        protected async override Task<RestMenu?> getItemForAuthentitcatedUser(int id)
        {
            RestMenu? menu = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if(rest!= null)
                {
                    try
                    {
                        menu = await r_DbContext.RestMenus.FirstAsync(rm => rm.ID == id && rm.RestaurantId == rest.ID);
                    }catch(Exception ex)
                    {
                        menu = null;
                    }
                }
            }

            return menu;
        }

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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if(rest != null)
                {
                    List<RestMenu> res = await r_Set
                        .Where(r => r.RestaurantId == rest.ID)
                        .Include(m => m.Dishes)
                        .ToListAsync();

                    return res;
                }

                return BadRequest();
            }
            else
            {
                return Forbid();
            }
        }

        override internal async Task<ActionResult<RestMenu>> getSpecific(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        RestMenu res = await r_Set
                        .Include(m => m.Dishes)
                        .FirstAsync(r => r.RestaurantId == rest.ID && r.ID == id);
                        return res;
                    }catch(Exception ex) { }
                }

                return BadRequest();
            }
            else
            {
                return Forbid();
            }
        }
    }
}
