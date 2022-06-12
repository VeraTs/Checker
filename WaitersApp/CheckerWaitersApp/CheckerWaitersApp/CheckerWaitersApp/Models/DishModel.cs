using CheckerWaitersApp.Enums;

namespace CheckerWaitersApp.Models
{
    public class DishModel
    {
        public int m_DishID { get; set; }
        public string m_DishName { get; set; }
        public int m_LineID { get; set; }
        public string m_TimeReq { get; set; }
        public string m_TimeReqHigh { get; set; }
        public int m_Output_screen { get; set; }
        public string m_Description { get; set; }
        public double m_Price { get; set; }
        public eOrderItemType m_DishType { get; set; }
    }
}
