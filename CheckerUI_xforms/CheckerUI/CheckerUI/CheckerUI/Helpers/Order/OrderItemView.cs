using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly OrderItemModel m_orderItem;

        private bool m_OrderRestored = false;
        // Commands
        public Command StopCommand { get; }
        public Command StartCommand { get; set; }
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
            OrderNotifier = idNotifier;
            ReadyCommand = new Command(() =>
            {
                if (OrderStatus == 0)
                {
                    OrderStatus = 1;
                    OrderStatusColor = idNotifier.StatusColor;
                    OrderStatusString = idNotifier.StatusString;
                }
            }); // this command will bind to timer , idNotifier is already a listener

            StopCommand = new Command(() =>
            {
                if(OrderStatus != -1)
                {
                    OrderStatus = 2;
                    OrderStatusString = idNotifier.StatusString;
                    OrderStatusColor = idNotifier.StatusColor;
                }
            });

            StartCommand = new Command(() =>
            {
                if (OrderStatus != -1)
                {
                    OrderStatus = 1;
                    OrderStatusColor = idNotifier.StatusColor;
                    OrderStatusString = idNotifier.StatusString;
                }
            });
            DoneCommand = new Command(() =>
            {
                if (IsInProgress)
                {
                    OrderStatus = 3;
                    OrderStatusColor = idNotifier.StatusColor;
                    OrderStatusString = idNotifier.StatusString;
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
                OrderStatusColor = idNotifier.StatusColor;
                OrderStatusString = idNotifier.StatusString;
            });
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

        public string OrderItemTimeLeft
        {
            get => m_orderItem.m_ID_Status_Notifier.TimeLeftString;
            set
            {
                m_orderItem.m_ID_Status_Notifier.TimeLeftString = value;
                OnPropertyChanged(nameof(OrderItemTimeLeft));
            }
        }
        public string OrderItemName
        {
            get => m_orderItem.m_OrderItemName;
            private set => m_orderItem.m_OrderItemName = value;
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

        public string OrderItemDescription
        {
            get => m_orderItem.m_Description;
            set
            {
                m_orderItem.m_Description = value;
                OnPropertyChanged(nameof(OrderItemDescription));
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
