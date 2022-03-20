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
            var baseLineVM = new BaseLineViewModel();
            
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            HotLineCommand = new Command(async () =>
            {
                var baseLinePage = new BaseLineView("Hot Line");
                baseLinePage.BindingContext = baseLineVM;
                await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
            });
            ColdLineCommand = new Command(async () =>
            {
                var baseLinePage = new BaseLineView("Cold Line");
                baseLinePage.BindingContext = baseLineVM;
                await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
            });
            OvenLineCommand = new Command(async () =>
            {
                var baseLinePage = new BaseLineView("Oven Line");
                baseLinePage.BindingContext = baseLineVM;
                await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
            });
        }


    }
}
