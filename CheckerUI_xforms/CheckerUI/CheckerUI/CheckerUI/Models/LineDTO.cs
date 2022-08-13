using System;
using System.Collections.Generic;
using System.Text;

namespace CheckerUI.Models
{
    public class LineDTO
    {
        public Line line { get; set; }
        public int lineId { get; set; }
        public virtual List<OrderItem> lockedItems { get; set; } = new List<OrderItem>();
        public virtual List<OrderItem> toDoItems { get; set; } = new List<OrderItem>();
        public virtual List<OrderItem> doingItems { get; set; } = new List<OrderItem>();
    }

}
