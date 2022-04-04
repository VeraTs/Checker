using CheckerUI.ViewModels;
using Xamarin.Forms;

//need to add login, fast login , onload when logged

namespace CheckerUI.Views // dynamic ui 
{
    public class KitchenPage : ContentPage
    {
     
        public KitchenPage()
        {
            this.Title = "Choose a Line ";
            BindingContext = new KitchenPageViewModel();
            BackgroundImageSource = "Checker_Logo";
            Button[] buttonsArr = new Button[10];
            string[] lines = { "Hot Line", "Cold Line", "Oven Line" };
            int i = 0;

            Button returnButton = new Button()
            {
                Text = "Return",
                FontFamily = "FAS",
                Margin = new Thickness(100,50,100,50)
            };
           returnButton.SetBinding(Button.CommandProperty, "ReturnCommand");
         
           foreach (string line in lines)
           {
               buttonsArr[i++] = new Button {Text = line, FontFamily = "FAS", Margin = new Thickness(10,5,10,5)};
           }
           buttonsArr[0].SetBinding(Button.CommandProperty, "HotLineCommand");
           buttonsArr[1].SetBinding(Button.CommandProperty, "ColdLineCommand");
           buttonsArr[2].SetBinding(Button.CommandProperty, "OvenLineCommand");

            //we can create all the options and then sort them by the file ,
            //save xml with the button and then load it up

            var inner = new StackLayout()
            {
                Children =
                {
                    buttonsArr[0],
                    buttonsArr[1],
                    buttonsArr[2],
                    returnButton
                }
            };
            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children = { inner }
            };
            Content = layout;




        }

        private string[] ReadAndSortLines()
        {
            // read stripes file or let the user choose his stripes , by that sort the array
            string[] lines = new string[10];
            return lines;
        }
    }
}