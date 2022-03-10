using System.ComponentModel;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class OvenStripePageViewModel 
    {
        public Command ReturnCommand { get; }
     
        public OvenStripePageViewModel()
        {
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
           
        }
    }
}
