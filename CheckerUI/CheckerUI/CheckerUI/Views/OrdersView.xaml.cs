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
        private OrdersViewModel m_ViewModel;
        private ServingAreasViewModel m_AreasViewModel;
        public string name { get; set; }
        public int AreaId { get; set; }
        public OrdersView(OrdersViewModel i_Vm)
        {
            InitializeComponent();
            m_ViewModel = i_Vm;
            BindingContext = m_ViewModel;
            AreaId = i_Vm.ViewId;
            name = m_ViewModel.Name;
        }

        public void SetBindingToMainVM(ServingAreasViewModel i_mainVM)
        {
            m_AreasViewModel = i_mainVM;
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
            var item = Zones.SelectedItem as ZoneViewModel;
            var id = item.ItemViewModel.OderItemID;

            var res = m_AreasViewModel.PickUpItemForServing(id).Result;

        }
    }
}