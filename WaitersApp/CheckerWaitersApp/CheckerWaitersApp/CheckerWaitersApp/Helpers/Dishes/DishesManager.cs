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
                eOrderType.AllTogether,
                eOrderType.Staggered,
                eOrderType.FIFO,
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
