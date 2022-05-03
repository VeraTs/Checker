using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerDTOs
{
    // Line in restaurant such as hot line, cold line, oven, etc.
    public class Line : BaseDBItem
    {
        //[Required]
        //public int RestaurantId { get; set; }
        
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        [Required]
        public string Name { get; set; }
        public ServingArea ServingArea { get; set; }
        public int ServingAreaId { get; set; } // id of serving area related to this line

        public int Limit { get; set; } = -1; // -1 means no limit
        
        public LineState State { get; set; } = LineState.Closed; // starts off closed, changes to open upon user request

        

        // practicals for actions
        private List<OrderItem> Locked = new List<OrderItem>();
        private List<OrderItem> ToDo = new List<OrderItem>();
        private List<OrderItem> Doing = new List<OrderItem>();
    }

    public enum LineState
    {
        Closed,
        Open,
        Busy
    }
}
