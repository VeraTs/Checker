﻿using System;
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
