using System.Collections.Generic;


namespace CheckerDTOs
{
    // Line in restaurant such as hot line, cold line, oven, etc.
    public class Line : BaseDBItem
    {   
        public string Name { get; set; }
        public int ServingAreaId { get; set; } // id of serving area related to this line

        public int Limit { get; set; } = -1; // -1 means no limit
        
        public LineState State { get; set; } = LineState.Closed; // starts off closed, changes to open upon user request

        public List<Dish> Dishes { get; set; } = new List<Dish>();

        

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
