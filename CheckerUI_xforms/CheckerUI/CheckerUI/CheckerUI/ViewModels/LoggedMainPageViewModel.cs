using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class LoggedMainPageViewModel 
    {
        public Command ReturnCommand { get; }
        public Command MyKitchenCommand { get; }
        public Command UpdateKitchenCommand { get; }
        public Command MangeKitchenCommand { get; }
        public Command ShowStatisticsCommand { get; }
        public LoggedMainPageViewModel()
        {
            MyKitchenCommand = new Command(async () =>
            {
                LinesPage page = new LinesPage();
                await Application.Current.MainPage.Navigation.PushAsync(page);
            });
            UpdateKitchenCommand = new Command(async () =>
            {
                var updateVm = new UpdateKitchenViewModel();
                var updatePage = new UpdateKitchenPage();
                updatePage.BindingContext = updateVm;
                await Application.Current.MainPage.Navigation.PushAsync(updatePage);
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
                //var statisticsVm = new ShowStatisticsPageViewModel();
                //var statisticsPage = new ShowStatisticsPage();
                //statisticsPage.BindingContext = statisticsVm;
                //await Application.Current.MainPage.Navigation.PushAsync(statisticsPage);
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
