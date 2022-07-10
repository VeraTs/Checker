using System.Collections.Generic;
using CheckerUI.Enums;


namespace CheckerUI.Models
{
    public class Line
    {
       
        public int id { get; set; }

       
        public string Name { get; set; }
       
        public int ServingAreaId { get; set; } // id of serving area related to this line

        public int Limit { get; set; } = -1; // -1 means no limit

        public eLineState State { get; set; } = eLineState.Closed; // starts off closed, changes to open upon user request

        public List<Dish> Dishes { get; set; } = new List<Dish>();  // dishes made in this Line

        public List<Maker> Makers { get; set; } = new List<Maker>();    // makers that live in this line
    }
}
