using CheckerUI.Enums;

namespace CheckerUI.Models
{
    public class Dish : BaseDBItem
    {
      
        public string name { get; set; }
        
        public int lineId { get; set; }
     
        public int restMenuId { get; set; }
      
        public float price { get; set; }

       
        public int makerId { get; set; } = -1;   // what maker is mainly used to create this dish (stovetop/ oven/ etc.)

        public int makerFit { get; set; } = -1; // how many of this dish can be made at the same time inside the maker
        public float estMakeTime { get; set; } = -1; // how much time in minutes it takes to make - an estimate

        public string description { get; set; }
        public eDishType type { get; set; }
    }
}
