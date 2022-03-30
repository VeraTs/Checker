using System;
using System.ComponentModel;
using CheckerUI.ViewModels;

namespace CheckerUI.Helpers.Order
{
    // an order model combined with a listener on status (ID,Status)
    internal class OrderItemModel : INotifyPropertyChanged
    {
        public string m_OrderItemName { get; set; }
        public string m_Description { get; set; }
        public int m_DeptID { get; set; }
        public int m_TableNumber { get; set; }
        public DateTime m_StartDate { get; set; }
        public DateTime m_CreatedDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public OrderIDNotifier m_ID_Status_Notifier { get; set; }
        public override string ToString()
        {
           return  m_OrderItemName+" , ID :" + m_ID_Status_Notifier.OrderID.ToString();
        }
    }
}
