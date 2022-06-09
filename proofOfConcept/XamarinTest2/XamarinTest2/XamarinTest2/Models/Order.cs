using System.Collections.Generic;


namespace CheckerDTOs
{
    public class Order : BaseDBItem
    {
        public int RestaurantId { get; set; }
        public int Table { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Ordered;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderType OrderType { get; set; } = OrderType.FIFO;
        public int TotalPrice { get; set; }
        public int TotalPayedFor { get; set; }

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
