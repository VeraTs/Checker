using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CheckerUI.Models;

namespace CheckerUI.Helpers.DishesFolder
{
    public class DishesManager
    {
        private Collection<Dish_item> m_dishes;

        public DishesManager()
        {
            m_dishes = new Collection<Dish_item>();
            generate();
        }

        private void generate()
        {
           var d1 = DishBuilder.GenerateDishItem(1, "Burger", 1, "10", "13", 0, "HotLine");
           var d2 = DishBuilder.GenerateDishItem(2, "Pizza", 1, "10", "13", 0, "HotLine");
           var d3 = DishBuilder.GenerateDishItem(3, "Fish", 1, "10", "13", 0, "HotLine");
           var d4 = DishBuilder.GenerateDishItem(4, "Salad", 2, "10", "13", 0, "ColdLine");
           var d5 = DishBuilder.GenerateDishItem(5, "Leak", 2, "10", "13", 0, "ColdLine");
           var d6 = DishBuilder.GenerateDishItem(6, "Tartar", 2, "10", "13", 0, "ColdLine");
           var d7 = DishBuilder.GenerateDishItem(7, "Bread", 3, "10", "13", 0, "OvenLine");
           var d8 = DishBuilder.GenerateDishItem(8, "Pizza", 3, "10", "13", 0, "OvenLine");
           var d9 = DishBuilder.GenerateDishItem(9, "Cake", 3, "10", "13", 0, "OvenLine");
            m_dishes.Add(d1);
            m_dishes.Add(d2);
            m_dishes.Add(d3);
            m_dishes.Add(d4);
            m_dishes.Add(d5);
            m_dishes.Add(d6);
            m_dishes.Add(d7);
            m_dishes.Add(d8);
            m_dishes.Add(d9);
        }

        public Collection<Dish_item> Dishes => m_dishes;
    }
}
