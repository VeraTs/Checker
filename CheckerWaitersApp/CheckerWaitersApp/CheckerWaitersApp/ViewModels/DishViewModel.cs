using System.Globalization;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.ViewModels
{
    public class DishViewModel : BaseViewModel
    {
        private bool m_IsOrdered = false;
        public bool IsOrdered
        {
            get => m_IsOrdered;
            set
            {
                m_IsOrdered = value;
                OnPropertyChanged(nameof(IsOrdered));
            }
        }
        
        public DishViewModel(Dish i_dish)
        {
            Model = new Dish();
            Model = i_dish;
            Price = "Price :"+ Model.price.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
        }
        public float DishPrice
        {
            get => Model.price;
            private set => Model.price = value;
        }
        public string DishName
        {
            get => Model.name;
            private set => Model.name = value;
        }
        public string Description
        {
            get => Model.description;
            set => Model.description = value;
        }
        public string Price { get; set; }
        public Dish Model { get; }
    }
}
