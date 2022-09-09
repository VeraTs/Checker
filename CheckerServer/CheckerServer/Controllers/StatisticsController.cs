using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Controllers
{
    [Route("Statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private CheckerDBContext r_DbContext;

        public StatisticsController(CheckerDBContext context)
        {
            r_DbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatisticsResponse>>> Get()
        {
            return null;
        }

        // GET specific with id
        [HttpGet("{id}/{month}")]
        public async Task<ActionResult<ICollection<StatisticsResponse>>> GetWithId(int id, int month)
        {
            Restaurant restaurant = await r_DbContext.Restaurants
                .Include("Menus.Dishes")
                .FirstOrDefaultAsync(rest => rest.ID == id);
            if (restaurant == null)
            {
                return NotFound("No such restaraunt!");
            }
            else if (month < 1 || month > 12)
            {
                return NotFound("Invalid input");

            }
            List<Statistic> stats = await r_DbContext.Statistics.Where(s => s.RestaurantId == id && s.Month == month).ToListAsync();


            if (stats == null)
            {
                return NotFound();
            }

            List<StatisticsResponse> res = new List<StatisticsResponse>();
            stats.ForEach(async stat =>
            {
                StatisticsResponse resp = new StatisticsResponse();
                resp.RestaurantId = stat.RestaurantId;
                resp.TimesOrdered = stat.TimesOrdered;
                resp.AvgPrepTime = stat.AccPrepTime / stat.TimesOrdered;
                resp.TimesOrdered = stat.TimesOrdered;
                resp.Month = stat.Month;
                resp.DishId = stat.DishId;

                foreach (RestMenu menu in restaurant.Menus)
                {
                    foreach (Dish dish in menu.Dishes)
                    {
                        if (dish.ID == stat.DishId)
                        {
                            resp.dishName = dish.Name;
                            resp.eDishType = dish.Type;
                        }
                    }
                }

                res.Add(resp);
            });


            return res;
        }
    }
}
