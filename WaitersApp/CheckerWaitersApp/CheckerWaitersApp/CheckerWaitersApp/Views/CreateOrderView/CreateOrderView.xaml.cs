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
        public CreateOrderView()
        {
            CreateOrderViewModel m_DishesViewModel;
            InitializeComponent();
            m_DishesViewModel = this.BindingContext as CreateOrderViewModel;
            DishesCardView.SetViewModel(m_DishesViewModel);
            OrderToCreateCardView.SetViewModel(m_DishesViewModel);
            AllOrdersCardView.SetViewModel(m_DishesViewModel.Orders);
        }
    }
}