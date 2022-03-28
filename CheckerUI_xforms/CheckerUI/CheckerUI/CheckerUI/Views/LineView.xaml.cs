using CheckerUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineView : ContentPage
    {
        private List<OrderItemView> OrdersViews = new List<OrderItemView>(10);

        private Grid m_currentViewableGrid =null;

        private BaseLineViewModel baseVM;

        public LineView(string i_Title)
        {
            InitializeComponent();
            this.Title = i_Title;
            BackgroundColor = Color.Transparent;
             baseVM = new BaseLineViewModel();
            baseVM.init(m_OrdersLayout);
            BindingContext = baseVM;
            m_GetOrdersButton.Command = baseVM.FeelOrdersCommand;
            
            //fillLayout();
        }

        private void M_ToMakeView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            baseVM.Button_Clicked(sender , EventArgs.Empty);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            baseVM.Button_Clicked(sender, EventArgs.Empty);
        }
    }
}