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

        protected override async Task<Dish?> createAuthenticatedUserItem(Dish item)
        {
            Restaurant? rest = await getUserRestaurant();
            Dish? res = null;
            if (rest != null && !String.IsNullOrEmpty(rest.Name))
            {
                // in dish, we need to make sure line and restmenu are connected, so we check that they are valid
                try
                {
                    Line line = await r_DbContext.Lines.FirstAsync(l => l.ID == item.LineId && l.RestaurantId == rest.ID);
                    RestMenu menu = await r_DbContext.RestMenus.FirstAsync(rm => rm.ID == item.RestMenuId && rm.RestaurantId == rest.ID);

                    // if it didn't pop over to exception - then these kine and rest menu exist and are relevant for this user
                    res = item;
                }
                catch (Exception ex)
                {
                    res = null;
                }
            }

            return res;            
        }

        protected override async Task<Dish> getItemForAuthentitcatedUser(int id)
        {
            Restaurant? rest = await getUserRestaurant();
            Dish res = null;
            if (rest != null && !String.IsNullOrEmpty(rest.Name))
            {
                // in dish, we need to make sure line and restmenu are connected, so we check that they are valid
                try
                {
                    res = await r_DbContext.Dishes.FirstAsync(d => d.ID == id);
                    RestMenu menu = await r_DbContext.RestMenus.FirstAsync(rm => rm.ID == res.RestMenuId && rm.RestaurantId == rest.ID);
                    // at this point - both exist - yay
                }
                catch (Exception ex)
                {
                    // either no dish with such id or it doesn't belong to this user
                    res = null;
                }
            }

            return res;
        }

        protected override async void updateItem(Dish existingItem, Dish updatedItem)
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

        internal override async Task<ActionResult<IEnumerable<Dish>>> get()
        {
            Restaurant restaurant = await getUserRestaurant();
            
            if(restaurant != null)
            {
                List<Dish> result = new List<Dish>();
                List<RestMenu> restMenus = await r_DbContext.RestMenus.Where(rm => rm.RestaurantId == restaurant.ID).Include("Dishes").ToListAsync();
                if(restMenus.Count > 0)
                {
                    restMenus.ForEach(rm =>
                    {
                        result.AddRange(rm.Dishes);
                    });   
                }

                return result;
            } else
            {
                return Forbid();
            }
        }

        internal override async Task<ActionResult<Dish>> getSpecific(int id)
        {
            Restaurant? restaurant = await getUserRestaurant();
            if(restaurant != null)
            {
                Dish? dish = await r_DbContext.Dishes.FirstOrDefaultAsync(d => d.ID == id);
                if(dish!= null && !String.IsNullOrEmpty(dish.Name))
                {
                    Line? line = await r_DbContext.Lines.FirstOrDefaultAsync(l => l.ID == dish.LineId && l.RestaurantId == restaurant.ID);
                    RestMenu? menu = await r_DbContext.RestMenus.FirstOrDefaultAsync(rm => rm.ID == dish.RestMenuId && rm.RestaurantId == restaurant.ID);
                    if(menu == null || line==null || string.IsNullOrEmpty(menu.Name) || string.IsNullOrEmpty(line.Name))
                    {
                        dish = null;
                    }
                }
                else
                {
                    dish = null;
                }
                
                if(dish == null)
                {
                    return NotFound();
                }

                return Ok(dish);
            }
            else
            {
                return Forbid();  
            }
        }
    }
}
