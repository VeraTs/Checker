using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class DishIngredient
    {
        public Ingredient Ingredient { get; set; }
        public Dish Dish { get; set; }
        public Measurement Measurement { get; set; }
        public int Amount { get; set; } = 0;
    }
}
