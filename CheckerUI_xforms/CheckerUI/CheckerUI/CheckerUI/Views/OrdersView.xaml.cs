using System;
using System.Threading.Tasks;
using CheckerUI.Models;
using CheckerUI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersView : ContentPage
    {
        private StackLayout lastStackLayout = null;

        public OrdersView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            stackLayout.BackgroundColor = Color.Gray;
            if (lastStackLayout != null)
            {
                lastStackLayout.BackgroundColor = Color.White;
            }

            lastStackLayout = stackLayout;
        }

        private async void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            if (sender is StackLayout stackLayout) Zones.SelectedItem = stackLayout.BindingContext;

            if (Zones.SelectedItem is ServingZone item &&
                ((OrdersViewModel) BindingContext).PickUpItemForServing(item.id).Result)
            {
                lastStackLayout.BackgroundColor = Color.White;
                lastStackLayout = null;
                await Task.Delay(300);
            }
        }
    }
}