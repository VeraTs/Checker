using System;
using CheckerWaitersApp.Views.CreateOrderView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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
