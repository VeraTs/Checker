using CheckerUI.ViewModels;
using Xamarin.Forms;

namespace CheckerUI.Views
{
    public class ColdStripePage : ContentPage
    {
        public ColdStripePage()
        {
            BindingContext = new ColdStripePageViewModel();
            BackgroundColor = Color.PowderBlue;
            this.Title = "Cold Stripe ";

            Button returnButton = new Button()
            {
                Text = "Return",
                BackgroundColor = Color.DarkOrange,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(50)
            };
            returnButton.SetBinding(Button.CommandProperty, "ReturnCommand");

            Content = new StackLayout
            {
                Children =
                {
                    returnButton
                }
            };
        }
    }
    
}