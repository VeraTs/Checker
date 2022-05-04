using CheckerUI.Helpers.DishesFolder;
using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class LoggedMainPageViewModel
    {
        private DishesManager m_manager;


        public Command ReturnCommand { get; }
        public Command MyKitchenCommand { get; }
        public Command UpdateKitchenCommand { get; }
        public Command MangeKitchenCommand { get; }
        public Command ShowStatisticsCommand { get; }
        public LoggedMainPageViewModel()
        {
            m_manager = new DishesManager();

            MyKitchenCommand = new Command(async () =>
            {
                LinesPage page = new LinesPage();
                await Application.Current.MainPage.Navigation.PushAsync(page);
            });
            UpdateKitchenCommand = new Command(async () =>
            {
                var options = m_manager.DishesNamesList();
                var searchDishView = new AutoSearchBarView(options);
                await Application.Current.MainPage.Navigation.PushAsync(searchDishView);
            });
            MangeKitchenCommand = new Command(async () =>
            {
                var mangeKitchenVm = new MangePageViewModel();
                var mangeKitchenPage = new MangePage();
               mangeKitchenPage.BindingContext = mangeKitchenVm;
                await Application.Current.MainPage.Navigation.PushAsync(mangeKitchenPage);
            });
            ShowStatisticsCommand = new Command(async () =>
            {
                var ordersView = new OrdersView();
                await Application.Current.MainPage.Navigation.PushAsync(ordersView);
            });
            ReturnCommand = new Command(async () =>
            {
                // logged out from user 
                await Application.Current.MainPage.Navigation.PopAsync();
            });

        }
    }
}
