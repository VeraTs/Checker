using System;


namespace CheckerDTOs
{
    public class DishStatistic : BaseDBItem
    {
        
        public Dish Dish { get; set; }
        public int DishId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }
}
