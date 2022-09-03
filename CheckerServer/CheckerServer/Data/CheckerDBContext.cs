using CheckerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Data
{
    public class CheckerDBContext : DbContext
    {
        public CheckerDBContext(DbContextOptions<CheckerDBContext> options)
            : base(options)
        {}

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DishStatistic> DishStatistics { get; set; }
        public DbSet<RestMenu> RestMenus { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<ServingArea> ServingAreas { get; set; }
        public DbSet<Maker> Maker { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
