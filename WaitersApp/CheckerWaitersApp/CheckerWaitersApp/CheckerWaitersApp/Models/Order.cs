using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CheckerWaitersApp.Enums;

namespace CheckerWaitersApp.Models
{
    public class Order : BaseDBItem
    {
        [ForeignKey("Restaurant")]
        public int restaurantId { get; set; }
        public int table { get; set; }
        public eOrderStatus status { get; set; } = eOrderStatus.Ordered;
        public List<OrderItem> items { get; set; } = new List<OrderItem>();
        public eOrderType orderType { get; set; } = eOrderType.FIFO; // to check
        public float totalCost { get; set; }
        public float remainsToPay { get; set; }

        public DateTime createdDate { get; set; }
        public DateTime doneDate { get; set; }
    }
}
