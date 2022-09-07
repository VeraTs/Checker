using System;
using CheckerUI.Helpers;
using CheckerUI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views.KitchenLineCardsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsDoneCardView : ContentView
    {
        private readonly ItemCardHelper r_ItemCardHelper = new ItemCardHelper();
        public LineViewModel ViewModel { get; set; }

        public ItemsDoneCardView()
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
            await ViewModel.ItemReadyOnDoubleClick(item);
        }

    }
}