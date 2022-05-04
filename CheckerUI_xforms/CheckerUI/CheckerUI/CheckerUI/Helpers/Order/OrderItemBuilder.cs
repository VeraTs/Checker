using System;
using CheckerUI.Enums;

namespace CheckerUI.Helpers.Order
{
    internal static class OrderItemBuilder
    {
        public static OrderItemModel GenerateOrderItem(int i_OrderID,string i_OrderItemName,int i_Table ,string i_Desc,
            int i_LineID,eOrderItemType i_Type, eOrderItemState i_State)
        {
            var m_Order = new OrderItemModel()
            {
                m_OrderID = i_OrderID,
                m_OrderItemName = i_OrderItemName,
                m_TableNumber = i_Table,
                m_LineID = i_LineID,
                m_Description = i_Desc,
                m_CreatedDate = DateTime.Now,
                m_StartDate = DateTime.Now,
                m_ItemType = i_Type,
                m_State = i_State,
                m_DoneDate = new DateTime(0)
            };
            return m_Order;
        }
        

    }
}
