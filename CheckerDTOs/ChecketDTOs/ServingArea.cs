using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerDTOs
{
    // like a window or a serving line
    // where completed dishes go, waiting to be picked up by servers 
    // to be carried to tables
    public class ServingArea : BaseDBItem
    {
        // how many distinct zones there are in the servbing area to place dishes
        [Required]
        public int ZoneNum { get; set; } 
        [Required]
        public string Name { get; set; }
        // for each zone depicts which orderitem resides there
        private Dictionary<int, OrderItem> zones = new Dictionary<int, OrderItem>();
    }
}
