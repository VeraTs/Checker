using System;
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
    }
}