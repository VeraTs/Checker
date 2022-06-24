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
            var res = await r_Set
                .Include(i=>i.Lines)
                .ToListAsync();

            return res;
        }

        override internal async Task<ActionResult<ServingArea>> getSpecific(int id)
        {
            var res = await r_Set
                .Include(i => i.Lines)
                .FirstOrDefaultAsync(d => d.ID == id);

            return res;
        }
    }
}
