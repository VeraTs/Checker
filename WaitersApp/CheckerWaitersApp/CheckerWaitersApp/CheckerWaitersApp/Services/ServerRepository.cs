using System.Collections.Generic;
using System.Linq;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.Services
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

        public async void LoadData()
        {
            foreach (var dish in App.restaurant.menus.SelectMany(menu => menu.dishes))
            {
                Dishes.Add(dish);
                DishesDictionary.Add(dish.id, dish);
            }

            lines = App.restaurant.lines;
            ServingAreas = App.restaurant.servingAreas;

        }
    }
}
