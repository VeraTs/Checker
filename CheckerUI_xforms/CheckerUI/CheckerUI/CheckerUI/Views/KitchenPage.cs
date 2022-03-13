using CheckerUI.ViewModels;
using Xamarin.Forms;

//need to add login, fast login , onload when logged

namespace CheckerUI.Views // dynamic ui 
{
    public class KitchenPage : ContentPage
    {
        private Label _text;
        private Button _saveButton;
        private Button _deleteButton;
        public KitchenPage()
        {
            this.Title = "Choose a Stripe ";
            BindingContext = new KitchenPageViewModel();
            BackgroundColor = Color.PowderBlue;
            Button[] buttonsArr = new Button[10];
            string[] lines = { "Hot Stripe", "Cold Stripe", "Oven Stripe" };
            int i = 0;

            Button returnButton = new Button()
            {
                Text = "Return",
                BackgroundColor = Color.DarkOrange,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(50)
            };
           returnButton.SetBinding(Button.CommandProperty, "ReturnCommand");
         
           foreach (string line in lines)
           {
               buttonsArr[i++] = new Button {Text = line, BackgroundColor = Color.DarkOrange, Margin = new Thickness(10)};
           }
           buttonsArr[0].SetBinding(Button.CommandProperty, "HotStripeCommand");
           buttonsArr[1].SetBinding(Button.CommandProperty, "ColdStripeCommand");
           buttonsArr[2].SetBinding(Button.CommandProperty, "OvenStripeCommand");
            
            //we can create all the options and then sort them by the file , save xml with the button and then load it up
            Content = new StackLayout
            {
                Children = 
                {
                    buttonsArr[0], 
                    buttonsArr[1],
                    buttonsArr[2],
                    returnButton
                }
            };
        }

        private string[] ReadStripes()
        {
            // read stripes file or let the user choose his stripes , by that sort the array
            string[] lines = new string[10];
            return lines;
        }
    }
}