using CheckerUI.Enums;
using CheckerUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

//Here we produce the display of a single order item in its line, the grid consists of three rows (currently)
//In the first row, the name of the dish and an ID card
//In the second row, the status of the dish varies depending on the button being pressed
//And in the third row three buttons are linked to the command respectively
//An order item knows to update those responsible for a change in its status


// To Do:  Status should be changed to Enum
namespace CheckerUI.Helpers.Order
{
    public class OrderItemView : BaseViewModel
    {
        //Properties
        private readonly OrderItemModel m_orderItem;
        
        // Commands
       
        private readonly Dictionary<eOrderItemState,Color> m_StateColors = new Dictionary<eOrderItemState,Color>();

        public OrderItemView()
        {

        }

        public OrderItemView(OrderItemModel i_item)
        {
            feelColorsState();
            m_orderItem = new OrderItemModel();
            m_orderItem = i_item;
            OrderStatus = i_item.m_State;
            OderID = i_item.m_OrdrID;
            OrderItemName = i_item.m_OrderItemName;
            OrderItemDescription = i_item.m_Description;
            FirstTimeToShowString = OrderItemTimeCreate;

        }

        private void feelColorsState()
        {
           m_StateColors.Add(eOrderItemState.Waiting, Color.Firebrick);
           m_StateColors.Add(eOrderItemState.Available, Color.Gold);
           m_StateColors.Add(eOrderItemState.InPreparation, Color.DarkOrange);
           m_StateColors.Add(eOrderItemState.Ready, Color.DarkGreen);
           m_StateColors.Add(eOrderItemState.Completed, Color.YellowGreen);
        }
        public int OderID
        {
            get => m_orderItem.m_OrdrID;
            set => m_orderItem.m_OrdrID = value;
        }

        public eOrderItemState OrderStatus
        {
            get => m_orderItem.m_State;
            set
            {
                m_orderItem.m_State = value;
                OrderStatusString = value.ToString();
                OrderStatusColor = m_StateColors[OrderStatus];
                OnPropertyChanged(nameof(OrderStatus));
            } 
        }

    
        public bool isRestored { get; set; } = false;

        public string OrderStatusString { get; set; }

       

        public DateTime OrderItemTimeStarted
        {
            get => m_orderItem.m_StartDate;
            set
            {
                m_orderItem.m_StartDate = value;
                OnPropertyChanged(nameof(OrderItemTimeStarted));
            }
        }

        public string FirstTimeToShowString { get; set; }

        public string OrderItemTimeCreate => "Created: " + m_orderItem.m_CreatedDate.ToString("hh:mm tt");
        public string OrderItemTimeStartedString => "Started: "+m_orderItem.m_StartDate.ToString(" hh: mm tt");

        public string OrderItemTimeDoneString => "Done: " + m_orderItem.m_DoneDate.ToString("hh:mm tt");
        

        public DateTime OrderItemTimeDone
        {
            get => m_orderItem.m_DoneDate;
            set
            {
                m_orderItem.m_DoneDate = value;
                OnPropertyChanged(nameof(OrderItemTimeStarted));
            }
        }


        public string OrderItemName
        {
            get => m_orderItem.m_OrderItemName;
            private set => m_orderItem.m_OrderItemName = value;
        }

        public Color OrderStatusColor { get; set; } = new Color();
        public string OrderItemDescription
        {
            get => m_orderItem.m_Description;
            set => m_orderItem.m_Description = value;
        }
        public eOrderItemType OrderItemType => m_orderItem.m_ItemType;
        //replace with enum !

        public ObservableCollection<int> OrderStatusChangedNotifier { get; set; } = new ObservableCollection<int>();

        internal OrderItemModel CreateItemModel(int i_OrderID,string i_Name, int i_Table, string i_Desc,
            int i_LineID, eOrderItemType i_Type, eOrderItemState i_State)
        {
            return OrderItemBuilder.GenerateOrderItem(i_OrderID,i_Name, i_Table, i_Desc, i_LineID, i_Type, i_State);
        }

        

    }
}
