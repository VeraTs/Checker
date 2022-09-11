using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServingWindowsView : ContentPage
    {
        public ServingWindowsView()
        {
            InitializeComponent();
        }

        private async void M_AreaButton_OnClicked(object sender, EventArgs e)
        {
            var name = (sender as Button).Text;
            var vm2 = this.BindingContext as ServingAreasViewModel;
            vm2.ClickedAreaName = name;
            await vm2.AreaButton_OnClickedString(sender, e);
        }

    }
}