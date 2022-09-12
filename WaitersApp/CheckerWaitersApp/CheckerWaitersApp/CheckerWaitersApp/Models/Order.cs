using System.Collections.Generic;
using CheckerWaitersApp.Enums;

namespace CheckerWaitersApp.Models
{
    public class Order : BaseDBItem
    {
        public int restaurantId { get; set; }
        public int table { get; set; }
        public eOrderStatus status { get; set; } = eOrderStatus.Ordered;
        public List<OrderItem> items { get; set; } = new List<OrderItem>();
        public eOrderType orderType { get; set; } = eOrderType.FIFO;
        public float totalCost { get; set; }
        public float remainsToPay { get; set; }

    }
}
