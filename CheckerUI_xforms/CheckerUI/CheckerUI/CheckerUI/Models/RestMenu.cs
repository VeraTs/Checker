using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CheckerUI.Models
{
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
