using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class KitchenPageViewModel 
    {
        public Command ReturnCommand { get; }
        public Command HotLineCommand { get; }
        public Command ColdLineCommand { get; }
        public Command OvenLineCommand { get; }
        
        public KitchenPageViewModel()
        {
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            HotLineCommand = new Command(async () =>
            {
                var baseLinePage = new LineView("Hot Line");
                await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
            });
            ColdLineCommand = new Command(async () =>
            {
                var baseLinePage = new LineView("Cold Line");
                await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
            });
            OvenLineCommand = new Command(async () =>
            {
                var baseLinePage = new LineView("Oven Line");
                await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
            });
        }


    }
}
