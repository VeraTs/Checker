using System;
using System.Collections.Generic;
using System.Net.Http;
using CheckerUI.Models;
using CheckerUI.Services;
using CheckerUI.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckerUI
{
    public partial class App : Application
    {
        public static ServerRepository Repository { get; private set; }
        public static HubConnection HubConn { get; private set; }
        private readonly HttpClientHandler handler = new HttpClientHandler();
        public static HubConnection OrderHubConnection { get; private set; }
        public static UserDataStore UserStore { get; private set; } = new UserDataStore();
        public static Restaurant restaurant { get; set; }
        public static int RestId = 1;
        private readonly bool isDebug = false;

        private string BaseAddress =
            DeviceInfo.Platform == DevicePlatform.Android ? "https://checkerapp.azurewebsites.net" : "https://checkerapp.azurewebsites.net";
        public static HttpClient client { get; private set; }

        private List<Dish> dishes = new List<Dish>();
        public App()
        {

#if DEBUG
            isDebug = true;
#endif

            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            InitializeComponent();
            client = isDebug ? new HttpClient(handler) : new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);
            client.Timeout = new TimeSpan(0, 0, 10);

            DependencyService.Register<UserDataStore>();

            string kitchenHubUrl = BaseAddress + "/KitchenHub";
            string OrderHubUrl = BaseAddress + "/OrdersHub";

            OrderHubConnection = new HubConnectionBuilder()
                .WithUrl(OrderHubUrl, options =>
                {
                    if (isDebug)
                    {
                        options.WebSocketConfiguration = conf =>
                        {
                            conf.RemoteCertificateValidationCallback = (message, cert, chain, errors) => { return true; };
                        };
                        options.HttpMessageHandlerFactory = factory =>
                        {
                            HttpClientHandler handler3 = new HttpClientHandler();
                            /*handler3.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                            {
                                if (cert.Issuer.Equals("CN=localhost"))
                                    return true;
                                return errors == System.Net.Security.SslPolicyErrors.None;
                            };*/
                            return handler3;
                        };
                        //options.AccessTokenProvider = () => Task.FromResult(Token);
                    }
                })
                .WithAutomaticReconnect()
                .Build();


            HubConn = new HubConnectionBuilder()
                .WithUrl(kitchenHubUrl, options =>
                {
                    if (isDebug)
                    {
                        options.WebSocketConfiguration = conf =>
                        {
                            conf.RemoteCertificateValidationCallback = (message, cert, chain, errors) => { return true; };
                        };
                        options.HttpMessageHandlerFactory = factory =>
                        {
                            HttpClientHandler handler2 = new HttpClientHandler();
                            /*handler2.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                            {
                                if (cert.Issuer.Equals("CN=localhost"))
                                    return true;
                                return errors == System.Net.Security.SslPolicyErrors.None;
                            };*/
                            return handler2;
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
                Application.Current.MainPage.DisplayAlert("Exception!", str, "OK");
            });
            HubConn.On<String>("NewGroupMember", (str) =>
            {
                Application.Current.MainPage.DisplayAlert("NewGroupMemberException!", str, "OK");
            });


            OrderHubConnection.On<String>("DBError", (str) =>
            {
                Application.Current.MainPage.DisplayAlert("OrdersHubException!", str, "OK");
            });
            OrderHubConnection.On<String>("SignalRError", (str) =>
            {
                Application.Current.MainPage.DisplayAlert("OrdersHubException!", str, "OK");
            });
            OrderHubConnection.On<String>("NewGroupMember", (str) =>
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
