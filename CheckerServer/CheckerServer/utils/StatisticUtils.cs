using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.utils
{
    public class StatisticUtils
    {
        public async static Task<Boolean> updateDishStat(IServiceProvider serviceProvider, OrderItem orderItem)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                using (var context = new CheckerDBContext(
                 scope.ServiceProvider.GetRequiredService<
                     DbContextOptions<CheckerDBContext>>()))
                {
                    Boolean success = false;
                    int currMonth = DateTime.Now.Month;
                    try
                    {
                        Statistic stat = await context.Statistics.FirstOrDefaultAsync(s => s.Month == currMonth && s.DishId == orderItem.DishId);
                        if (stat == null || stat.AccPrepTime < 0)
                        {
                            // no such record in table
                            Order order = await context.Orders.FindAsync(orderItem.OrderId);
                            stat = new Statistic();
                            stat.Month = currMonth;
                            stat.DishId = orderItem.DishId;
                            stat.TimesOrdered = 0;
                            stat.AccPrepTime = 0;
                            stat.RestaurantId = order.RestaurantId;
                            context.Statistics.Add(stat);
                        }

                        stat.TimesOrdered++;
                        long elapsedTicks = orderItem.Finish.Ticks - orderItem.Start.Ticks;
                        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                        double doneTime = elapsedSpan.TotalMinutes;
                        stat.AccPrepTime += doneTime;

                        success = (await context.SaveChangesAsync()) > 0;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                    }

                    return success;
                }
            }
        }
    }
}
