using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    // like a window or a serving line
    // where completed dishes go, waiting to be picked up by servers 
    // to be carried to tables
    public class ServingArea
    {
        public int ID;
        public int RestaurantId { get; set; }
        
        public int ZoneNum { get; set; }    // how many distinct zones there are in the servbing area to place dishes

        public string Name { get; set; }
        // for each zone depicts which orderitem resides there
        private Dictionary<int, OrderItem> zones = new Dictionary<int, OrderItem>();

        public List<Line> Lines { get; set; } = new List<Line>();
    }
}
