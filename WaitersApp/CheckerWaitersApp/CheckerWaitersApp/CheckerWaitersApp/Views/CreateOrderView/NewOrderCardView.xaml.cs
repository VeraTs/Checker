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
    public partial class NewOrderCardView : ContentView
    {
        private DishesViewModel m_vm = new DishesViewModel();
        public NewOrderCardView()
        {
            InitializeComponent();
        }
        public void SetViewModel(DishesViewModel i_ViewModel)
        {
            m_vm = i_ViewModel;
            BindingContext = m_vm;
        }
    }
}