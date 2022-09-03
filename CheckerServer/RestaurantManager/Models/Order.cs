using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
    public class Order : BaseDBItem
    {
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public int Table { get; set; }
        public eOrderStatus Status { get; set; } = eOrderStatus.Ordered;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public eOrderType OrderType { get; set; } = eOrderType.FIFO;
        public float TotalCost { get; set; }
        public float RemainsToPay { get; set; }
    }

    public enum eOrderStatus
    {
        Ordered,
        InProgress,
        Done
    }

    public enum eOrderType
    {
        AllTogether, // all ordered items served together
        Staggered, // starters first, then mains, then desserts
        FIFO // whatever is ready first is sent first to the table
    }
}
