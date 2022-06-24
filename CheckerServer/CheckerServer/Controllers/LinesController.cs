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
            var res = await r_Set
                .Include("Dishes")
                .ToListAsync();

            return res;
        }

        override internal async Task<ActionResult<Line>> getSpecific(int id)
        {
            var res = await r_Set
                .Include("Dishes")
                .FirstOrDefaultAsync(d => d.ID == id);

            return res;
        }
        }
    }
