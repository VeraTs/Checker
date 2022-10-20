using System.Collections.Generic;
using System.Linq;
using CheckerUI.Models;

namespace CheckerUI.Services
{
    public class ServerRepository
    {
        public List<Dish> Dishes { get; private set; }
        public List<ServingArea> ServingAreas { get; private set; }
        public Dictionary<int, Dish> DishesDictionary { get; private set; }
        public List<Line> lines { get; private set; }
      
        public ServerRepository()
        {
            Dishes = new List<Dish>();
            DishesDictionary = new Dictionary<int, Dish>();
            lines = new List<Line>();
            ServingAreas = new List<ServingArea>();
        }

        public void LoadData()
        {
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
