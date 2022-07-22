using System;
using CheckerUI.Helpers;
using CheckerUI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views.KitchenLineCardsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsLockedCardView : ContentView
    {
        public LineViewModel ViewModel { get; set; }

        private readonly ItemCardHelper r_ItemCardHelper = new ItemCardHelper();  

        public ItemsLockedCardView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var frame = card.Children[0] as Frame;
            var expander = frame.Children[0] as Expander;
            r_ItemCardHelper.OnSingleTap(frame, expander);
        }
        private async void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var item = card.BindingContext as OrderItemViewModel;
            var answer = await Application.Current.MainPage.DisplayAlert("Order Locked", "Are You Sure ?", "Yes", "No");
            if (answer)
            {
               await ViewModel.ItemLockedOnDoubleClicked(item);
            }
        }
    }
}