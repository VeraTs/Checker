using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class OrderItem : BaseDBItem
    {
        public Dish Dish { get; set; }        
        
        [ForeignKey("Dish")]
        public int DishId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required]
        public int ServingAreaZone { get; set; }

        public string Changes { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.Ordered;
        public LineItemStatus LineStatus { get; set; } = LineItemStatus.Locked;
    }

    public enum ItemStatus      // the status of the order item overall
    {
        Ordered,                // initial state
        AtLine,                 // during entire time it is at preperation line
        WaitingToBeServed,      // at serving zone
        Served,                 // served to table
        Returned                // customer returned item for some reason
    }

    public enum LineItemStatus  // status of the order item in the line of preperation 
    {
        Locked,
        ToDo,
        Doing,
        Done,
        Rejected
    }
}
