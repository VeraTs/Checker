using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class UpdateKitchenViewModel 
    {
        public Command ReturnCommand { get; }

        public UpdateKitchenViewModel()
        {
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}
