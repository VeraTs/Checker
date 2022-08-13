using System;
using CheckerWaitersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.OrdersViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderCardView : ContentView
    {
        public OrderCardView()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void CloseOrderButton_OnClicked(object sender, EventArgs e)
        {

        }

        public void setVm(OrderViewModel VM)
        {

        }
    }
}