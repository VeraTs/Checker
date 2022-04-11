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
    public partial class DeptsPage : ContentPage
    {
        private readonly DeptsViewModel vm;
        public DeptsPage()
        {
            InitializeComponent();
            vm = new DeptsViewModel();
            BindingContext = vm;
            
        }
        private async void DeptButton_OnClicked(object sender, EventArgs e)
        {
            await vm.DeptButton_OnClicked(sender, e);
        }
    }
}