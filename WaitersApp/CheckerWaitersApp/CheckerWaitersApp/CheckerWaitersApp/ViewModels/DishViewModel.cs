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
        
        public DishViewModel(DishModel i_dish)
        {
            Model = new DishModel();
            Model = i_dish;
        }
        public string DishName
        {
            get => Model.m_DishName;
            private set => Model.m_DishName = value;
        }
        public string Description
        {
            get => Model.m_Description;
            set => Model.m_Description = value;
        }

        public DishModel Model { get; }
    }
}
