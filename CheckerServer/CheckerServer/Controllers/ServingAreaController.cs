using CheckerServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckerServer.utils;
using CheckerDTOs;

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
    }
}
