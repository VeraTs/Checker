using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CheckerUI.Helpers.Order
{
    internal class OrderItemModel : INotifyPropertyChanged
    {
        public int m_OrderItemID { get; set; }
        public string m_OrderItemName { get; set; }   
        public int m_Status { get; set; }
        public string m_Description { get; set; }
        public int m_DeptID { get; set; }
        public int m_TableNumber { get; set; }
        public DateTime m_StartDate { get; set; }
        public DateTime m_CreatedDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
           return  m_OrderItemName+" , ID :" + m_OrderItemID.ToString();
        }
    }
}
