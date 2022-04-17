using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers.Order;
using CheckerUI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewLineView : ContentPage
    {
        private BaseLineViewModel m_ViewModel;
        public NewLineView()
        {
            InitializeComponent();
            BackgroundColor = Color.Transparent;
            m_ViewModel = new BaseLineViewModel();
            m_ViewModel.init();
            BindingContext = m_ViewModel;
            m_ItemsToMakeCardView.ViewModel = m_ViewModel;
            m_ItemsInProgressCardView.ViewModel = m_ViewModel;
            m_ItemsDoneCardView.ViewModel = m_ViewModel;
        }

     
    }
}