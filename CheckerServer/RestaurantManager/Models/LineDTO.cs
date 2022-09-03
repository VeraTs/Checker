namespace RestaurantManager.Models
{
    public class LineDTO
    {
        public Line line { get; set; }
        public int lineId { get; set; }
        public virtual List<OrderItem> LockedItems { get; set; } = new List<OrderItem>();
        public virtual List<OrderItem> ToDoItems { get; set; } = new List<OrderItem>();
        public virtual List<OrderItem> DoingItems { get; set; } = new List<OrderItem>();
        public virtual List<OrderItem> DoneItems { get; set; } = new List<OrderItem>();
    }
}
