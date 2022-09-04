using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Controllers
{
    [Route("Makers")]
    [ApiController]
    public class MakerController : BasicDbController<Maker>
    {
        public MakerController(CheckerDBContext context) : base(context, context.Maker)
        {
        }

        protected override async Task<Maker> createAuthenticatedUserItem(Maker item)
        {
            Restaurant? rest = await getUserRestaurant();
            Maker? res = null;
            if (rest != null && !String.IsNullOrEmpty(rest.Name))
            {
                // in dish, we need to make sure line and restmenu are connected, so we check that they are valid
                try
                {
                    Line line = await r_DbContext.Lines.FirstAsync(l => l.ID == item.LineId && l.RestaurantId == rest.ID);

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

        protected override async Task<Maker> getItemForAuthentitcatedUser(int id)
        {
            Restaurant? rest = await getUserRestaurant();
            Maker res = null;
            if (rest != null && !String.IsNullOrEmpty(rest.Name))
            {
                // in dish, we need to make sure line and restmenu are connected, so we check that they are valid
                try
                {
                    res = await r_DbContext.Maker.FirstAsync(d => d.ID == id);
                    Line line = await r_DbContext.Lines.FirstAsync(l => l.ID == res.LineId && l.RestaurantId == rest.ID);
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

        protected override void updateItem(Maker existingItem, Maker updatedItem)
        {
            if (!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }
        }

        internal override async Task<ActionResult<IEnumerable<Maker>>> get()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if(rest != null)
                {
                    List<Line> lines = await r_DbContext.Lines.Include("Makers").Where(l => l.RestaurantId == rest.ID).ToListAsync();
                    List<Maker> res = new List<Maker>();
                    lines.ForEach(line =>
                    {
                        res.AddRange(line.Makers);
                    });

                    return Ok(res);
                }
                else
                {
                    return Forbid();
                }    
            }

            return Forbid();
        }

        internal override async Task<ActionResult<Maker>> getSpecific(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        Maker maker = await r_DbContext.Maker.FirstAsync(mk => mk.ID == id);
                        Line lines = await r_DbContext.Lines.FirstAsync(l => l.RestaurantId == rest.ID && l.ID == maker.LineId);

                        return Ok(maker);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("No such maker in the restaurant");
                    }
                }
                else
                {
                    return Forbid();
                }
            }

            return Forbid();
        }
    }
}
