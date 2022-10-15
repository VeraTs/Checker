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
        private readonly Dictionary<int, Dictionary<int, LineDTO>> r_KitchenLines = new Dictionary<int, Dictionary<int, LineDTO>>();
        private readonly Dictionary<int, int> r_RestsActiveKitchens = new Dictionary<int, int>();
        private readonly Dictionary<int, List<int>> r_OrderItemsStartedByOrder = new Dictionary<int,List<int>>();
        private readonly Dictionary<int, List<int>> r_OrderItemsFinishedByOrder = new Dictionary<int,List<int>>();

        private readonly object r_RestUsesLock = new object();
        private readonly IHubContext<KitchenHub> _hubContext;
        private Timer? _timer = null;
        public IServiceProvider Services { get; }
        private static int counter = 0;
        private static bool shouldRun = true;

        internal void closeForDay()
        {
            r_isOpen = false;
        }
        internal void openForDay()
        {
            r_isOpen = true;
        }

        private List<int> readyToRunRests = new List<int>();
        private bool r_isOpen = false;
        private Restaurant r_Restaurant;

        public KitchenManager(IServiceProvider serviceProvider, IHubContext<KitchenHub> kitchenHubContext, int restId)
        {
            Services = serviceProvider;
            _hubContext = kitchenHubContext;
            using (var scope = Services.CreateScope())
            {
                using (var context = new CheckerDBContext(
                 scope.ServiceProvider.GetRequiredService<
                     DbContextOptions<CheckerDBContext>>()))
                {
                    r_KitchenUtils = new KitchenUtils(context);

                    Restaurant? rest = context.Restaurants.Find(restId);
                    if(rest != null)
                    {
                        r_Restaurant = rest;
                        setUpRest(context, rest);
                    }
                }
            }
        }

        private void setUpRest(CheckerDBContext context, Restaurant r)
        {
            r_KitchenLines.Add(r.ID, new Dictionary<int, LineDTO>());
            // init the kitchen lines with order items
            initLines(context, r);
            r_RestsActiveKitchens.Add(r.ID, 0);
            List<ServingArea> areas = context.ServingAreas.Where(sa => sa.RestaurantId == r.ID).ToList();
            areas.ForEach(area =>
            {
                OrdersUtils.addServingArea(area);
            });
        }

        private void initLines(CheckerDBContext context, Restaurant rest)
        {
            // Orders
            List<Order>? orders = context.Orders.Where(o => o.RestaurantId == rest.ID && o.Status != eOrderStatus.Done).ToList();
            if (orders != null)
                orders.ForEach(o => {
                    List<OrderItem>? items = context.OrderItems.Include("Dish").Where(oi => oi.OrderId == o.ID).ToList();
                    if (items != null)
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
                                switch (i.LineStatus)
                                {
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
                            }
                            else if (i.Status == eItemStatus.Ordered)
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

            if (r_OrderItemsStartedByOrder.ContainsKey(orderId))
            {
                lock (r_OrderItemsStartedByOrder)
                {
                    if (r_OrderItemsStartedByOrder.ContainsKey(orderId))
                    {
                        r_OrderItemsStartedByOrder.Remove(orderId);
                    }
                }
            }

            if (r_OrderItemsFinishedByOrder.ContainsKey(orderId))
            {
                lock (r_OrderItemsFinishedByOrder)
                {
                    if (r_OrderItemsFinishedByOrder.ContainsKey(orderId))
                    {
                        r_OrderItemsFinishedByOrder.Remove(orderId);
                    }
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
             *      2.5) Check for new restaurants to load
             * 
             */

            using (var scope = Services.CreateScope())
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

                    Dictionary<int, List<int>> startedItems;
                    Dictionary<int, List<int>> finishedItems;
                    
                    lock (r_OrderItemsFinishedByOrder)
                    {
                        finishedItems = new Dictionary<int, List<int>>(r_OrderItemsFinishedByOrder);
                        r_OrderItemsFinishedByOrder.Clear();
                    }

                    lock (r_OrderItemsStartedByOrder)
                    {
                        startedItems = new Dictionary<int, List<int>>(r_OrderItemsStartedByOrder);
                        r_OrderItemsStartedByOrder.Clear();
                    }
                    Dictionary<int, List<LineDTO>> updatedLines = await r_KitchenUtils.GetUpdatedLines(activeRests, startedItems, finishedItems);
                    

                    // step 2.2
                    foreach (int restId in r_RestsActiveKitchens.Keys)
                    {
                        if (r_RestsActiveKitchens[restId] > 0)
                        {
                            Restaurant rest = await context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
                            if (updatedLines.ContainsKey(restId))
                            {
                                foreach(LineDTO lineDTO in updatedLines[restId])
                                {
                                    updateOutgoingLines(restId, lineDTO);
                                }
                            }

                            // not awaited since this pretains to different restaurants
                            _hubContext.Clients.Group(rest.Name).SendAsync("UpdatedLines", r_KitchenLines[restId].Values);
                        }
                    }

                    // step 2.3
                    r_KitchenUtils.LoadNewOrders();

                    // step 2.4
                    r_KitchenUtils.ClearOrders();

                    // step 2.5
                    lock (readyToRunRests)
                    {
                        if(readyToRunRests.Count > 0)
                        {
                            readyToRunRests.ForEach(async id =>
                            {
                                Restaurant? rest = await context.Restaurants.FindAsync(id);
                                if(rest!= null)
                                {
                                    setUpRest(context, rest);
                                }
                            });
                        }

                        readyToRunRests.Clear();
                    }
                }
            }
        }

        /****
         *  update the output for "update lines" to reflect changes made
         *  
         *  add new locked items
         *  remove from locked items that belong in todo, add to todo
         *  remove from todo items that belong in doing, add to doing
         *  remove from doing items that belong in done, add to done
         */
        private void updateOutgoingLines(int restId, LineDTO lineDTO)
        {
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

                    lineDTO.ToDoItems.ForEach(item => 
                        removeAndAdd(r_KitchenLines[restId][lineDTO.lineId].LockedItems, r_KitchenLines[restId][lineDTO.lineId].ToDoItems, item)
                     );

                    lineDTO.DoingItems.ForEach(item => 
                        removeAndAdd(r_KitchenLines[restId][lineDTO.lineId].ToDoItems, r_KitchenLines[restId][lineDTO.lineId].DoingItems, item)
                     );

                    lineDTO.DoneItems.ForEach(item => 
                        removeAndAdd(r_KitchenLines[restId][lineDTO.lineId].DoingItems, r_KitchenLines[restId][lineDTO.lineId].DoneItems, item)
                     );
                }
            }
        }

        private void removeAndAdd(List<OrderItem> from, List<OrderItem> to, OrderItem item)
        {
            OrderItem? oi = from.Find(i => i.ID == item.ID);
            if (oi != null)
            {
                from.Remove(oi);
            }

            oi = to.Find(i => i.ID == item.ID);
            if (oi == null)
                to.Add(item);
            else
            {
                oi.LineStatus = item.LineStatus;
                oi.Status = item.Status;
            }
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
                        OrderItem savedItem = r_KitchenLines[restId][lineId].ToDoItems.First(i => i.ID == item.ID);
                        r_KitchenLines[restId][lineId].ToDoItems.Remove(savedItem);
                        r_KitchenLines[restId][lineId].DoingItems.Add(item);
                    }

                    if (!r_OrderItemsStartedByOrder.ContainsKey(order.ID))
                    {
                        lock (r_OrderItemsStartedByOrder)
                        {
                            if (!r_OrderItemsStartedByOrder.ContainsKey(order.ID))
                            {
                                r_OrderItemsStartedByOrder.Add(order.ID, new List<int>());
                            }
                        }
                    }

                    lock (r_OrderItemsStartedByOrder){
                        r_OrderItemsStartedByOrder[order.ID].Add(item.ID);
                    }
                    
                    break;
                case eLineItemStatus.Done:
                    lock (r_KitchenLines[restId][lineId])
                    {
                        OrderItem savedItem = r_KitchenLines[restId][lineId].DoingItems.First(i => i.ID == item.ID);
                        r_KitchenLines[restId][lineId].DoingItems.Remove(savedItem);
                    }

                    if (!r_OrderItemsFinishedByOrder.ContainsKey(order.ID))
                    {
                        lock (r_OrderItemsFinishedByOrder)
                        {
                            if (!r_OrderItemsFinishedByOrder.ContainsKey(order.ID))
                            {
                                r_OrderItemsFinishedByOrder.Add(order.ID, new List<int>());
                            }
                        }
                    }

                    lock (r_OrderItemsFinishedByOrder)
                    {
                        r_OrderItemsFinishedByOrder[order.ID].Add(item.ID);
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

        internal void RestaurantReadyToWork(int restId)
        {
            lock (readyToRunRests)
            {
                readyToRunRests.Add(restId);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
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
