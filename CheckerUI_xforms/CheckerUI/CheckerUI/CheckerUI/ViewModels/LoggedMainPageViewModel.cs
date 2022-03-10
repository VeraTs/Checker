using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class LoggedMainPageViewModel 
    {
        public Command SignCommand { get; }
        public Command ReturnCommand { get; }

        public Command MyKitchenCommand { get; }
        public Command UpdateKitchenCommand { get; }
        public Command MangeKitchenCommand { get; }
        public Command ShowStatisticsCommand { get; }
        public LoggedMainPageViewModel()
        {
            SignCommand = new Command(() =>
            {
                // check if user already exists
                // save user details
                //login user
                //navigate main page
            });
            MyKitchenCommand = new Command(async () =>
            {
                var KitchenVM = new KitchenPageViewModel();
                var KitchenPage = new KitchenPage();
                KitchenPage.BindingContext = KitchenVM;
                await Application.Current.MainPage.Navigation.PushAsync(KitchenPage);
            });
            UpdateKitchenCommand = new Command(async () =>
            {
                var UpdateVM = new UpdateKitchenViewModel();
                var UpdatePage = new UpdateKitchenPage();
                UpdatePage.BindingContext = UpdateVM;
                await Application.Current.MainPage.Navigation.PushAsync(UpdatePage);
            });
            MangeKitchenCommand = new Command(async () =>
            {
                var mangeKitchenVM = new MangePageViewModel();
                var mangeKitchenPage = new MangePage();
               mangeKitchenPage.BindingContext = mangeKitchenVM;
                await Application.Current.MainPage.Navigation.PushAsync(mangeKitchenPage);
            });
            ShowStatisticsCommand = new Command(async () =>
            {
                var statisticsVM = new ShowStatisticsPageViewModel();
                var statisticsPage = new ShowStatisticsPage();
                statisticsPage.BindingContext = statisticsVM;
                await Application.Current.MainPage.Navigation.PushAsync(statisticsPage);
            });
            ReturnCommand = new Command(async () =>
            {
                // logged out from user 
                await Application.Current.MainPage.Navigation.PopAsync();
            });

        }
    }
}
