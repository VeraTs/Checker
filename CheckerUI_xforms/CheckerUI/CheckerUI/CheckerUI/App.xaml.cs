using System;
using CheckerUI.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI
{
    public partial class App : Application
    {
        public static WebDataStore Store { get; private set; }
        public static HubConnection HubConn { get; private set; }
        public App()
        {
            InitializeComponent();

             DependencyService.Register<WebDataStore>();
            DependencyService.Register<MockDataStore>();

            Store = new WebDataStore("https://coresqltester.azurewebsites.net/JsonToDos/");
            HubConn = new HubConnectionBuilder()
                .WithUrl("https://coresqltester.azurewebsites.net/todosHub")
                .WithAutomaticReconnect()
                .Build();
            Application.Current.SavePropertiesAsync();
            MainPage = new NavigationPage(new MainPage())
            {
            };

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
