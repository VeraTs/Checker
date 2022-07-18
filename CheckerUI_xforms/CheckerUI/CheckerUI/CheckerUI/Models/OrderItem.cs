using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheckerUI.Enums;

namespace CheckerUI.Models
{
    public class OrderItem : BaseDBItem
    {
        public int table { get; set; } // change to inside property ?
        public Dish dish { get; set; }

        [ForeignKey("Dish")]
        public int dishId { get; set; }
        [ForeignKey("Order")]
        public int orderId { get; set; }
        [Required]
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
