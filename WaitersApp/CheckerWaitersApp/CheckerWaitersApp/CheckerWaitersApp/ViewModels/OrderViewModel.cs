using System.Collections.ObjectModel;
using System.Linq;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;


namespace CheckerWaitersApp.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public OrderModel Order { get; private set; }
        public string TableNumber { get; private set; }
        public string OrderID { get; private set; }
        public string OrderHeader { get; private set; }
        public eOrderState OrderState
        {
            get => Order.m_OrderState; set => Order.m_OrderState = value;
        }

        public eOrderType OrderType
        {
            get => Order.m_OrderType;
            set => Order.m_OrderType = value;
        }
        public string Counter => "Total Items :" + Order.m_Items.Count;
        public string CreatedTime => Order.m_CreatedDate.ToShortTimeString();

        public ObservableCollection<OrderItemModel> Items { get; private set; }

        public ObservableCollection<OrderItemViewModel> OrderItemsViews { get; private set; } =
            new ObservableCollection<OrderItemViewModel>();

        public OrderViewModel(OrderModel i_Model)
        {
            Order = new OrderModel();
            Order = i_Model;
            TableNumber = "(Table :" + i_Model.m_TableNumber.ToString() +")";
            OrderID = "Order #" + Order.m_OrderID.ToString() +"   ";
            OrderHeader = OrderID + TableNumber;
            OrderState = i_Model.m_OrderState;
            
            foreach (var card in i_Model.m_Items.Select(item => new OrderItemViewModel(item)))
            {
                OrderItemsViews.Add(card);
            }
        }
    }
}
