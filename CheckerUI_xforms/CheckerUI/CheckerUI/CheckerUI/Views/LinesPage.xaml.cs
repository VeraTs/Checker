using System;
using CheckerUI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinesPage : ContentPage
    {
        public LinesPage()
        {
            InitializeComponent();
        }
        private async void LineButton_OnClicked(object sender, EventArgs e)
        {
            var name = (sender as Button).Text;
            var vm2 = this.BindingContext as LinesViewModel;
          //  vm2.ClickedLineName = name;
          vm2.m_ClickedLineId = int.Parse(name);
            await vm2.LineButton_OnClicked(sender,e);
        }
    }
}