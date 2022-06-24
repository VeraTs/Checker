using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class Dish
    {
        public int ID;
        public string Name { get; set; }
        public int LineId { get; set; }
        public int RestMenuId { get; set; }
        public float Price { get; set; }
        public int MakerId { get; set; }    // what maker is mainly used to create this dish (stovetop/ oven/ etc.)
        public int MakerFit { get; set; } = -1; // how many of this dish can be made at the same time inside the maker
        public float EstMakeTime { get; set; } = -1; // how much time in minutes it takes to make - an estimate

        public string Description { get; set; }
        public DishType Type { get; set; }
    }
}
