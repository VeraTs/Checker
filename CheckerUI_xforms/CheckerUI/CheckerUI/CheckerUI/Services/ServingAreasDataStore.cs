using System;
using System.Collections.Generic;
using System.Text;
using CheckerUI.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckerUI.Services
{
    public class ServingAreasDataStore : IDataStore<ServingArea>
    {
        public List<ServingArea> servingAreas { get; set; } = new List<ServingArea>();

        public Task<bool> AddItemAsync(ServingArea item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServingArea> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ServingArea>> GetItemsAsync(bool forceRefresh = false)
        {
            List<ServingArea> listy = new List<ServingArea>();
           
           
            try
            {
                // var orderedItems = App.Repository.OrderedItems;
                string res = App.client.GetStringAsync("/ServingAreas").Result;
                listy = JsonSerializer.Deserialize<List<ServingArea>>(res);
                foreach (var servingArea in listy)
                {

                    servingAreas.Add(servingArea);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listy;
        }
        public Task<bool> UpdateItemAsync(ServingArea item)
        {
            throw new NotImplementedException();
        }
    }
}
