
using CheckerWaitersApp.Services;
using CheckerWaitersApp.Views.CreateOrderView;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CheckerWaitersApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace CheckerWaitersApp
{
    public partial class App : Application
    {
        //public static DishDataStore DishesStore { get; private set; }
        //public static OrderItemDataStore AllOrdersItemsStore { get; private set; }
        //public static OrdersDataStore OrdersStore { get; private set; }
        public static ServerRepository Repository { get; private set; }
        public static HubConnection HubConn { get; private set; }
        private readonly HttpClientHandler handler = new HttpClientHandler();

        bool isDebug = false;

        private string BaseAddress =
            DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7059" : "https://localhost:7059";
        public static HttpClient client { get; private set; }

        private List<Dish> dishes = new List<Dish>();
        public App()
        {
             #if DEBUG
            isDebug = true;
            #endif

            InitializeComponent();
            client = isDebug ? new HttpClient(handler) : new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);
            client.Timeout = new TimeSpan(0, 0, 30);

            this.handler = handler;
            DependencyService.Register<DishDataStore>();


            /*Store = new WebDataStore("https://checkertester.azurewebsites.net/JsonToDos/");
            HubConn = new HubConnectionBuilder()
                .WithUrl("https://checkertester.azurewebsites.net/todosHub")
                .WithAutomaticReconnect()
                .Build();*/

            // makes SSL certificate for using localhost at debug
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            Repository = new ServerRepository();
            Repository.LoadData();
            string ordersHubUrl = BaseAddress + "/OrdersHub";
            HubConn = new HubConnectionBuilder()
                .WithUrl(ordersHubUrl, options =>
                {
                    if (isDebug)
                    {
                        options.WebSocketConfiguration = conf =>
                        {
                            conf.RemoteCertificateValidationCallback = (message, cert, chain, errors) => { return true; };
                        };
                        options.HttpMessageHandlerFactory = factory => handler;
                        //options.AccessTokenProvider = () => Task.FromResult(Token);
                    }
                })
                .WithAutomaticReconnect()
                .Build();
            HubConn.On<String>("DBError", (str) =>
            {
                Application.Current.MainPage.DisplayAlert("Exception!", str, "OK");
            });


            MainPage = new CreateOrderView();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }



    }
}