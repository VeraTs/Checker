using System.Collections.Generic;
using CheckerUI.Enums;


namespace CheckerUI.Models
{
    public class Line : BaseDBItem
    {
         public string name { get; set; }
        public int servingAreaId { get; set; } // id of serving area related to this line
         public int? restaurantId { get; set; }

        public virtual ServingArea servingArea { get; set; }

        public int limit { get; set; } = -1; // -1 means no limit

        public eLineState state { get; set; } =
            eLineState.Closed; // starts off closed, changes to open upon user request

        public List<Dish> dishes { get; set; } = new List<Dish>(); // dishes made in this Line

        public List<Maker> makers { get; set; } = new List<Maker>(); // makers that live in this line
    }
}