using System;
using System.Threading.Tasks;
using CheckerUI.Helpers.Order;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderCardView : ContentView
    {
        private Frame m_LastFrameTapped;
        private OrderViewModel m_ViewModel;
        public OrderCardView()
        {
            InitializeComponent();
            m_ViewModel = this.BindingContext as OrderViewModel;
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {

            var stackLayout = sender as StackLayout;
            m_ItemsCollection.SelectedItem = stackLayout.BindingContext;
            OrderItemCardView card = stackLayout.LogicalChildren[0] as OrderItemCardView;
            var frame = card.LogicalChildren[0] as Frame;
            if (m_LastFrameTapped != null)
            {
                m_LastFrameTapped.BackgroundColor = Color.Transparent;
            }
            m_LastFrameTapped = frame;
            frame.BackgroundColor = Color.Gray;
        }

        private async  void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            m_ItemsCollection.SelectedItem = stackLayout.BindingContext;
            
            var item = m_ItemsCollection.SelectedItem as OrderItemView;
            if (((OrderViewModel) BindingContext).CheckOutItem(item))
            {
                await Task.Delay(300);
            }
            
            
        }
    }
}