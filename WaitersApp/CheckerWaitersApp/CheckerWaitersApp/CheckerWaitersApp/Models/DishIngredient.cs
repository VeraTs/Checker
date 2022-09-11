using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerWaitersApp.Models
{
    public class DishIngredient
    {
       
        public Ingredient ingredient { get; set; }
       
        public Dish dish { get; set; }
      
        public Measurement measurement { get; set; }
        public int amount { get; set; } = 0;
    }
}
