using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using CheckerUI.Helpers;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class BaseLineViewModel : BaseViewModel
    {
        private Button buttonToMake;
        public Command FeelOrdersCommand;
    //    public ObservableCollection<OrderView> AllOrders { get; set; } = new ObservableCollection<OrderView>();
        public Dictionary<int, OrderItemView> m_Orders { get;set;} = new Dictionary<int, OrderItemView>();
        private Grid m_currentViewableGrid = null;
        private OrderItemView m_currentOrder;
        private ObservableCollection<int> m_ordersStatus;
        private readonly Grid m_GridToMake;
        private readonly Grid m_GridToLoc;
        private readonly Grid m_GridOrders;
        private readonly Grid m_InProgressGrid;
        private int m_counterID =0;
        private  List<OrderItemView> m_DoneOrdersList { get; set; } = new List<OrderItemView>();
        public BaseLineViewModel(Grid i_ToMakeGrid, Grid i_LockedGird, Grid i_InProgressGrid, Grid i_GridOrders)
        {
            
            m_GridToMake = new Grid();
            m_GridOrders = new Grid();
            m_GridToLoc = new Grid();
            m_InProgressGrid = new Grid();
            m_GridToMake = i_ToMakeGrid;
            m_GridToLoc = i_LockedGird;
            m_InProgressGrid = i_InProgressGrid;
            m_GridOrders = i_GridOrders;
            FeelOrdersCommand = new Command(fillLayout);
            m_currentOrder = new OrderItemView(-1,"Dummy",null); //dummy order
            m_currentViewableGrid = m_currentOrder.OderGrid;
            m_ordersStatus.Add(m_currentOrder.OrderStatus);
           
            
        }

        private void currentOrder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OrderItemView current = sender as OrderItemView;
            Button orderButton = current.m_Button;
            if (current.IsStarted)
            {
                current.OderGrid.BackgroundColor = Color.DarkGoldenrod;
                m_InProgressGrid.Children.AddVertical(orderButton);
                m_GridToMake.Children.Remove(orderButton);
                orderButton.BackgroundColor = Color.DarkGoldenrod;
            }
            else if (current.IsHolding)
            {
                current.OderGrid.BackgroundColor = Color.OrangeRed;
                orderButton.BackgroundColor = Color.OrangeRed;
            }
            else //completed
            {
                current.OderGrid.BackgroundColor = Color.YellowGreen;
                orderButton.BackgroundColor = Color.YellowGreen;
                m_InProgressGrid.Children.Remove(orderButton);
                m_DoneOrdersList.Add(current);
                m_Orders.Remove(current.OderID);
            }
        }

     
        private void fillLayout()
        {
            OrderItemView order;
            OrderItemView order2;
            double delta = 0;
            for (int i = m_counterID; i < m_counterID+3; i++)
            {
                buttonToMake = new Button();
                buttonToMake.Text = i.ToString();
                buttonToMake.Clicked += Button_Clicked;
                buttonToMake.CornerRadius = 10;
                buttonToMake.BackgroundColor = Color.Gold;
                //buttonInProgress.Text = (i + 5).ToString();
                //buttonInProgress.Clicked += Button_Clicked;
                //buttonInProgress.BackgroundColor = Color.Red;
                //m_GridLayoutToMake.Children.AddVertical(buttonToMake);
                
                m_GridToMake.Children.AddVertical(buttonToMake);
                order = new OrderItemView(i, buttonToMake);
                //order2 = new OrderView(i + 5);
                m_Orders.Add(i,order);
                //AllOrders.Add(order2);
                //m_GridToLoc.Children.AddVertical(buttonInProgress);
                //m_OrdersLayout.Children.AddVertical(order.OderGrid);
            }

            m_counterID += 3;
        }
      

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int id = int.Parse(button.Text);
            Grid currentGrid = new Grid();
            if (m_currentViewableGrid.IsVisible)
            {
                m_currentViewableGrid.IsVisible = false;
                m_GridOrders.Children.Remove(m_currentViewableGrid);
            }
            currentGrid = m_Orders[id].OderGrid;
            currentGrid.IsVisible = true;
            m_currentViewableGrid = currentGrid;
            m_GridOrders.Children.Add(currentGrid);
            
        }
    }
}
