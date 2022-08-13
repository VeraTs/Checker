using System.Collections.Generic;
namespace CheckerWaitersApp.Models
{
    public class LineDTO
    {
        public Line line { get; set; }
        public virtual List<OrderItem> lockedItems { get; set; } = new List<OrderItem>();
        public virtual List<OrderItem> toDoItems { get; set; } = new List<OrderItem>();
        public virtual List<OrderItem> doingItems { get; set; } = new List<OrderItem>();
    }
}
