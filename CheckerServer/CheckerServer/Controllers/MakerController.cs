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
        public MakerController(CheckerDBContext context, DbSet<Maker> set) : base(context, set)
        {
        }

        protected override void updateItem(Maker existingItem, Maker updatedItem)
        {
            if (!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }
        }
    }
}
