using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CheckerServer.Controllers
{
    [Route("Lines")]
    [ApiController]
    public class LinesController : BasicDbController<Line>
    {
        public LinesController(CheckerDBContext context)
            : base(context, context.Lines)
        { }

        protected override async Task<Line?> createAuthenticatedUserItem(Line item)
        {
            Line? line = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    if (item.RestaurantId == 0)
                    {
                        item.RestaurantId = rest.ID;
                    }

                    if (item.RestaurantId == rest.ID)
                    {
                        line = item;
                    }
                }
            }

            return line;
        }

        protected override async Task<Line?> getItemForAuthentitcatedUser(int id)
        {
            Line? line = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        line = await r_DbContext.Lines.FirstAsync(l => l.ID == id && l.RestaurantId == rest.ID);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return line;
        }

        protected override void updateItem(Line existingItem, Line updatedItem)
        {
            if (!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }

            if (updatedItem.Limit != 0)
            {
                existingItem.Limit = updatedItem.Limit;
            }

            existingItem.State = updatedItem.State;

            if (updatedItem.ServingAreaId != 0)
            {
                existingItem.ServingAreaId = updatedItem.ServingAreaId;
            }

            // I do not allow a line to change the restaurant it is in
        }

        override internal async Task<ActionResult<IEnumerable<Line>>> get()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    var res = await r_Set
                        .Where(l => l.RestaurantId == rest.ID)
                        .Include("Dishes")
                        .ToListAsync();

                    return res;
                }

                return BadRequest();
            }
            return Forbid();
        }

        override internal async Task<ActionResult<Line>> getSpecific(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        var res = await r_Set
                        .Include("Dishes")
                        .FirstAsync(l => l.RestaurantId == rest.ID);

                        return res;
                    }
                    catch (Exception)
                    {
                    }
                }

                return BadRequest();
            }
            return Forbid();
        }

    }
}
