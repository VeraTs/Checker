using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.Services
{
    public class DishDataStore : IDataStore<Dish>
    {

        public List<Dish> dishes { get; set; }
        public DishDataStore()
        {
        }
        public Task<bool> AddItemAsync(Dish item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Dish> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Dish>> GetItemsAsync(bool forceRefresh = false)
        {
            List<Dish> listy = new List<Dish>();
            dishes = new List<Dish>();
            try
            {
                string res = App.client.GetStringAsync("/Dishes").Result;
               listy = JsonSerializer.Deserialize<List<Dish>>(res);
               foreach (var item in listy)
               {
                  
                   dishes.Add(item);
               }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listy;
        }

        public Task<bool> UpdateItemAsync(Dish item)
        {
            throw new NotImplementedException();
        }
    }
}
