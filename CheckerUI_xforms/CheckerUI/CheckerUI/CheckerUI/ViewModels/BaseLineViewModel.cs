using System;
using System.Collections.Generic;
using System.ComponentModel;
using CheckerUI.Helpers;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class BaseLineViewModel : BaseViewModel
    {
        private static int m_counterID = 0; // static counter well be changed as db will contains pk id

        private Button buttonToMake;
        public Command FeelOrdersCommand;
        public Dictionary<int, OrderItemView> m_Orders { get;set;} = new Dictionary<int, OrderItemView>();
        private Grid m_currentViewableGrid = null;
        private readonly Grid m_GridToMake;
        private readonly Grid m_GridToLoc;
        private readonly Grid m_GridOrders;
        private readonly Grid m_InProgressGrid;
       
        
        private  List<OrderItemView> m_DoneOrdersList { get; set; } = new List<OrderItemView>();

        // in here we are getting ref of the ui layouts , those method need to become async !
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
            OrderIDNotifier m = new OrderIDNotifier(-1, -1);
            var mCurrentOrder = new OrderItemView("Dummy",null, m);
            m_currentViewableGrid = mCurrentOrder.OderGrid;
            
        }

        private void currentOrder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OrderItemView current = sender as OrderItemView;
            Button orderButton = current.m_Button;
            if (current.IsStarted)
            {
                current.OderGrid.BackgroundColor = Color.YellowGreen;
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
            int nameIdx = 0;
            List<string> nmList = new List<string>()
            {
                "pizza", "burger", "fish"
            };
            ;
            for (int i = m_counterID; i < m_counterID + 3; i++)
            {
                if (m_GridToMake.Children.Count < 5)
                {
                    int status = -1;
                    buttonToMake = new Button();
                    buttonToMake.Padding = new Thickness(20, 20, 20, 20);
                    buttonToMake.Text = i.ToString();
                    buttonToMake.Clicked += Button_Clicked;
                    buttonToMake.CornerRadius = 10;
                    buttonToMake.BackgroundColor = Color.Gold;
                    OrderIDNotifier m = new OrderIDNotifier(i, status);
                    m_GridToMake.Children.AddVertical(buttonToMake);
                    order = new OrderItemView(nmList[nameIdx++], buttonToMake, m);

                    m.PropertyChanged += M_PropertyChanged;
                    m_Orders.Add(i, order);
                }
                else
                {
                    // add to waiting list 
                }
            }
          
            m_counterID += 3;
        }

        private void M_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OrderIDNotifier m = sender as OrderIDNotifier;
            OrderItemView order = m_Orders[m.OrderID];

            if (order.IsHolding)
            {
                order.m_Button.BackgroundColor = Color.OrangeRed;
                m_currentViewableGrid.IsVisible = false;
                m_currentViewableGrid = order.OderGrid;
                m_currentViewableGrid.BackgroundColor = Color.Tomato;
                m_currentViewableGrid.IsVisible = true;
            }
            else if (order.IsInProgress)
            {
                order.m_Button.BackgroundColor = Color.Gold;
                m_InProgressGrid.Children.Add(order.m_Button);
                m_GridToMake.Children.Remove(order.m_Button);
                m_currentViewableGrid.IsVisible = false;
                m_currentViewableGrid = order.OderGrid;
                m_currentViewableGrid.BackgroundColor = Color.YellowGreen;
                m_currentViewableGrid.IsVisible = true;
            }
            else if (order.IsCompleted) // check with else
            {
                m_InProgressGrid.Children.Remove(order.m_Button);
                m_currentViewableGrid.IsVisible = false;
                m_currentViewableGrid = order.OderGrid;
                m_currentViewableGrid.BackgroundColor = Color.ForestGreen;
                m_currentViewableGrid.IsVisible = true;
                // add to done list
            }
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

        //nested class to follow order status changes , 
        // need to check if we can delete his OnPropertyChanged methods cuz we are not doing any with them 
        public class OrderIDNotifier : INotifyPropertyChanged 
        {
            private int m_OrderID;
            private int m_status;
            public OrderIDNotifier(int i_ID,  int i_Status)
            {
                m_OrderID = i_ID;
                m_status = i_Status;
            }
            protected void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, e);
            }
            protected void OnPropertyChanged(string propertyName)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }

            public int Status
            {
                get => m_status;
                set
                {
                    if (m_status != value)
                    {
                        m_status = value;
                        OnPropertyChanged(nameof(m_OrderID));
                    }
                }
            }
            public int OrderID => m_OrderID;
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
