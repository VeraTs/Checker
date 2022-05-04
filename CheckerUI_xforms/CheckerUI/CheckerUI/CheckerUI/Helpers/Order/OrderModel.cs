using System.Collections.Generic;
using CheckerUI.Enums;

namespace CheckerUI.Helpers.Order
{
    public class OrderModel
    {
        public int m_OrderID { get; set; }
        public int m_TableNumber { get; set; }
        public eOrderState m_OrderState { get; set; }
        public eOrderType m_OrderType { get; set; }
        public List<OrderItemModel> m_Items { get; set; }
    }
}
