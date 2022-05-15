using System.Collections.ObjectModel;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Helpers.Dishes;
using CheckerWaitersApp.Models;

namespace CheckerWaitersApp.ViewModels
{
    public class DishesViewModel : BaseViewModel
    {
        private ObservableCollection<DishModel> m_Dishes;
        private DishesManager m_Manager;

        public DishesViewModel()
        {
            m_Manager = new DishesManager();
            m_Dishes = new ObservableCollection<DishModel>();
            m_Dishes = m_Manager.Dishes;

            foreach (var model in m_Dishes)
            {
                var dishViewModel = new DishViewModel(model);
                DishesOnView.Add(dishViewModel);
                Dishes.Add(dishViewModel);

                switch (model.m_DishType)
                {
                    case eOrderItemType.Starter:
                        Starters.Add(dishViewModel);
                        break;
                    case eOrderItemType.Main:
                        Mains.Add(dishViewModel);
                        break;
                    case eOrderItemType.Dessert:
                        Desserts.Add(dishViewModel);
                        break;
                }
            }
        }
        public ObservableCollection<DishViewModel> DishesOnView { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Dishes { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Starters { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Mains { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Desserts { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> ToOrderCollection { get; private set; } = new ObservableCollection<DishViewModel>();
        public void AddToOrderCollection(DishViewModel i_ToAdd) { ToOrderCollection.Add(i_ToAdd); }
        public void RemoveFromOrderCollection(DishViewModel i_ToRemove)
        {
            ToOrderCollection.Remove(i_ToRemove);
        }
    }
}
