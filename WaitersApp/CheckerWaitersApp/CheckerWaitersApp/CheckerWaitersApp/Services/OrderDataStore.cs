using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.Services
{
    internal class OrderDataStore : IDataStore<Order>
    {
        public Task<bool> AddItemAsync(Order item)
        {
            var orderModel = new Order();
            orderModel.createdDate = item.createdDate;
            orderModel.orderType = item.orderType;
            orderModel.remainsToPay = item.remainsToPay;
            orderModel.restaurantId = item.restaurantId;
            orderModel.status = item.status;
            orderModel.table = item.table;
            orderModel.totalCost = item.totalCost;
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
           var orders = new List<Order>();
            try
            {
                string res = App.client.GetStringAsync("/Orders").Result;
                listy = JsonSerializer.Deserialize<List<Order>>(res);
                foreach (var item in listy)
                {
                    orders.Add(item);
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
