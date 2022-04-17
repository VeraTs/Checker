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
    public partial class ItemsInProgressCardView : ContentView
    {
        public BaseLineViewModel ViewModel { get; set; } = new BaseLineViewModel();
        public ItemsInProgressCardView()
        {
            InitializeComponent();
        }
        
        private Frame m_LastFrameTapped;
        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            m_InProgressCollection.SelectedItem = stackLayout.BindingContext;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemInProgressCardView;


            var frame = card.Children[0] as Frame;
          
            if ( m_LastFrameTapped != null)
            {
                m_LastFrameTapped.BackgroundColor = Color.White;
            }

            if (m_LastFrameTapped != frame)
            {
                m_LastFrameTapped = frame;
                m_LastFrameTapped.BackgroundColor = Color.BurlyWood;
            }
            else
            {
                m_LastFrameTapped = null;
            }

        }

        private void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            m_InProgressCollection.SelectedItem = stackLayout.BindingContext;
            KitchenOrderItemInProgressCardView card = stackLayout.LogicalChildren[0] as KitchenOrderItemInProgressCardView;
            var item = card.BindingContext as OrderItemView;
            ViewModel.ItemInProgressOnDoubleClick(item);


        }
    }
}