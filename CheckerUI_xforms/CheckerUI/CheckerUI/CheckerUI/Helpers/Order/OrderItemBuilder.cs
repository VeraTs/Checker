using System;
using System.Collections.Generic;
using System.Text;

namespace CheckerUI.Helpers.Order
{
    internal static class OrderItemBuilder
    {
        public static OrderItemModel GenerateOrderItem(int i_ID, string i_OrderItemName, int i_Status,int i_Table ,string i_Desc,
            int i_DeptID)
        {
            OrderItemModel m_Order = new OrderItemModel()
            {
                m_OrderItemID = i_ID,
                m_OrderItemName = i_OrderItemName,
                m_Status = i_Status,
                m_TableNumber = i_Table,
                m_DeptID = i_DeptID,
                m_Description = i_Desc,
                m_CreatedDate = DateTime.Now,
                m_StartDate = DateTime.MinValue
            };
            return m_Order;
        }
    }
}
