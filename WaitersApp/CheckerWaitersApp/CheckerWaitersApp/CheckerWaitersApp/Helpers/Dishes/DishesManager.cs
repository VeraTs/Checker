using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.Helpers.Dishes
{
    public class DishesManager
    {
        public DishesManager()
        {
            Dishes = new ObservableCollection<DishModel>();
            generate();
        }

        private void generate()
        {
            var d1 = DishBuilder.GenerateDishItem(1, "Burger", 1, "10", "13", 0, "With Fires", eOrderItemType.Main,10.0);
            var d2 = DishBuilder.GenerateDishItem(2, "Pizza", 1, "10", "13", 0, "No info", eOrderItemType.Main,8.99);
            var d3 = DishBuilder.GenerateDishItem(3, "Fish", 1, "10", "13", 0, "Sea bream fillet", eOrderItemType.Main, 7.5);
            var d4 = DishBuilder.GenerateDishItem(4, "Salad", 2, "10", "13", 0, "Chopped", eOrderItemType.Starter, 5.5);
            var d5 = DishBuilder.GenerateDishItem(5, "Leak", 2, "10", "13", 0, "", eOrderItemType.Starter, 4.25);
            var d6 = DishBuilder.GenerateDishItem(6, "Tartar", 2, "10", "13", 0, "Chopped tuna", eOrderItemType.Starter,5.0);
            var d7 = DishBuilder.GenerateDishItem(7, "Bread", 3, "10", "13", 0, "With Tahini", eOrderItemType.Starter, 3.25);
            var d8 = DishBuilder.GenerateDishItem(8, "Pai", 3, "10", "13", 0, "", eOrderItemType.Dessert, 3.0);
            var d9 = DishBuilder.GenerateDishItem(9, "Cake", 3, "10", "13", 0, "", eOrderItemType.Dessert,3.0);
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

        public List<eOrderType> AllDishTypesList()
        {
            var list = new List<eOrderType>
            {
                eOrderType.Unknown,
                eOrderType.AllTogether,
                eOrderType.ByLevels
            };
            return list;
        }

        public string OrderTypeToString(eOrderType i_Type)
        {
            string output;
            switch (i_Type)
            {
                case eOrderType.Unknown:
                {
                    output = "Unknown";
                    break;
                }
                case eOrderType.AllTogether:
                {
                    output = "All Together";
                    break;
                }
                case eOrderType.ByLevels:
                {
                    output = "By Levels";
                    break;
                }
                default:
                {
                    output = "By Levels";
                    break;
                }
            }
            return output;
        }
        public ObservableCollection<DishModel> Dishes { get; }

    }
}
