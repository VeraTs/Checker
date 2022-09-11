using CheckerWaitersApp.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using CheckerWaitersApp.Models;
using CheckerWaitersApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckerWaitersApp
{
    public partial class App : Application
    {
        public static ServerRepository Repository { get; private set; }
        public static HubConnection HubConn { get; private set; }
        private readonly HttpClientHandler handler = new HttpClientHandler();
        public static UserDataStore UserStore { get; private set; } = new UserDataStore();
        public static Restaurant restaurant { get; set; }
        public static int RestId = 1;
        bool isDebug = false;

        private string BaseAddress =
            DeviceInfo.Platform == DevicePlatform.Android ? "https://checkerapp.azurewebsites.net" : "https://checkerapp.azurewebsites.net";
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
            DependencyService.Register<UserDataStore>();


         
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
                        options.HttpMessageHandlerFactory = factory =>
                        {
                            HttpClientHandler handler = new HttpClientHandler();
                            //handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                            //{
                            //    if (cert.Issuer.Equals("CN=localhost"))
                            //        return true;
                            //    return errors == System.Net.Security.SslPolicyErrors.None;
                            //};
                            return handler;
                        };
                        //options.AccessTokenProvider = () => Task.FromResult(Token);
                    }
                })
                .WithAutomaticReconnect()
                .Build();
            HubConn.On<String>("DBError", (str) =>
            {
                Application.Current.MainPage.DisplayAlert("Exception!", str, "OK");
            });
            HubConn.On<String>("SignalRError", (str) =>
            {
                Application.Current.MainPage.DisplayAlert("OrdersHubException!", str, "OK");
            });
            HubConn.On<String>("NewGroupMember", (str) =>
            {
                Application.Current.MainPage.DisplayAlert("OrdersHubNewGroupMemberException!", str, "OK");
            });
            MainPage = new NavigationPage(new MainPage())
            {
            };
        }

        protected override void OnStart()
        {
            Repository = new ServerRepository();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}