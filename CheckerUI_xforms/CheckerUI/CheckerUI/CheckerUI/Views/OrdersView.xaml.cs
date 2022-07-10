using CheckerUI.Helpers.OrdersHelpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersView : ContentPage
    {
        public OrdersView()
        {
            InitializeComponent();
            var manager = new OrdersManager();
            this.BindingContext = manager;
        }
    }
}