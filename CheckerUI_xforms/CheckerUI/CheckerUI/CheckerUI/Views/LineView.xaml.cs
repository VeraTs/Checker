using CheckerUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers;
using CheckerUI.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineView : ContentPage
    {
        private List<OrderItemView> OrdersViews = new List<OrderItemView>(10);

        private readonly BaseLineViewModel baseVM;
        private ListView m_LastListWithItemSelected = null;

        public LineView(string i_Title)
        {
            InitializeComponent();
            this.Title = i_Title;
            BackgroundColor = Color.Transparent;
            baseVM = new BaseLineViewModel();
            baseVM.init(m_OrdersLayout);
            BindingContext = baseVM;
            m_GetOrdersButton.Command = baseVM.FeelOrdersCommand;
        }
        private void toMakeView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            baseVM.LastSelectedOBM = lv.SelectedItem as OrderButtonModel;
            m_LastListWithItemSelected = lv;
        }
        private void Cell_OnTapped(object sender, EventArgs e)
        {
            ViewCell vc = sender as ViewCell;
            baseVM.LastTappedCell = vc;
            baseVM.Button_Clicked(sender, EventArgs.Empty);
            m_LastListWithItemSelected.SelectedItem = null;
            
        }
        private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           Grid g = sender as Grid;
           baseVM.m_LastGridInCell = g;
        }
    }
}