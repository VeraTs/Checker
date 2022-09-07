using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerWaitersApp.Models;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;


namespace CheckerWaitersApp.Services
{
    public class OrdersDataStore : IDataStore<Order>
    {
        public List<Order> orders { get; set; }
        public int Checkout_orderId { get; set; } = 0;
        public float Checkout_price { get; set; } = 0;
        public OrdersDataStore()
        {
        }
        public Task<bool> AddItemAsync(Order item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server

                }
                catch (System.Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Server Close Order Error!", ex.Message, "OK");
                }
            }

            try
            {
                await App.HubConn.InvokeAsync("PayForOrder", Checkout_orderId, Checkout_price);
            }
            catch (System.Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Server Close Order !", ex.Message, "OK");
            }

            return true;
        }

        public  Task<Order> GetItemAsync(string id)
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
                //var orderedItems = App.oItemsStore.items;
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
