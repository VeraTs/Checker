using Microsoft.AspNetCore.Mvc;
using CheckerServer.Models;
using CheckerServer.Data;
using Microsoft.EntityFrameworkCore;


namespace CheckerServer.Controllers
{
    [Route("Restaurants")]
    [ApiController]
    public class RestaurantController : BasicDbController<Restaurant>
    {
        public RestaurantController(CheckerDBContext context)
            : base(context, context.Restaurants) 
        { }

        protected override async Task<Restaurant?> createAuthenticatedUserItem(Restaurant item)
        {
            Restaurant? rest = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                rest = await getUserRestaurant();
            }

            return rest;
        }

        protected override async Task<Restaurant> getItemForAuthentitcatedUser(int id)
        {
            Restaurant? restaurant = await getUserRestaurant();
            if(restaurant.ID != id)
            {
                restaurant = null;
            }

            return restaurant;
        }

        protected override void updateItem(Restaurant existingItem, Restaurant updatedItem)
        {
            if (!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }

            if (!string.IsNullOrEmpty(updatedItem.ContactName))
            {
                existingItem.ContactName = updatedItem.ContactName;
            }

            if (!string.IsNullOrEmpty(updatedItem.Phone))
            {
                existingItem.Phone = updatedItem.Phone;
            }
        }

        override internal async Task<ActionResult<IEnumerable<Restaurant>>> get()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                List<Restaurant> res = new List<Restaurant>();
                if (rest!= null)
                {
                    res = await r_Set
                    .Include("ServingAreas")
                    .Include("Lines")
                    .Include("Menus.Dishes")
                    .Where(r => r.ID == rest.ID)
                    .ToListAsync();
                }
                 
                return res;
            }
            else
            {
                return Forbid();
            }
        }

        override internal async Task<ActionResult<Restaurant>> getSpecific(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                Restaurant? res = null;
                if (rest != null && rest.ID == id)
                {
                    res = await r_Set
                    .Include("ServingAreas")
                    .Include("Lines")
                    .Include("Menus.Dishes")
                    .FirstOrDefaultAsync(d => d.ID == id);

                    return res;
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Forbid();
            }
        }
    }
}
