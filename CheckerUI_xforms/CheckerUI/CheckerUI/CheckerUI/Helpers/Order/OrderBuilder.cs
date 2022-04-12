using System;
using System.Collections.Generic;
using System.Text;
using CheckerUI.Enums;

namespace CheckerUI.Helpers.Order
{
    public static class OrderBuilder
    {
        public static OrderModel GenerateOrder(int i_Id, int i_TableNumber, eOrderState i_state, List<OrderItemModel> i_OrderItemsList)
        {
            var orderModel = new OrderModel()
            {
                m_OrderID = i_Id,
                m_TableNumber = i_TableNumber,
                m_OrderState = i_state,
                m_OrderType = eOrderType.Unknown,
                m_Items = new List<OrderItemModel>()
            };
            orderModel.m_Items = i_OrderItemsList;
            return orderModel;
        }
    }
}
