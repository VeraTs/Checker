using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CheckerUI.Enums;
using CheckerUI.Helpers.OrdersHelpers;
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

// update :
//Following the changes made:

//Now the whole UI is completely separated from the logic
//The entire main window is displayed in four parts of the stack layout
//Each part consists of a card that displays items. Each item is a card that displays an order item
//When clicked, the item is highlighted
//With a double click the item moves to the next window
//Updated according to the hours

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

       // private OrdersManager m_Manager;
        public Command ReturnCommand { get; set; }

        public void init()
        {
            allocations();

            m_Orders.CollectionChanged += ordersCh_CollectionChanged;
           
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        private void allocations()
        {
            m_Orders = new ObservableCollection<OrderItemView>();
            m_OrdersList = new Dictionary<int, OrderItemView>();
            m_ButtonsInProgress = new ObservableCollection<OrderItemView>();
            m_ButtonsLocked = new ObservableCollection<OrderItemView>();
            m_ButtonsToMake = new ObservableCollection<OrderItemView>();
            m_ButtonsDone = new ObservableCollection<OrderItemView>();
        }
        public void deAllocations()
        {
            m_ButtonsInProgress.Clear();
            m_ButtonsDone.Clear();
            m_ButtonsLocked.Clear();
            m_ButtonsToMake.Clear();
            m_Orders.Clear();
            m_OrdersList.Clear();
        }
        private void ordersCh_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (m_counterID < m_Orders.Count) // new order is entered
            {
                OrderItemView last = m_Orders.Last();
                addItemToLinePlace(last);
                last.OrderStatusChangedNotifier.CollectionChanged += OrderStatusChangedNotifierOnCollectionChanged;
                m_OrdersList.Add(last.OderID, last);
                m_counterID++;
            }
        }
      
        public void AddOrderItem(OrderItem i_ToAdd)
        {
            var view = new OrderItemView(i_ToAdd);
            addItemToLinePlace(view);
        }
        public void addItemToLinePlace(OrderItemView i_ToAdd)
        {
            switch (i_ToAdd.OrderStatus)
            {
                case eLineItemStatus.Locked:
                    {
                        m_ButtonsLocked.Add(i_ToAdd);
                        break;
                    }
                case eLineItemStatus.ToDo:
                    {
                        m_ButtonsToMake.Add(i_ToAdd);
                        break;
                    }
                case eLineItemStatus.Doing:
                    {
                        m_ButtonsInProgress.Add(i_ToAdd);
                        break;
                    }
                case eLineItemStatus.Done:
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
                case eLineItemStatus.Locked:
                    {
                        caseIsHolding(order);
                        break;
                    }
                case eLineItemStatus.ToDo:
                    {
                        caseIsAvailable(order);
                        break;
                    }
                case eLineItemStatus.Doing:
                    {
                        caseIsInProgress(order);
                        break;
                    }
                case eLineItemStatus.Done:
                    {
                        caseIsCompleted(order);
                        break;
                    }

                default:
                    {
                        caseIsRestored(order);
                        break;
                    }
            }
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
            i_Item.OrderStatus = eLineItemStatus.Doing;
            i_Item.OrderItemTimeStarted = DateTime.Now;
            m_ButtonsInProgress.Add(i_Item);
        }

        public void ItemInProgressOnDoubleClick(OrderItemView i_Item)
        {
            m_ButtonsInProgress.Remove(i_Item);
            i_Item.OrderStatus = eLineItemStatus.Done;
            i_Item.OrderItemTimeDone = DateTime.Now;
            i_Item.FirstTimeToShowString = i_Item.OrderItemTimeDoneString;
            m_ButtonsDone.Add(i_Item);
        }
        public async void ItemLockedOnDoubleClicked(OrderItemView i_Item)
        {
            m_ButtonsLocked.Remove(i_Item);
            i_Item.OrderStatus = eLineItemStatus.ToDo;
           
            m_ButtonsToMake.Add(i_Item);
        }

        public void ItemReadyOnDoubleClick(OrderItemView i_Item)
        {
            m_ButtonsDone.Remove(i_Item);
            i_Item.OrderStatus = eLineItemStatus.Doing;
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
