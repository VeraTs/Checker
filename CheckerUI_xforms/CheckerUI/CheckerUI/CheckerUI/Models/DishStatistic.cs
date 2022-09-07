using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CheckerUI.Models
{
    public class DishStatistic : BaseDBItem
    {
        public Dish dish { get; set; }
        [ForeignKey("Dish")]
        public int dishId { get; set; }
        [Required]
        public DateTime start { get; set; }
        [Required]
        public DateTime finish { get; set; }
    }
}
