using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckerUI.ViewModels;
using Xamarin.Forms;

namespace CheckerUI.Views
{
    public class OvenStripePage : ContentPage
    {
        public OvenStripePage()
        {
            BindingContext = new OvenStripePageViewModel();
            BackgroundColor = Color.PowderBlue;
            this.Title = "Oven Stripe ";

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