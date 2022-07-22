using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class ShowStatisticsPageViewModel 
    {
        public Command ReturnCommand { get; }
        public ShowStatisticsPageViewModel()
        {
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}