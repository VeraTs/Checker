using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class OrderItemDataStore : IDataStore<OrderItem>
    {
        public List<OrderItem> items { get; set; } = new List<OrderItem>();
        public List<Dish> dishes { get; set; } = new List<Dish>(); 
        public OrderItemDataStore()
        {
            dishes = App.Store.dishes;
        }
        public Task<bool> AddItemAsync(OrderItem item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderItem>> GetItemsAsync(bool forceRefresh = false)
        {
            List<OrderItem> listy = new List<OrderItem>();
            items = new List<OrderItem>();
            try
            {
                string res = App.client.GetStringAsync("/OrderItems").Result;
                listy = JsonSerializer.Deserialize<List<OrderItem>>(res);
                foreach (var item in listy)
                {
                    item.dish = App.Store.dishes.Find(dish => dish.id == item.dishId);
                    items.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listy;
        }

        public Task<bool> UpdateItemAsync(OrderItem item)
        {
            throw new NotImplementedException();
        }
    }
}
