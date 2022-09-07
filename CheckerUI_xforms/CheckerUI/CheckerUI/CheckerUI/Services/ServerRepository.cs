using System.Collections.Generic;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class ServerRepository
    {
        public static LinesDataStore LinesStore { get; private set; }
        public static DishDataStore DishesStore { get; private set; }
        public static OrderItemDataStore AllOrdersItemsStore { get; private set; }
        public static ServingAreasDataStore ServingAreasStore { get; private set; }
        public static OrdersDataStore OrdersStore { get; private set; }
        public List<Dish> Dishes { get; private set; }
        public List<ServingArea> ServingAreas { get; private set; }
        public Dictionary<int, Dish> DishesDictionary { get; private set; } = new Dictionary<int, Dish>();
        public List<Line> lines { get; private set; }
        public List<OrderItem> OrderedItems { get; private set; }
        public List<Order> Orders { get; private set; }

        public ServerRepository()
        {
            DishesStore = new DishDataStore();
            LinesStore = new LinesDataStore();
            AllOrdersItemsStore = new OrderItemDataStore();
            OrdersStore = new OrdersDataStore();
            Dishes = new List<Dish>();
            lines = new List<Line>();
            Orders = new List<Order>();
            OrderedItems = new List<OrderItem>();
            ServingAreasStore = new ServingAreasDataStore();
            ServingAreas = new List<ServingArea>();
        }

        public async void LoadData()
        {
            await DishesStore.GetItemsAsync();
            Dishes = DishesStore.dishes;
            DishesDictionary = DishesStore.DishesDict;
            await LinesStore.GetItemsAsync();
            lines = LinesStore.lines;
            await AllOrdersItemsStore.GetItemsAsync();
            OrderedItems = AllOrdersItemsStore.items;
            await OrdersStore.GetItemsAsync();
            await ServingAreasStore.GetItemsAsync();
            ServingAreas = ServingAreasStore.servingAreas;
            Orders = OrdersStore.orders;
            Dishes = DishesStore.dishes;
            lines = LinesStore.lines;
            OrderedItems = AllOrdersItemsStore.items;
            Orders = OrdersStore.orders;

        }
    }
}
