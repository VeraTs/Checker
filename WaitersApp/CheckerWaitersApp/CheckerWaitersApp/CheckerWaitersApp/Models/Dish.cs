using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheckerWaitersApp.Enums;

namespace CheckerWaitersApp.Models
{
    public class Dish : BaseDBItem
    {
        [Required]
        public string name { get; set; }
        [ForeignKey("Line")]
        public int lineId { get; set; }
        [ForeignKey("RestMenu")]
        public int restMenuId { get; set; }
        [Required]
        public float price { get; set; }
        [ForeignKey("Maker")]
        public int makerId { get; set; }    // what maker is mainly used to create this dish (stovetop/ oven/ etc.)
        public int makerFit { get; set; } = -1; // how many of this dish can be made at the same time inside the maker
        public float estMakeTime { get; set; } = -1; // how much time in minutes it takes to make - an estimate

        public string description { get; set; }
        public eDishType type { get; set; }
    }
}
