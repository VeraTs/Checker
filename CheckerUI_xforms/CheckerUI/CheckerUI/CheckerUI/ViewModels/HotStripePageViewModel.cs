using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class HotStripePageViewModel 
    {
        public Command ReturnCommand { get; }
        public Command HotStripeCommand { get; }
        public Command ColdStripeCommand { get; }
        public Command OvenStripeCommand { get; }

        public HotStripePageViewModel()
        {

            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            HotStripeCommand = new Command(async () =>
            {
                var hotStripeVm = new HotStripePageViewModel();
                var hotStripePage = new HotStripePage1();
                hotStripePage.BindingContext = hotStripeVm;
                await Application.Current.MainPage.Navigation.PushAsync(hotStripePage);
            });
            ColdStripeCommand = new Command(async () =>
            {
                var coldStripeVm = new ColdStripePageViewModel();
                var coldStripePage = new HotStripePage1();
                coldStripePage.BindingContext = coldStripeVm;
                await Application.Current.MainPage.Navigation.PushAsync(coldStripePage);
            });
            OvenStripeCommand = new Command(async () =>
            {
                var ovenStripeVm = new OvenStripePageViewModel();
                var ovenStripePage = new HotStripePage1();
                ovenStripePage.BindingContext = ovenStripeVm;
                await Application.Current.MainPage.Navigation.PushAsync(ovenStripePage);
            });
        }
    }
}
