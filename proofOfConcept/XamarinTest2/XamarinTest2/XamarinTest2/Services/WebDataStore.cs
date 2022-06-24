using XamarinTest2.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XamarinTest2.Services
{
    public class WebDataStore : IDataStore<ToDo>
    {
        private readonly HttpClient client;
        private HttpClientHandler handler;
        public WebDataStore(string url, HttpClientHandler handler, bool isDebug)
        {
            client = isDebug ? new HttpClient(handler) : new HttpClient();
            client.BaseAddress = new Uri(url);
            client.Timeout = new TimeSpan(0, 0, 30);
            
            this.handler = handler;
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
            List<ToDo> listy = JsonSerializer.Deserialize<List<ToDo>>(await client.GetStringAsync(""));
            return listy;
        }

        public Task<bool> UpdateItemAsync(ToDo item)
        {
            throw new NotImplementedException();
        }
    }
}
