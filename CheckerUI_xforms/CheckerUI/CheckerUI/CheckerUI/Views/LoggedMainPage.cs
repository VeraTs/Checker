using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckerUI.ViewModels;
using Xamarin.Forms;

namespace CheckerUI.Views
{
    internal class LoggedMainPage : ContentPage
    {
        public LoggedMainPage()
        {
            BindingContext = new LoggedMainPageViewModel();
            this.Title = " Options Menu ";
            BackgroundColor = Color.PowderBlue;

            Button enterKitchenButton = new Button()
            {
                Text = "Kitchen",
                BackgroundColor = Color.DarkOrange,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(20)
            };
            enterKitchenButton.SetBinding(Button.CommandProperty, "MyKitchenCommand");

            Button updateButton = new Button()
            {
                Text = "Update Kitchen",
                BackgroundColor = Color.DarkOrange,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(20)
            };
            updateButton.SetBinding(Button.CommandProperty, "UpdateKitchenCommand");

            Button mangeIngButton = new Button()
            {
                Text = "Mange Ingredients",
                BackgroundColor = Color.DarkOrange,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(20)
            };
            mangeIngButton.SetBinding(Button.CommandProperty, "MangeKitchenCommand");

            Button showStatisticsButton = new Button()
            {
                Text = "Show Statistics",
                BackgroundColor = Color.DarkOrange,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(20)
            };
            showStatisticsButton.SetBinding(Button.CommandProperty, "ShowStatisticsCommand");

            Button returnButton = new Button()
            {
                Text = "Exit",
                BackgroundColor = Color.OrangeRed,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(20),
            };
            returnButton.SetBinding(Button.CommandProperty, "ReturnCommand");

            Content = new StackLayout
            {
                Padding = new Thickness(20, 20, 20, 20),
                
                Children =
                {
                    enterKitchenButton,
                    updateButton,
                    mangeIngButton,
                    showStatisticsButton,
                    returnButton
                }
            };
        }
    }
}