using System.ComponentModel;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class OrderIDNotifier : INotifyPropertyChanged
    {
        private int m_status;
        private string m_StatusString;
        private Color m_StatusColor;
        public event PropertyChangedEventHandler PropertyChanged;

        public OrderIDNotifier(int i_ID, int i_Status)
        {
            OrderID = i_ID;
            m_status = i_Status;
            m_StatusColor = new Color();
            OrderStatusToString();
        }
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }
        protected void OnPropertyChanged(string propertyName)
        {

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// properties, some signed to event delegate
        /// </summary>
        public int Status
        {
            get => m_status;
            set
            {
                if (m_status == value) return;
                m_status = value;
                OrderStatusToString();
                OnPropertyChanged(nameof(Status));
            }
        }

        public string StatusString
        {
            get => m_StatusString;
            set
            {
                OrderStatusToString();
                OnPropertyChanged(nameof(StatusString));
            }
        }
        public int OrderID { get; set; }

        public Xamarin.Forms.Color StatusColor
        {
            get => m_StatusColor;
            set
            {
                m_StatusColor = value;
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public void OrderStatusToString() // expensive , try to swap it
        {
            string output = "Status : ";
            switch (Status)
            {
                case -1:
                {
                    output += "Locked";
                    m_StatusColor = Color.Firebrick;
                    break;
                }
                case 0:
                {
                    output += "Available";
                    m_StatusColor = Color.Gold;
                    break;
                }
                case 1:
                {
                    output += "In Progress";
                    m_StatusColor = Color.YellowGreen;
                    break;
                }
                case 2:
                {
                    output += "Holding";
                    m_StatusColor = Color.Firebrick;
                    break;
                }
                default:
                {
                    output += "Done";
                    m_StatusColor = Color.Chartreuse;
                    break;
                }
            }
            m_StatusString = output;
        }
    }
}
