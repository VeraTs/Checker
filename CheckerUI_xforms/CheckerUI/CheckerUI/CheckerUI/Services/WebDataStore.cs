using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class WebDataStore : IDataStore<ToDo>
    {
        private readonly ToDoHttpClient client;

        public WebDataStore(string url)
        {
            client = new ToDoHttpClient(url);
        }

        public Task<bool> AddItemAsync(ToDo item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ToDo> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ToDo>> GetItemsAsync(bool forceRefresh = false)
        {
            // generate the response into a ToDoList object (list of the DTO with prettier printing)
            /*HttpResponseMessage res = (await client.SendRequest());
            if (res.IsSuccessStatusCode)
            {
                HttpContent content = res.Content;

                List<ToDo> listy = JsonSerializer.Deserialize<List<ToDo>>(await content.ReadAsStringAsync());
                return listy;
            }
            else
            {
                return new List<ToDo>();
            }*/
            List<ToDo> listy = JsonSerializer.Deserialize<List<ToDo>>(await client.SendRequest());
            return listy;
        }

        public Task<bool> UpdateItemAsync(ToDo item)
        {
            throw new NotImplementedException();
        }
    }
}