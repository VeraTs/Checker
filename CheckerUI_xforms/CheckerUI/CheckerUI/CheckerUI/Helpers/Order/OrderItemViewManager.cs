using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerUI.ViewModels;



//This Class is responsible for scattering an order item to its correct cooker line,
//every time a new order enters the server it updates (currently)
//the todo list and from there in response we create
//a new order model and update the list of the correct line in a new order

namespace CheckerUI.Helpers.Order
{
    public class OrderItemViewManager : BaseViewModel
    {
        public Dictionary<int, OrderItemView> m_Orders { get; set; } = new Dictionary<int, OrderItemView>();

        private ObservableCollection<OrderItemView> m_HotLineOrders { get; set; } = new ObservableCollection<OrderItemView>();
        public OrderItemViewManager(ObservableCollection<OrderItemView> i_Items)
        {
            convertItemsToItemsView(i_Items);
        }

        private void convertItemsToItemsView(ObservableCollection<OrderItemView> i_Items)
        {

            foreach (var item in i_Items)
            {
                m_HotLineOrders.Add(item);
            }
            //foreach (var item in i_Items)
            //{
            //    OrderItemView order = new OrderItemView( item);
            //    m_Orders.Add(item.m_ID_Status_Notifier.OrderID, order);
            //    m_HotLineOrders.Add(order);
            //}
        }
    }
}
