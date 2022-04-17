using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CheckerUI.Enums;
using CheckerUI.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace CheckerUI.Helpers.Order
{
    public class OrdersManager : BaseViewModel
    {
        //  private List<OrderModel> m_OrdersList = new List<OrderModel>();
        private ObservableCollection<OrderViewModel> m_OrdersViews = new ObservableCollection<OrderViewModel>();
        private readonly Dictionary<int, OrderViewModel> m_Orders = new Dictionary<int, OrderViewModel>();
        private int m_Length = 0;
        private int m_ItemsGenerated = 1;
        // private ObservableCollection<ToDo> m_ToDoList;

        private ObservableCollection<OrderItemView> m_OrderItemsLineView = new ObservableCollection<OrderItemView>();
        public OrdersManager()
        {
            //CreateSignal();
            //m_ToDoList = new ObservableCollection<ToDo>();
            //App.HubConn.On<String, DateTime>("ReceiveList", (desc, date) =>
            //{
            //    ToDo todo = new ToDo() { createdDate = date, description = desc };
            //    m_ToDoList.Add(todo);
            //});
            generateOrders(15);
            //  updateLines();
            //foreach (var item in m_OrdersList[0].m_Items)
            //{
            //    m_OrderItemModelList.Add(item);
            //}
            //m_ItemsViewManager = new OrderItemViewManager(m_OrderItemModelList);
        }

        public void UpdateLines()
        {
            var items = m_OrdersViews[0].Items;
            foreach (var item in items)
            {
                itemsLineView.Add(item);
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

        private void generateOrders(int i_Size)
        {
            for (int i = 0; i < i_Size; i++)
            {
                OrderModel model = generateOrderModel();
                OrderViewModel viewModel = new OrderViewModel(model);
                viewModel.AllItemsCheckedOrderID.CollectionChanged += AllItemsCheckedOnCollectionChanged;
                m_Orders.Add(model.m_OrderID, viewModel);
                m_OrdersViews.Add(viewModel);
            }
        }

        private void AllItemsCheckedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var observer = sender as ObservableCollection<int>;
            var id = observer[0];
            var viewToRemove = m_Orders[id];
            m_OrdersViews.Remove(viewToRemove);
        }

        private OrderModel generateOrderModel()
        {
            eOrderState[] allStates = { eOrderState.Pending, eOrderState.Started, eOrderState.Done };
            var items = generateOrderItemModels(m_Length + 100);
            var orderModel = OrderBuilder.GenerateOrder(m_Length, m_Length + 100
                , allStates[m_Length % 3], items);
            m_Length++;
            return orderModel;
        }
        private List<OrderItemModel> generateOrderItemModels(int i_TableNumber)
        {
            var items = new List<OrderItemModel>();
            string[] names = { "Bread", "Salad", "Fish Tartar", "Burger ", "Pizza", "Pasta", "Cake" };
            eOrderItemType[] types =
            {
                eOrderItemType.First, eOrderItemType.First, eOrderItemType.First, eOrderItemType.Main,
                eOrderItemType.Main, eOrderItemType.Unknown, eOrderItemType.Last
            };
            int[] deptId = { 1, 1, 1, 2, 2, 2, 3 };
            eOrderItemState[] itemStates =
            {
                eOrderItemState.Waiting, eOrderItemState.Available, eOrderItemState.InPreparation, eOrderItemState.Ready
            };
            for (int i = 0; i < types.Length; i++)
            {
                var orderItem =
                    OrderItemBuilder.GenerateOrderItem(m_ItemsGenerated, names[i], i_TableNumber,
                    "Notes : " + Environment.NewLine + "No Cheese, Medium " + Environment.NewLine + "etc",
                    deptId[i], eOrderItemType.First, itemStates[i % 3]);

                m_ItemsGenerated++;

                items.Add(orderItem);
            }
            return items;
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

        public ObservableCollection<OrderItemView> itemsLineView
        {
            get => m_OrderItemsLineView;
            set => m_OrderItemsLineView = value;
        }
    }
}
