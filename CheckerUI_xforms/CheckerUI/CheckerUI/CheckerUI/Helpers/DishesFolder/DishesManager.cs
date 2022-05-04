using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerUI.Models;

namespace CheckerUI.Helpers.DishesFolder
{
    public class DishesManager
    {
        public DishesManager()
        {
            Dishes = new ObservableCollection<Dish_item>();
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
            var d8 = DishBuilder.GenerateDishItem(8, "Humus", 3, "10", "13", 0, "OvenLine");
            var d9 = DishBuilder.GenerateDishItem(9, "Cake", 3, "10", "13", 0, "OvenLine");
            Dishes.Add(d1);
            Dishes.Add(d2);
            Dishes.Add(d3);
            Dishes.Add(d4);
            Dishes.Add(d5);
            Dishes.Add(d6);
            Dishes.Add(d7);
            Dishes.Add(d8);
            Dishes.Add(d9);
        }

        public List<string> DishesNamesList()
        {
            var list = new List<string>();
            foreach (var dish in Dishes)
            {
                list.Add(dish.m_DishName);
            }

            return list;
        }
        public ObservableCollection<Dish_item> Dishes { get; }

    }
}
