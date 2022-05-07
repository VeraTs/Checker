using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerDTOs
{
    public class DishIngredient
    {
        [Required]
        [ForeignKey("Id")]
        public Ingredient Ingredient { get; set; }
        [Required]
        [ForeignKey("DishId")]
        public Dish Dish { get; set; }
        [Required]
        [ForeignKey("Type")]
        public Measurement Measurement { get; set; }
        public int Amount { get; set; } = 0;
    }
}
