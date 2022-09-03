using CheckerServer.Data;
using CheckerServer.Hubs;
using CheckerServer.utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Models
{
    // manages the kitchens for all the restaurants
    public class RestaurantManager : IHostedService, IDisposable
    {
        private readonly Dictionary<int, Restaurant> r_Restaurants = new Dictionary<int, Restaurant>();
        private readonly Dictionary<int, KitchenManager2> r_KitchenManagers = new Dictionary<int, KitchenManager2>();
        private CancellationToken? cancellationToken = null;

        public IServiceProvider Services { get; }
        public IHubContext<KitchenHub> KitchenHubContext { get; }
        public IHubContext<OrdersHub> OrdersHubContext { get; }

        public RestaurantManager(IServiceProvider serviceProvider, IHubContext<KitchenHub> kitchenHubContext, IHubContext<OrdersHub> ordersHubContext)
        {
            Services = serviceProvider;
            KitchenHubContext = kitchenHubContext;
            OrdersHubContext = ordersHubContext;
            using (var scope = Services.CreateScope())
            {
                using (var context = new CheckerDBContext(
                 scope.ServiceProvider.GetRequiredService<
                     DbContextOptions<CheckerDBContext>>()))
                {
                    List<Restaurant> rests = context.Restaurants.ToList();
                    rests.ForEach(r =>
                    {
                        r_Restaurants.Add(r.ID, r);
                        r_KitchenManagers.Add(r.ID, new KitchenManager2(serviceProvider, kitchenHubContext, r.ID));
                    });
                }
            }
        }

        public async Task<Boolean> AddNewRestaurantAsync(Restaurant restaurant)
        {
            Boolean result = false;
            if (!r_Restaurants.ContainsKey(restaurant.ID))
            {
                r_Restaurants.Add(restaurant.ID, restaurant);
                r_KitchenManagers.Add(restaurant.ID, new KitchenManager2(Services, KitchenHubContext, restaurant.ID));
                if(cancellationToken != null)
                    await r_KitchenManagers[restaurant.ID].StartAsync(cancellationToken.Value);
                result = true;
            }

            return result;
        }

        

        public void Dispose()
        {
            foreach (KitchenManager2 kitchen in r_KitchenManagers.Values)
            {
                kitchen.Dispose();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            foreach(KitchenManager2 kitchen in r_KitchenManagers.Values)
            {
                kitchen.StartAsync(cancellationToken);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (KitchenManager2 kitchen in r_KitchenManagers.Values)
            {
                kitchen.StopAsync(cancellationToken);
            }
            return Task.CompletedTask;
        }
    }
}
