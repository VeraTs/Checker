using System;
using CheckerUI.Enums;
using CheckerUI.Models;

namespace CheckerUI.Helpers.OrdersHelpers
{
    internal static class OrderItemBuilder
    {
        public static OrderItem GenerateOrderItem(int i_OrderID,Dish dish ,int i_Table,eOrderItemType i_Type, eItemStatus i_State, eLineItemStatus i_LineStatus)
        {

            OrderItem m_Order = new OrderItem()
            {
                id = i_OrderID,
               
                table = i_Table,
                status = i_State,
                lineStatus =  i_LineStatus
            };
            return m_Order;
        }
    }
}
