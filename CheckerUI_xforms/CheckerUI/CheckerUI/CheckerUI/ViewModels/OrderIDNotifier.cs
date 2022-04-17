using System;
using System.ComponentModel;
using CheckerUI.Enums;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class OrderIDNotifier : INotifyPropertyChanged
    {
       
        private eOrderItemState m_state;
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime m_CreatedTime;

        public OrderIDNotifier()
        {
            
        }
        public OrderIDNotifier(int i_ID, eOrderItemState i_State)
        {
            OrderID = i_ID;
            m_state = i_State;
            StatusColor = new Color();
            OrderStatusToString();
            m_CreatedTime = new DateTime();
            m_CreatedTime = DateTime.Now;
        }
        
        /// <summary>
        /// properties, some signed to event delegate
        /// </summary>
        public eOrderItemState Status
        {
            get => m_state;
            set
            {
                if (m_state == value) return;
                m_state = value;
                OrderStatusToString();
               
            }
        }

        public string StatusString { get; set; }

        public int OrderID { get; set; }

        public Xamarin.Forms.Color StatusColor { get; set; }

        public string TimeLeftString => m_CreatedTime.ToString();

        public void OrderStatusToString() // expensive , try to swap it
        {
            string output = "";
            switch (Status)
            {
                case eOrderItemState.Waiting:
                {
                    output += "Waiting";
                    StatusColor = Color.Firebrick;
                    break;
                }
                case eOrderItemState.Available:
                {
                    output += "Available";
                    StatusColor = Color.Gold;
                    break;
                }
                case eOrderItemState.InPreparation:
                {
                    output += "In Preparation";
                    StatusColor = Color.DarkOrange;
                    break;
                }
                case eOrderItemState.Ready:
                {
                    output += "Ready";
                    StatusColor = Color.Firebrick;
                    break;
                }
                case eOrderItemState.Completed:
                {
                    output += "Completed";
                    StatusColor = Color.Firebrick;
                    break;
                }
                default:
                {
                    output += "Waiting";
                    StatusColor = Color.Chartreuse;
                    break;
                }
            }
            StatusString = output;
        }
    }
}
