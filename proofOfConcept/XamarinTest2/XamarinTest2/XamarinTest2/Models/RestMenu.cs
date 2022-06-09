using System.Collections.Generic;

namespace CheckerDTOs
{
    // representing an actual menu with dishes divided by Departments (starter, entry, main, etc.)
    public class RestMenu : BaseDBItem
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }

        // practicals
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
