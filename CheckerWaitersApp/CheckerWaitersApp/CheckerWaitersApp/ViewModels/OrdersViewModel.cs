﻿using System.Collections.ObjectModel;
using System.Linq;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<OrderViewModel> views { get; set; }
        public string Details { get; private set; }

        private static int count = 0;

        public OrdersViewModel()
        {
            Orders = new ObservableCollection<Order>();
            views = new ObservableCollection<OrderViewModel>();
            Details = "Total Orders :" + views.Count;
            Orders.CollectionChanged += Orders_CollectionChanged;
        }

        private void Orders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (count < Orders.Count)
            {
                var view = new OrderViewModel(Orders.Last(), this);
                views.Add(view);
                count++;
            }
            else if (count > Orders.Count)
            {
                count--;
            }
        }

        public OrdersViewModel(ObservableCollection<Order> i_Orders)
        {
            Orders = new ObservableCollection<Order>();
            Orders = i_Orders;
        }
        
        public void AddNewOrder(Order i_Order)
        {
            Orders.Add(i_Order);
        }
        public void RemovePaidOrder(Order i_Order)
        {
            views.Remove(views.First(orderView => orderView.Order.id == i_Order.id));
            Orders.Remove(i_Order);
        }
        public void UpdatePartialPay(Order i_Order, float i_sum)
        {
            var ret = views.First(orderView => orderView.Order.id == i_Order.id).UpdatePayPartialForOrder(i_sum);
        }
    }
}