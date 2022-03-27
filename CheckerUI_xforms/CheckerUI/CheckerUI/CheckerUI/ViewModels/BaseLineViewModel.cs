using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CheckerUI.Helpers;
using CheckerUI.Models;
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
        private  Grid m_GridOrders;
        
       
       // public ObservableCollection<Button> m_ButtonsToMake = new ObservableCollection<Button>();
        public ObservableCollection<OrderButtonModel> m_ButtonsInProgress = new ObservableCollection<OrderButtonModel>();
        public ObservableCollection<OrderButtonModel> m_ButtonsLocked = new ObservableCollection<OrderButtonModel>();
        public ObservableCollection<OrderButtonModel> m_ButtonsTomake = new ObservableCollection<OrderButtonModel>();
        private  List<OrderItemView> m_DoneOrdersList { get; set; } = new List<OrderItemView>();

        public BaseLineViewModel()
        {
            
        }

        // in here we are getting ref of the ui layouts , those method need to become async !

        public void init( Grid i_GridOrders)
        {
           
            m_GridOrders = new Grid();
           
            m_GridOrders = i_GridOrders;
            FeelOrdersCommand = new Command(fillLayout);
            OrderIDNotifier m = new OrderIDNotifier(-1, -1);
            var mCurrentOrder = new OrderItemView("Dummy", null, m);
            m_currentViewableGrid = mCurrentOrder.OderGrid;
        }
        public void currentOrder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OrderItemView current = sender as OrderItemView;
            OrderButtonModel orderButton = current.m_Button;
            if (current.IsStarted)
            {
                current.OderGrid.BackgroundColor = Color.YellowGreen;
                m_ButtonsInProgress.Add(orderButton);
                m_ButtonsTomake.Remove(orderButton);
              //  m_ButtonsToMake.Remove(orderButton);
                orderButton.m_OrderButton.BackgroundColor = Color.DarkGoldenrod;
            }
            else if (current.IsHolding)
            {
                current.OderGrid.BackgroundColor = Color.OrangeRed;
                orderButton.m_OrderButton.BackgroundColor = Color.OrangeRed;
            }
            else //completed
            {
                current.OderGrid.BackgroundColor = Color.YellowGreen;
                orderButton.m_OrderButton.BackgroundColor = Color.YellowGreen;
                m_ButtonsInProgress.Remove(orderButton);
                m_DoneOrdersList.Add(current);
                m_Orders.Remove(current.OderID);
            }
        }
        private void fillLayout()
        {
            OrderItemView order;
           
            int nameIdx = 0;
            List<string> nmList = new List<string>()
            {
                "pizza", "burger", "fish"
            };
            ;
            for (int i = m_counterID; i < m_counterID + 3; i++)
            {
                
                    int status = -1;
                    buttonToMake = new Button();
                    buttonToMake.Padding = new Thickness(20, 20, 20, 20);
                    buttonToMake.Text = i.ToString();
                    buttonToMake.Clicked += Button_Clicked;
                    buttonToMake.CornerRadius = 10;
                    buttonToMake.BackgroundColor = Color.Gold;
                    buttonToMake.IsVisible = true;
                    OrderIDNotifier m = new OrderIDNotifier(i, status);
                    OrderButtonModel model = new OrderButtonModel(i, buttonToMake);
                    m_ButtonsTomake.Add(model);

                    //m_ButtonsInProgress.Add(model);
                    /// m_ButtonsToMake.Add(buttonToMake);
                    // m_GridToMake.Children.AddVertical(buttonToMake);
                    order = new OrderItemView(nmList[nameIdx++],model, m);

                    m.PropertyChanged += OrderIDNotifierPropertyChanged;
                    m_Orders.Add(i, order);
                
              
                
                    // add to waiting list 
                
            }
          
            m_counterID += 3;
        }

        private void OrderIDNotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OrderIDNotifier m = sender as OrderIDNotifier;
            OrderItemView order = m_Orders[m.OrderID];

            if (order.IsHolding)
            {
                order.m_Button.m_OrderButton.BackgroundColor = Color.OrangeRed;
                m_currentViewableGrid.IsVisible = false;
                m_currentViewableGrid = order.OderGrid;
                m_currentViewableGrid.BackgroundColor = Color.Tomato;
                m_currentViewableGrid.IsVisible = true;
            }
            else if (order.IsInProgress)
            {
                order.m_Button.m_OrderButton.BackgroundColor = Color.Gold;
                m_ButtonsInProgress.Add(order.m_Button);
                m_ButtonsTomake.Remove(order.m_Button);
//                m_ButtonsToMake.Remove(order.m_Button);

                //m_InProgressGrid.Children.Add(order.m_Button);
                //m_GridToMake.Children.Remove(order.m_Button);
                m_currentViewableGrid.IsVisible = false;
                m_currentViewableGrid = order.OderGrid;
                m_currentViewableGrid.BackgroundColor = Color.YellowGreen;
                m_currentViewableGrid.IsVisible = true;
            }
            else if (order.IsCompleted) // check with else
            {
                //m_InProgressGrid.Children.Remove(order.m_Button);
                m_ButtonsInProgress.Remove(order.m_Button);
                m_currentViewableGrid.IsVisible = false;
                m_currentViewableGrid = order.OderGrid;
                m_currentViewableGrid.BackgroundColor = Color.ForestGreen;
                m_currentViewableGrid.IsVisible = true;
                // add to done list
            }
        }

        public void Button_Clicked(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            //OrderButtonModel current = new OrderButtonModel();

            OrderButtonModel b = new OrderButtonModel();
            b = lv.SelectedItem as OrderButtonModel;
            //  Button button = sender as Button;
          int id = int.Parse(b.m_OrderButtonID);
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

        public ObservableCollection<OrderButtonModel> ButtonsInProgress
        {
            get => m_ButtonsInProgress;
            set => m_ButtonsInProgress = value;
        }

        public ObservableCollection<OrderButtonModel> ToMakeOrderIdCollection
        {
            get => m_ButtonsTomake;
            set => m_ButtonsTomake = value;
        }

        //public ObservableCollection<Button> ButtonsToMake
        //{
        //    get => m_ButtonsToMake;
        //    set => m_ButtonsToMake = value;
        //}
        public ObservableCollection<OrderButtonModel> ButtonsLocked
        {
            get => m_ButtonsLocked;
            set => m_ButtonsLocked = value;
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
