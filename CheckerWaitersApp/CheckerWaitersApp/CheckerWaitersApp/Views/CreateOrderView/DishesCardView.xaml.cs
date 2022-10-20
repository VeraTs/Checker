using System;
using CheckerWaitersApp.Helpers;
using CheckerWaitersApp.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.CreateOrderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DishesCardView : ContentView
    {
        private CreateOrderViewModel m_vm;
        private readonly ItemCardHelper r_ItemCardHelper = new ItemCardHelper();
      
        public DishesCardView()
        {
            InitializeComponent();
        }

        public void SetViewModel(CreateOrderViewModel i_ViewModel)
        {
            m_vm = i_ViewModel;
            BindingContext = m_vm;
        }
        public void ViewStarters()
        {
            DishesView.ItemsSource = m_vm.Starters;
        }

        public void ViewMains()
        {
            DishesView.ItemsSource = m_vm.Mains;
        }
        public void ViewDesserts()
        {
            DishesView.ItemsSource = m_vm.Desserts;
        }
        public void ViewDishes()
        {
            DishesView.ItemsSource = m_vm.Dishes;
        }

        public void ViewDrinks()
        {
            DishesView.ItemsSource = m_vm.Drinks;
        }
        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            DishesView.SelectedItem = stackLayout.BindingContext;
            var card = stackLayout.LogicalChildren[0] as DishCardView;
            var model = card.BindingContext as DishViewModel;
            var expander = card.Children[0] as Expander;
            r_ItemCardHelper.OnSingleTap(expander);
        }

        private void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            DishesView.SelectedItem = stackLayout.BindingContext;
            var card = stackLayout.LogicalChildren[0] as DishCardView;
            var model = card.BindingContext as DishViewModel;
            m_vm.AddToOrderCollection(model);
        }

        private void StartersButton_OnClicked(object sender, EventArgs e)
        {
            ViewStarters();
        }
        private void MainsButton_OnClicked(object sender, EventArgs e)
        {
            ViewMains();
        }
        private void DessertsButton_OnClicked(object sender, EventArgs e)
        {
            ViewDesserts();
        }
        private void DrinksButton_OnClicked(object sender, EventArgs e)
        {
            ViewDesserts();
        }
        private void AllDishesButton_OnClicked(object sender, EventArgs e)
        {
            ViewDishes();
        }

        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}