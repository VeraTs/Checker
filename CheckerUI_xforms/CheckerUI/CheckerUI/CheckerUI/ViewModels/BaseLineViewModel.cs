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
//// </summary>

namespace CheckerUI.ViewModels
{
    public class BaseLineViewModel : BaseViewModel
    {
        private static int m_counterID = 0;
        
        private Button buttonToMake;
        public Command FeelOrdersCommand;
        public Command ReturnCommand;
        ObservableCollection<OrderItemView> m_Orders { get; set; } = new ObservableCollection<OrderItemView>();
        
        public ObservableCollection<OrderButtonModel> m_ButtonsInProgress = new ObservableCollection<OrderButtonModel>();
        public ObservableCollection<OrderButtonModel> m_ButtonsLocked = new ObservableCollection<OrderButtonModel>();
        public ObservableCollection<OrderButtonModel> m_ButtonsToMake = new ObservableCollection<OrderButtonModel>();
        
        public Grid m_LastGridInCell = new Grid();
        private List<OrderItemView> m_DoneOrdersList { get; set; } = new List<OrderItemView>();
        private Grid m_currentViewableGrid = null;
        private Grid m_GridOrders;
        // in here we are getting ref of the ui layouts , those method need to become async !

        public void init(Grid i_GridOrders)
        {
            m_GridOrders = new Grid();
            m_GridOrders = i_GridOrders;
            FeelOrdersCommand = new Command(refresh);
            OrderIDNotifier m = new OrderIDNotifier(-1, -1);
            var mCurrentOrder = new OrderItemView("Dummy", null, m);
            m_currentViewableGrid = mCurrentOrder.OderGrid;
            m_Orders.CollectionChanged += ordersCh_CollectionChanged;
            OrderManager om = new OrderManager(m_Orders);

            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        private void ordersCh_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (m_counterID < m_Orders.Count) // new order is entered
            {
                OrderItemView last = m_Orders.Last();
                OrderButtonModel obm = last.OrderButton;
                m_ButtonsToMake.Add(obm);
                obm.m_OrderButton.Clicked += Button_Clicked;
                OrderIDNotifier notifier = last.OrderNotifier;
                notifier.PropertyChanged += OrderIDNotifierPropertyChanged;
                m_counterID++;
            }
        }

        private void refresh()
        {

        }
        private void OrderIDNotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var m = sender as OrderIDNotifier;
            var order = getOrderByID(m.OrderID);
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
            if (m_ButtonsLocked.Contains(i_Order.OrderButton))
            {
                m_ButtonsToMake.Add(i_Order.OrderButton);
                m_ButtonsLocked.Remove(i_Order.OrderButton);
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
            if (!m_ButtonsInProgress.Contains(i_Order.OrderButton))
            {
                m_ButtonsInProgress.Add(i_Order.OrderButton);
            }
            m_ButtonsToMake.Remove(i_Order.OrderButton);
            m_currentViewableGrid.BackgroundColor = Color.YellowGreen;
        }

        private void caseIsCompleted(OrderItemView i_Order)
        {
            if (m_ButtonsInProgress.Contains(i_Order.OrderButton))
            {
                m_ButtonsInProgress.Remove(i_Order.OrderButton);
                m_currentViewableGrid.BackgroundColor = Color.ForestGreen;
                // done list
            }
            else
            {

            }
        }

        public Button generateButton(int i_ID)
        {
            buttonToMake = new Button()
            {
                Padding = new Thickness(20, 20, 20, 20),
                Text = i_ID.ToString(),
                CornerRadius = 10,
                BackgroundColor = Color.Gold,
                IsVisible = true,
            };
            buttonToMake.Clicked += Button_Clicked; // keep it !
            return buttonToMake;
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
       
    }
}
