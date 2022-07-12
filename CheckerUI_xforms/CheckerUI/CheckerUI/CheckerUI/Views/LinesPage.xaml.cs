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
    public partial class LinesPage : ContentPage
    {
        private readonly LinesViewModel vm;
        public LinesPage()
        {
            InitializeComponent();
            vm = new LinesViewModel();
            BindingContext = vm;

        }
        private async void LineButton_OnClicked(object sender, EventArgs e)
        {
            int id = int.Parse((sender as Button).Text);
            vm.m_ClickedLineId = id;
            await vm.LineButton_OnClicked(sender, e);
        }
    }
}