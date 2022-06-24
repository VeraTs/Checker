using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    // representing an actual menu with dishes divided by Departments (starter, entry, main, etc.)
    public class RestMenu
    {
        public int ID;
        public int RestaurantId { get; set; }
        public string Name { get; set; }

        // practicals
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
