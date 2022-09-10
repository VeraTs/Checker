namespace CheckerServer.Models
{
    public class StatisticsResponse
    {
        public int RestaurantId { get; set; }

        public int Month { get; set; }
        public int DishId { get; set; }
        public long TimesOrdered { get; set; } = -1;
        public double AvgPrepTime { get; set; } = -1;

        public string dishName { get; set; }

        public eDishType eDishType { get; set; }
    }
}
