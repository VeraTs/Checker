using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.Helpers.Dishes
{
    internal static class DishBuilder
    {
        public static DishModel GenerateDishItem(int i_Id, string i_Name, int i_LineID, string i_TimeReq, string i_TimeReqHigh, int i_outputScreen, string i_Des, eOrderItemType i_Type)
        {
            var m_Dish = new DishModel()
            {
                m_DishID = i_Id,
                m_DishName = i_Name,
                m_LineID = i_LineID,
                m_TimeReq = i_TimeReq,
                m_TimeReqHigh = i_TimeReqHigh,
                m_Output_screen = i_outputScreen,
                m_Description = i_Des,
                m_DishType = i_Type
            };
            return m_Dish;
        }
    }
}
