using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.utils
{
    public class StatisticUtils
    {
        
        public async static Task<Boolean> updateDishStat(CheckerDBContext context, OrderItem orderItem)
        {
            Boolean success = false;
            int currMonth = DateTime.Now.Month;
            try
            {
                Statistic stat = await context.Statistics.FirstOrDefaultAsync(stat => stat.Month == currMonth && stat.DishId == orderItem.DishId);
                if(stat.AccPrepTime < 0)
                {
                    // no such record in table
                    stat.Month = currMonth;
                    stat.DishId = orderItem.DishId;
                    stat.TimesOrdered = 0;
                    stat.AccPrepTime = 0;
                }

                stat.TimesOrdered++;
                long elapsedTicks = orderItem.Finish.Ticks - orderItem.Start.Ticks;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                double doneTime = elapsedSpan.TotalMinutes;
                stat.AccPrepTime += doneTime;

                success = ( await context.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {

            }
            
            //this.Dish.AvrageMonthSales = (this.Dish.AvrageMonthSales * this.Dish.ThisMonthSales + (int)doneTime) / (this.Dish.ThisMonthSales + 1);
            return success;
        }
    }
}
