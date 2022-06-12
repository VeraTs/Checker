using System;
using CheckerWaitersApp.Enums;

namespace CheckerWaitersApp.Models
{
    public class OrderItemModel
    {
        public int m_OrderID { get; set; }
        public string m_OrderItemName { get; set; }
        public string m_Note { get; set; }
        public int m_LineID { get; set; }
        public int m_TableNumber { get; set; }
        public DateTime m_StartDate { get; set; }
        public DateTime m_CreatedDate { get; set; }
        public DateTime m_DoneDate { get; set; }
        public eOrderItemType m_ItemType { get; set; }
        public eOrderItemState m_State { get; set; }
    }
}
