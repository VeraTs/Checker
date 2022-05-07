using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Data
{
    public class CheckerDBContext : DbContext
    {
        public CheckerDBContext(DbContextOptions<CheckerDBContext> options)
            : base(options)
        {}

        public DbSet<CheckerDTOs.Restaurant> Restaurants { get; set; }
        public DbSet<CheckerDTOs.Dish> Dishes { get; set; }
        public DbSet<CheckerDTOs.Ingredient> Ingredients { get; set; }
        public DbSet<CheckerDTOs.Measurement> Measurements { get; set; }
        public DbSet<CheckerDTOs.Order> Orders { get; set; }
        public DbSet<CheckerDTOs.OrderItem> OrderItems { get; set; }
        public DbSet<CheckerDTOs.DishStatistic> DishStatistics { get; set; }
        public DbSet<CheckerDTOs.RestMenu> RestMenus { get; set; }
        public DbSet<CheckerDTOs.Line> Lines { get; set; }
        public DbSet<CheckerDTOs.ServingArea> ServingAreas { get; set; }
    }
}
