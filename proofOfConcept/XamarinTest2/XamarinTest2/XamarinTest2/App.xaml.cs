﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTest2.Services;
using XamarinTest2.Views;

namespace XamarinTest2
{
    public partial class App : Application
    {
        public static WebDataStore Store { get; private set; }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<WebDataStore>();
            DependencyService.Register<MockDataStore>();
            Store = new WebDataStore("https://coresqltester.azurewebsites.net/JsonToDos/");
            
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
