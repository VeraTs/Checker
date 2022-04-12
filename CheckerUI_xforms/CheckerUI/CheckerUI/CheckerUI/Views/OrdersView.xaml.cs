using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers.Order;
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
            OrdersManager manager = new OrdersManager();
            this.BindingContext = manager;
        }
    }
}