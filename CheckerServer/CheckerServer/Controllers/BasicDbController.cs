using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckerServer.utils;


namespace CheckerServer.Controllers
{
    [ApiController]
    public abstract class BasicDbController<T> : ControllerBase where T: BaseDBItem
    {
        protected readonly CheckerDBContext r_DbContext;
        protected DbSet<T> r_Set;
        public BasicDbController(CheckerDBContext context, DbSet<T> set)
        {
            r_DbContext = context;
            r_Set = set;
        }

        // recognizing user and restaurant for him
        internal virtual async Task<Restaurant> getUserRestaurant()
        {
            Restaurant? userRestaurant = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (ManagementUtils.hasUserRest(HttpContext.User.Identity.Name))
                {
                    userRestaurant = ManagementUtils.getUserRest(HttpContext.User.Identity.Name);
                } else
                {
                    try
                    {
                        userRestaurant = await r_DbContext.Restaurants.FirstAsync(r => r.OwnerEmail == HttpContext.User.Identity.Name.ToString());
                        ManagementUtils.addUserRest(HttpContext.User.Identity.Name, userRestaurant);
                    }
                    catch (Exception ex)
                    {
                        userRestaurant = null;
                    }
                }
            }

            return userRestaurant;
        }

        // getting all instances of this entity type. 
        // for more specified Includes and terms, please override.
        internal virtual async Task<ActionResult<IEnumerable<T>>> get()
        {
            var res = await r_Set.ToListAsync();
            return res;
        }

        // getting a specific instance of this entity type. 
        // for more specified Includes and terms, please override.
        internal virtual async Task<ActionResult<T>> getSpecific(int id)
        {
            var res = await r_Set.FirstOrDefaultAsync(item => item.ID == id);
            return res;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> Get()
        {
            return await get();
        }

        // GET specific with id
        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetWithId(int id)
        {
            var res = await getSpecific(id);

            if (res == null)
            {
                return NotFound();
            }

            return res;
        }

        // POST : add new instance
        [HttpPost]
        public async Task<ActionResult<T>> Add(T item)
        {
            T userItem = await createAuthenticatedUserItem(item);
            if (ModelState.IsValid && userItem != null)
            {
                try
                {
                    int res = (await DBSetHelper.AddHelper<T>(r_DbContext, userItem, r_Set)).Value;
                    if(res > 0)
                    {
                        // return the added item
                        return userItem;
                    }else
                    {
                        // return null item
                        return BadRequest("DB error: cannot insert item");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(CRUDutils.onRejectFromDB(userItem));
                }
            }
            else
            {
                String msg = "";
                if (userItem == null)
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

        protected abstract Task<T?> createAuthenticatedUserItem(T item);

        // this method is specific to each entity type, so must be configured.
        // Don't forget to check for null!
        protected abstract void updateItem(T existingItem, T updatedItem);

        // PUT : edit existing instance
        [HttpPut]
        public async Task<ActionResult<int>> Edit(T item)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var res = await getItemForAuthentitcatedUser(item.ID);
                if (ModelState.IsValid && res != null)
                {
                    try
                    {
                        var existing = await r_Set.FirstOrDefaultAsync(i => i.ID == res.ID);
                        if (existing == null)
                        {
                            throw (new Exception("No such item"));
                        }
                        else
                        {
                            // editing existing serving area
                            updateItem(existing, res);
                            return await DBSetHelper.EditHelper<T>(r_DbContext, existing, r_Set);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(CRUDutils.onRejectFromDB(res));
                    }
                }
                else
                {
                    String msg = "";
                    if (item == null)
                    {
                        msg += "Invalid Sytax for Object\n";
                    }

                    if (!ModelState.IsValid)
                    {
                        msg += "Internal error";
                    }
                    return BadRequest(msg);
                }
            }
            else
            {
                return Forbid();
            }
        }

        // DELETE specific instance
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var res = await getItemForAuthentitcatedUser(id);
                if (res != null && ModelState.IsValid)
                {
                    try
                    {
                        r_Set.Remove(res);
                        return await r_DbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Something went wrong!");
                    }
                }
                else
                {
                    return BadRequest("No such item");
                }
            }
            else
            {
                return Forbid();
            }
        }

        protected abstract Task<T> getItemForAuthentitcatedUser(int id);
    }
}
