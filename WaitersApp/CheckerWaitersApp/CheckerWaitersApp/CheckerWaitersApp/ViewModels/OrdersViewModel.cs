using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        public ObservableCollection<OrderModel> Orders { get;  set; }
        public ObservableCollection<OrderViewModel> views { get; set; }
       

        private static int count = 0;

        public OrdersViewModel()
        {
              Orders = new ObservableCollection<OrderModel>();
              views = new ObservableCollection<OrderViewModel>();
              Details = "Total Orders :" + views.Count;
        }
        public OrdersViewModel(ObservableCollection<OrderModel> i_Orders)
        {
            Orders = new ObservableCollection<OrderModel>();
            Orders = i_Orders;
        }
        public string Details { get; private set; }
        public void AddNewOrder(OrderModel i_Order)
        {
            Orders.Add(i_Order);
            var view = new OrderViewModel(i_Order);
            views.Add(view);
        }
        public void RemoveOrder(OrderModel i_Order)
        {
            Orders.Remove(i_Order);
        }

        public void generateOrders() // can be deleted someday
        {
            var item1 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Burger",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Main
            };


            var item2 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Pizza",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Main
            };

            var item3 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Tartar",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Starter
            };
          
            var list1 = new List<OrderItemModel>
            {
                item1,
                item2,
                item3
            };

            var order1 = new OrderModel
            {
                m_CreatedDate = DateTime.Now,
                m_OrderState = eOrderState.Pending,
                m_OrderType = eOrderType.AllTogether,
                m_TableNumber = count,
                m_OrderID = count++,
                m_Items = list1
            };


            var item4 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Bread",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Starter
            };


            var item5 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Cake",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Dessert
            };

            var item6 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Tartar",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Starter
            };

            var list2 = new List<OrderItemModel>
            {
                item1,
                item2,
                item3
            };

            var order2 = new OrderModel
            {
                m_CreatedDate = DateTime.Now,
                m_OrderState = eOrderState.Pending,
                m_OrderType = eOrderType.AllTogether,
                m_TableNumber = count,
                m_OrderID = count++,
                m_Items = list2
            };


            var item10 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Burger",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Main
            };
            var item11 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Cake",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Dessert
            };

            var item12 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Tartar",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Starter
            };
            var list3 = new List<OrderItemModel>
            {
                item1,
                item2,
                item3
            };
            var order3 = new OrderModel
            {
                m_CreatedDate = DateTime.Now,
                m_OrderState = eOrderState.Pending,
                m_OrderType = eOrderType.AllTogether,
                m_TableNumber = count,
                m_OrderID = count++,
                m_Items = list3
            };
            var item13 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Cake",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Dessert
            };

            var item14 = new OrderItemModel
            {
                m_TableNumber = count,
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = count,
                m_Description = "None",
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = "Tartar",
                m_LineID = 1,
                m_ItemType = eOrderItemType.Starter
            };
            var list4 = new List<OrderItemModel>
            {
                item12,
                item13,
                item14
            };
            var order4 = new OrderModel
            {
                m_CreatedDate = DateTime.Now,
                m_OrderState = eOrderState.Pending,
                m_OrderType = eOrderType.AllTogether,
                m_TableNumber = count,
                m_OrderID = count++,
                m_Items = list4
            };
            AddNewOrder(order1);
            AddNewOrder(order2);
            AddNewOrder(order3);
            AddNewOrder(order4);
        }
    }
}