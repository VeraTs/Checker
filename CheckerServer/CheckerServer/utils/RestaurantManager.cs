using CheckerServer.Data;
using CheckerServer.Hubs;
using CheckerServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.utils
{
    /***
     * Manages all restaurants and keeps references to their kitchens
     * 
     * ***/
    public class RestaurantManager : IHostedService, IDisposable
    {
        private readonly Dictionary<int, KitchenManager> kitchens = new Dictionary<int, KitchenManager>();

        public IServiceProvider Services { get; }

        private IHubContext<KitchenHub> _hubContext;

        public RestaurantManager(IServiceProvider serviceProvider, IHubContext<KitchenHub> kitchenHubContext)
        {
            Services = serviceProvider;
            _hubContext = kitchenHubContext;
            using (var scope = Services.CreateScope())
            {
                using (var context = new CheckerDBContext(
                 scope.ServiceProvider.GetRequiredService<
                     DbContextOptions<CheckerDBContext>>()))
                {
                    List<Restaurant> rests = context.Restaurants.ToList();
                    rests.ForEach(r =>
                    {
                        setUpRest(r.ID);
                    });
                }
            }
        }

        /*** add new restaurant to service ***/
        private void setUpRest(int restId)
        {
            lock (kitchens)
            {
                if (!kitchens.ContainsKey(restId))
                {
                    kitchens.Add(restId, new KitchenManager(Services, _hubContext, restId));
                }
            }
        }

        /*** returns the kitchen for given restaurant ***/
        public KitchenManager getKitchenForRestaurant(int restId)
        {
            KitchenManager kitchenManager = null;
            if (kitchens.ContainsKey(restId))
            {
                kitchenManager = kitchens[restId];
            }

            return kitchens[restId];
        }

        internal void AddGroupMember(int restId)
        {
            if (kitchens.ContainsKey(restId))
            {
                kitchens[restId].AddGroupMember();
            }
        }

        internal void RemoveGroupMember(int restId)
        {
            if (kitchens.ContainsKey(restId))
            {
                kitchens[restId].RemoveGroupMember();
            }
        }

        internal void CloseKitchenForDay(int restId)
        {
            if (kitchens.ContainsKey(restId))
            {
                kitchens[restId].closeForDay();
            }
        }

        internal void RestaurantReadyToWork(int restId)
        {
            setUpRest(restId);
        }

        /*** utilities ***/

        public void Dispose()
        {
            foreach(KitchenManager manager in kitchens.Values)
            {
                manager.Dispose();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach(KitchenManager kitchen in kitchens.Values)
            {
                kitchen.StartAsync(cancellationToken);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach(KitchenManager manager in kitchens.Values)
            {
                manager.StopAsync(cancellationToken);
            }
            return Task.CompletedTask;
        }
    }
}
