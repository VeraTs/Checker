using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CheckerUI.Enums;
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

        public ObservableCollection<OrderItemView> m_Orders { get; set; }
        private Dictionary<int, OrderItemView> m_OrdersList { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsInProgress { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsLocked { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsToMake { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsDone { get; set; }

        private List<OrderItemView> m_DoneOrdersList { get; set; }

        private OrdersManager m_Manager;
        public Command FeelOrdersCommand { get; set; }
        public Command ReturnCommand { get; set; }



        public void init()
        {
            allocations();

            m_Orders.CollectionChanged += ordersCh_CollectionChanged;
            m_Manager = new OrdersManager(); // connect to OrderManger orders
            m_Manager.itemsLineView = m_Orders;
            m_Manager.UpdateLines();
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

            FeelOrdersCommand = new Command(refresh);
        }
        private void ordersCh_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (m_counterID < m_Orders.Count) // new order is entered
            {
                OrderItemView last = m_Orders.Last();
                addItemToLinePlace(last);
               // m_ButtonsInProgress.Add(last);
                last.OrderStatusChangedNotifier.CollectionChanged += OrderStatusChangedNotifierOnCollectionChanged;
                m_OrdersList.Add(last.OderID, last);
                m_counterID++;

            }
        }

        private void addItemToLinePlace(OrderItemView i_ToAdd)
        {
            switch (i_ToAdd.OrderStatus)
            {
                case eOrderItemState.Waiting:
                    {
                        m_ButtonsLocked.Add(i_ToAdd);
                        break;
                    }
                case eOrderItemState.Available:
                    {
                        m_ButtonsToMake.Add(i_ToAdd);
                        break;
                    }
                case eOrderItemState.InPreparation:
                    {
                        m_ButtonsInProgress.Add(i_ToAdd);
                        break;
                    }
                case eOrderItemState.Ready:
                    {
                        m_ButtonsDone.Add(i_ToAdd);
                        break;
                    }

            }
        }
        private void OrderStatusChangedNotifierOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var observer = sender as ObservableCollection<int>;

            var id = observer[0];
            var order = m_Orders[id];

            switch (order.OrderStatus)
            {
                case eOrderItemState.Waiting:
                    {
                        caseIsHolding(order);
                        break;
                    }
                case eOrderItemState.Available:
                    {
                        caseIsAvailable(order);
                        break;
                    }
                case eOrderItemState.InPreparation:
                    {
                        caseIsInProgress(order);
                        break;
                    }
                case eOrderItemState.Ready:
                    {
                        caseIsCompleted(order);
                        break;
                    }
                case eOrderItemState.Completed:
                    {
                        break;
                    }
                default:
                    {
                        caseIsRestored(order);
                        break;
                    }
            }
        }

        public void refresh()
        {

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


        public ObservableCollection<OrderItemView> InProgressItemsCollection
        {
            get => m_ButtonsInProgress;
            set => m_ButtonsInProgress = value;
        }
        public void ItemToMakeOnDoubleClicked(OrderItemView i_Item)
        {
            m_ButtonsToMake.Remove(i_Item);
            i_Item.OrderStatus = eOrderItemState.InPreparation;
            i_Item.OrderItemTimeStarted = DateTime.Now;
            m_ButtonsInProgress.Add(i_Item);
        }

        public void ItemInProgressOnDoubleClick(OrderItemView i_Item)
        {
            m_ButtonsInProgress.Remove(i_Item);
            i_Item.OrderStatus = eOrderItemState.Ready;
            i_Item.OrderItemTimeDone = DateTime.Now;
            i_Item.FirstTimeToShowString = i_Item.OrderItemTimeDoneString;
            m_ButtonsDone.Add(i_Item);
        }

        public void ItemReadyOnDoubleClick(OrderItemView i_Item)
        {
            m_ButtonsDone.Remove(i_Item);
            i_Item.OrderStatus = eOrderItemState.InPreparation;
            i_Item.OrderItemTimeDone = DateTime.MinValue;
            i_Item.FirstTimeToShowString = i_Item.OrderItemTimeCreate;
            m_ButtonsInProgress.Add(i_Item);
        }
        public ObservableCollection<OrderItemView> ToMakeItemsCollection
        {
            get => m_ButtonsToMake;
            set => m_ButtonsToMake = value;
        }

        public ObservableCollection<OrderItemView> DoneItemsCollection
        {
            get => m_ButtonsDone;
            set => m_ButtonsDone = value;
        }
        public ObservableCollection<OrderItemView> LockedItemsCollection
        {
            get => m_ButtonsLocked;
            set => m_ButtonsLocked = value;
        }
        public OrderItemView LastSelectedItem { get; set; } = new OrderItemView();

    }

   
}
