using Microsoft.EntityFrameworkCore;
using CheckerServer.Models;

namespace CheckerServer.Data
{
    public class dbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CheckerDBContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CheckerDBContext>>()))
            {
                if (context.Restaurants.Any())
                {
                    return;
                }

                context.Restaurants.Add(
                    new Restaurant
                    {
                        Name = "My First Shop!",
                        Email = "Eliranshimonov@gmail.com",
                        Password = "123456",
                        ContactName = "John",
                        Phone = "052-446-7089"
                    });
                context.SaveChanges();
            }
        }
    }
}
