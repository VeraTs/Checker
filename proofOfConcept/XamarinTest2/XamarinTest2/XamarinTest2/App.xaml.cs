<<<<<<< HEAD
﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
=======
﻿using System;
>>>>>>> 39f682f6971b099aad92f1757aeee439c6ba4674
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTest2.Services;
using XamarinTest2.Views;

namespace XamarinTest2
{
    public partial class App : Application
    {
        public static WebDataStore Store { get; private set; }
<<<<<<< HEAD
        public static HubConnection HubConn { get; private set; }
=======
>>>>>>> 39f682f6971b099aad92f1757aeee439c6ba4674
        public App()
        {
            InitializeComponent();

            DependencyService.Register<WebDataStore>();
            DependencyService.Register<MockDataStore>();
<<<<<<< HEAD

            Store = new WebDataStore("https://coresqltester.azurewebsites.net/JsonToDos/");
            HubConn = new HubConnectionBuilder()
                .WithUrl("https://coresqltester.azurewebsites.net/todosHub")
                .WithAutomaticReconnect()
                .Build();

=======
            Store = new WebDataStore("https://coresqltester.azurewebsites.net/JsonToDos/");
            
>>>>>>> 39f682f6971b099aad92f1757aeee439c6ba4674
            MainPage = new AppShell();
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
