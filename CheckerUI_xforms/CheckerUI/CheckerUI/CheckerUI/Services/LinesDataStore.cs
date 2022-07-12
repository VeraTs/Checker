using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class LinesDataStore : IDataStore<Line>
    {
        public List<Line> lines { get; set; }
        public List<Line> listy { get; private set; } = new List<Line>();
        public LinesDataStore()
        {
            
        }

        public Task<bool> AddItemAsync(Line item)
        {
            throw new NotImplementedException();
        }

      
        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Line> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Line>> GetItemsAsync(bool forceRefresh = false)
        {
           
            lines = new List<Line>();
            try
            {
                string res = App.client.GetStringAsync("/Lines").Result;
                listy = System.Text.Json.JsonSerializer.Deserialize<List<Line>>(res);
                foreach (var item in listy)
                {
                  
                  var currentLine = new Line();
                  currentLine = item;
                 
                  lines.Add(currentLine);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listy;
        }

        public Task<bool> UpdateItemAsync(Line item)
        {
            throw new NotImplementedException();
        }
    }
}
