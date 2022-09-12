using System.Collections.Generic;

namespace CheckerWaitersApp.Models
{
    public class RestMenu : BaseDBItem
    {

        public int restaurantId { get; set; }

        public string name { get; set; }

        // practicals
        public List<Dish> dishes { get; set; } = new List<Dish>();
    }
}
