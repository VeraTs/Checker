using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CheckerUI.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace CheckerUI.Helpers.OrdersHelpers
{
    public class OrdersManager : BaseViewModel
    {
        //  private List<OrderModel> m_OrdersList = new List<OrderModel>();
        private ObservableCollection<OrderViewModel> m_OrdersViews = new ObservableCollection<OrderViewModel>();
        private readonly Dictionary<int, OrderViewModel> m_Orders = new Dictionary<int, OrderViewModel>();

        // private ObservableCollection<ToDo> m_ToDoList;

        public OrdersManager()
        {
            //CreateSignal();
            //m_ToDoList = new ObservableCollection<ToDo>();
            //App.HubConn.On<String, DateTime>("ReceiveList", (desc, date) =>
            //{
            //    ToDo todo = new ToDo() { createdDate = date, description = desc };
            //    m_ToDoList.Add(todo);
            //});
           
            //  updateLines();
            //foreach (var item in m_OrdersList[0].m_Items)
            //{
            //    m_OrderItemModelList.Add(item);
            //}
            //m_ItemsViewManager = new OrderItemViewManager(m_OrderItemModelList);

            //var orders = App.Repository.Orders;
            //foreach (var order in orders)
            //{
            //    var view = new OrderViewModel(order);
            //    m_OrdersViews.Add(view);
            //    m_Orders.Add(order.id, view);
            //}
        }
        public void UpdateLines(ObservableCollection<OrderItemViewModel> i_OrderItems)
        {
            foreach (var item in i_OrderItems)
            {
                itemsLineView.Add(item);
            }
        }
        public void UpdateAllLinesByOrders()
        {
            foreach (var order in m_OrdersViews)
            {
                UpdateLines(order.Items);
            }
        }
        public async void CreateSignal()
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server
                    await App.HubConn.InvokeAsync("InitialToDos");  // invoke initial event - to get all current listings
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void AllItemsCheckedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var observer = sender as ObservableCollection<int>;
            var id = observer[0];
            var viewToRemove = m_Orders[id];
            m_OrdersViews.Remove(viewToRemove);
        }

        public ObservableCollection<OrderViewModel> OrdersViews
        {
            get => m_OrdersViews;
            set
            {
                m_OrdersViews = value;
                OnPropertyChanged(nameof(OrdersViews));
            }
        }
        public ObservableCollection<OrderItemViewModel> itemsLineView { get; set; } = new ObservableCollection<OrderItemViewModel>();
    }
}
