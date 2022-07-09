using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;
using CheckerWaitersApp.Services;

namespace CheckerWaitersApp.Helpers.Dishes
{
    public class DishesManager
    {
        private DishDataStore Store { get; set; }
        public DishesManager(DishDataStore i_Store)
        {
            Dishes = new ObservableCollection<Dish>();
            Store = i_Store;
           GetDishes();
        }

        private async void GetDishes()
        {
           await Store.GetItemsAsync();
           var list = Store.dishes;
           foreach (var item in list)
           {
               Dishes.Add(item);
           }
        }
        private void generate()
        {
            
            var d1 = DishBuilder.GenerateDishItem(1, "Burger", 1,"With Fires", eDishType.Main, 10, 1);
            var d2 = DishBuilder.GenerateDishItem(2, "Pizza", 1, "No info", eDishType.Main,8, 1);
            var d3 = DishBuilder.GenerateDishItem(3, "Fish", 1, "Sea bream fillet", eDishType.Main, 7, 1);
            var d4 = DishBuilder.GenerateDishItem(4, "Salad", 2, "Chopped", eDishType.Starter, 5,1);
            var d5 = DishBuilder.GenerateDishItem(5, "Leak", 2, "", eDishType.Starter, 4,1);
            var d6 = DishBuilder.GenerateDishItem(6, "Tartar", 2,  "Chopped tuna", eDishType.Starter,5,1);
            var d7 = DishBuilder.GenerateDishItem(7, "Bread", 3,  "With Tahini", eDishType.Starter, 3,1);
            var d8 = DishBuilder.GenerateDishItem(8, "Pai", 3, "",eDishType.Dessert, 3,1);
            var d9 = DishBuilder.GenerateDishItem(9, "Cake", 3,  "", eDishType.Dessert,3,1);
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
                list.Add(dish.name);
            }

            return list;
        }

        public List<eOrderType> AllDishTypesList()
        {
            var list = new List<eOrderType>
            {
                eOrderType.FIFO,
                eOrderType.AllTogether,
                eOrderType.Staggered
            };
            return list;
        }

        public string OrderTypeToString(eOrderType i_Type)
        {
            string output;
            switch (i_Type)
            {
                case eOrderType.FIFO:
                {
                    output = "FIFO";
                    break;
                }
                case eOrderType.AllTogether:
                {
                    output = "All Together";
                    break;
                }
                case eOrderType.Staggered:
                {
                    output = "Staggered";
                    break;
                }
                default:
                {
                    output = "Staggered";
                    break;
                }
            }
            return output;
        }
        public ObservableCollection<Dish> Dishes { get; }

    }
}
