using CheckerWaitersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.OrdersViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllOrdersCardView : ContentView
    {
        public OrdersViewModel m_vm { get; set; }
      
        public AllOrdersCardView()
        {
            InitializeComponent();
        }

        public void SetViewModel(OrdersViewModel i_ViewModel)
        {
            m_vm = i_ViewModel;
            BindingContext = m_vm;
        }

    }
}