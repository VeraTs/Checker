using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerWaitersApp.Models
{
    // representing an actual menu with dishes divided by Departments (starter, entry, main, etc.)
    public class RestMenu : BaseDBItem
    {
        [ForeignKey("Restaurant")]
        public int restaurantId { get; set; }
        [Required]
        public string name { get; set; }

        // practicals
        public List<Dish> dishes { get; set; } = new List<Dish>();
    }
}
