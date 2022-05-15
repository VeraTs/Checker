using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerWaitersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.CreateOrderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateOrderView : ContentPage
    {
        private readonly DishesViewModel m_DishesViewModel = new DishesViewModel();
        public CreateOrderView()
        {
            InitializeComponent();
            DishesCardView.SetViewModel(m_DishesViewModel);
            OrderToCreateCardView.SetViewModel(m_DishesViewModel);
        }
    }
}