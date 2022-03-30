using System;
using CheckerUI.ViewModels;

namespace CheckerUI.Helpers.Order
{
    internal static class OrderItemBuilder
    {
        public static OrderItemModel GenerateOrderItem(string i_OrderItemName,int i_Table ,string i_Desc,
            int i_DeptID, OrderIDNotifier i_Notifier)
        {
            OrderItemModel m_Order = new OrderItemModel()
            {
                m_OrderItemName = i_OrderItemName,
                m_TableNumber = i_Table,
                m_DeptID = i_DeptID,
                m_Description = i_Desc,
                m_CreatedDate = DateTime.Now,
                m_StartDate = DateTime.MinValue,
                m_ID_Status_Notifier = i_Notifier
            };
            return m_Order;
        }
        
    }
}
