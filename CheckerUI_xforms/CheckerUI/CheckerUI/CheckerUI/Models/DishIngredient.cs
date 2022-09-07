using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Text;

namespace CheckerUI.Models
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
