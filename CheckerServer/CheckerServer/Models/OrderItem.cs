using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class OrderItem : BaseDBItem
    {
        public virtual Dish? Dish { get; set; }

        [ForeignKey("Dish")]
        public int DishId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required]
        public int ServingAreaZone { get; set; }
        
        public string Changes { get; set; }
        public eItemStatus Status { get; set; } = eItemStatus.Ordered;
        public eLineItemStatus LineStatus { get; set; } = eLineItemStatus.Locked;
    }

    public enum eItemStatus      // the status of the order item overall
    {
        Ordered,                // initial state
        AtLine,                 // during entire time it is at preperation line
        WaitingToBeServed,      // at serving zone
        Served,                 // served to table
        Returned                // customer returned item for some reason
    }

    public enum eLineItemStatus  // status of the order item in the line of preperation 
    {
        Locked,
        ToDo,
        Doing,
        Done,
        Rejected
    }
}
