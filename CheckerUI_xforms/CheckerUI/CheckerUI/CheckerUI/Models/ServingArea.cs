using System.Collections.Generic;


namespace CheckerUI.Models
{
    public class ServingArea 
    {


        public int RestaurantId { get; set; }
        public int ZoneNum { get; set; }
        public string Name { get; set; }
        // for each zone depicts which orderitem resides there
        private Dictionary<int, OrderItem> zones = new Dictionary<int, OrderItem>();

        public List<Line> Lines { get; set; } = new List<Line>();
    }
}
