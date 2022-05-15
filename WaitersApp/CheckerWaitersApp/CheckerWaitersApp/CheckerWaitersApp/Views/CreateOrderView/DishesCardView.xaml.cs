using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerWaitersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.CreateOrderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DishesCardView : ContentView
    {
        private DishesViewModel m_vm = new DishesViewModel();
        private StackLayout m_LastTappedLayout;
        public DishesCardView()
        {
            InitializeComponent();
        }

        public void SetViewModel(DishesViewModel i_ViewModel)
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

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            DishesView.SelectedItem = stackLayout.BindingContext;
            var card = stackLayout.LogicalChildren[0] as DishCardView;
            var model = card.BindingContext as DishViewModel;


            var frame = card.Children[0] as Frame;
            var layout = frame.Children[0] as StackLayout;
            if (m_LastTappedLayout != null && m_LastTappedLayout != layout)
            {
                layout.BackgroundColor = Color.Bisque;
                m_LastTappedLayout.BackgroundColor = Color.Transparent;
            }

            m_LastTappedLayout = layout;
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
        private void AllDishesButton_OnClicked(object sender, EventArgs e)
        {
            ViewDishes();
        }
    }
}