using System;
using CheckerUI.Helpers;
using CheckerUI.Helpers.OrdersHelpers;
using CheckerUI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views.KitchenLineCardsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsToMakeCardView : ContentView
    {
        public CollectionView m_ToMakeView;
        private readonly ItemCardHelper r_ItemCardHelper = new ItemCardHelper();

        public BaseLineViewModel ViewModel { get; set; } = new BaseLineViewModel();

        public ItemsToMakeCardView()
        {
            InitializeComponent();
            m_ToMakeView = ToMakeListView;
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            ToMakeListView.SelectedItem = stackLayout.BindingContext;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var frame = card.Children[0] as Frame;
            var expander = frame.Children[0] as Expander;
            r_ItemCardHelper.OnSingleTap(frame, expander);
        }

        private void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            ToMakeListView.SelectedItem = stackLayout.BindingContext;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var item = card.BindingContext as OrderItemView;
            ViewModel.ItemToMakeOnDoubleClicked(item);
        }
    }
}