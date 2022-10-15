using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CheckerServer.utils
{
    public class KitchenUtils
    {
        //  major list of queues that depict orders
        private readonly List<OrderPyramid> r_RestaurantOrderQueuesList = new List<OrderPyramid>();

        // lineID -> Line with ORDERED LISTS!~!!! so this needs to be kept updated!
        private Dictionary<int, LineDTO> r_Lines = new Dictionary<int, LineDTO>();

        private CheckerDBContext _Context;
        private readonly int r_RestId;

        // the BIG INIT!
        public KitchenUtils(CheckerDBContext context, int restId)
        {
            _Context = context;
            r_RestId = restId;
            List<Line> lines = context.Lines.Include("ServingArea").Where(l => l.RestaurantId == restId).ToList();

            foreach (Line line in lines)
            {
                r_Lines.Add(line.ID, new LineDTO() { line = line });
            }

            LoadAllOrders();
        }

        internal void UpdateContext(CheckerDBContext context)
        {
            _Context = context;
        }

        // load all active orders in system
        public void LoadAllOrders()
        {
            List<Order> orders = _Context.Orders
                .Include(o => o.Items)
                .ThenInclude(item => item.Dish)
                .Where(o => o.Status != eOrderStatus.Done && o.RestaurantId == r_RestId).ToList();

            orders.ForEach(o =>
            {
                r_RestaurantOrderQueuesList.Add(getQueuesForOrder(o));
                o.Status = eOrderStatus.InProgress;
            });
        }

        private OrderPyramid getQueuesForOrder(Order order)
        {
            OrderPyramid orderQueue = new OrderPyramid(order.ID);
            orderQueue.enterItems(order.Items.Where(i => i.LineStatus == eLineItemStatus.Locked && i.Status != eItemStatus.AtLine).ToList(), order.OrderType);
            order.Status = eOrderStatus.InProgress;
            _Context.SaveChanges();
            return orderQueue;
        }

        internal async Task<List<LineDTO>> GetUpdatedLines(Dictionary<int, List<int>> startedOrederItemsByOrder, Dictionary<int, List<int>> finishedOrederItemsByOrder)
        {
            List<LineDTO> updatedLines = new List<LineDTO>();
            List<OrderItem>? availableItems = null;
            List<OrderItem>? lockedItems = null;
            List<int> availableItemIDS = new List<int>();

            foreach (OrderPyramid order in r_RestaurantOrderQueuesList)
            {
                if (startedOrederItemsByOrder.ContainsKey(order.Id))
                {
                    List<int> started;
                    lock (startedOrederItemsByOrder[order.Id])
                    {
                        started = new List<int>(startedOrederItemsByOrder[order.Id]);
                        startedOrederItemsByOrder[order.Id].Clear();
                    }

                    order.updateStartedItems(started);
                }

                if (finishedOrederItemsByOrder.ContainsKey(order.Id))
                {
                    List<int> finished;
                    lock (finishedOrederItemsByOrder[order.Id])
                    {
                        finished = new List<int>(finishedOrederItemsByOrder[order.Id]);
                        finishedOrederItemsByOrder[order.Id].Clear();
                    }

                    order.updateFinishedItems(finished);
                }

                var items = order.GetAvailableItems();
                if (items != null)
                {
                    foreach (OrderItem item in items)
                    {
                        availableItemIDS.Add(item.ID);
                    }
                }
            }

            availableItems = _Context.OrderItems.Where(x => availableItemIDS.Contains(x.ID)).Include("Dish").ToList();


            if (availableItems != null)
            {
                // now go through the things and sort into lines
                foreach (OrderItem item in availableItems)
                {
                    // check for line entry registered here
                    if (!r_Lines.ContainsKey(item.Dish.LineId))
                    {
                        // create the entry
                        Line line = _Context.Lines.FirstOrDefault(line => line.ID == item.Dish.LineId);
                        if (line == null)
                        {
                            Console.WriteLine("ERROR! No such line omg");
                        }
                        else
                        {
                            if (!r_Lines.ContainsKey(item.Dish.LineId))
                                r_Lines.Add(item.Dish.LineId, new LineDTO() { line = line });
                        }
                    }

                    // add to line list and update item status
                    // assumes that these items are tracking from DB
                    r_Lines[item.Dish.LineId].ToDoItems.Add(item);
                    bool hasItem = r_Lines[item.Dish.LineId].LockedItems.Any(i => i.ID == item.ID);
                    if (hasItem)
                    {
                        OrderItem? oi = r_Lines[item.Dish.LineId].LockedItems.FirstOrDefault(i => i.ID == item.ID);
                        r_Lines[item.Dish.LineId].LockedItems.Remove(oi);
                    }

                    // issue detected - this is not a tracked item any more, so they all need to be retracked.
                    item.LineStatus = eLineItemStatus.ToDo;
                    item.Status = eItemStatus.AtLine;
                }
            }

            int success = await _Context.SaveChangesAsync();

            List<Order> orders = await _Context.Orders.Where(o => o.RestaurantId == r_RestId).ToListAsync();
            List<int> orderIds = new List<int>();
            orders.ForEach(o => orderIds.Add(o.ID));
           
            lockedItems = _Context.OrderItems.Where(oi =>  orderIds.Contains(oi.OrderId) && oi.LineStatus == eLineItemStatus.Locked && oi.Status != eItemStatus.AtLine).Include("Dish").ToList();
            if (lockedItems != null)
            {
                foreach (OrderItem item in lockedItems)
                {
                    // check for line entry registered here
                    if (!r_Lines.ContainsKey(item.Dish.LineId))
                    {
                        // create the entry
                        Line line = _Context.Lines.FirstOrDefault(line => line.ID == item.Dish.LineId);
                        if (line == null)
                        {
                            Console.WriteLine("ERROR! No such line omg");
                        }
                        else
                        {
                            if (!r_Lines.ContainsKey(item.Dish.LineId))
                                r_Lines.Add(item.Dish.LineId, new LineDTO() { line = line });
                        }
                    }

                    // add to line list and update item status
                    // assumes that these items are tracking from DB
                    bool hasItem = r_Lines[item.Dish.LineId].LockedItems.Any(i => i.ID == item.ID);
                    if (!hasItem)
                        r_Lines[item.Dish.LineId].LockedItems.Add(item);
                    // issue detected - this is not a tracked item any more, so they all need to be retracked.
                    //item.Status = eItemStatus.AtLine;
                }
            }

            var doingItems = _Context.OrderItems.Where(oi => orderIds.Contains(oi.OrderId) && oi.LineStatus == eLineItemStatus.Doing && oi.Status == eItemStatus.AtLine).Include("Dish").ToList();
            if(doingItems!= null && doingItems.Count > 0)
            {
                foreach(OrderItem item in doingItems)
                {
                    if(r_Lines[item.Dish.LineId].ToDoItems.Any(i => i.ID == item.ID))
                    {
                        OrderItem oi = r_Lines[item.Dish.LineId].ToDoItems.First(i => i.ID == item.ID);
                        r_Lines[item.Dish.LineId].ToDoItems.Remove(oi);
                        r_Lines[item.Dish.LineId].DoingItems.Add(oi);
                    }
                    
                }
            }

            var finishedItems = _Context.OrderItems.Where(oi => orderIds.Contains(oi.OrderId) && oi.LineStatus == eLineItemStatus.Done).Include("Dish").ToList();
            if(finishedItems!= null && finishedItems.Count > 0)
            {
                foreach(OrderItem item in finishedItems)
                {
                    if(r_Lines[item.Dish.LineId].DoingItems.Any(i => i.ID == item.ID))
                    {
                        OrderItem oi = r_Lines[item.Dish.LineId].DoingItems.First(i => i.ID == item.ID);
                        r_Lines[item.Dish.LineId].DoingItems.Remove(oi);
                        r_Lines[item.Dish.LineId].DoneItems.Add(oi);
                    }
                }
            }

            success += await _Context.SaveChangesAsync();
            if (success > 0)
            {
                foreach (int thing in r_Lines.Keys)
                {
                    List<OrderItem> toDoItems = new List<OrderItem>();
                    r_Lines[thing].ToDoItems.ForEach(item => toDoItems.Add(new OrderItem() { ID = item.ID, Changes = item.Changes, DishId = item.DishId, LineStatus = item.LineStatus, OrderId = item.OrderId, ServingAreaZone = item.ServingAreaZone, Status = item.Status }));

                    List<OrderItem> doingItems2 = new List<OrderItem>();
                    r_Lines[thing].DoingItems.ForEach(item => doingItems2.Add(new OrderItem() { ID = item.ID, Changes = item.Changes, DishId = item.DishId, LineStatus = item.LineStatus, OrderId = item.OrderId, ServingAreaZone = item.ServingAreaZone, Status = item.Status }));

                    //List<OrderItem> doneItems = new List<OrderItem>();
                    //r_Lines[thing].DoingItems.ForEach(item => doneItems.Add(new OrderItem() { ID = item.ID, Changes = item.Changes, DishId = item.DishId, LineStatus = item.LineStatus, OrderId = item.OrderId, ServingAreaZone = item.ServingAreaZone, Status = item.Status }));

                    updatedLines.Add(new LineDTO()
                    {
                        lineId = thing,
                        DoingItems = doingItems2,
                        LockedItems = r_Lines[thing].LockedItems,
                        ToDoItems = toDoItems,
                        DoneItems = r_Lines[thing].DoneItems
                    });  // this is always the first time the line is added to the list, since lines are uniquely indexed
                }
            }

            return updatedLines;
        }

        internal void RemoveOrder(Order order)
        {
            OrderPyramid? op = r_RestaurantOrderQueuesList.Find(p => p.Id == order.ID);
            if (op != null)
            {
                r_RestaurantOrderQueuesList.Remove(op);
            }
        }

        // loads new orders from dbContext
        internal int LoadNewOrders()
        {
            List<Order> orders = _Context.Orders
                .Include(o => o.Items)
                .ThenInclude(item => item.Dish)
                .Where(o => o.Status != eOrderStatus.Done && o.RestaurantId == r_RestId).ToList();

            orders.ForEach(o =>
            {
                // only add orders that have items that need to go to kitchen
                if (o.Items.Any(item => item.Status == eItemStatus.Ordered))
                {
                    r_RestaurantOrderQueuesList.Add(getQueuesForOrder(o));
                    o.Status = eOrderStatus.InProgress;
                }
            });

            return _Context.SaveChanges();
        }

        // this removes closed orders from the collection
        internal void ClearOrders()
        {
            List<Order> closedOrders = _Context.Orders.Where(o => o.RestaurantId == r_RestId && o.Status == eOrderStatus.Done).ToList();
            List<OrderPyramid> toClose = new List<OrderPyramid>();
            r_RestaurantOrderQueuesList.ForEach(o =>
            {
                if (closedOrders.Any(co => co.ID == o.Id))
                {
                    toClose.Add(o);
                }
            });

            toClose.ForEach(o => r_RestaurantOrderQueuesList.Remove(o));
        }
    }



    public class OrderPyramid
    {
        public int Id { get; private set; }
        public TimedItemsStack? starters = null;
        public TimedItemsStack? mains = null;
        public TimedItemsStack? desserts = null;

        public OrderPyramid(int id)
        {
            Id = id;
        }

        // to set up, you get an order item and a time limit.
        // when the time limit is up, the status will change to AVAILABLE
        public class TimedOrderItem
        {
            public OrderItem item;
            private System.Timers.Timer? timer = null;
            public Boolean available = false;
            public Boolean timerUp = false;

            public TimedOrderItem(OrderItem item, double limit, bool? _available = null, bool? _timerUp = null)
            {
                this.item = item; // set item

                // if needed, set timer
                if (_available != null)
                {
                    this.available = _available.Value;
                } else if (limit == 0)
                {
                    this.available = true;
                }
                else if (limit > 0)
                {
                    this.timer = new System.Timers.Timer(limit) { AutoReset = false };
                    timer.Elapsed += (o, e) => {
                        available = true;
                        timer.Stop();
                    };

                    if (_timerUp != null)
                        timerUp = _timerUp.Value;
                }
            }

            public void StartTimer()
            {
                if (timer != null && timerUp)
                    timer.Start();
                else if(timer == null)
                {
                    available = true;
                }
            }
        }

        public class TimedItemsStack : Stack<TimedOrderItem>
        {
            Dictionary<int, TimedOrderItem> items = new Dictionary<int, TimedOrderItem>();
            bool stackDone = false;
            public bool isStackDone() {  return stackDone; }
            public bool hasStartedItem(int itemId)
            {
                bool res = false;
                if (items.ContainsKey(itemId))
                {
                    res = true;
                    items.Remove(itemId);   // remove from items list - there will be no need for this again
                    TimedOrderItem nextItem;
                    if (base.TryPeek(out nextItem))
                    {
                        this.startTimer();   // start timer of next thing in stack
                    }
                }

                return res;
            }

            public bool hasFinishedItem(int itemId)
            {
                bool res = false;
                if (items.ContainsKey(itemId))
                {
                    res = true;
                    items.Remove(itemId);   // remove from items list - there will be no need for this again
                    if(items.Count == 0){
                        // state that the stack is done
                        stackDone = true;
                    }
                }

                return res;
            }

            public new void Push(TimedOrderItem timedItem)
            {
                base.Push(timedItem);
                if (items.ContainsKey(timedItem.item.ID))
                {
                    items[timedItem.item.ID] = timedItem;
                }else
                {
                    items.Add(timedItem.item.ID, timedItem);
                }
                
                if (stackDone)
                {
                    stackDone=false;
                }
            }

            internal void startItems()
            {
                TimedOrderItem firstItem;
                if(base.TryPeek(out firstItem))
                {
                    this.startTimer();
                }
            }

            internal void startTimer()
            {
                TimedOrderItem top = base.Pop();
                if (top != null)
                {
                    top.StartTimer();
                    base.Push(top);
                }
            }
        }

        // using peek after try peek for getting actual object and not a copy - TODO : find out why peek returns item with timer = null
        public void StartTimers()
        {
            TimedOrderItem firstItem;
            if (starters != null && starters.Count > 0)
            {
                if(starters.TryPeek(out firstItem))
                {
                    starters.startTimer();
                }
            }
            else if (mains != null && mains.Count > 0)
            {
                if (mains.TryPeek(out firstItem))
                {
                    starters.startTimer();
                }
            }
            else if (desserts != null && desserts.Count > 0)
            {
                if (desserts.TryPeek(out firstItem))
                {
                    starters.startTimer();
                }
            }
        }

        internal void enterItems(List<OrderItem> items, eOrderType orderType)
        {
            // enters things according to order type

            // regardless of orderType, if there are drinks, they are entered into starters immediately
            if (items.Any(item => item.Dish.Type == eDishType.Drink))
            {
                if (starters == null)
                    starters = new TimedItemsStack();

                items.ForEach(item =>
                {
                    if (item.Dish.Type == eDishType.Drink)
                    {
                        starters.Push(new TimedOrderItem(item, 0, true, true));    // drinks are to be made available immediately
                    }
                });
            }

            // now act according to order type
            SortedList<Double, OrderItem> sortedItems = new SortedList<Double, OrderItem>(new DuplicateKeyComparer<double>());
            switch (orderType)
            {
                case eOrderType.AllTogether:
                    // put everythinginto starters
                    if (starters == null)
                    {
                        starters = new TimedItemsStack();
                    }

                    // while SortedDictionary is quicker for entering unsorted data, SortedList uses less memory and also
                    // has a very comfortable retrieval of values, using list.values[3] for example, which will be used in this method
                    items.ForEach(item =>
                    {
                        if (item.Dish.Type != eDishType.Drink)
                        {
                            sortedItems.Add(item.Dish.EstMakeTime, item);
                        }
                    });

                    // now, using the fact that we can enter things from smallest estPrepTime to largest,
                    // we can also detect the intervals we need for the timers
                    addFromSortedList(sortedItems, starters);

                    break;

                case eOrderType.FIFO:
                    // first ready is first out
                    // so we actually sort it according to the smallest prep time to the largest, all in one category
                    // and basically just send everything to the kitchen at once and let them do whatever
                    if (starters == null)
                    {
                        starters = new TimedItemsStack();
                    }


                    items.Where(item => item.Dish.Type != eDishType.Drink)
                        .ToList()
                        .ForEach(item =>
                        {
                            sortedItems.Add(item.Dish.EstMakeTime, item);
                        });

                    for (int i = sortedItems.Count - 1; i >= 0; i--)
                    {
                        starters.Push(new TimedOrderItem(sortedItems.Values[i], 0));
                    }

                    break;

                case eOrderType.Staggered:
                    // now here we need to consider 3 ordered lists
                    SortedList<double, OrderItem> startersList = new SortedList<double, OrderItem>(new DuplicateKeyComparer<double>());
                    SortedList<double, OrderItem> mainsList = new SortedList<double, OrderItem>(new DuplicateKeyComparer<double>());
                    SortedList<double, OrderItem> dessertsList = new SortedList<double, OrderItem>(new DuplicateKeyComparer<double>());

                    items.ForEach(item =>
                    {
                        item.Status = eItemStatus.AtLine;
                        Dish dish = item.Dish;
                        switch (dish.Type)
                        {
                            case eDishType.Starter:
                                startersList.Add(dish.EstMakeTime, item);
                                break;
                            case eDishType.Main:
                                mainsList.Add(dish.EstMakeTime, item);
                                break;
                            case eDishType.Dessert:
                                dessertsList.Add(dish.EstMakeTime, item);
                                break;
                            case eDishType.UnDefined:
                                startersList.Add(dish.EstMakeTime, item);
                                break;
                        }
                    });

                    // now order into the queue each list as the allTogether scheme

                    if (startersList.Count > 0 && starters == null)
                    {
                        if (starters == null)
                            starters = new TimedItemsStack();
                        addFromSortedList(startersList, starters);
                    }

                    if (mainsList.Count > 0 && mains == null)
                    {
                        if (mains == null)
                            mains = new TimedItemsStack();
                        addFromSortedList(mainsList, mains);
                    }

                    if (dessertsList.Count > 0 && desserts == null)
                    {
                        if (desserts == null)
                            desserts = new TimedItemsStack();
                        addFromSortedList(dessertsList, desserts);
                    }

                    break;
            }

            // will start timers for initial list
            // if there is starters list - for that
            // if starters is empty or null - will start mains
            // if mains is also empty or null - will start desserts

            items.ForEach(item => item.Status = eItemStatus.AtLine);

            StartTimers();
        }

        // something here isn't right :SWEAT  maybe I should use a stack and not a queue for this to be easier
        // since I want the last thing entered to be the first thing out
        private void addFromSortedList(SortedList<double, OrderItem> sortedList, TimedItemsStack timedQueue)
        {
            if (sortedList.Count > 0)
            {
                for (int i = 1; i <= sortedList.Count - 1; i++)
                {
                    double interval = Math.Abs(sortedList.Keys[i] - sortedList.Keys[i - 1]);
                    timedQueue.Push(new TimedOrderItem(sortedList.Values[i - 1], interval));
                }

                // for the last thing in the list:
                timedQueue.Push(new TimedOrderItem(sortedList.Last().Value, 0, true, true));
            }
        }

        internal IEnumerable<OrderItem> GetAvailableItems()
        {
            List<OrderItem> availableItems = null;
            if (starters != null && starters.Count > 0) { availableItems = getFromStack(starters); }
            else if (mains != null && mains.Count > 0) { availableItems = getFromStack(mains); }
            else if ( desserts != null && desserts.Count > 0) { availableItems = getFromStack(desserts); }

            if(starters!= null && starters.isStackDone()) 
            {
                mains.startItems();
            } else if (mains != null && mains.isStackDone())
            {
                desserts.startItems();
            }



            return availableItems;
        }

        private List<OrderItem> getFromStack(TimedItemsStack stack)
        {
            List<OrderItem> availableItems = new List<OrderItem>();
            if (stack != null && stack.Count > 0)
            {
                TimedOrderItem temp = null;
                bool hasItem = false;
                do
                {
                    hasItem = stack.TryPeek(out temp);
                    if (hasItem)
                    {
                        temp = stack.Pop();
                        if (temp.available)
                        {
                            availableItems.Add(temp.item);
                        }
                        else
                        {
                            // if temp is not available, and timer for it hasn't been started - give it the option
                            if (!temp.timerUp)
                                temp.timerUp = true;
                            stack.Push(temp);
                            hasItem = false;
                        }
                    }
                } while (hasItem);
            }

            return availableItems;
        }

        internal void updateStartedItems(List<int> startedItems)
        {
            foreach (int itemId in startedItems)
            {
                bool hasItem = false;
                if (starters != null && starters.Count > 0)
                {
                    hasItem = starters.hasStartedItem(itemId);
                }

                if (!hasItem && mains!=null && mains.Count > 0)
                {
                    hasItem = mains.hasStartedItem(itemId);
                }

                if (!hasItem && desserts != null && desserts.Count > 0)
                {
                    hasItem = desserts.hasStartedItem(itemId);
                }
            }
        }

        internal void updateFinishedItems(List<int> finishedItems)
        {
            foreach (int itemId in finishedItems)
            {
                bool hasItem = false;
                if (starters != null && starters.Count > 0)
                {
                    hasItem = starters.hasFinishedItem(itemId);
                }

                if (!hasItem && mains != null && mains.Count > 0)
                {
                    hasItem = mains.hasFinishedItem(itemId);
                }

                if (!hasItem && desserts != null && desserts.Count > 0)
                {
                    hasItem = desserts.hasFinishedItem(itemId);
                }
            }
        }
    }

    
    public class DuplicateKeyComparer<Tkey> : IComparer<Tkey> where Tkey : IComparable
    {
        public int Compare(Tkey? x, Tkey? y)
        {
            int result = 0;
            if (x != null && y != null)
                result = x.CompareTo(y);

            if (result == 0)
                return 1;
            else
                return result;
        }
    }
}