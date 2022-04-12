using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CheckerUI.Helpers;
using CheckerUI.Helpers.Order;
using CheckerUI.Models;
using CheckerUI.Views;
using Lottie.Forms;
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
        private static int m_tempCounter = 60;
        public ObservableCollection<OrderItemView> m_Orders { get; set; }
        private Dictionary<int, OrderItemView> m_OrdersList { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsInProgress { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsLocked { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsToMake { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsDone { get; set; }
      
        private List<OrderItemView> m_DoneOrdersList { get; set; }
        private Grid m_GridOrders { get; set; }
        private OrdersManager m_Manager;
        public Command FeelOrdersCommand { get; set; }
        public Command ReturnCommand { get; set; }



        public void init()
        {
            allocations();

            OrderIDNotifier notifier = new OrderIDNotifier(-1, -1);
            var currentOrder = new OrderItemView("Dummy", notifier);
            m_Orders.CollectionChanged += ordersCh_CollectionChanged;
            m_Manager  = new OrdersManager(); // connect to OrderManger orders

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
         
            m_GridOrders = new Grid();
            FeelOrdersCommand = new Command(refresh);
        }
        private void ordersCh_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (m_counterID < m_Orders.Count) // new order is entered
            {
                OrderItemView last = m_Orders.Last();
                m_ButtonsToMake.Add(last);
                //last.OrderButton.Clicked += Button_Clicked;
                OrderIDNotifier notifier = last.OrderNotifier;
                notifier.PropertyChanged += OrderStatusNotifierPropertyChanged;
                m_OrdersList.Add(last.OderID, last);
                m_counterID++;
                
            }
        }
        public void refresh()
        {
            
        }
        private void OrderStatusNotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var m = sender as OrderIDNotifier;
            var order = getOrderByID(m.OrderID);
          
            

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
        }

        private void caseIsRestored(OrderItemView i_Order)
        {
            i_Order.isRestored = false;
            m_ButtonsInProgress.Add(i_Order);
            m_ButtonsDone.Remove(i_Order);
            m_DoneOrdersList.Remove(i_Order);
        }
        private void caseIsHolding(OrderItemView i_Order)
        {
            
        }

        private void caseIsInProgress(OrderItemView i_Order)
        {
            if (!m_ButtonsInProgress.Contains(i_Order))
            {
                m_ButtonsInProgress.Add(i_Order);
            }
            m_ButtonsToMake.Remove(i_Order);
        }

        private void caseIsCompleted(OrderItemView i_Order)
        {
            if (m_ButtonsInProgress.Contains(i_Order))
            {
                m_ButtonsInProgress.Remove(i_Order);
                m_ButtonsDone.Add(i_Order);
                m_DoneOrdersList.Add(i_Order);
                m_counterID--;
                
            }
            else
            {

            }
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

        public void StartButton_Clicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Grid g = b.Parent as Grid;
            g.BackgroundColor = Color.White;
        }

    }
}
