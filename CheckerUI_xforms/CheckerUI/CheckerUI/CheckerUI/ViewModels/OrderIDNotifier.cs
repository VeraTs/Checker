using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CheckerUI.ViewModels
{
    public class OrderIDNotifier : INotifyPropertyChanged
    {
        private int m_OrderID;
        private int m_status;
        public OrderIDNotifier(int i_ID, int i_Status)
        {
            m_OrderID = i_ID;
            m_status = i_Status;
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

        public int Status
        {
            get => m_status;
            set
            {
                if (m_status != value)
                {
                    m_status = value;
                    OnPropertyChanged(nameof(m_OrderID));
                }
            }
        }
        public int OrderID
        {
            get =>  m_OrderID; 
            set => m_OrderID = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
