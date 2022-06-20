﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class Dish : BaseDBItem
    {
        [Required]
        public string Name { get; set; }
        [ForeignKey("Line")]
        public int LineId { get; set; }
        [ForeignKey("RestMenu")]
        public float RestMenuId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int MakerId { get; set; }    // what maker is mainly used to create this dish (stovetop/ oven/ etc.)
        public int MakerFit { get; set; } = -1; // how many of this dish can be made at the same time inside the maker

        public string Description { get; set; }
        public DishType Type { get; set; }
    }
}
