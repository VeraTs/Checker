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
        private List<OrderView> OrdersViews = new List<OrderView>(10);

        bool isVisible = false;
        private Grid m_currentViewableGrid =null;

        private void fillLayout()
        {
            OrderView order;
            OrderView order2;
            double delta = 0;
            for (int i = 0; i < 5; i++)
            {

                buttonToMake = new Button();
                buttonToMake.Text = i.ToString();
                buttonToMake.Clicked += Button_Clicked;
                buttonToMake.CornerRadius = 10;
                buttonToMake.BackgroundColor = Color.Gold;
                m_GridLayoutToMake.Children.AddVertical(buttonToMake);

                order = new OrderView(i);
                OrdersViews.Add(order);
                //m_OrdersLayout.Children.AddVertical(order.OderGrid);
            }
            
        }

        public LineView(string i_Title)
        {
            InitializeComponent();
            BindingContext = new BaseLineViewModel();
            this.Title = i_Title;
            BackgroundColor = Color.PowderBlue;
            
           fillLayout();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int id = int.Parse(button.Text);
            Grid currentGrid = new Grid();
            if (m_currentViewableGrid != null)
            {
                m_currentViewableGrid.IsVisible = false;
                m_OrdersLayout.Children.Remove(m_currentViewableGrid);
            }

            currentGrid = OrdersViews[id].OderGrid;
            currentGrid.IsVisible = true;
            m_currentViewableGrid = currentGrid;
            m_OrdersLayout.Children.Add(currentGrid);
            if (button.Parent == m_GridLayoutToMake)
            {
                m_GridLayoutInProgress.Children.AddVertical(button);
                m_GridLayoutToMake.Children.Remove(button);
                button.BackgroundColor = Color.LightGreen;
            }
        }
    }
}