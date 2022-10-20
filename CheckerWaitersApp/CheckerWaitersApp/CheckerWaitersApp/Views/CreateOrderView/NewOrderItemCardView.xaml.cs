using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.CreateOrderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewOrderItemCardView : ContentView
    {
        public NewOrderItemCardView()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            OrderItemFrame.BackgroundColor = Color.CadetBlue;
            await Task.Delay(100);
            OrderItemFrame.BackgroundColor = Color.Azure;
        }
    }
}