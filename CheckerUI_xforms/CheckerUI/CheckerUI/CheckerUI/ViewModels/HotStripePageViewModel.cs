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
                var hotStripeVM = new HotStripePageViewModel();
                var hotStripePage = new HotStripePage();
                hotStripePage.BindingContext = hotStripeVM;
                await Application.Current.MainPage.Navigation.PushAsync(hotStripePage);
            });
            ColdStripeCommand = new Command(async () =>
            {
                var coldStripeVM = new ColdStripePageViewModel();
                var coldStripePage = new ColdStripePage();
                coldStripePage.BindingContext = coldStripeVM;
                await Application.Current.MainPage.Navigation.PushAsync(coldStripePage);
            });
            OvenStripeCommand = new Command(async () =>
            {
                var ovenStripeVM = new OvenStripePageViewModel();
                var ovenStripePage = new OvenStripePage();
                ovenStripePage.BindingContext = ovenStripeVM;
                await Application.Current.MainPage.Navigation.PushAsync(ovenStripePage);
            });
        }
    }
}
