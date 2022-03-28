using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CheckerUI.Helpers;
using CheckerUI.Models;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class BaseLineViewModel : BaseViewModel
    {
        private static int m_counterID = 0;
        private static int m_LockedCounterID = 100;// static counter well be changed as db will contains pk id

        private Button buttonToMake;
        public Command FeelOrdersCommand;
        public Dictionary<int, OrderItemView> m_Orders { get; set; } = new Dictionary<int, OrderItemView>();
        private Grid m_currentViewableGrid = null;
        private Grid m_GridOrders;

        // public ObservableCollection<Button> m_ButtonsToMake = new ObservableCollection<Button>();
        public ObservableCollection<OrderButtonModel> m_ButtonsInProgress = new ObservableCollection<OrderButtonModel>();
        public ObservableCollection<OrderButtonModel> m_ButtonsLocked = new ObservableCollection<OrderButtonModel>();
        public ObservableCollection<OrderButtonModel> m_ButtonsToMake = new ObservableCollection<OrderButtonModel>();
        private List<OrderItemView> m_DoneOrdersList { get; set; } = new List<OrderItemView>();
        public ViewCell LastViewCell = new ViewCell();
        public Grid m_LastGridInCell = new Grid();

        // in here we are getting ref of the ui layouts , those method need to become async !

        public void init(Grid i_GridOrders)
        {
            m_GridOrders = new Grid();
            m_GridOrders = i_GridOrders;
            FeelOrdersCommand = new Command(fillLayout);
            OrderIDNotifier m = new OrderIDNotifier(-1, -1);
            var mCurrentOrder = new OrderItemView("Dummy", null, m);
            m_currentViewableGrid = mCurrentOrder.OderGrid;
        }

        private void fillLayout() //dummy function to add new orders
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

                int status = 0;
                buttonToMake = new Button();
                buttonToMake.Padding = new Thickness(20, 20, 20, 20);
                buttonToMake.Text = i.ToString();
                buttonToMake.Clicked += Button_Clicked;
                buttonToMake.CornerRadius = 10;
                buttonToMake.BackgroundColor = Color.Gold;
                buttonToMake.IsVisible = true;
                OrderIDNotifier m = new OrderIDNotifier(i, status);
                OrderButtonModel model = new OrderButtonModel(i, buttonToMake);
                m_ButtonsToMake.Add(model);
                order = new OrderItemView(nmList[nameIdx++], model, m);
                m.PropertyChanged += OrderIDNotifierPropertyChanged;
                m_Orders.Add(i, order);

            }
            m_counterID += 3;
            buttonToMake = new Button();
            buttonToMake.Padding = new Thickness(20, 20, 20, 20);
            buttonToMake.Text = m_LockedCounterID.ToString();
            buttonToMake.Clicked += Button_Clicked;
            buttonToMake.CornerRadius = 10;
            buttonToMake.BackgroundColor = Color.Gold;
            buttonToMake.IsVisible = true;
            var m2 = new OrderIDNotifier(m_LockedCounterID, -1);
            var model2 = new OrderButtonModel(m_LockedCounterID, buttonToMake);
            m_ButtonsLocked.Add(model2);
            order = new OrderItemView(nmList[0], model2, m2);
            m2.PropertyChanged += OrderIDNotifierPropertyChanged;
            m_Orders.Add(m_LockedCounterID, order);
            m_LockedCounterID++;
        }

        private void OrderIDNotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var m = sender as OrderIDNotifier;
            var order = m_Orders[m.OrderID];
            m_currentViewableGrid.IsVisible = false;
            m_currentViewableGrid = order.OderGrid;
            m_LastGridInCell = LastTappedCell.LogicalChildren.Last() as Grid;

            if (order.IsAvailable)
            {
                caseIsAvailable(order);
            }
            if (order.IsHolding)
            {
                caseIsHolding(order);
            }
            else if (order.IsInProgress)
            {
                caseIsInProgress(order);

            }
            else if (order.IsCompleted)
            {
                caseIsCompleted(order);
            }
            m_currentViewableGrid.IsVisible = true;
        }


        private void caseIsAvailable(OrderItemView i_Order)
        {
            if (m_ButtonsLocked.Contains(i_Order.m_Button))
            {
                m_ButtonsToMake.Add(i_Order.m_Button);
                m_ButtonsLocked.Remove(i_Order.m_Button);
            }
            m_LastGridInCell.BackgroundColor = Color.DarkOrange;
            m_currentViewableGrid.BackgroundColor = Color.DarkOrange;
        }

        private void caseIsHolding(OrderItemView i_Order)
        {
            m_LastGridInCell.BackgroundColor = Color.OrangeRed;
            m_currentViewableGrid.BackgroundColor = Color.OrangeRed;
        }

        private void caseIsInProgress(OrderItemView i_Order)
        {
            m_LastGridInCell.BackgroundColor = Color.YellowGreen;
            if (!m_ButtonsInProgress.Contains(i_Order.m_Button))
            {
                m_ButtonsInProgress.Add(i_Order.m_Button);
            }
            m_ButtonsToMake.Remove(i_Order.m_Button);
            m_currentViewableGrid.BackgroundColor = Color.YellowGreen;
        }

        private void caseIsCompleted(OrderItemView i_Order)
        {
            if (m_ButtonsInProgress.Contains(i_Order.m_Button))
            {
                m_ButtonsInProgress.Remove(i_Order.m_Button);
                m_currentViewableGrid.BackgroundColor = Color.ForestGreen;
            }
            else
            {

            }
        }

        public void Button_Clicked(object sender, EventArgs e)
        {
            var vc = sender as ViewCell;
            var id = int.Parse(LastSelectedOBM.m_OrderButtonID);
            if (m_currentViewableGrid.IsVisible)
            {
                m_currentViewableGrid.IsVisible = false;
                m_GridOrders.Children.Remove(m_currentViewableGrid);
            }
            var currentGrid = m_Orders[id].OderGrid;
            currentGrid.IsVisible = true;
            m_currentViewableGrid = currentGrid;
            m_GridOrders.Children.Add(currentGrid);
            m_LastGridInCell = vc.LogicalChildren[0] as Grid;
        }

        public ObservableCollection<OrderButtonModel> ButtonsInProgress
        {
            get => m_ButtonsInProgress;
            set => m_ButtonsInProgress = value;
        }

        public ObservableCollection<OrderButtonModel> ToMakeOrderIdCollection
        {
            get => m_ButtonsToMake;
            set => m_ButtonsToMake = value;
        }
        public ObservableCollection<OrderButtonModel> LockedCollection
        {
            get => m_ButtonsLocked;
            set => m_ButtonsLocked = value;
        }
        public OrderButtonModel LastSelectedOBM { get; set; } = new OrderButtonModel();

        public ViewCell LastTappedCell { get; set; } = new ViewCell();

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
            public OrderIDNotifier(int i_ID, int i_Status)
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
