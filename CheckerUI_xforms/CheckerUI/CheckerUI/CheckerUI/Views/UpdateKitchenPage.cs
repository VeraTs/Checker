using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckerUI.ViewModels;
using Xamarin.Forms;

namespace CheckerUI.Views
{
    public class UpdateKitchenPage : ContentPage
    {
        public UpdateKitchenPage()
        {
            BindingContext = new UpdateKitchenViewModel();
            BackgroundColor = Color.PowderBlue;
            this.Title = " Update Kitchen Menu";

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
