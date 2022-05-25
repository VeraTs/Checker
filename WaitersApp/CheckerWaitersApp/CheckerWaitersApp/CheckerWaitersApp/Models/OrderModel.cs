using System;
using System.Collections.Generic;
using CheckerWaitersApp.Enums;

namespace CheckerWaitersApp.Models
{
    public class OrderModel
    {
        public int m_OrderID { get; set; }
        public int m_TableNumber { get; set; }
        public eOrderState m_OrderState { get; set; }
        public eOrderType m_OrderType { get; set; }
        public DateTime m_CreatedDate { get; set; }
        public List<OrderItemModel> m_Items { get; set; }
    }
}
