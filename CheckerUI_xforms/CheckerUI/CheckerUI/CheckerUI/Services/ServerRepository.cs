using System.Collections.Generic;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class ServerRepository
    {
        public List<Dish> dishes { get; private set; }
        public List<Line> lines { get; private set; }
        public List<OrderItem> OrderedItems { get; private set; }
        public List<Order> orders { get; private set; }

        public ServerRepository()
        {
            LoadData();
            dishes = App.Store.dishes;
            lines = App.linesStore.lines;
            OrderedItems = App.oItemsStore.items;
            orders = App.ordersStore.orders;
        }

        public async void LoadData()
        {
            await App.Store.GetItemsAsync();
            await App.linesStore.GetItemsAsync();
            await App.oItemsStore.GetItemsAsync();
            await App.ordersStore.GetItemsAsync();
        }
    }
}
