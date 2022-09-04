using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CheckerServer.Controllers
{
    [Route("ServingAreas")]
    [ApiController]
    public class ServingAreaController : BasicDbController<ServingArea>
    {
        public ServingAreaController(CheckerDBContext context) 
            : base(context, context.ServingAreas)
        {}

        protected override async Task<ServingArea?> createAuthenticatedUserItem(ServingArea item)
        {
            ServingArea? sa = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    if(item.RestaurantId == 0)
                    {
                        item.RestaurantId = rest.ID;
                    }

                    if(item.RestaurantId == rest.ID)
                    {
                        sa = item;
                    }
                }
            }
            return sa;
        }

        protected override async Task<ServingArea> getItemForAuthentitcatedUser(int id)
        {
            ServingArea? sa = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        sa = await r_DbContext.ServingAreas.FirstAsync(s => s.ID == id && sa.RestaurantId == rest.ID);
                    }
                    catch(Exception ex) {}
                }
            }
            return sa;
        }

        protected override void updateItem(ServingArea existingItem, ServingArea updatedItem)
        {
            if(!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }

            if (updatedItem.ZoneNum != 0)
            {
                existingItem.ZoneNum = updatedItem.ZoneNum;
            }
        }

        override internal async Task<ActionResult<IEnumerable<ServingArea>>> get()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                List<ServingArea> areas = new List<ServingArea>();
                if (rest != null)
                {
                    areas = await r_Set.Where(sa => sa.RestaurantId == rest.ID).ToListAsync();
                    return areas;
                } else
                {
                    return BadRequest();
                }
            } else
            {
                return Forbid();
            }
        }

        override internal async Task<ActionResult<ServingArea>> getSpecific(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                ServingArea? area = null;
                if (rest != null)
                {
                    try
                    {
                        area = await r_Set.FirstAsync(sa => sa.RestaurantId == rest.ID && sa.ID == id);
                    }
                    catch (Exception ex) {
                        return BadRequest("No such serving area in this restaurant");
                    }
                    
                    return area;
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
