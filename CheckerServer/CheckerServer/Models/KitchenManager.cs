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
        private readonly Dictionary<int, Dictionary<int, LineDTO>> r_KitchenLines = new Dictionary<int, Dictionary<int,LineDTO>>();
        private readonly Dictionary<int, int> r_RestsActiveKitchens = new Dictionary<int, int>();
        private readonly object r_RestUsesLock  = new object();
        private readonly IHubContext<KitchenHub> _hubContext;
        private Timer? _timer = null;
        public IServiceProvider Services { get; }
        private static int counter = 0;
        private static bool shouldRun = true;

        public KitchenManager(IServiceProvider serviceProvider, IHubContext<KitchenHub> kitchenHubContext)
        {
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
                        r_KitchenLines.Add(r.ID, new Dictionary<int, LineDTO>());
                        // init the kitchen lines with order items
                        initLines(context, r);
                        r_RestsActiveKitchens.Add(r.ID, 0);
                    });
                    r_KitchenUtils = new KitchenUtils(context);
                }
            }
        }

        private void initLines(CheckerDBContext context, Restaurant rest)
        {
            // Orders
            List<Order>? orders = context.Orders.Where(o => o.RestaurantId == rest.ID && o.Status != eOrderStatus.Done).ToList();
            if(orders != null)
                orders.ForEach(o => {
                    List<OrderItem>? items = context.OrderItems.Include("Dish").Where(oi => oi.OrderId == o.ID).ToList();
                    if(items != null)
                    {
                        items.ForEach(i =>
                        {
                            OrderItem iDTO = new OrderItem() { Changes = i.Changes, DishId = i.DishId, ID = i.ID, LineStatus = i.LineStatus, OrderId = i.OrderId, ServingAreaZone = i.ServingAreaZone, Status = i.Status };
                            int lineId = i.Dish.LineId;
                            if (!r_KitchenLines[rest.ID].ContainsKey(lineId))
                            {
                                r_KitchenLines[rest.ID].Add(lineId, new LineDTO() { lineId = lineId });
                            }

                            if (i.Status == eItemStatus.AtLine || i.Status == eItemStatus.Ordered)
                            {
                                switch(i.LineStatus){
                                    case eLineItemStatus.ToDo:
                                        if (r_KitchenLines[rest.ID][lineId].ToDoItems == null)
                                            r_KitchenLines[rest.ID][lineId].ToDoItems = new List<OrderItem>();
                                        r_KitchenLines[rest.ID][lineId].ToDoItems.Add(iDTO);
                                        break;
                                    case eLineItemStatus.Locked:
                                        if (r_KitchenLines[rest.ID][lineId].LockedItems == null)
                                            r_KitchenLines[rest.ID][lineId].LockedItems = new List<OrderItem>();
                                        r_KitchenLines[rest.ID][lineId].LockedItems.Add(iDTO);
                                        break;
                                    case eLineItemStatus.Doing:
                                        if (r_KitchenLines[rest.ID][lineId].DoingItems == null)
                                            r_KitchenLines[rest.ID][lineId].DoingItems = new List<OrderItem>();
                                        r_KitchenLines[rest.ID][lineId].DoingItems.Add(iDTO);
                                        break;
                                }
                            } else if (i.Status == eItemStatus.Ordered)
                            {
                                if (r_KitchenLines[rest.ID][lineId].LockedItems == null)
                                    r_KitchenLines[rest.ID][lineId].LockedItems = new List<OrderItem>();

                                r_KitchenLines[rest.ID][lineId].LockedItems.Add(iDTO);
                            }
                        });
                    }
                });
        }

        internal async Task CloseOrder(int orderId)
        {
            using (var scope = Services.CreateScope())
            {
                using (var context = new CheckerDBContext(
                    scope.ServiceProvider.GetRequiredService<
                    DbContextOptions<CheckerDBContext>>()))
                {
                    Order? order = await context.Orders.FirstAsync(o => o.ID == orderId);
                    r_KitchenUtils.UpdateContext(context);
                    r_KitchenUtils.RemoveOrder(order);
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

                    // step 2.2
                    foreach (int restId in r_RestsActiveKitchens.Keys)
                    {
                        if (r_RestsActiveKitchens[restId] > 0)
                        {
                            Restaurant rest = await context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
                            if (updatedLines.ContainsKey(restId))
                            {
                                updatedLines[restId].ForEach(lineDTO => {
                                    if (!r_KitchenLines[restId].ContainsKey(lineDTO.lineId))
                                    {
                                        r_KitchenLines[restId][lineDTO.lineId] = lineDTO;
                                    }
                                    else
                                    {
                                        lock (r_KitchenLines[restId][lineDTO.lineId])
                                        {
                                            // the only actual thing that is updated in KitchenUtils.GetUpdatedLines
                                           

                                            lineDTO.LockedItems.ForEach(item => {
                                                OrderItem? oi = r_KitchenLines[restId][lineDTO.lineId].LockedItems.Find(i => i.ID == item.ID);
                                                if (oi == null)
                                                {
                                                    r_KitchenLines[restId][lineDTO.lineId].LockedItems.Add(item);
                                                }
                                            });

                                            lineDTO.ToDoItems.ForEach(item => {
                                                OrderItem? oi = r_KitchenLines[restId][lineDTO.lineId].LockedItems.Find(i => i.ID == item.ID);
                                                if(oi != null)
                                                {
                                                    r_KitchenLines[restId][lineDTO.lineId].LockedItems.Remove(oi);
                                                }

                                                oi = r_KitchenLines[restId][lineDTO.lineId].ToDoItems.Find(i => i.ID == item.ID);
                                                if(oi == null)
                                                    r_KitchenLines[restId][lineDTO.lineId].ToDoItems.Add(item);

                                            });


                                            
                                        }
                                    }
                                });
                            }

                            // not awaited since this pretains to different restaurants
                            _hubContext.Clients.Group(rest.Name).SendAsync("UpdatedLines", r_KitchenLines[restId].Values);
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

        

        internal void ItemWasMoved(Order order, OrderItem item)
        {
            int lineId = item.Dish.LineId;
            int restId = order.RestaurantId;
            eLineItemStatus lineStatus = item.LineStatus;
            switch (lineStatus)
            {
                case eLineItemStatus.Doing:
                    lock (r_KitchenLines[restId][lineId])
                    {
                        r_KitchenLines[restId][lineId].ToDoItems.Remove(item);
                        r_KitchenLines[restId][lineId].DoingItems.Add(item);
                    }
                    
                    break;
                case eLineItemStatus.Done:
                    lock (r_KitchenLines[restId][lineId])
                    {
                        r_KitchenLines[restId][lineId].DoingItems.Remove(item);
                    }
                    break;
            }
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
