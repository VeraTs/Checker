using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class OrdersDataStore : IDataStore<Order>
    {
        public List<Order> orders { get; set; }

        public Task<bool> AddItemAsync(Order item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetItemsAsync(bool forceRefresh = false)
        {
            List<Order> listy = new List<Order>();
            orders = new List<Order>();
            var dishes = App.Repository.Dishes;
            try
            {
               // var orderedItems = App.Repository.OrderedItems;
                string res = App.client.GetStringAsync("/Orders").Result;
                listy = JsonSerializer.Deserialize<List<Order>>(res);
                foreach (var order in listy)
                {
                    foreach (var orderItem in order.items)
                    {
                        orderItem.dish = dishes.Find(dish => dish.id == orderItem.dishId);
                    }
                   orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listy;
        }
        public Task<bool> UpdateItemAsync(Order item)
        {
            throw new NotImplementedException();
        }
    }
}
