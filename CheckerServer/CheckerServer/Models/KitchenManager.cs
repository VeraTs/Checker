using CheckerServer.Data;
using CheckerServer.Hubs;
using CheckerServer.utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Models
{
    // manages the kitchens for all the restaurants
    public class KitchenManager : IHostedService, IDisposable
    {
        private CheckerDBContext _context = null;
        private readonly KitchenUtils r_KitchenUtils = null;
        private readonly Dictionary<int, List<LineDTO>> r_KitchenLines = new Dictionary<int, List<LineDTO>>();
        private readonly Dictionary<int, int> r_RestsActiveKitchens = new Dictionary<int, int>();
        private readonly object r_RestUsesLock  = new object();
        private readonly IHubContext<KitchenHub> _hubContext;
        private Timer? _timer = null;
        public IServiceProvider Services { get; }
        private static int counter = 0;
        private static bool shouldRun = true;

        public KitchenManager(IServiceProvider serviceProvider, IHubContext<KitchenHub> kitchenHubContext)
        {
            // used to determine how many times this is initialized - answer was 1
            /*Interlocked.Increment(ref counter);
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$" + counter.ToString() + "$$$$$$$$$$$$$$$$$$$$$$$$$$$");*/
            Services = serviceProvider;
            _hubContext = kitchenHubContext;
            using (var scope = Services.CreateScope())
            {
                using (var context = new CheckerDBContext(
                 scope.ServiceProvider.GetRequiredService<
                     DbContextOptions<CheckerDBContext>>())){
                    List<Restaurant> rests = context.Restaurants.ToList();
                    rests.ForEach(r =>
                    {
                        r_KitchenLines.Add(r.ID, new List<LineDTO>());
                        r_RestsActiveKitchens.Add(r.ID, 0);
                    });
                    r_KitchenUtils = new KitchenUtils(context);
                }
            }
        }

        internal void UpdateContext(CheckerDBContext context)
        {
            lock (this)
            {
                _context = context;
                r_KitchenUtils.UpdateContext(context);
            }
        }

        // manage the kitchen utils
        public async Task ManageKitchenAsync()
        {
            // we're currently ignoring multiple restaurants, and assuming singal restaurant
            // for multiple restaurants, we will have multiple users, and in the SignalR case,
            // multiple Clients associated with the user, in a Group


            /**
             * How To manage a kitchen?
             *  1) Load everything (done in ctor!)
             *  2) while server operating:
             *      2.1) Check all open orders, and update the OrderITem lists for each Line
             *              \----- 'GetUpdatedLines' - will return a mapping restId -> {line | line in rest, with updated orderItem lists inside}
             *      2.2) For each line, send the Line and lists to each relevant restaurant
             *              \----- foreach(int key in keys) { await Clients.Group(...).SendAsync(event thing) }
             *      2.3) if there are new orders, sift them into the KitchenUtils
             *              \----- 'LoadNewOrders' with input of active orders from DB (actually, is input needed? KitchenUtils has context access)
             *      2.4) if there are closed orders, remove them from the KitchenUtils
             *              \----- 'ClearDeadOrders' with no input needed
             * 
             */

            using(var scope = Services.CreateScope())
            {
                using (var context = new CheckerDBContext(
                    scope.ServiceProvider.GetRequiredService<
                    DbContextOptions<CheckerDBContext>>()))
                {
                    // step 2.1
                    // restId to Lines
                    List<int> activeRests = new List<int>();
                    lock (r_RestUsesLock)
                    {
                        r_RestsActiveKitchens.Where(r => r.Value > 0).ToList().ForEach(r => activeRests.Add(r.Key));
                    }

                    r_KitchenUtils.UpdateContext(context);
                    Dictionary<int, List<LineDTO>> updatedLines = await r_KitchenUtils.GetUpdatedLines(activeRests);

                    // step 2.2 - delayed
                    foreach (int restId in updatedLines.Keys)
                    {
                        Restaurant rest = await context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
                        if (rest != null)
                        {
                            // not awaited since this pretains to different restaurants
                            _hubContext.Clients.Group(rest.Name).SendAsync("UpdatedLines", updatedLines[rest.ID]);
                        }
                    }

                    // step 2.3
                    int success = r_KitchenUtils.LoadNewOrders();
                    if (success > 0)
                    {
                        // yay, things loaded and hoarded - continue with my life, this will happen again, obvs.
                    }
                    else
                    {
                        // should I report a signalR error? And if so - to whom?
                        // actually there could be a situation where there is nothing to save
                        //await Clients.All.SendAsync("SignalRError", "Error in uploading new orders to the management system");
                    }

                    // step 2.4
                    r_KitchenUtils.ClearOrders();

                    /*if(updatedLines != null)
                        await _hubContext.Clients.All.SendAsync("updatedLines", updatedLines);*/
                }
            }
            

            // can't lock if there us async inside :CRY:
        }

        internal void AddGroupMember(int restId)
        {
            lock (r_RestUsesLock)
            {
                r_RestsActiveKitchens[restId]++;
            }
        }

        internal void RemoveGroupMember(int restId)
        {
            lock (r_RestUsesLock)
            {
                r_RestsActiveKitchens[restId]--;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            if (shouldRun)
            {
                shouldRun = false;
                await ManageKitchenAsync();
                shouldRun = true;
            }
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
