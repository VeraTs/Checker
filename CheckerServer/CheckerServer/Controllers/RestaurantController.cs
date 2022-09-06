using Microsoft.AspNetCore.Mvc;
using CheckerServer.Models;
using CheckerServer.Data;
using Microsoft.EntityFrameworkCore;
using CheckerServer.utils;
using Microsoft.AspNetCore.SignalR;

namespace CheckerServer.Controllers
{
    [Route("Restaurants")]
    [ApiController]
    public class RestaurantController : BasicDbController<Restaurant>
    {
        public RestaurantController(CheckerDBContext context)
            : base(context, context.Restaurants) 
        { }

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
            var res = await r_Set
                .Include("ServingAreas")
                .Include("Lines")
                .Include("Menus.Dishes")
                .ToListAsync();

            return res;
        }

        override internal async Task<ActionResult<Restaurant>> getSpecific(int id)
        {
            var res = await r_Set
                .Include("ServingAreas")
                .Include("Lines")
                .Include("Menus.Dishes")
                .FirstOrDefaultAsync(d => d.ID == id);

            return res;
        }

        public async Task<ActionResult<Restaurant>> Add(Restaurant item)
        {
            if (ModelState.IsValid && item != null)
            {
                Restaurant rest = await r_DbContext.Restaurants.FirstOrDefaultAsync(r => r.Email.Equals( item.Email));
                if (rest != null)
                {
                    return BadRequest("The Email is already in use");
                }
                try
                {
                    int res = (await DBSetHelper.AddHelper<Restaurant>(r_DbContext, item, r_Set)).Value;
                    if (res > 0)
                    {
                        // return the added item
                        return item;
                    }
                    else
                    {
                        // return null item
                        return BadRequest("DB error: cannot insert item");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(CRUDutils.onRejectFromDB(item));
                }
            }
            else
            {
                String msg = "";
                if (item == null)
                {
                    msg += "Invalid Syntax for Object\n";
                }

                if (!ModelState.IsValid)
                {
                    msg += "Internal error";
                }
                return BadRequest(msg);
            }
        }
    }
}
