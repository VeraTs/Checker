using System.Collections.Generic;
using System.Threading.Tasks;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.Helpers.Dishes
{
    public class AddDishesData
    {
        public List<DishModel> m_DishList;
        public List<DishModel> m_DishList2;
        public List<DishModel> m_DishList3;
        public AddDishesData()
        {
            m_DishList = new List<DishModel>();
            m_DishList2 = new List<DishModel>();
            m_DishList3 = new List<DishModel>();
        }

        public async Task<List<DishModel>> GetDishList()
        {
            var d1 = DishBuilder.GenerateDishItem(1, "Burger", 1, "10", "13", 0, "HotLine", eOrderItemType.Main);
            var d2 = DishBuilder.GenerateDishItem(2, "Pizza", 1, "10", "13", 0, "HotLine", eOrderItemType.Main);
            var d3 = DishBuilder.GenerateDishItem(3, "Fish", 1, "10", "13", 0, "HotLine", eOrderItemType.Main);
            m_DishList.Add(d1);
            m_DishList.Add(d2);
            m_DishList.Add(d3);
            return m_DishList;
        }
        public async Task<List<DishModel>> GetDishList2()
        {
            var d1 = DishBuilder.GenerateDishItem(4, "Salad", 2, "10", "13", 0, "ColdLine", eOrderItemType.Starter);
            var d2 = DishBuilder.GenerateDishItem(5, "Leak", 2, "10", "13", 0, "ColdLine", eOrderItemType.Starter);
            var d3 = DishBuilder.GenerateDishItem(6, "Tartar", 2, "10", "13", 0, "ColdLine", eOrderItemType.Starter);
            m_DishList2.Add(d1);
            m_DishList2.Add(d2);
            m_DishList2.Add(d3);
            return m_DishList2;
        }
        public async Task<List<DishModel>> GetDishList3()
        {
            var d1 = DishBuilder.GenerateDishItem(7, "Bread", 3, "10", "13", 0, "OvenLine", eOrderItemType.Starter);
            var d2 = DishBuilder.GenerateDishItem(8, "Pai", 3, "10", "13", 0, "OvenLine", eOrderItemType.Dessert);
            var d3 = DishBuilder.GenerateDishItem(9, "Cake", 3, "10", "13", 0, "OvenLine", eOrderItemType.Dessert);
            m_DishList2.Add(d1);
            m_DishList2.Add(d2);
            m_DishList2.Add(d3);
            return m_DishList3;
        }
        public List<DishModel> d1
        {
            get => m_DishList; private set => m_DishList = value;
        }
        public List<DishModel> d2
        {
            get => m_DishList2; private set => m_DishList2 = value;
        }
        public List<DishModel> d3
        {
            get => m_DishList3; private set => m_DishList3 = value;
        }
    }
}
