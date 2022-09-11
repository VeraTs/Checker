using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CheckerUI.Models
{
    public class ServingArea : BaseDBItem
    {
        // how many distinct zones there are in the servbing area to place dishes
       
        public int restaurantId { get; set; }
       
        public int zoneNum { get; set; }
      
        public string name { get; set; }
        // for each zone depicts which orderitem resides there
        private Dictionary<int, OrderItem> zones = new Dictionary<int, OrderItem>();

        public List<Line> lines { get; set; } = new List<Line>();
    }
}
