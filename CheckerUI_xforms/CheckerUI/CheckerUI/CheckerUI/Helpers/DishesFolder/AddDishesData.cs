using System;
using System.Collections.Generic;
using System.Text;
using CheckerUI.Models;
using System.Threading.Tasks;

namespace CheckerUI.Helpers.DishesFolder
{
    public class AddDishesData
    {
        private List<Dish_item> m_DishList;

        public AddDishesData()
        {
            m_DishList = new List<Dish_item>();
        }

        public async Task<List<Dish_item>> GetDishList()
        {
            Dish_item d1 = DishBuilder.GenerateDishItem(1, "Burger", 2, "10", "13", 0, "HotLine");
            Dish_item d2 = DishBuilder.GenerateDishItem(2, "Pizza", 3, "10", "13", 0, "OvenLine");
            Dish_item d3 = DishBuilder.GenerateDishItem(3, "Fish", 2, "10", "13", 0, "HotLine");
            m_DishList.Add(d1);
            m_DishList.Add(d2);
            m_DishList.Add(d3);
            return m_DishList;
        }
    }
}
