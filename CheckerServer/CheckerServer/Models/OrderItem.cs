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
        public ItemStatus Status { get; set; } = ItemStatus.Unstarted;
        public LineItemStatus LineStatus { get; set; } = LineItemStatus.Locked;
    }

    public enum ItemStatus
    {
        Unstarted,
        InProgress,
        Done,
        Served,
        Returned
    }

    public enum LineItemStatus
    {
        Locked,
        ToDo,
        Doing,
        Done,
        Rejected
    }
}
