using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerUI.Models;
using CheckerUI.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;


//This Class is responsible for scattering an order item to its correct cooker line,
//every time a new order enters the server it updates (currently)
//the todo list and from there in response we create
//a new order model and update the list of the correct line in a new order

namespace CheckerUI.Helpers.Order
{
    public class OrderItemViewManager : BaseViewModel
    {
        private static int m_TotalOrdersCounterID = 0;
        public Dictionary<int, OrderItemView> m_Orders { get; set; } = new Dictionary<int, OrderItemView>();

        private ObservableCollection<OrderItemView> m_HotLineOrders { get; set; } = new ObservableCollection<OrderItemView>();
        public OrderItemViewManager(ObservableCollection<OrderItemModel> i_Items)
        {
            convertItemsToItemsView(i_Items);
        }

        private void convertItemsToItemsView(ObservableCollection<OrderItemModel> i_Items)
        {
            foreach (var item in i_Items)
            {
                OrderItemView order = new OrderItemView(item.m_OrderItemName, item.m_ID_Status_Notifier);
                m_Orders.Add(item.m_ID_Status_Notifier.OrderID, order);
                m_HotLineOrders.Add(order);
            }
        }
    }
}
