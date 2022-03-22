using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CheckerUI.Helpers;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class BaseLineViewModel : BaseViewModel
    {
        private Button buttonToMake;
        private Button buttonInProgress;
        private List<OrderView> OrdersViews = new List<OrderView>(10);
        public ObservableCollection<OrderView> AllOrders { get; set; } = new ObservableCollection<OrderView>();
        bool isVisible = false;
        private Grid m_currentViewableGrid = null;
        
        private void fillLayout(Grid i_GridLayout)
        {
            OrderView order;
            OrderView order2;
            double delta = 0;
            for (int i = 0; i < 3; i++)
            {

                buttonToMake = new Button();
                buttonToMake.Text = i.ToString();
                //  buttonToMake.Clicked += Button_Clicked;
                buttonToMake.CornerRadius = 10;
                buttonToMake.BackgroundColor = Color.Gold;
                i_GridLayout.Children.AddVertical(buttonToMake);

                order = new OrderView(i);
                OrdersViews.Add(order);

                //m_OrdersLayout.Children.AddVertical(order.OderGrid);
            }

        }
    }
}
