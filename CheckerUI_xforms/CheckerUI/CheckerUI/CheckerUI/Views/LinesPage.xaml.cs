using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinesPage : ContentPage
    {
        private readonly LinesViewModel vm;
        public LinesPage()
        {
            //vm = new LinesViewModel();
            InitializeComponent();
           

        }
        private async void LineButton_OnClicked(object sender, EventArgs e)
        {
            var name = (sender as Button).Text;
            var vm2 = this.BindingContext as LinesViewModel;
            vm2.ClickedLineName = name;
            await vm2.LineButton_OnClickedString(sender,e);
        }
    }
}