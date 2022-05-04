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
    public partial class AutoSearchBarView : ContentPage
    {
        public AutoSearchBarView(List<string> i_Options)
        {
            InitializeComponent();
            m_AutoSearchBar.SetOptionsData(i_Options);
        }
    }
}