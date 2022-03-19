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
        public String SendRequest()
        {
            client.Timeout = TimeSpan.FromMinutes(5);
            Task<HttpResponseMessage> response = client.GetAsync("");
            response.Wait();
            if(response != null)
            {
                var stream = response.Result.Content.ReadAsStream();
                byte[] buffer = new byte[1024];
                StringBuilder res = new StringBuilder();
                int offset = 0;
                bool read = true;
                int bytesRead = 0;
                while(read)
                {

                    bytesRead = stream.Read(buffer, offset, buffer.Length);
                    if(bytesRead > 0)
                    {
                        String temp = System.Text.Encoding.UTF8.GetString(buffer);
                        res.Append(temp.Substring(0, bytesRead));
                        offset += bytesRead;
                    } else
                    {
                        read = false;
                    }

                    if(offset >= stream.Length)
                    {
                        read = false;
                    }
                }

                return res.ToString();
            }

            return "";            
        }
    }
}
