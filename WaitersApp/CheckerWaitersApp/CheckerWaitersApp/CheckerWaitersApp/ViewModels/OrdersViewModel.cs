using System;
using System.Collections.Generic;

using System.Collections.ObjectModel;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;
using CheckerWaitersApp.ViewModels;


namespace CheckerWaitersApp.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        public ObservableCollection<Order> Orders { get;  set; }
        public ObservableCollection<OrderViewModel> views { get; set; }
       

        private static int count = 0;

        public OrdersViewModel()
        {
              Orders = new ObservableCollection<Order>();
              views = new ObservableCollection<OrderViewModel>();
              Details = "Total Orders :" + views.Count;
        }
        public OrdersViewModel(ObservableCollection<Order> i_Orders)
        {
            Orders = new ObservableCollection<Order>();
            Orders = i_Orders;
        }
        public string Details { get; private set; }
        public void AddNewOrder(Order i_Order)
        {
            Orders.Add(i_Order);
            var view = new OrderViewModel(i_Order);
            views.Add(view);
        }
        public void RemoveOrder(Order i_Order)
        {
            Orders.Remove(i_Order);
        }

        //public void generateOrders() // can be deleted someday
        //{
        //    var item1 = new OrderItem
        //    {
        //        Table = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderId = count,
        //        Changes = "None",
        //        LineStatus = eLineItemStatus.Locked, 
        //        Name = "Burger",
        //        LineID = 1,
        //        ItemType = eDishType.Main
        //    };


        //    var item2 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Pizza",
        //        LineID = 1,
        //        ItemType = eDishType.Main
        //    };

        //    var item3 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Tartar",
        //        LineID = 1,
        //        ItemType = eDishType.Starter
        //    };

        //    var list1 = new List<OrderItem>
        //    {
        //        item1,
        //        item2,
        //        item3
        //    };

        //    var order1 = new Order
        //    {
        //        CreatedDate = DateTime.Now,
        //        OrderState = eOrderState.Pending,
        //        OrderType = eOrderType.AllTogether,
        //        TableNumber = count,
        //        OrderID = count++,
        //        Items = list1
        //    };


        //    var item4 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Bread",
        //        LineID = 1,
        //        ItemType = eDishType.Starter
        //    };


        //    var item5 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Cake",
        //        LineID = 1,
        //        ItemType = eDishType.Dessert
        //    };

        //    var item6 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Tartar",
        //        LineID = 1,
        //        ItemType = eDishType.Starter
        //    };

        //    var list2 = new List<OrderItem>
        //    {
        //        item1,
        //        item2,
        //        item3
        //    };

        //    var order2 = new Order
        //    {
        //        CreatedDate = DateTime.Now,
        //        OrderState = eOrderState.Pending,
        //        OrderType = eOrderType.AllTogether,
        //        TableNumber = count,
        //        OrderID = count++,
        //        Items = list2
        //    };


        //    var item10 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Burger",
        //        LineID = 1,
        //        ItemType = eDishType.Main
        //    };
        //    var item11 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Cake",
        //        LineID = 1,
        //        ItemType = eDishType.Dessert
        //    };

        //    var item12 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Tartar",
        //        LineID = 1,
        //        ItemType = eDishType.Starter
        //    };
        //    var list3 = new List<OrderItem>
        //    {
        //        item1,
        //        item2,
        //        item3
        //    };
        //    var order3 = new Order
        //    {
        //        CreatedDate = DateTime.Now,
        //        OrderState = eOrderState.Pending,
        //        OrderType = eOrderType.AllTogether,
        //        TableNumber = count,
        //        OrderID = count++,
        //        Items = list3
        //    };
        //    var item13 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Cake",
        //        LineID = 1,
        //        ItemType = eDishType.Dessert
        //    };

        //    var item14 = new OrderItem
        //    {
        //        TableNumber = count,
        //        CreatedDate = DateTime.Now,
        //        DoneDate = DateTime.MinValue,
        //        OrderID = count,
        //        Note = "None",
        //        State = eOrderItemState.Waiting,
        //        OrderItemName = "Tartar",
        //        LineID = 1,
        //        ItemType = eDishType.Starter
        //    };
        //    var list4 = new List<OrderItem>
        //    {
        //        item12,
        //        item13,
        //        item14
        //    };
        //    var order4 = new Order
        //    {
        //        CreatedDate = DateTime.Now,
        //        OrderState = eOrderState.Pending,
        //        OrderType = eOrderType.AllTogether,
        //        TableNumber = count,
        //        OrderID = count++,
        //        Items = list4
        //    };
        //    AddNewOrder(order1);
        //    AddNewOrder(order2);
        //    AddNewOrder(order3);
        //    AddNewOrder(order4);
        //}
    }
}