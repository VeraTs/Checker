using System;
using CheckerUI.Enums;

namespace CheckerUI.Models
{
    public class OrderItem
    {
        public int id { get; set; }
        public int table { get; set; } // change to inside property ?
        public Dish dish { get; set; }

        public int dishId { get; set; }
     
        public int orderId { get; set; }
       
        public int servingAreaZone { get; set; }
        public string changes { get; set; }
        public eItemStatus status { get; set; } = eItemStatus.Ordered;
        public eLineItemStatus lineStatus { get; set; } = eLineItemStatus.Locked;
        public bool isPayedFor { get; set; } = false;

        public DateTime startDate { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime doneDate { get; set; }

    }
}
