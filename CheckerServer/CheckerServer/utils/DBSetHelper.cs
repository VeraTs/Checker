using Microsoft.AspNetCore.Mvc;
using CheckerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.utils
{
    public class DBSetHelper
    {
        internal static async Task<ActionResult<int>> AddHelper<T>(DbContext context, BaseDBItem item, DbSet<T> set) where T : BaseDBItem
        {
            if (item != null && item is T)
            {
                T classItem = item as T;
                set.Add(classItem);
            }

            return await context.SaveChangesAsync();
        }

        internal static async Task<ActionResult<int>> EditHelper<T>(DbContext context, BaseDBItem item, DbSet<T> set) where T : BaseDBItem
        {
            if (item != null && item is T)
            {
                T classItem = item as T;
                set.Update(classItem);
            }

            return await context.SaveChangesAsync();
        }
    }
}
