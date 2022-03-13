using CheckerUI.ViewModels;
using Xamarin.Forms;

namespace CheckerUI.Views
{
    public class HotStripePage : ContentPage
    {
        public HotStripePage()
        {
            
            BindingContext = new HotStripePageViewModel();
            this.Title = "Hot Stripe ";
            BackgroundColor = Color.PowderBlue;
         

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