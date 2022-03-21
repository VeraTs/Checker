using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientSampler
{
    internal class ToDoHttpClient
    {
        private readonly HttpClient client;
        public ToDoHttpClient(String url)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }
        
        // returns the content of the response as a string
        public async Task<String> SendRequest()
        {
            client.Timeout = TimeSpan.FromMinutes(5);
            String res = await client.GetStringAsync("");    // get the content of the response

            return res;            
        }
    }
}
