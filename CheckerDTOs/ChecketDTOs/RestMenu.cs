using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerDTOs
{
    // representing an actual menu with dishes divided by Departments (starter, entry, main, etc.)
    public class RestMenu : BaseDBItem
    {
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        [Required]
        public string Name { get; set; }

        // practicals
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
