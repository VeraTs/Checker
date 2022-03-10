using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class ColdStripePageViewModel 
    {
        public Command ReturnCommand { get; }
        
        public ColdStripePageViewModel()
        {
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}
