using CheckerWaitersApp.Enums;

namespace CheckerWaitersApp.Models
{
    public class StatisticsResponse
    {
        public int restaurantId { get; set; }

        public int month { get; set; }
        public int dishId { get; set; }
        public long timesOrdered { get; set; } = -1;
        public double avgPrepTime { get; set; } = -1;

        public string dishName { get; set; }

        public eDishType eDishType { get; set; }
        public double grows { get; set; }
    }
}
