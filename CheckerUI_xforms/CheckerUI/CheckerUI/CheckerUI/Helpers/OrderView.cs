using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheckerUI.Helpers
{
    public class OrderView
    {
        private Grid m_Grid { get; set; }
       
        private readonly List<Label> m_Labels;
        private int m_OrderID { get; set; }
        private readonly ImageButton m_DoneButton;
        private readonly ImageButton m_StartButton;
        private readonly ImageButton m_StopButton;
        public Command StopCommand { get; }
        public Command StartCommand { get; }
        public Command DoneCommand { get; }

        private int m_OrderStatus = 0;

        public OrderView(int i_ID)
        {
            m_OrderID = i_ID;
            m_Grid = new Grid
            {
                Margin = new Thickness(5, 5, 5, 5),
                IsVisible   = false,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(){Width = new GridLength(2, GridUnitType.Star)},
                    new ColumnDefinition(){Width = new GridLength(2, GridUnitType.Star)},
                    new ColumnDefinition(){Width = new GridLength(2, GridUnitType.Star)}
                },
                BackgroundColor = Color.CadetBlue
            };

            StopCommand = new Command(() =>
            {
                m_Labels[0].Text = " Stopped ";
                m_OrderStatus = 2;
            });

            StartCommand = new Command(() =>
            {
                m_Labels[0].Text = " In progress ";
                m_OrderStatus = 1;
            });
            DoneCommand = new Command(() =>
            {
                if (m_OrderStatus != 0)
                {
                    m_Labels[0].Text = "Done";
                    m_Grid.IsVisible = false;
                    m_OrderStatus = 3;
                }
                else
                {
                    //display alert
                }
            });

            m_Labels = new List<Label>();
            m_Labels.Add(new Label { Text = "Status: " });
            m_Labels.Add(new Label { Text = "Table: " + m_OrderID });
            m_Labels.Add(new Label { Text = "Description: " });

             m_DoneButton = new ImageButton {Source = "icon_Checked", HorizontalOptions = LayoutOptions.Center, Command = DoneCommand,};
            // doneButton.Clicked += doneButton_Clicked;
             m_StartButton = new ImageButton { Source = "icon_Start", HorizontalOptions = LayoutOptions.Center, Command = StartCommand };
            //  startButton.Clicked += StartButton_Clicked;
             m_StopButton = new ImageButton { Source = "icon_Hold", HorizontalOptions = LayoutOptions.Center, Command = StopCommand };
          
            GenerateGrid();
            //  stopButton.Clicked += StopButton_Clicked;
        }

        public void GenerateGrid()
        {
     //       m_Grid.IsVisible = false;
           
            m_Grid.Children.Add(m_Labels[0], 0, 2,0,1); 
            m_Grid.Children.Add(m_Labels[1],0,2,1,2); 
            m_Grid.Children.Add(m_Labels[2],0,2,2,3); 
            m_Grid.Children.Add(m_DoneButton,0,1,3,4);
            m_Grid.Children.Add(m_StartButton,1,2,3,4);
            m_Grid.Children.Add(m_StopButton,2,3,3,4);
           
        }

        public Grid OderGrid
        {
            get => m_Grid;
            private set => m_Grid = value;
        }
        public int OderID
        {
            get => m_OrderID;
            set => m_OrderID = value;
        }
        //replace with enum !
        public bool IsStarted => m_OrderStatus != 0;
        public bool IsInProgress => m_OrderStatus == 1;
        public bool IsHolding => m_OrderStatus == 2;
        public bool IsCompleted => m_OrderStatus == 3;
       
       
    }
}
