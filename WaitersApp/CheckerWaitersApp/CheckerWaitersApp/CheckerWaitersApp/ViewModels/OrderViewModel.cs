using System.Collections.ObjectModel;
using System.Linq;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;


namespace CheckerWaitersApp.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public Order Order { get; private set; }
        public string TableNumber { get; private set; }
        public string OrderID { get; private set; }
        public string OrderHeader { get; private set; }
        public eOrderStatus OrderState
        {
            get => Order.status; set => Order.status = value;
        }

        public eOrderType OrderType
        {
            get => Order.orderType;
            set => Order.orderType = value;
        }
        public string Counter => "Total Items :" + Order.items.Count;
        public string Cost => "Price :" + Order.totalCost;
        public string CreatedTime => Order.createdDate.ToShortTimeString();

        public ObservableCollection<OrderItem> Items { get; private set; }

        public ObservableCollection<OrderItemViewModel> OrderItemsViews { get; private set; } =
            new ObservableCollection<OrderItemViewModel>();

        public OrderViewModel(Order i_Model)
        {
            Order = new Order();
            Order = i_Model;
            TableNumber = "(Table :" + i_Model.table.ToString() +")";
            OrderID = "Order #" + Order.id.ToString() +"   ";
            OrderHeader = OrderID + TableNumber;
            OrderState = i_Model.status;
            
            foreach (var card in i_Model.items.Select(item => new OrderItemViewModel(item)))
            {
                OrderItemsViews.Add(card);
            }
        }
    }
}
