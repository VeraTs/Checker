using System;
using CheckerUI.Helpers;
using CheckerUI.Helpers.Order;
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
       
        public ItemsDoneCardView()
        {
            InitializeComponent();
        }
        public BaseLineViewModel ViewModel { get; set; } = new BaseLineViewModel();
        
        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var frame = card.Children[0] as Frame;
            var expander = frame.Children[0] as Expander;
            r_ItemCardHelper.OnSingleTap(frame, expander);
        }

        private void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var item = card.BindingContext as OrderItemView;
            ViewModel.ItemReadyOnDoubleClick(item);
        }

    }
}