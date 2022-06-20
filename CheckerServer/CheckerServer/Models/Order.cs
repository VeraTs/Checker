using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class Order : BaseDBItem
    {
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public int Table { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Ordered;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderType OrderType { get; set; } = OrderType.FIFO;
        public float TotalCost { get; set; }
        public float RemainsToPay { get; set; }
    }

    public enum OrderStatus
    {
        Ordered,
        InProgress,
        Done
    }

    public enum OrderType
    {
        AllTogether, // all ordered items served together
        Staggared, // starters first, then mains, then desserts
        FIFO // whatever is ready first is sent first to the table
    }
}
