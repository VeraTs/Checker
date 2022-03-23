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
        private Button buttonToMake;
        private Button buttonInProgress;
        private List<OrderItemView> OrdersViews = new List<OrderItemView>(10);

        bool isVisible = false;
        private Grid m_currentViewableGrid =null;

       

        public LineView(string i_Title)
        {
            InitializeComponent();
            this.Title = i_Title;
            BackgroundColor = Color.PowderBlue;
            var baseVM =  new BaseLineViewModel(m_GridLayoutToMakeAvail, m_GridLayoutToMakeLoc, m_GridLayoutInProgress, m_OrdersLayout);
            BindingContext = baseVM;
            m_GetOrdersButton.Command = baseVM.FeelOrdersCommand;

            //fillLayout();
        }
    }
}