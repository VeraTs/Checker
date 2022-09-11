using System.Collections.Generic;
using CheckerWaitersApp.Models;


namespace CheckerWaitersApp.Services
{
    public class ServerRepository
    {

        public static DishDataStore DishesStore { get; private set; }
        public static OrderItemDataStore AllOrdersItemsStore { get; private set; }
        public static OrdersDataStore OrdersStore { get; private set; }
        public List<Dish> Dishes { get; private set; }

        public List<OrderItem> OrderedItems { get; private set; }
        public List<Order> Orders { get; private set; }

        public ServerRepository()
        {
            Dishes = new List<Dish>();
            //Orders = new List<Order>();
            //OrderedItems = new List<OrderItem>();
            // Orders = new List<Order>();
            // OrderedItems = new List<OrderItem>();

            // OrderedItems = AllOrdersItemsStore.items;
            // Orders = OrdersStore.orders;
        }

        public async void LoadData()
        {
            await DishesStore.GetItemsAsync();
            Dishes = DishesStore.dishes;
           // await AllOrdersItemsStore.GetItemsAsync();
           // OrderedItems = AllOrdersItemsStore.items;
            //await OrdersStore.GetItemsAsync();
          //  Orders = OrdersStore.orders;
            
        }
    }
}
