using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CheckerUI.Enums;
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
        public Dictionary<int, OrderItemViewModel> m_OrderItemsViewsList { get; set; }
        private ObservableCollection<OrderItemViewModel> m_ButtonsInProgress { get; set; }
        private ObservableCollection<OrderItemViewModel> m_ButtonsLocked { get; set; }
        private ObservableCollection<OrderItemViewModel> m_ButtonsToMake { get; set; }
        private ObservableCollection<OrderItemViewModel> m_ButtonsDone { get; set; }

        public Command ReturnCommand { get; set; }

        public void init()
        {
            allocations();
            ReturnCommand = new Command(() => { Application.Current.MainPage.Navigation.PopAsync(); });
        }

        private void allocations()
        {
            m_OrderItemsViewsList = new Dictionary<int, OrderItemViewModel>();
            m_ButtonsInProgress = new ObservableCollection<OrderItemViewModel>();
            m_ButtonsLocked = new ObservableCollection<OrderItemViewModel>();
            m_ButtonsToMake = new ObservableCollection<OrderItemViewModel>();
            m_ButtonsDone = new ObservableCollection<OrderItemViewModel>();
        }

        public void deAllocations()
        {
            m_ButtonsInProgress.Clear();
            m_ButtonsDone.Clear();
            m_ButtonsLocked.Clear();
            m_ButtonsToMake.Clear();
            m_OrderItemsViewsList.Clear();
        }

        private bool CheckNewOrderItemByID(int i_id)
        {
            return !m_OrderItemsViewsList.ContainsKey(i_id);
        }

        public void AddOrderItemToInProgress(OrderItem i_ToAdd)
        {
            if (!CheckNewOrderItemByID(i_ToAdd.id))
            {
                var view = m_OrderItemsViewsList[i_ToAdd.id];
               //view.OrderItemLineStatus = eLineItemStatus.Doing;
                view.OrderItemTimeStarted = DateTime.Now;
                if (m_ButtonsToMake.Remove(view))
                    m_ButtonsInProgress.Add(view);
            }
            else
            {
               // i_ToAdd.lineStatus = eLineItemStatus.Doing;
                var view = new OrderItemViewModel(i_ToAdd);
                if (!m_ButtonsInProgress.Contains(view))
                    m_ButtonsInProgress.Add(view);
                m_OrderItemsViewsList.Add(i_ToAdd.id, view);
            }
        }

        public void AddOrderItemToAvailable(OrderItem i_ToAdd)
        {
            if (!CheckNewOrderItemByID(i_ToAdd.id))
            {
                var view = m_OrderItemsViewsList[i_ToAdd.id];
               // view.OrderItemLineStatus = eLineItemStatus.ToDo;
                if (m_ButtonsLocked.Remove(view))
                    m_ButtonsToMake.Add(view);
            }
            else
            {
               // i_ToAdd.lineStatus = eLineItemStatus.ToDo;
                var view = new OrderItemViewModel(i_ToAdd);
                if (!m_ButtonsToMake.Contains(view))
                    m_ButtonsToMake.Add(view);
                m_OrderItemsViewsList.Add(i_ToAdd.id, view);
            }
        }

        public void AddOrderItemToLocked(OrderItem i_ToAdd)
        {
            //i_ToAdd.lineStatus = eLineItemStatus.Locked;
            if (!CheckNewOrderItemByID(i_ToAdd.id)) return;
            var view = new OrderItemViewModel(i_ToAdd);
            if (!m_OrderItemsViewsList.ContainsKey(i_ToAdd.id) && !m_ButtonsLocked.Contains(view))
            {
                m_ButtonsLocked.Add(view);
                m_OrderItemsViewsList.Add(i_ToAdd.id, view);
            }
        }

        public void AddOrderItemToDone(OrderItem i_ToAdd)
        {
            if (!CheckNewOrderItemByID(i_ToAdd.id))
            {
                var view = m_OrderItemsViewsList[i_ToAdd.id];
             //   view.OrderItemLineStatus = eLineItemStatus.Done;
                view.OrderItemTimeDone = DateTime.Now;
                if (m_ButtonsInProgress.Remove(view))
                    m_ButtonsDone.Add(view);
            }
            else
            {
             //   i_ToAdd.lineStatus = eLineItemStatus.Done;
                var view = new OrderItemViewModel(i_ToAdd);
                if (!m_ButtonsDone.Contains(view))
                    m_ButtonsDone.Add(view);
                m_OrderItemsViewsList.Add(i_ToAdd.id, view);
            }
        }

        public void moveItemViewToRightView(OrderItem i_item)
        {
            switch (i_item.lineStatus)
            {
                case eLineItemStatus.Locked:
                    {
                        AddOrderItemToLocked(i_item);
                        break;
                    }
                case eLineItemStatus.ToDo:
                    {
                        AddOrderItemToAvailable(i_item);
                        break;
                    }
                case eLineItemStatus.Doing:
                    {
                        AddOrderItemToInProgress(i_item);

                        break;
                    }
                case eLineItemStatus.Done:
                {

                    AddOrderItemToDone(i_item);
                        break;
                }
            }
        }
        public ObservableCollection<OrderItemViewModel> InProgressItemsCollection
        {
            get => m_ButtonsInProgress;
            set => m_ButtonsInProgress = value;
        }
        public async Task ItemToMakeOnDoubleClicked(OrderItemViewModel i_Item)
        {
          //  i_Item.OrderItemTimeStarted = DateTime.Now;
          //  i_Item.OrderItemLineStatus = eLineItemStatus.ToDo;
            try
            {
                await App.HubConn.InvokeAsync("MoveOrderItemToDoing", i_Item.OderItemID);

            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception! moving to Doing", ex.Message, "OK");
            }
        }

        public async Task ItemInProgressOnDoubleClick(OrderItemViewModel i_Item)
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

        public async Task ItemLockedOnDoubleClicked(OrderItemViewModel i_Item)
        {
            // m_ButtonsLocked.Remove(i_Item);
           // i_Item.OrderItemLineStatus = eLineItemStatus.ToDo;

            // m_ButtonsToMake.Add(i_Item);
        }

        public async Task ItemReadyOnDoubleClick(OrderItemViewModel i_Item)
        {
            //m_ButtonsDone.Remove(i_Item);
            //i_Item.OrderItemLineStatus = eLineItemStatus.Doing;
            //i_Item.OrderItemTimeDone = DateTime.MinValue;
            //i_Item.FirstTimeToShowString = i_Item.OrderItemTimeCreate;
            //m_ButtonsInProgress.Add(i_Item);
        }
        public ObservableCollection<OrderItemViewModel> ToMakeItemsCollection
        {
            get => m_ButtonsToMake;
            set => m_ButtonsToMake = value;
        }

        public ObservableCollection<OrderItemViewModel> DoneItemsCollection
        {
            get => m_ButtonsDone;
            set => m_ButtonsDone = value;
        }
        public ObservableCollection<OrderItemViewModel> LockedItemsCollection
        {
            get => m_ButtonsLocked;
            set => m_ButtonsLocked = value;
        }

        public string LockedCapacity => "Capacity :" + m_ButtonsLocked.Count;
        public string ToDoCapacity => "Capacity :" + m_ButtonsToMake.Count;
        public string DoingCapacity => "Capacity :" + m_ButtonsInProgress.Count;
        public string ReadyCapacity => "Capacity :" + m_ButtonsDone.Count;
        public OrderItemViewModel LastSelectedItem { get; set; }
    }
}
