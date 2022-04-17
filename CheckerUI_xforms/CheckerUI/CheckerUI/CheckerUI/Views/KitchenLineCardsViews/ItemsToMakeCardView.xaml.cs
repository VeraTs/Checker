using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers.Order;
using CheckerUI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views.KitchenLineCardsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsToMakeCardView : ContentView
    {
        public CollectionView m_ToMakeView = new CollectionView();
        private Expander m_LastTappedExpander;
        private Frame m_LastFrameTapped;

        public BaseLineViewModel ViewModel { get; set; } = new BaseLineViewModel();

        public ItemsToMakeCardView()
        {
            InitializeComponent();
            m_ToMakeView = m_ToMakeListView;
        }

        

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            m_ToMakeListView.SelectedItem = stackLayout.BindingContext;
            KitchenOrderItemCardView card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
           

            var frame = card.Children[0] as Frame;
            var expander = frame.Children[0] as Expander;
            if (m_LastTappedExpander != null && m_LastFrameTapped != null)
            {
                m_LastFrameTapped.BackgroundColor = Color.White;
                m_LastTappedExpander.IsExpanded = false;
            }

            if (m_LastTappedExpander != expander)
            {
                m_LastFrameTapped = frame;
                m_LastFrameTapped.BackgroundColor = Color.BurlyWood;
                m_LastTappedExpander = expander;
                m_LastTappedExpander.IsExpanded = true;
            }
            else
            {
                m_LastTappedExpander = null;
            }
           
        }

        private void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            m_ToMakeListView.SelectedItem = stackLayout.BindingContext;
            KitchenOrderItemCardView card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var item = card.BindingContext as OrderItemView;
            ViewModel.ItemToMakeOnDoubleClicked(item);
        }
    }
}