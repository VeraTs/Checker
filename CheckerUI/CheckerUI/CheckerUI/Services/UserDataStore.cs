using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class UserDataStore
    {
        public async Task<bool> LoginAsync(User item)
        {
            try
            {
                string extentionUri = "Users/login";
                var itemInJson = JsonSerializer.Serialize(item);
                var input = new StringContent(itemInJson, Encoding.UTF8, "application/json");
                var res = await App.client.PostAsync(extentionUri, input);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resInString = await res.Content.ReadAsStringAsync();
                    App.restaurant = System.Text.Json.JsonSerializer.Deserialize<Restaurant>(resInString);
                    App.RestId = App.restaurant.id;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RegisterAsync(Restaurant item)
        {
            try
            {
                string extentionUri = "Users/register";
                var itemInJson = JsonSerializer.Serialize(item);
                var input = new StringContent(itemInJson, Encoding.UTF8, "application/json");
                var res = await App.client.PostAsync(extentionUri, input);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    App.restaurant = System.Text.Json.JsonSerializer.Deserialize<Restaurant>(await res.Content.ReadAsStringAsync());
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> LogoutAsync()
        {
            try
            {
                string extentionUri = "logout";
                var res = await App.client.GetAsync(extentionUri);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
