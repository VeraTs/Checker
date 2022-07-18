using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CheckerUI.Models
{
    public class ServingArea : BaseDBItem
    {
        // how many distinct zones there are in the servbing area to place dishes
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        [Required]
        public int ZoneNum { get; set; }
        [Required]
        public string Name { get; set; }
        // for each zone depicts which orderitem resides there
        private Dictionary<int, OrderItem> zones = new Dictionary<int, OrderItem>();

        public List<Line> Lines { get; set; } = new List<Line>();
    }
}
