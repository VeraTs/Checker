using CheckerWaitersApp.Services;
using CheckerWaitersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.CreateOrderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateOrderView : ContentPage
    {
        private readonly CreateOrderViewModel m_DishesViewModel;
        public CreateOrderView(DishDataStore i_Store)
        {
            CreateOrderViewModel m_DishesViewModel = new CreateOrderViewModel(i_Store);
            InitializeComponent();
            DishesCardView.SetViewModel(m_DishesViewModel);
            OrderToCreateCardView.SetViewModel(m_DishesViewModel);
            AllOrdersCardView.SetViewModel(m_DishesViewModel.Orders);
        }
    }
}