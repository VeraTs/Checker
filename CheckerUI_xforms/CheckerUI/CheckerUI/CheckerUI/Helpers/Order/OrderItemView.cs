using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CheckerUI.Helpers.Order;
using CheckerUI.ViewModels;
using Xamarin.Forms;
using FontAwesome;
namespace CheckerUI.Helpers
{
    public class OrderItemView : BaseViewModel
    {
        private Grid m_Grid { get; set; }
        public Button m_Button { get; set; }
       
        private readonly List<Label> m_Labels;
        
        private readonly Button m_DoneButton;
        private readonly Button m_StartButton;
        private readonly Button m_StopButton;
        public Command StopCommand { get; }
        public Command StartCommand { get; }
        public Command DoneCommand { get; }

        private OrderItemModel m_orderItem;
        private string m_OrderStatusText;
        public OrderItemView(int i_ID, string i_Name,Button i_Button)
        {
            m_Button = new Button();
            m_Button = i_Button;
            m_orderItem = new OrderItemModel();
            m_orderItem = CreateItemView(i_ID, i_Name, -1, 100,"some notes", 1);
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
                OrderStatus = 2;
                m_OrderStatusText = OrderStatusToString();
            });

            StartCommand = new Command(() =>
            {
                m_Labels[0].Text = " In progress ";
                OrderStatus = 1;
                m_OrderStatusText = OrderStatusToString();
            });
            DoneCommand = new Command(() =>
            {
                if (OrderStatus > 0)
                {
                    m_Labels[0].Text = "Done";
                    m_Grid.IsVisible = false;
                    OrderStatus = 3;
                    m_OrderStatusText = OrderStatusToString();
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Order Locked", m_orderItem.m_OrderItemName, "Ok");
                }
            });
            m_OrderStatusText = this.OrderStatusToString();
            m_Labels = new List<Label>();
            m_Labels.Add(new Label
            { 
                Text = m_OrderStatusText, FontFamily = "FAS",BindingContext = m_OrderStatusText,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            m_Labels.Add(new Label
            {
                Text = "Table : " + m_orderItem.m_TableNumber, FontFamily = "FAS",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            m_Labels.Add(new Label
            {
                Text = "Description :" + m_orderItem.m_Description, FontFamily = "FAS",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            m_DoneButton = new Button
            {
                Text = FontAwesomeIcons.Check,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                FontFamily = "FAS",
                Command = DoneCommand
            };
            
            // doneButton.Clicked += doneButton_Clicked;
             m_StartButton = new Button
             {
                 Text = FontAwesomeIcons.Play,
                 HorizontalOptions = LayoutOptions.StartAndExpand,
                 VerticalOptions = LayoutOptions.StartAndExpand,
                 BackgroundColor = Color.Transparent,
                 FontFamily = "FAS",
                 Command = StartCommand
             };
            //  startButton.Clicked += StartButton_Clicked;
             m_StopButton = new Button
             {
                 Text = FontAwesomeIcons.Stop, 
                 HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand,
                 BackgroundColor = Color.Transparent,
                FontFamily = "FAS",
                Command = StopCommand
             };
          
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
            get => m_orderItem.m_OrderItemID;
            set => m_orderItem.m_OrderItemID = value;
        }

        public int OrderStatus
        {
            get => m_orderItem.m_Status;
            set => m_orderItem.m_Status = value;
        }

        public string OrderStatusToString()
        {
            string output = "";
            switch (m_orderItem.m_Status)
            {
                case -1:
                {
                    output += "Locked";
                    break;
                }
                case 0:
                {
                    output += "Available";
                    break;
                }
                case 1:
                {
                    output += "In Progress";
                    break;
                }
                default:
                {
                    output += "Done";
                    break;
                }
            }

            return output;
        }
        //replace with enum !
        public bool IsLocked => OrderStatus < 0;
        public bool IsStarted => OrderStatus > 0;
        public bool IsInProgress => OrderStatus == 1;
        public bool IsHolding => OrderStatus == 2;
        public bool IsCompleted => OrderStatus == 3;

        internal OrderItemModel CreateItemView(int i_ID, string i_Name, int i_Status, int i_Table,string i_Desc, int i_DeptID)
        {
            return OrderItemBuilder.GenerateOrderItem(i_ID, i_Name, i_Status, i_Table,i_Desc, i_DeptID);
        }
    }
}
