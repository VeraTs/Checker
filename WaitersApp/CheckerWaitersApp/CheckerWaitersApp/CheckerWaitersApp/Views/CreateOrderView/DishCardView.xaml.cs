using System;
using System.Threading.Tasks;
using CheckerWaitersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.CreateOrderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DishCardView : ContentView
    {
        public DishCardView()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            ItemFrame.BackgroundColor = Color.CadetBlue;
            await Task.Delay(100);
            (BindingContext as DishViewModel).IsOrdered = true;
            ItemFrame.BackgroundColor = Color.Azure;
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            ItemFrame.BackgroundColor = Color.CadetBlue;
            await Task.Delay(100);
            ItemFrame.BackgroundColor = Color.Azure;
        }
    }
}