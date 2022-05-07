using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerDTOs
{
    public class Order : BaseDBItem
    {
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public int Table { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Ordered;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderType OrderType { get; set; } = OrderType.FIFO;

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
