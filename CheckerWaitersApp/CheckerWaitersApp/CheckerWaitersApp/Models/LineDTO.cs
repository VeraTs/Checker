using System.Collections.Generic;
namespace CheckerWaitersApp.Models
{
    public class LineDTO
    {
        public Line line { get; set; }
        public int lineId { get; set; }
        public List<OrderItem> lockedItems { get; set; } = new List<OrderItem>();
        public List<OrderItem> toDoItems { get; set; } = new List<OrderItem>();
        public List<OrderItem> doingItems { get; set; } = new List<OrderItem>();
        public List<OrderItem> doneItems { get; set; } = new List<OrderItem>();
    }
}
