using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinTest2.ViewModels;
using XamarinTest2.Views;

namespace XamarinTest2
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
