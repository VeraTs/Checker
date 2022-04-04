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
            BackgroundImageSource = "Checker_Logo";

            Button enterKitchenButton = new Button()
            {
                Text = "Kitchen",
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(5)
            };
            enterKitchenButton.SetBinding(Button.CommandProperty, "MyKitchenCommand");

            Button updateButton = new Button()
            {
                Text = "Update Kitchen",
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(5)
            };
            updateButton.SetBinding(Button.CommandProperty, "UpdateKitchenCommand");

            Button mangeIngButton = new Button()
            {
                Text = "Mange Ingredients",
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(5)
            };
            mangeIngButton.SetBinding(Button.CommandProperty, "MangeKitchenCommand");

            Button showStatisticsButton = new Button()
            {
                Text = "Show Statistics",
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(5)
            };
            showStatisticsButton.SetBinding(Button.CommandProperty, "ShowStatisticsCommand");

            Button returnButton = new Button()
            {
                Text = "Exit",
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(5),
            };
            returnButton.SetBinding(Button.CommandProperty, "ReturnCommand");

            var inner = new StackLayout()
            {
                Children =
                {
                    enterKitchenButton,
                    updateButton,
                    mangeIngButton,
                    showStatisticsButton,
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
    }
}