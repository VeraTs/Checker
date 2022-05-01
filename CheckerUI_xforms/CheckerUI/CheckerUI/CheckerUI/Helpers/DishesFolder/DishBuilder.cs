using System;
using System.Collections.Generic;
using System.Text;
using CheckerUI.Models;

namespace CheckerUI.Helpers
{
    internal static class DishBuilder
    {
        public static Dish_item GenerateDishItem(int i_Id, string i_Name, int i_LineID, string i_TimeReq, string i_TimeReqHigh, int i_outputScreen, string i_Des)
        {
            Dish_item m_Dish = new Dish_item()
            {
                m_DishID = i_Id,
                m_DishName = i_Name,
                m_LineID = i_LineID,
                m_TimeReq = i_TimeReq,
                m_TimeReqHigh = i_TimeReqHigh,
                m_Output_screen = i_outputScreen,
                m_Description = i_Des
            };
            return m_Dish;
        }
    }
}
