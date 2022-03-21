using CheckerUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineView : ContentPage
    {
        private Button buttonToMake;
        private Button buttonInProgress;
        private List<GridAndButton> gridsButtonsList = new List<GridAndButton>(10);
        bool isVisible = false;

        public LineView(string i_Title)
        {
            InitializeComponent();
            BindingContext = new BaseLineViewModel();
            this.Title = i_Title;
            BackgroundColor = Color.PowderBlue;
            GridAndButton gridAndButton = null;
            double delta = 0;
            for (int i = 0; i < 10; i++)
            {
                if (i > 2)
                {
                    buttonToMake = new Button();
                    buttonToMake.Text = i.ToString();
                    buttonToMake.Clicked += Button_Clicked;
                    buttonToMake.CornerRadius = 10;
                    buttonToMake.BackgroundColor = Color.Gray;
                    AL.Children.Add(buttonToMake,
                    new Rectangle(0.1, 0.08 + delta, 0.4, 0.08),
                    AbsoluteLayoutFlags.All);
                    gridAndButton = new GridAndButton(buttonToMake, AL);
                    gridsButtonsList.Add(gridAndButton);
                    AL.Children.Add(gridAndButton.Grid,
                    new Rectangle(0, 0.08, 0.5, 0.28),
                    AbsoluteLayoutFlags.All);
                }
                buttonInProgress = new Button();
                buttonInProgress.Text = (i + 10).ToString();
                buttonInProgress.Clicked += Button_Clicked;
                buttonInProgress.CornerRadius = 10;
                buttonInProgress.BackgroundColor = Color.Gray;
                AL.Children.Add(buttonInProgress,
                    new Rectangle(0.95, 0.08 + delta, 0.4, 0.08),
                    AbsoluteLayoutFlags.All);
                delta = delta + 0.1;
                gridAndButton = new GridAndButton(buttonInProgress, AL);
                gridsButtonsList.Add(gridAndButton);
                AL.Children.Add(gridAndButton.Grid,
                    new Rectangle(0, 0.08, 0.5, 0.28),
                    AbsoluteLayoutFlags.All);
            }
        }
        private void Button_Clicked(object sender, EventArgs e)
        {

            Grid currentGrid = null;
            foreach (GridAndButton gridAndButton in gridsButtonsList)
            {
                if (gridAndButton.Button == sender as Button)
                {
                    currentGrid = gridAndButton.Grid;
                }
            }

            if ((sender as Button).BackgroundColor == Color.CadetBlue)
            {
                currentGrid.IsVisible = isVisible = false;
                (sender as Button).BackgroundColor = Color.Gray;
            }
            else if (isVisible == false)
            {
                (sender as Button).BackgroundColor = currentGrid.BackgroundColor;
                currentGrid.IsVisible = isVisible = true;

            }
            else
            {
                DisplayAlert("Alert", "You have been alerted", "OK");

            }
        }


    }
    public class GridAndButton
    {
        AbsoluteLayout AbsoluteLayout { get; set; }
        public Button Button { get; set; }
        public Grid Grid { get; set; }

        public GridAndButton(Button i_Button, AbsoluteLayout i_AL)
        {
            Button = i_Button;
            AbsoluteLayout = i_AL;
            Label statusLabel = new Label { Text = "Status : ", };
            Label tabaleNumber = new Label { Text = "Tabel number : " };
            Label commentsLabel = new Label { Text = "Commnets : " };
            Button doneButton = new Button { Text = "Done" };
            doneButton.Clicked += doneButton_Clicked;
            Button startButton = new Button { Text = "Start" };
            startButton.Clicked += StartButton_Clicked;
            Button stopButton = new Button { Text = "Stop" };
            stopButton.Clicked += StopButton_Clicked;
            Grid = new Grid
            {
                RowDefinitions =
                {
                     new RowDefinition{Height=new GridLength(2 ,GridUnitType.Star) },
                },
                IsVisible = false,
                BackgroundColor = Color.CadetBlue,

            };
            Grid.SetColumnSpan(commentsLabel, 3);
            Grid.SetRow(commentsLabel, 0);
            Grid.Children.Add(commentsLabel);
            Grid.SetColumnSpan(statusLabel, 3);
            Grid.SetRow(statusLabel, 1);
            Grid.Children.Add(statusLabel);
            Grid.SetColumnSpan(tabaleNumber, 3);
            Grid.SetRow(tabaleNumber, 2);
            Grid.Children.Add(tabaleNumber);
            Grid.SetColumn(doneButton, 0);
            Grid.SetRow(doneButton, 3);
            Grid.Children.Add(doneButton);
            Grid.SetColumn(startButton, 1);
            Grid.SetRow(startButton, 3);
            Grid.Children.Add(startButton);
            Grid.SetColumn(stopButton, 2);
            Grid.SetRow(stopButton, 3);
            Grid.Children.Add(stopButton);

        }

        private void StopButton_Clicked(object sender, EventArgs e)
        {
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            foreach (var child in Grid.Children)
            {
                if (child is Label)
                {
                    if (string.Equals((child as Label).Text.Substring(0, 6), "Status"))
                    {
                        (child as Label).Text = (child as Label).Text.Insert(8, " In Progress");
                    }
                }
            }
        }
        private void doneButton_Clicked(object sender, EventArgs e)
        {
            AbsoluteLayout.Children.Remove(Button);
            Grid.IsVisible = false;
            AbsoluteLayout.Children.Remove(Grid);
        }
    }
}