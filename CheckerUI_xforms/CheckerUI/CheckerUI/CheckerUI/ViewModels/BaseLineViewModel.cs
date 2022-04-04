using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CheckerUI.Helpers;
using CheckerUI.Helpers.Order;
using CheckerUI.Models;
using Xamarin.Forms;
//// <summary>
// Basic line, if we want to add features to it we will do it by inheritance
// The line works in front of the ui
// so it is important to note that there will probably be locks here
// and access to it should be in async
// The board is divided into three different lists as described
// Each list is an order model list button
// when you add an item to any list it is automatically updated in the panel
// In addition, this panel also has an order display with buttons,
// so by clicking on one of them (item status changes) and we must respond accordingly.
// There are pointers for the last user selection.
// in here we are getting ref of the ui layouts , those method need to become async !

// to do : adding / removing / updating
// any one of the lists should be a critical sections 


//// </summary>

namespace CheckerUI.ViewModels
{
    public class BaseLineViewModel : BaseViewModel
    {
        private static int m_counterID = 0;
        public ObservableCollection<OrderItemView> m_Orders { get; set; }
        private Dictionary<int, OrderItemView> m_OrdersList { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsInProgress { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsLocked { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsToMake { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsDone { get; set; }
        public Grid m_LastGridInCell { get; set; }
        private List<OrderItemView> m_DoneOrdersList { get; set; }
        private Grid m_currentViewableGrid { get; set; }
        private Grid m_GridOrders { get; set; }
       
        public Command FeelOrdersCommand { get; set; }
        public Command ReturnCommand { get; set; }

        public void init(Grid i_GridOrders)
        {
            allocations();

            m_GridOrders = i_GridOrders;
            
            OrderIDNotifier notifier = new OrderIDNotifier(-1, -1);
            var currentOrder = new OrderItemView("Dummy", notifier);
            m_currentViewableGrid = currentOrder.OderGrid;
            m_Orders.CollectionChanged += ordersCh_CollectionChanged;
            OrderManager om = new OrderManager(m_Orders); // connect to OrderManger orders

            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            //   for(int i = 0;i<3;i++) feel_layout(i);
        }

        private void allocations()
        {
            m_Orders = new ObservableCollection<OrderItemView>();
            m_OrdersList = new Dictionary<int, OrderItemView>();
            m_ButtonsInProgress = new ObservableCollection<OrderItemView>();
            m_ButtonsLocked = new ObservableCollection<OrderItemView>();
            m_ButtonsToMake = new ObservableCollection<OrderItemView>();
            m_ButtonsDone = new ObservableCollection<OrderItemView>();
            m_DoneOrdersList = new List<OrderItemView>();
            m_LastGridInCell = new Grid();
            m_currentViewableGrid = new Grid();
            m_GridOrders = new Grid();
            FeelOrdersCommand = new Command(refresh);
        }
        private void ordersCh_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (m_counterID < m_Orders.Count) // new order is entered
            {
                OrderItemView last = m_Orders.Last();
                m_ButtonsToMake.Add(last);
                last.OrderButton.Clicked += Button_Clicked;
                OrderIDNotifier notifier = last.OrderNotifier;
                notifier.PropertyChanged += OrderStatusNotifierPropertyChanged;
                m_OrdersList.Add(last.OderID, last);
                m_counterID++;
            }
        }
        private void refresh()
        {

        }
        private void OrderStatusNotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var m = sender as OrderIDNotifier;
            var order = getOrderByID(m.OrderID);
            m_currentViewableGrid.IsVisible = false;
            m_currentViewableGrid = order.OderGrid;
            m_LastGridInCell = LastTappedCell.LogicalChildren.Last() as Grid;

            if (order.isRestored)
            {
                caseIsRestored(order);
            }
            else if (order.IsAvailable)
            {
                caseIsAvailable(order);
            }
            else if (order.IsHolding)
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

        private OrderItemView getOrderByID(int i_ID) // expensive method !! 
        {
            OrderItemView res = null;
            foreach (var order in m_Orders)
            {
                if (order.OderID == i_ID)
                {
                    res = order;
                }
            }
            return res;
        }

        private void caseIsAvailable(OrderItemView i_Order)
        {
            if (m_ButtonsLocked.Contains(i_Order))
            {
                m_ButtonsToMake.Add(i_Order);
                m_ButtonsLocked.Remove(i_Order);
            }
            m_currentViewableGrid.BackgroundColor = Color.DarkOrange;
        }

        private void caseIsRestored(OrderItemView i_Order)
        {
            i_Order.isRestored = false;
            m_ButtonsInProgress.Add(i_Order);
            m_ButtonsDone.Remove(i_Order);
            m_DoneOrdersList.Remove(i_Order);
            m_currentViewableGrid.BackgroundColor = Color.DarkOrange;
        }
        private void caseIsHolding(OrderItemView i_Order)
        {
            m_currentViewableGrid.BackgroundColor = Color.Firebrick;
        }

        private void caseIsInProgress(OrderItemView i_Order)
        {
            if (!m_ButtonsInProgress.Contains(i_Order))
            {
                m_ButtonsInProgress.Add(i_Order);
            }
            m_ButtonsToMake.Remove(i_Order);
            m_currentViewableGrid.BackgroundColor = Color.YellowGreen;
        }

        private void caseIsCompleted(OrderItemView i_Order)
        {
            if (m_ButtonsInProgress.Contains(i_Order))
            {
                m_ButtonsInProgress.Remove(i_Order);
                m_ButtonsDone.Add(i_Order);
                m_currentViewableGrid.BackgroundColor = Color.Chartreuse;
                m_DoneOrdersList.Add(i_Order);
                m_counterID--;
                
            }
            else
            {

            }
        }
        public void Button_Clicked(object sender, EventArgs e)
        {
            var vc = sender as ViewCell;
            var id = LastSelectedItem.OderID;
            var currentGrid = m_OrdersList[id].OderGrid;

            if (currentGrid != m_currentViewableGrid)
            {
                if (m_currentViewableGrid.IsVisible)
                {
                    m_currentViewableGrid.IsVisible = false;
                    m_GridOrders.Children.Clear();
                }
                m_currentViewableGrid = currentGrid;
                m_GridOrders.Children.Add(currentGrid);
            }
            m_currentViewableGrid.IsVisible = true;
        }

        public ObservableCollection<OrderItemView> ButtonsInProgress
        {
            get => m_ButtonsInProgress;
            set => m_ButtonsInProgress = value;
        }

        public ObservableCollection<OrderItemView> ToMakeOrderIdCollection
        {
            get => m_ButtonsToMake;
            set => m_ButtonsToMake = value;
        }

        public ObservableCollection<OrderItemView> DoneOrdersCollection
        {
            get => m_ButtonsDone;
            set => m_ButtonsDone = value;
        }
        public ObservableCollection<OrderItemView> LockedCollection
        {
            get => m_ButtonsLocked;
            set => m_ButtonsLocked = value;
        }
        public OrderItemView LastSelectedItem { get; set; } = new OrderItemView();

        public ViewCell LastTappedCell { get; set; } = new ViewCell();
    }
}
