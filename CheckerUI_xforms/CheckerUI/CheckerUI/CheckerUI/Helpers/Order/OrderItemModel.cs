using System;
using CheckerUI.Enums;

namespace CheckerUI.Helpers.Order
{
    // an order model combined with a listener on status (ID,Status)
    public class OrderItemModel
    {
        public int m_OrdrID { get; set; }
        public string m_OrderItemName { get; set; }
        public string m_Description { get; set; }
        public int m_LineID { get; set; }
        public int m_TableNumber { get; set; }
        public DateTime m_StartDate { get; set; }
        public DateTime m_CreatedDate { get; set; }
        public DateTime m_DoneDate { get; set; }
        public eOrderItemType m_ItemType { get; set; }
        public eOrderItemState m_State { get; set; }
       
        public override string ToString()
        {
           return  m_OrderItemName;
        }
    }
}
