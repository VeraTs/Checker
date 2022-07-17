using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CheckerUI.Enums;
using CheckerUI.Helpers.OrdersHelpers;
using CheckerUI.Models;
using Microsoft.AspNetCore.SignalR.Client;
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

        public ObservableCollection<OrderItemView> m_OrderItemsViews { get; set; }
        private Dictionary<int, OrderItemView> m_OrderItemsViewsList { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsInProgress { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsLocked { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsToMake { get; set; }
        private ObservableCollection<OrderItemView> m_ButtonsDone { get; set; }

        public Command ReturnCommand { get; set; }

        public void init()
        {
            allocations();
            ReturnCommand = new Command(async () => { await Application.Current.MainPage.Navigation.PopAsync(); });
        }

        private void allocations()
        {
            m_OrderItemsViews = new ObservableCollection<OrderItemView>();
            m_OrderItemsViewsList = new Dictionary<int, OrderItemView>();
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
            m_OrderItemsViews.Clear();
            m_OrderItemsViewsList.Clear();

        }

       
        private bool CheckNewOrderItemByID(int i_id)
        {
            return !m_OrderItemsViewsList.ContainsKey(i_id);
        }

        public void AddOrderItemToInProgress(OrderItem i_ToAdd)
        {
            if (!CheckNewOrderItemByID(i_ToAdd.id)) return;
            var view = new OrderItemView(i_ToAdd);
            m_ButtonsInProgress.Add(view);
            m_OrderItemsViews.Add(view);
            m_OrderItemsViewsList.Add(i_ToAdd.id, view);
        }

        public void AddOrderItemToAvailable(OrderItem i_ToAdd)
        {
            if (!CheckNewOrderItemByID(i_ToAdd.id)) return;
            var view = new OrderItemView(i_ToAdd);
            m_ButtonsToMake.Add(view);
            m_OrderItemsViews.Add(view);
            m_OrderItemsViewsList.Add(i_ToAdd.id, view);
        }

        public void AddOrderItemToLocked(OrderItem i_ToAdd)
        {
            if (!CheckNewOrderItemByID(i_ToAdd.id)) return;
            var view = new OrderItemView(i_ToAdd);
            m_OrderItemsViews.Add(view);
            m_ButtonsLocked.Add(view);
            m_OrderItemsViewsList.Add(i_ToAdd.id, view);

        }

        public void AddOrderItemToDone(OrderItem i_ToAdd)
        {
            if (!CheckNewOrderItemByID(i_ToAdd.id)) return;
            var view = new OrderItemView(i_ToAdd);
            m_OrderItemsViews.Add(view);
            m_ButtonsDone.Add(view);
            m_OrderItemsViewsList.Add(i_ToAdd.id, view);
        }

        public void moveItemViewToRightView(int i_itemID)
        {
            var itemView = m_OrderItemsViewsList[i_itemID];
            switch (itemView.OrderItemLineStatus)
            {
                case eLineItemStatus.Locked:
                {
                    itemView.OrderItemLineStatus = eLineItemStatus.ToDo;
                    m_ButtonsToMake.Add(itemView);
                    m_ButtonsLocked.Remove(itemView);
                        break;
                }
                case eLineItemStatus.ToDo:
                {
                    itemView.OrderItemLineStatus = eLineItemStatus.Doing;
                    m_ButtonsToMake.Remove(itemView);
                    itemView.OrderItemTimeStarted = DateTime.Now;
                    m_ButtonsInProgress.Add(itemView);
                    break;
                }
                case eLineItemStatus.Doing:
                {
                    itemView.OrderItemLineStatus = eLineItemStatus.Done;
                    m_ButtonsDone.Add(itemView);
                    itemView.OrderItemTimeDone = DateTime.Now; 
                    itemView.FirstTimeToShowString = itemView.OrderItemTimeDoneString; 
                    m_ButtonsInProgress.Remove(itemView);
                    break;
                }
            }
        }

    //private async void OrderStatusChangedNotifierOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        var observer = sender as ObservableCollection<int>;

    //        var id = observer[0];
    //        var order = m_OrderItemsViews[id];

    //        switch (order.OrderStatus)
    //        {
    //            case eLineItemStatus.Locked:
    //                {
    //                    await caseIsHolding(order);
    //                    break;
    //                }
    //            case eLineItemStatus.ToDo:
    //                {
    //                    await caseIsAvailable(order);
    //                    break;
    //                }
    //            case eLineItemStatus.Doing:
    //                {
    //                    await caseIsInProgress(order);
    //                    break;
    //                }
    //            case eLineItemStatus.Done:
    //                {
    //                    await caseIsCompleted(order);
    //                    break;
    //                }

    //            default:
    //                {
    //                    await caseIsRestored(order);
    //                    break;
    //                }
    //        }
    //    }

      
        public ObservableCollection<OrderItemView> InProgressItemsCollection
        {
            get => m_ButtonsInProgress;
            set => m_ButtonsInProgress = value;
        }
        public async Task ItemToMakeOnDoubleClicked(OrderItemView i_Item)
        {
            try
            {
                await App.HubConn.InvokeAsync("MoveOrderItemToDoing", i_Item.OderItemID);
                
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception! moving to Doing", ex.Message, "OK");
            }
        }

        public async Task ItemInProgressOnDoubleClick(OrderItemView i_Item)
        {
            try
            {
                await App.HubConn.InvokeAsync("MoveOrderItemToDone", i_Item.OderItemID);
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception! moving to Done", ex.Message, "OK");
            }
        }

        public async Task ItemLockedOnDoubleClicked(OrderItemView i_Item)
        {
            m_ButtonsLocked.Remove(i_Item);
            i_Item.OrderItemLineStatus = eLineItemStatus.ToDo;
           
            m_ButtonsToMake.Add(i_Item);
        }

        public async Task ItemReadyOnDoubleClick(OrderItemView i_Item)
        {
            m_ButtonsDone.Remove(i_Item);
            i_Item.OrderItemLineStatus = eLineItemStatus.Doing;
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
