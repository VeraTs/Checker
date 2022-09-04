using CheckerServer.Data;
using CheckerServer.Models;
using CheckerServer.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CheckerServer.Controllers
{
    [Route("Orders")]
    [ApiController]
    public class OrderController : BasicDbController<Order>
    {
        public OrderController(CheckerDBContext context)
            : base(context, context.Orders) 
        {
            
        }

        protected override async Task<Order?> createAuthenticatedUserItem(Order item)
        {
            Order? order = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        if (item.RestaurantId == 0)
                        {
                            item.RestaurantId = rest.ID;
                        }

                        if (item.RestaurantId == rest.ID)
                        {
                            order = item;
                        }
                    }
                    catch (Exception ex) { }
                    
                }
            }

            return order;
        }

        protected override async Task<Order?> getItemForAuthentitcatedUser(int id)
        {
            Order? order = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        order = await r_DbContext.Orders.FirstAsync(o => o.ID == id && o.RestaurantId == rest.ID);
                    }
                    catch (Exception ex) { }
                }
            }

            return order;
        }

        protected override void updateItem(Order existingItem, Order updatedItem)
        {
            if (updatedItem.Table != -1)
            {
                existingItem.Table = updatedItem.Table;
            }

            existingItem.Status = updatedItem.Status;
            existingItem.OrderType = updatedItem.OrderType;
        }

        override internal async Task<ActionResult<IEnumerable<Order>>> get()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    var res = await r_Set
                            .Include("Items")
                            .Where(o => o.RestaurantId == rest.ID)
                            .ToListAsync();

                    return res;
                }
            }

            return Forbid();
        }

        override internal async Task<ActionResult<Order>> getSpecific(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        var res = await r_Set
                            .Include("Items")
                            .FirstAsync(o => o.RestaurantId == rest.ID && o.ID == id);

                        return res;
                    }
                    catch (Exception ex) { }

                    return BadRequest();
                }
            }

            return Forbid();
        }
    }
}
