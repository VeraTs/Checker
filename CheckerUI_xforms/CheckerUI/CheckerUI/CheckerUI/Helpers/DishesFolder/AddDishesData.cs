using System;
using System.Collections.Generic;
using System.Text;
using CheckerUI.Models;
using System.Threading.Tasks;

namespace CheckerUI.Helpers.DishesFolder
{
    public class AddDishesData
    {
        public List<Dish_item> m_DishList;
        public List<Dish_item> m_DishList2;
        public List<Dish_item> m_DishList3;
        public AddDishesData()
        {
            m_DishList = new List<Dish_item>();
            m_DishList2 = new List<Dish_item>();
            m_DishList3 = new List<Dish_item>();
        }

        public async Task<List<Dish_item>> GetDishList()
        {
            Dish_item d1 = DishBuilder.GenerateDishItem(1, "Burger", 1, "10", "13", 0, "HotLine");
            Dish_item d2 = DishBuilder.GenerateDishItem(2, "Pizza", 1, "10", "13", 0, "HotLine");
            Dish_item d3 = DishBuilder.GenerateDishItem(3, "Fish", 1, "10", "13", 0, "HotLine");
            m_DishList.Add(d1);
            m_DishList.Add(d2);
            m_DishList.Add(d3);
            return m_DishList;
        }
        public async Task<List<Dish_item>> GetDishList2()
        {
            Dish_item d1 = DishBuilder.GenerateDishItem(4, "Salad", 2, "10", "13", 0, "ColdLine");
            Dish_item d2 = DishBuilder.GenerateDishItem(5, "Leak", 2, "10", "13", 0, "ColdLine");
            Dish_item d3 = DishBuilder.GenerateDishItem(6, "Tartar", 2, "10", "13", 0, "ColdLine");
            m_DishList2.Add(d1);
            m_DishList2.Add(d2);
            m_DishList2.Add(d3);
            return m_DishList2;
        }
        public async Task<List<Dish_item>> GetDishList3()
        {
            Dish_item d1 = DishBuilder.GenerateDishItem(7, "Bread", 3, "10", "13", 0, "OvenLine");
            Dish_item d2 = DishBuilder.GenerateDishItem(8, "Pizza", 3, "10", "13", 0, "OvenLine");
            Dish_item d3 = DishBuilder.GenerateDishItem(9, "Cake", 3, "10", "13", 0, "OvenLine");
            m_DishList2.Add(d1);
            m_DishList2.Add(d2);
            m_DishList2.Add(d3);
            return m_DishList3;
        }
        public List<Dish_item> d1
        {
            get => m_DishList; private set => m_DishList = value;
        }
        public List<Dish_item> d2
        {
            get => m_DishList2; private set => m_DishList2 = value;
        }
        public List<Dish_item> d3
        {
            get => m_DishList3; private set => m_DishList3 = value;
        }
    }
}
