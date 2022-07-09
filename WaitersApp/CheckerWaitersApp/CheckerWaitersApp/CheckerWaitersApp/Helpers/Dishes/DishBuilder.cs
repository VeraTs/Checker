using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.Helpers.Dishes
{
    internal static class DishBuilder
    {
        public static Dish GenerateDishItem(int i_Id, string i_Name, int i_LineID,string i_Des, eDishType i_Type, float i_Price, int i_RestMenuId)
        {
            var m_Dish = new Dish()
            {
                id = i_Id,
                name = i_Name,
                lineId = i_LineID,
                description = i_Des,
                price = i_Price,
                restMenuId = i_RestMenuId,
                type = i_Type
            };
            return m_Dish;
        }
    }
}
