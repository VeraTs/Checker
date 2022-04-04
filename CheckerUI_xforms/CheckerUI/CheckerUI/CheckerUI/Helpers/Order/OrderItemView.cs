using System.Collections.Generic;
using CheckerUI.Helpers.Order;
using CheckerUI.Models;
using CheckerUI.ViewModels;
using Xamarin.Forms;
using FontAwesome;

//Here we produce the display of a single order item in its line, the grid consists of three rows (currently)
//In the first row, the name of the dish and an ID card
//In the second row, the status of the dish varies depending on the button being pressed
//And in the third row three buttons are linked to the command respectively
//An order item knows to update those responsible for a change in its status


// To Do:  Status should be changed to Enum
namespace CheckerUI.Helpers
{
    public class OrderItemView : BaseViewModel
    {
        //Properties
        private Grid m_Grid { get; set; }
        private readonly List<Label> m_Labels;
        private readonly OrderItemModel m_orderItem;
        private readonly Button m_DoneButton;
        private readonly Button m_StartButton;
        private readonly Button m_StopButton;
        private readonly Button m_RestoreButton;

        private bool m_OrderRestored = false;
        // Commands
        public Command StopCommand { get; }
        public Command StartCommand { get; }
        public Command DoneCommand { get; }
        public Command ReadyCommand { get; }
        public Command RestoreCommand { get; }

        public OrderItemView()
        {

        }
        public OrderItemView(string i_Name, OrderIDNotifier idNotifier)
        {
           
            m_orderItem = new OrderItemModel();
            m_orderItem = CreateItemView( i_Name,  100, "Notes ...", 1, idNotifier);
            CreateGrid();
            ReadyCommand = new Command(() =>
            {
                if (OrderStatus == 0)
                {
                    OrderStatus = 1;
                    m_Labels[2].Text = idNotifier.StatusString;
                    OrderStatusColor = idNotifier.StatusColor;
                    OrderStatusString = idNotifier.StatusString;
                }
            }); // this command will bind to timer , idNotifier is already a listener

            StopCommand = new Command(() =>
            {
                if(OrderStatus != -1)
                {
                    OrderStatus = 2;
                    m_Labels[2].Text = idNotifier.StatusString;
                    OrderStatusColor = idNotifier.StatusColor;
                    OrderStatusString = idNotifier.StatusString;
                }
            });

            StartCommand = new Command(() =>
            {
                if (OrderStatus != -1)
                {
                    OrderStatus = 1;
                    m_Labels[2].Text = idNotifier.StatusString;
                    OrderStatusColor = idNotifier.StatusColor;
                    OrderStatusString = idNotifier.StatusString;
                }
                else
                {
                    // need to think about that case
                }
            });
            DoneCommand = new Command(() =>
            {
                if (IsInProgress)
                {
                    OrderStatus = 3;
                    m_Labels[2].Text = idNotifier.StatusString;
                    OrderStatusColor = idNotifier.StatusColor;
                    OrderStatusString = idNotifier.StatusString;
                    removeGridButtons();
                    m_Grid.Children.Add(m_RestoreButton, 0, 1, 5, 6);
                    m_OrderRestored = true;
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Order Locked", m_orderItem.m_OrderItemName, "Ok");
                }
            });
            RestoreCommand = new Command(() =>
            {
                idNotifier.Status = 1;
                OrderStatus = 1;
                m_Labels[2].Text = idNotifier.StatusString;
                OrderStatusColor = idNotifier.StatusColor;
                OrderStatusString = idNotifier.StatusString;
                m_Grid.Children.Remove(m_RestoreButton);
                addGridButtons();
            });
            m_Labels = new List<Label>();
            m_Labels.Add(new Label
            {
                Text = "Type :" + m_orderItem.m_OrderItemName,
                FontFamily = "FAS",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            m_Labels.Add(new Label
            {
                Text = "ID : " + m_orderItem.m_ID_Status_Notifier.OrderID,
                FontFamily = "FAS",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            });
            m_Labels.Add(new Label
            {
                Text =   m_orderItem.m_ID_Status_Notifier.StatusString ,
                FontFamily = "FAS",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                BindingContext = m_orderItem.m_ID_Status_Notifier.StatusString
            });
            m_Labels.Add(new Label
            {
                Text = "Time Left :",
                FontFamily = "FAS",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            m_Labels.Add(new Label
            {
                Text = "Description :" + m_orderItem.m_Description, FontFamily = "FAS",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            });
            m_DoneButton = new Button
            {
                Text = FontAwesomeIcons.Check,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontFamily = "FAS",
                Command = DoneCommand
            };

            m_StartButton = new Button
            {
                Text = FontAwesomeIcons.Play,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontFamily = "FAS",
                Command = StartCommand
            };
            
            m_StopButton = new Button
            {
                Text = FontAwesomeIcons.Stop,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontFamily = "FAS",
                Command = StopCommand
            };
            m_RestoreButton = new Button()
            {
                Text = FontAwesomeIcons.Undo,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontFamily = "FAS",
                Command = RestoreCommand
            };
            GenerateGrid();
        }
        public void GenerateGrid()
        {
            int i = 0;
            foreach (var label in m_Labels)
            {
                m_Grid.Children.Add(label, 0, 3, i, i+1);
                i++;
            }

            addGridButtons();
        }

        private void CreateGrid()
        {
            m_Grid = new Grid
            {
                Margin = new Thickness(-1, 0, 0, 0),
                IsVisible = false,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(2, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(2, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(2, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(2, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(2, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(2, GridUnitType.Star)},

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition() {Width = new GridLength(2, GridUnitType.Star)},
                    new ColumnDefinition() {Width = new GridLength(2, GridUnitType.Star)},
                    new ColumnDefinition() {Width = new GridLength(2, GridUnitType.Star)}
                },
                BackgroundColor = Color.CadetBlue
            };
        }

        private void removeGridButtons()
        {
            m_Grid.Children.Remove(m_DoneButton);
            m_Grid.Children.Remove(m_StopButton);
            m_Grid.Children.Remove(m_StartButton);
        }

        private void addGridButtons()
        {
            m_Grid.Children.Add(m_DoneButton, 0, 1, 5, 6);
            m_Grid.Children.Add(m_StartButton, 1, 2, 5, 6);
            m_Grid.Children.Add(m_StopButton, 2, 3, 5, 6);
        }
        public Grid OderGrid
        {
            get => m_Grid;
            private set => m_Grid = value;
        }

        public int OderID
        {
            get => m_orderItem.m_ID_Status_Notifier.OrderID;
            set => m_orderItem.m_ID_Status_Notifier.OrderID = value;
        }

        public int OrderStatus
        {
            get => m_orderItem.m_ID_Status_Notifier.Status;
            set => m_orderItem.m_ID_Status_Notifier.Status = value;
        }

        public bool isRestored
        {
            get => m_OrderRestored;
            set => m_OrderRestored = value;
        }
        public string OrderStatusString
        {
            get => m_orderItem.m_ID_Status_Notifier.StatusString;
            set
            {
                m_orderItem.m_ID_Status_Notifier.StatusString = value;
                OnPropertyChanged(nameof(OrderStatusString));
            }
        }

        public string OrderItemName
        {
            get => m_orderItem.m_OrderItemName;
            private set => m_orderItem.m_OrderItemName = value;
        }
        public Button OrderButton
        {
            get => m_orderItem.m_OrderButton;
            set => m_orderItem.m_OrderButton = value;
        }

        public OrderIDNotifier OrderNotifier
        {
            get => m_orderItem.m_ID_Status_Notifier;
            set => m_orderItem.m_ID_Status_Notifier = value;
        }

        public Color OrderStatusColor
        {
            get => m_orderItem.m_ID_Status_Notifier.StatusColor;
            set
            {
                m_orderItem.m_ID_Status_Notifier.StatusColor = value;
                OnPropertyChanged(nameof(OrderStatusColor));
            }
        }

        //replace with enum !
        public bool IsLocked => OrderStatus < 0;
        public bool IsAvailable => OrderStatus == 0;
        public bool IsInProgress => OrderStatus == 1;
        public bool IsHolding => OrderStatus == 2;
        public bool IsCompleted => OrderStatus == 3;

        internal OrderItemModel CreateItemView(string i_Name, int i_Table, string i_Desc,
            int i_DeptID, OrderIDNotifier i_Notifier)
        {
            return OrderItemBuilder.GenerateOrderItem(i_Name, i_Table, i_Desc, i_DeptID, i_Notifier);
        }
    }
}
