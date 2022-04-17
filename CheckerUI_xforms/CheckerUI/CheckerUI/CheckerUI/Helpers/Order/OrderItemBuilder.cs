using System;
using CheckerUI.Enums;
using CheckerUI.ViewModels;

namespace CheckerUI.Helpers.Order
{
    internal static class OrderItemBuilder
    {
        public static OrderItemModel GenerateOrderItem(int i_OrderID,string i_OrderItemName,int i_Table ,string i_Desc,
            int i_DeptID,eOrderItemType i_Type, eOrderItemState i_State)
        {

            OrderItemModel m_Order = new OrderItemModel()
            {
                m_OrdrID = i_OrderID,
                m_OrderItemName = i_OrderItemName,
                m_TableNumber = i_Table,
                m_DeptID = i_DeptID,
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
