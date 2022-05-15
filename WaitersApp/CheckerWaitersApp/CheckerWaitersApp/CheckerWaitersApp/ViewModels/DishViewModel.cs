using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.ViewModels
{
    public class DishViewModel : BaseViewModel
    {
        private readonly DishModel m_model;

        public DishViewModel(DishModel i_dish)
        {
            m_model = new DishModel();
            m_model = i_dish;
        }
        public string DishName
        {
            get => m_model.m_DishName;
            private set => m_model.m_DishName = value;
        }
        public string Description
        {
            get => m_model.m_Description;
            set => m_model.m_Description = value;
        }
    }
}
