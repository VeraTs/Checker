using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Models
{
    public class Statistic
    {
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        public int Month { get; set; }
        [ForeignKey("Dish")]
        public int DishId { get; set; }
        public long TimesOrdered { get; set; } = -1;
        public double AccPrepTime { get; set; } = -1;
    }
}
