using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTest2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetListViewPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public GetListViewPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            MyListView.ItemsSource = await App.Store.GetItemsAsync();
        }
    }
}
