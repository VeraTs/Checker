using CheckerUI.Enums;
using CheckerUI.Models;

namespace CheckerUI.Helpers.DishesFolder
{
    internal static class DishBuilder
    {
        public static Dish GenerateDishItem(int i_Id, string i_Name, int i_LineID, float i_TimeReq, float i_TimeReqHigh, int i_outputScreen, string i_Des)
        {
            var m_Dish = new Models.Dish()
            {
                id = i_Id,
                name = i_Name,
                lineId = i_LineID,
                estMakeTime = i_TimeReqHigh,
                description = i_Des,
                makerId = 0,
                makerFit = 6, type = eDishType.Starter, price = 13, restMenuId = 1
            };
            return m_Dish;
        }
    }
}
