using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerWaitersApp.Models
{
    public class DishIngredient
    {
        [Required]
        [ForeignKey("Id")]
        public Ingredient ingredient { get; set; }
        [Required]
        [ForeignKey("DishId")]
        public Dish dish { get; set; }
        [Required]
        [ForeignKey("Type")]
        public Measurement measurement { get; set; }
        public int amount { get; set; } = 0;
    }
}
