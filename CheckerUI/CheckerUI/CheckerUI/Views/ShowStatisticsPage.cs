using CheckerUI.ViewModels;
using Xamarin.Forms;

namespace CheckerUI.Views
{
    public class ShowStatisticsPage : ContentPage
    {
        public ShowStatisticsPage()
        {
            BindingContext = new ShowStatisticsPageViewModel();
            BackgroundImageSource = "Checker_Logo";
            this.Title = " Restaurant Statistics ";

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
