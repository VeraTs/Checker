using System;
using CheckerUI.Enums;

namespace CheckerUI.Models
{
    public class OrderItem : BaseDBItem
    {
        public  Dish dish { get; set; }

        public int dishId { get; set; }
        
        public int orderId { get; set; }
      
        public int servingAreaZone { get; set; } = -1;

        public string changes { get; set; }
        public eItemStatus status { get; set; } = eItemStatus.Ordered;
        public eLineItemStatus lineStatus { get; set; } = eLineItemStatus.Locked;

        //statistics 
        public DateTime start { get; set; }

        public DateTime finish { get; set; }

    }
}
