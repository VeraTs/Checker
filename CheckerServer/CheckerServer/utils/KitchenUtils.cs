using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.utils
{
    public class KitchenUtils
    {
        // restaurantID -> major list of queues that depict orders
        private readonly Dictionary<int, List<OrderPyramid>> r_RestaurantOrderQueuesList= new Dictionary<int, List<OrderPyramid>>();

        // lineID -> Line with ORDERED LISTS!~!!! so this needs to be kept updated!
        private Dictionary<int, LineDTO> r_Lines = new Dictionary<int, LineDTO>();

        private CheckerDBContext _Context;

        // the BIG INIT!
        public KitchenUtils(CheckerDBContext context)
        {
            _Context = context;
            List<Restaurant> rests = context.Restaurants.ToList();
            List<Line> lines = context.Lines.Include("ServingArea").ToList();
            foreach (Restaurant restaurant in rests)
            {
                r_RestaurantOrderQueuesList.Add(restaurant.ID, new List<OrderPyramid>());
            }

            foreach(Line line in lines)
            {
                r_Lines.Add(line.ID, new LineDTO(){line = line});
            }

            LoadAllOrders();
        }

        internal void UpdateContext(CheckerDBContext context)
        {
            _Context = context;
        }

        // assumes rest is already in context now
        // for when an entirely new restaurant is added
        public void AddRestaurant(Restaurant rest)
        {
            r_RestaurantOrderQueuesList.Add(rest.ID, new List<OrderPyramid>());
            List<Line> lines = _Context.Lines.Where(l => l.ServingArea.RestaurantId == rest.ID).Include("ServingArea").ToList();

            foreach(Line line in lines)
            {
                r_Lines.Add(line.ID, new LineDTO() { line = line });
            }
        }

        // load all active orders in system
        public void LoadAllOrders()
        {
            List<Order> orders = _Context.Orders
                .Include(o => o.Items)
                .ThenInclude(item => item.Dish)
                .Where(o => o.Status != eOrderStatus.Done).ToList();
            
            orders.ForEach(o =>
            {
                r_RestaurantOrderQueuesList[o.RestaurantId].Add(getQueuesForOrder(o));
                o.Status = eOrderStatus.InProgress;
            });

            _Context.SaveChanges();
        }

        private OrderPyramid getQueuesForOrder(Order order)
        {
            OrderPyramid orderQueue = new OrderPyramid(order.ID);
            orderQueue.enterItems(order.Items.Where(i => i.LineStatus == eLineItemStatus.Locked && i.Status!= eItemStatus.AtLine).ToList(), order.OrderType);
            return orderQueue;
        }

        // SHIT thing
        // receives an order and sorts it into appropriate queues for related restaurant
        public static void AddOrder(Order order) {


            // if no such restaurant has orders here yet, fix that


            OrderPyramid orderQueue = new OrderPyramid(order.ID);
            if (order.OrderType.Equals(eOrderType.AllTogether)) { 
                





                // if all together, what takes most time must be made first and then the second longest make time, etc.
                SortedList<float, OrderItem> items = new SortedList<float, OrderItem> ();
                foreach (OrderItem item in order.Items)
                {
                    // for each item add to sorted lost according to estimated make time
                    items.Add(item.Dish.EstMakeTime, item); 
                }

                items.Reverse();
            }
            else if (order.OrderType.Equals(eOrderType.Staggered)) { 
            }
            else
            {
                foreach (OrderItem item in order.Items)
                {

                }
            }
        }

        // ALSO SHIT thing
        // returns a list of all orderItems that are in this Line currently or in future
        public static void getAllLineOrderItems(Line line)
        {
            
        }

        internal async Task<Dictionary<int, List<LineDTO>>> GetUpdatedLines(List<int> activeRests)
        {
            // restId to Line list
            Dictionary<int, List<LineDTO>> updatedLines = new Dictionary<int, List<LineDTO>>();
            List<OrderItem>? availableItems = null;
            List<OrderItem>? lockedItems = null;
            List<int> availableItemIDS = new List<int>();
            foreach (int restId in r_RestaurantOrderQueuesList.Keys)
            {
                if(activeRests != null && activeRests.Contains(restId))
                {
                    foreach (OrderPyramid order in r_RestaurantOrderQueuesList[restId])
                    {
                        var items = order.GetAvailableItems();
                        if(items!= null)
                        {
                            foreach (OrderItem item in items)
                            {
                                availableItemIDS.Add(item.ID);
                            }
                        }
                        
                        //availableItems.AddRange(order.GetAvailableItems()); // returns all items that have available = true (by popping!)
                    }
                }
            }

            availableItems = _Context.OrderItems.Where(x => availableItemIDS.Contains(x.ID)).Include("Dish").ToList();
            

            if(availableItems != null)
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
                    if(hasItem)
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
            

            lockedItems = _Context.OrderItems.Where(oi => oi.LineStatus == eLineItemStatus.Locked && oi.Status != eItemStatus.AtLine).Include("Dish").ToList();
            if(lockedItems != null)
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
                    if(!hasItem)
                        r_Lines[item.Dish.LineId].LockedItems.Add(item);
                    // issue detected - this is not a tracked item any more, so they all need to be retracked.
                    //item.Status = eItemStatus.AtLine;
                }
            }

            success += await _Context.SaveChangesAsync();
            if(success > 0)
            {
                foreach(int thing in r_Lines.Keys)
                {
                    var temp = r_Lines[thing].line.ServingArea.RestaurantId;
                    if (!updatedLines.ContainsKey(temp))
                    {
                        updatedLines.Add(r_Lines[thing].line.ServingArea.RestaurantId, new List<LineDTO>());
                    }

                    List<OrderItem> toDoItems = new List<OrderItem>();
                    r_Lines[thing].ToDoItems.ForEach(item => toDoItems.Add(new OrderItem() { ID = item.ID, Changes = item.Changes, DishId = item.DishId, LineStatus = item.LineStatus, OrderId = item.OrderId, ServingAreaZone = item.ServingAreaZone, Status = item.Status }));

                    List<OrderItem> doingItems = new List<OrderItem>();
                    r_Lines[thing].DoingItems.ForEach(item => doingItems.Add(new OrderItem() { ID = item.ID, Changes = item.Changes, DishId = item.DishId, LineStatus = item.LineStatus, OrderId = item.OrderId, ServingAreaZone = item.ServingAreaZone, Status = item.Status }));

                    //List<OrderItem> doneItems = new List<OrderItem>();
                    //r_Lines[thing].DoingItems.ForEach(item => doneItems.Add(new OrderItem() { ID = item.ID, Changes = item.Changes, DishId = item.DishId, LineStatus = item.LineStatus, OrderId = item.OrderId, ServingAreaZone = item.ServingAreaZone, Status = item.Status }));

                    updatedLines[r_Lines[thing].line.ServingArea.RestaurantId].Add(new LineDTO() 
                    {
                        lineId = thing, DoingItems = doingItems, LockedItems = r_Lines[thing].LockedItems, ToDoItems = toDoItems
                    }) ;  // this is always the first time the line is added to the list, since lines are uniquely indexed
                }
            }

            return updatedLines;
        }

        internal void RemoveOrder(Order order)
        {
            OrderPyramid? op = r_RestaurantOrderQueuesList[order.RestaurantId].Find(p => p.Id == order.ID);
            if (op != null)
            {
                r_RestaurantOrderQueuesList[order.RestaurantId].Remove(op);
            }
        }

        // loads new orders from dbContext
        internal int LoadNewOrders()
        {
            List<Order> orders = _Context.Orders
                .Include(o => o.Items)
                .ThenInclude(item => item.Dish)
                .Where(o => o.Status != eOrderStatus.Done).ToList();

            orders.ForEach(o =>
            {
                // only add orders that have items that need to go to kitchen
                if(o.Items.Any(item => item.Status == eItemStatus.Ordered))
                {
                    r_RestaurantOrderQueuesList[o.RestaurantId].Add(getQueuesForOrder(o));
                    o.Status = eOrderStatus.InProgress;
                }
            });

            return _Context.SaveChanges();
        }

        // this removes closed orders from the collection
        internal void ClearOrders()
        {
            List<Order> closedOrders = _Context.Orders.Where(o => o.Status == eOrderStatus.Done).ToList();
            foreach (int id in r_RestaurantOrderQueuesList.Keys)
            {
                List<OrderPyramid> toClose = new List<OrderPyramid>();
                r_RestaurantOrderQueuesList[id].ForEach(o =>
                {
                    if (closedOrders.Any(co => co.ID == o.Id))
                    {
                        toClose.Add(o);
                    }
                });

                toClose.ForEach(o => r_RestaurantOrderQueuesList[id].Remove(o));
            }
        }
    }



    public class OrderPyramid
    {
        public int Id { get; private set; }
        public Stack<TimedOrderItem>? starters = null;
        public Stack<TimedOrderItem>? mains = null;
        public Stack<TimedOrderItem>? desserts = null;

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
                } else if (limit <= 0)
                {
                    this.available = true;
                    /*this.timer = new System.Timers.Timer(8000) { AutoReset = false };
                    timer.Elapsed += (o, e) => {
                        available = true;
                        Console.WriteLine("---------------------------Hoho, timer! CAN SENT TO KITCHEN!-------------------------");
                        Console.WriteLine("---------------------------Hoho, timer! Item:" + item.Dish.Name + "!-------------------------");

                        timer.Stop();
                    };*/
                } else
                {
                    this.timer = new System.Timers.Timer(limit) { AutoReset = false };
                    timer.Elapsed += (o, e) => { 
                        available = true; 
                        Console.WriteLine("---------------------------Hoho, timer! CAN SENT TO KITCHEN!-------------------------"); 
                        Console.WriteLine("---------------------------Hoho, timer! Item:" + item.Dish.Name +"!-------------------------"); 

                        timer.Stop(); 
                    };

                    if (_timerUp != null)
                        timerUp = _timerUp.Value;
                }
            }

            public void StartTimer()
            {
                if(timer!= null && timerUp)
                    timer.Start();
            }
        }

        public void StartTimers()
        {
            if(starters != null && starters.Count > 0)
            {
                foreach (TimedOrderItem item in starters)
                {
                    item.StartTimer();
                }
            } else if (mains != null && mains.Count > 0)
            {
                foreach (TimedOrderItem item in mains)
                {
                    item.StartTimer();
                }
            } else if (desserts != null && desserts.Count > 0)
            {
                foreach (TimedOrderItem item in desserts)
                {
                    item.StartTimer();
                }
            }
        }

        public void enterItems(List<OrderItem> items, eOrderType orderType)
        {
            // enters things according to order type

            // regardless of orderType, if there are drinks, they are entered into starters immediately
            if(items.Any(item => item.Dish.Type == eDishType.Drink))
            {
                if(starters == null)
                    starters = new Stack<TimedOrderItem>();
                
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
                    if(starters == null)
                    {
                        starters = new Stack<TimedOrderItem>();
                    }

                    // while SortedDictionary is quicker for entering unsorted data, SortedList uses less memory and also
                    // has a very comfortable retrieval of values, using list.values[3] for example, which will be used in this method
                    items.ForEach(item => 
                    {
                        if(item.Dish.Type != eDishType.Drink)
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
                    if(starters == null)
                    {
                        starters = new Stack<TimedOrderItem>();
                    }


                    items.Where(item => item.Dish.Type != eDishType.Drink)
                        .ToList()
                        .ForEach(item =>
                        {
                            sortedItems.Add(item.Dish.EstMakeTime, item);
                        });

                    for(int i= sortedItems.Count - 1; i>= 0; i--)
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
                        if(starters == null)
                            starters = new Stack<TimedOrderItem>();
                        addFromSortedList(startersList, starters);
                    }

                    if(mainsList.Count > 0 && mains == null)
                    {
                        if (mains == null)
                            mains = new Stack<TimedOrderItem>();
                        addFromSortedList(mainsList, mains);
                    }

                    if(dessertsList.Count > 0 && desserts == null)
                    {
                        if (desserts == null)
                            desserts = new Stack<TimedOrderItem>();
                        addFromSortedList(dessertsList, desserts);
                    }

                    break;
            }

            // will start timers for initial list
            // if there is starters list - for that
            // if starters is empty or null - will start mains
            // if mains is also empty or null - will start desserts

            StartTimers();
        }

        // something here isn't right :SWEAT  maybe I should use a stack and not a queue for this to be easier
        // since I want the last thing entered to be the first thing out
        private void addFromSortedList(SortedList<double, OrderItem> sortedList, Stack<TimedOrderItem> timedQueue)
        {
            if(sortedList.Count > 0)
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
            if(starters != null && starters.Count > 0)      { availableItems = getFromStack(starters);}
            else if(mains != null && mains.Count > 0)    { availableItems = getFromStack(mains); }
            else if (desserts != null &&  desserts.Count > 0 )  { availableItems = getFromStack(desserts); }

            StartTimers();  // start timers for newly allowed items

            return availableItems;
        }

        private List<OrderItem> getFromStack(Stack<TimedOrderItem> stack)
        {
            List<OrderItem> availableItems = new List<OrderItem>();
            if (stack != null  && stack.Count > 0)
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
    }


    public class DuplicateKeyComparer<Tkey> : IComparer<Tkey> where Tkey : IComparable
    {
        public int Compare(Tkey? x, Tkey? y)
        {
            int result = 0;
            if(x!= null && y!= null)
                result = x.CompareTo(y);

            if (result == 0)
                return 1;
            else
                return result;
        }
    }
}
