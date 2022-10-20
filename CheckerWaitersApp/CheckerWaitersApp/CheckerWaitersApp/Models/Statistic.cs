namespace CheckerWaitersApp.Models
{
    public class Statistic
    {
        public int restaurantId { get; set; }
        public int month { get; set; }
        public int dishId { get; set; }
        public long timesOrdered { get; set; } = -1;
        public double accPrepTime { get; set; } = -1;
    }
}
