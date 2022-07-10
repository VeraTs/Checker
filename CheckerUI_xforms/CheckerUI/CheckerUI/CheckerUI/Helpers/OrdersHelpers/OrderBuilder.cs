using System.Collections.Generic;
using CheckerUI.Enums;
using CheckerUI.Models;

namespace CheckerUI.Helpers.OrdersHelpers
{
    public static class OrderBuilder
    {
        public static Order GenerateOrder(int i_Id, int i_TableNumber, eOrderStatus i_state, List<OrderItem> i_OrderItemsList)
        {
            var orderModel = new Order()
            {
               id = i_Id,
               table = i_TableNumber,
               status = i_state,
               orderType = eOrderType.FIFO,
            };
            orderModel.items.AddRange(i_OrderItemsList);
            return orderModel;
        }
    }
}
