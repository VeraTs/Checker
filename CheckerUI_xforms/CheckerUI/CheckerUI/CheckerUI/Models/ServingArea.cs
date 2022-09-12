using System.Collections.Generic;
namespace CheckerUI.Models
{
    public class ServingArea : BaseDBItem
    {
        public int restaurantId { get; set; }
        public int zoneNum { get; set; }
        public string name { get; set; }

        private Dictionary<int, OrderItem> zones = new Dictionary<int, OrderItem>();
    }
}
