using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZoneCardView : ContentView
    {
        private OrdersViewModel vm { get; set; }
        public ZoneCardView()
        {
            InitializeComponent();
        }

        public void SetVm(OrdersViewModel i_vm)
        {
            vm = i_vm;
        }

        private async void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
          
            var item = m_ItemsCollection.SelectedItem as OrderItemViewModel;
            if (((OrderViewModel)BindingContext).CheckOutItem(item))
            {
                await Task.Delay(300);
            }
        }
    }
}