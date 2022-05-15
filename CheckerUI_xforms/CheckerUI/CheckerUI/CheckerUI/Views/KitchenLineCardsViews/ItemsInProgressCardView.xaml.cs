using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers;
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
        private readonly ItemCardHelper r_ItemCardHelper = new ItemCardHelper();

        public BaseLineViewModel ViewModel { get; set; } = new BaseLineViewModel();
        public ItemsInProgressCardView()
        {
            InitializeComponent();
        }
        
      //   private Frame m_LastFrameTapped;
        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            //m_InProgressCollection.SelectedItem = stackLayout.BindingContext;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemInProgressCardView;
            var frame = card.Children[0] as Frame;
            var expander = frame.Children[0] as Expander;

            r_ItemCardHelper.OnSingleTap(frame, expander);

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