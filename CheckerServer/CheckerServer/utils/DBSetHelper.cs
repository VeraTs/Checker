using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.utils
{
    public class DBSetHelper
    {
        internal static async Task<ActionResult<int>> AddHelper<T>(DbContext context, CheckerDTOs.BaseDBItem item, DbSet<T> set) where T : CheckerDTOs.BaseDBItem
        {
            if (item != null && item is T)
            {
                T classItem = item as T;
                set.Add(classItem);
            }

            return await context.SaveChangesAsync();
        }

        internal static async Task<ActionResult<int>> EditHelper<T>(DbContext context, CheckerDTOs.BaseDBItem item, DbSet<T> set) where T : CheckerDTOs.BaseDBItem
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
