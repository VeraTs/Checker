using CheckerUI.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerUI.Models;
using Xamarin.Forms;

//Here we produce the display of a single order item in its line, the grid consists of three rows (currently)
//In the first row, the name of the dish and an ID card
//In the second row, the status of the dish varies depending on the button being pressed
//And in the third row three buttons are linked to the command respectively
//An order item knows to update those responsible for a change in its status
namespace CheckerUI.ViewModels
{
    public class OrderItemViewModel : BaseViewModel
    {
        public OrderItemViewModel(OrderItem i_item)
        {
            feelColorsState();
            
            m_orderItem = i_item;
            m_orderItem.dish = i_item.dish;
            OrderItemLineStatus = i_item.lineStatus;
            OderItemID = i_item.id;
            OrderItemName = i_item.dish.name;
            OrderItemDescription = "Note :" + i_item.changes;
            FirstTimeToShowString = OrderItemTimeCreate;
        }

        private void feelColorsState()
        {
           m_StateColors.Add(eLineItemStatus.Locked, Color.SeaGreen);
           m_StateColors.Add(eLineItemStatus.ToDo, Color.Coral);
           m_StateColors.Add(eLineItemStatus.Doing, Color.LightGoldenrodYellow);
           m_StateColors.Add(eLineItemStatus.Done, Color.DarkGreen);
        }
        public int OderItemID
        {
            get => m_orderItem.id;
            set => m_orderItem.id = value;
        }
        public int Table { get; set; } = 0;

        public eLineItemStatus OrderItemLineStatus
        {
            get => m_orderItem.lineStatus;
            set
            {
                m_orderItem.lineStatus = value;
                OrderStatusString = value.ToString();
                OrderStatusColor = m_StateColors[OrderItemLineStatus];
            } 
        }
        //Properties
        private readonly OrderItem m_orderItem;
        private readonly Dictionary<eLineItemStatus, Color> m_StateColors = new Dictionary<eLineItemStatus, Color>();

        public bool isRestored { get; set; } = false;
        public string OrderStatusString { get; set; }

        public string FirstTimeToShowString { get; set; }
        public Color OrderStatusColor { get; set; } = new Color();
        public eDishType OrderItemType => m_orderItem.dish.type;
        public ObservableCollection<int> OrderStatusChangedNotifier { get; set; } = new ObservableCollection<int>();
        public string OrderItemTimeCreate => "Created: " + m_orderItem.createdDate.ToString("hh:mm tt");
        public string OrderItemTimeStartedString => "Started: " + m_orderItem.startDate.ToString("hh: mm tt");

        public string OrderItemTimeDoneString => "Done: " + m_orderItem.doneDate.ToString("hh:mm tt");

        public DateTime OrderItemTimeStarted
        {
            get => m_orderItem.createdDate;
            set
            {
                m_orderItem.createdDate = value;
                OnPropertyChanged(nameof(OrderItemTimeStarted));
            }
        }
        
        public DateTime OrderItemTimeDone
        {
            get => m_orderItem.doneDate;
            set
            {
                m_orderItem.doneDate = value;
                OnPropertyChanged(nameof(OrderItemTimeStarted));
            }
        }

        public string OrderItemName
        {
            get => m_orderItem.dish.name;
            private set => m_orderItem.dish.name = value;
        }

        public string OrderItemDescription
        {
            get => m_orderItem.changes;
            set => m_orderItem.changes = value;
        }
    }
}
