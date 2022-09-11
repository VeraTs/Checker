using System.Collections.Generic;
using System.Linq;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class ServerRepository
    {
        // public static LinesDataStore LinesStore { get; private set; }
        // public static DishDataStore DishesStore { get; private set; }
        // public static OrderItemDataStore AllOrdersItemsStore { get; private set; }
        //  public static ServingAreasDataStore ServingAreasStore { get; private set; }
        // public static OrdersDataStore OrdersStore { get; private set; }
        public List<Dish> Dishes { get; private set; }
        public List<ServingArea> ServingAreas { get; private set; }
        public Dictionary<int, Dish> DishesDictionary { get; private set; }
        public List<Line> lines { get; private set; }
        //  public List<OrderItem> OrderedItems { get; private set; }
        // public List<Order> Orders { get; private set; }

        public ServerRepository()
        {
            // DishesStore = new DishDataStore();
            // LinesStore = new LinesDataStore();
            Dishes = new List<Dish>();
            DishesDictionary = new Dictionary<int, Dish>();
            lines = new List<Line>();
            //  ServingAreasStore = new ServingAreasDataStore();
            ServingAreas = new List<ServingArea>();
        }

        public void LoadData()
        {
            //await DishesStore.GetItemsAsync();
            foreach (var dish in App.restaurant.menus.Where(menu=>menu.restaurantId == App.RestId).SelectMany(menu => menu.dishes))
            {
                Dishes.Add(dish);
                DishesDictionary.Add(dish.id, dish);
            }

            foreach (var line in App.restaurant.lines.Where(line => line.restaurantId == App.RestId))
            {
                lines.Add(line);
            }

            foreach (var sa in App.restaurant.servingAreas.Where(area => area.restaurantId == App.RestId))
            {
                ServingAreas.Add(sa);
            }
        }
    }
}
