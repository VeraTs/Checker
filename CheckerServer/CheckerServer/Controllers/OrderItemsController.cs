using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CheckerServer.Controllers
{
    [Route("OrderItems")]
    [ApiController]
    public class OrderItemsController : BasicDbController<OrderItem>
    {
        public OrderItemsController(CheckerDBContext context)
            : base(context, context.OrderItems)
        { }

        protected override async Task<OrderItem?> createAuthenticatedUserItem(OrderItem item)
        {
            OrderItem? orderItem = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        Order? order = await r_DbContext.Orders.FirstAsync(o => o.ID == item.OrderId && o.RestaurantId == rest.ID);
                        if(order!= null)
                        {
                            orderItem = item;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return orderItem;
        }

        protected override async Task<OrderItem?> getItemForAuthentitcatedUser(int id)
        {
            OrderItem? orderItem = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    try
                    {
                        orderItem = await r_DbContext.OrderItems.FirstAsync(oi => oi.ID == id);
                        if(orderItem != null)
                        {
                            Order? order = await r_DbContext.Orders.FirstAsync(o => o.ID == orderItem.OrderId && o.RestaurantId == rest.ID);
                            if (order == null)
                            {
                                orderItem = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return orderItem;
        }

        protected override void updateItem(OrderItem existingItem, OrderItem updatedItem)
        {
            if (updatedItem.ServingAreaZone != -1)
            {
                existingItem.ServingAreaZone = updatedItem.ServingAreaZone;
            }

            if (!string.IsNullOrEmpty(updatedItem.Changes))
            {
                existingItem.Changes = updatedItem.Changes;
            }

            existingItem.LineStatus = updatedItem.LineStatus;

            existingItem.Status = updatedItem.Status;
        }

        override internal async Task<ActionResult<IEnumerable<OrderItem>>> get()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    List<Order> orders = await r_DbContext.Orders.Where(o => o.RestaurantId == rest.ID).ToListAsync();
                    List<OrderItem> items = new List<OrderItem>();

                    foreach (Order item in orders)
                    {
                        items.AddRange(
                             r_Set
                            .Include(i => i.Dish)
                            .Where(i => i.OrderId == item.ID)
                            );
                    }                

                    return items;
                }

                return BadRequest();
            }

            return Forbid();
        }

        override internal async Task<ActionResult<OrderItem>> getSpecific(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Restaurant? rest = await getUserRestaurant();
                if (rest != null)
                {
                    OrderItem orderItem = await r_Set
                            .Include(i => i.Dish)
                            .FirstAsync(i => i.ID == id);
                    if(orderItem != null)
                    {
                        Order? order = await r_DbContext.Orders.FirstAsync(o => o.ID == orderItem.OrderId && o.RestaurantId == rest.ID);
                        if(order != null)
                        {
                            return Ok(orderItem);
                        }
                    }
                }

                return BadRequest();
            }

            return Forbid();
        }
    }
}
