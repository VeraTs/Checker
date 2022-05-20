using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    // Line in restaurant such as hot line, cold line, oven, etc.
    public class Line : BaseDBItem
    {   
        [Required]
        public string Name { get; set; }
        [ForeignKey("ServingArea")]
        public int ServingAreaId { get; set; } // id of serving area related to this line

        public int Limit { get; set; } = -1; // -1 means no limit
        
        public LineState State { get; set; } = LineState.Closed; // starts off closed, changes to open upon user request

        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }

    public enum LineState
    {
        Closed,
        Open,
        Busy
    }
}
