using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XamarinTest2.Services
{
    internal class ToDoHttpClient
    {
        private readonly HttpClient client;
        public ToDoHttpClient(String url)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.Timeout = TimeSpan.FromMinutes(5);
        }
        
        // returns the content of the response as a string
        public async Task<String> SendRequest()
		{
            // get the content of the response
            return await client.GetStringAsync("");
        }
    }
}
