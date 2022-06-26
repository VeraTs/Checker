using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckerWaitersApp.Services
{
    internal class CheckerHttpClient
    {

       public HttpRequestMessage request;
       public HttpClient client;
        public CheckerHttpClient(String url)
        {
            request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            client = new HttpClient();
            Task<HttpResponseMessage> response = SendRequest();
        }

        // returns the content of the response as a string
        public async Task<HttpResponseMessage> SendRequest()
        {
            
            // get the content of the response
            return await client.SendAsync(request);
        }
       
    }
}
