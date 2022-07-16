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

        
    }
}