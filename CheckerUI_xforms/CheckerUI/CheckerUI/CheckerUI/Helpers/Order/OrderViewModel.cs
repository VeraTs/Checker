using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using CheckerUI.Enums;
using CheckerUI.ViewModels;

namespace CheckerUI.Helpers.Order
{
    public class OrderViewModel : BaseViewModel
    {
        private OrderModel m_Order;
        private ObservableCollection<OrderItemView> m_itemsCollection = new ObservableCollection<OrderItemView>();
        private Color m_OrderColorByState;
       
        
        public OrderViewModel(OrderModel i_model)
        {
            m_Order = i_model;
            foreach (var itemModel in i_model.m_Items)
            {
                var itemView = new OrderItemView(itemModel);
                m_itemsCollection.Add(itemView);
            }

            OrderID = i_model.m_OrderID;
            OrderType = i_model.m_OrderType;
            TableNumber = i_model.m_TableNumber;
          
            m_OrderColorByState = new Color();
            setColorState();
        }

        private void setColorState()
        {
            switch (State)
            {
                case eOrderState.Pending:
                {
                    OrderStateColor = Color.Brown;
                    break;
                }
                case eOrderState.Started:
                {
                        OrderStateColor = Color.DarkOrange;
                    break;
                }
                case eOrderState.Done:
                {
                    OrderStateColor = Color.YellowGreen;
                        break;
                }
            }
        }
        public int OrderID
        {
            get => m_Order.m_OrderID;
            set
            {
                m_Order.m_OrderID = value;
                OnPropertyChanged(nameof(OrderID));
            }
        }

        public int TableNumber
        {
            get => m_Order.m_TableNumber;
            set
            {
                m_Order.m_TableNumber = value;
                OnPropertyChanged(nameof(TableNumber));
            } 
        }

        public string RemainingItems
        {
            get => "Remaining : " + Items.Count.ToString();
        }
        public string TableNumberString
        {
            get => "Table : " + m_Order.m_TableNumber.ToString();
            
        }

        public ObservableCollection<OrderItemView> Items
        {
            get => m_itemsCollection;
            set
            {
                m_itemsCollection = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public eOrderState State
        {
            get => m_Order.m_OrderState;
            set
            {
                m_Order.m_OrderState = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public eOrderType OrderType
        {
            get => m_Order.m_OrderType;
            set
            {
                m_Order.m_OrderType = value;
                OnPropertyChanged(nameof(OrderType));
            }
        }

        public Color OrderStateColor
        {
            get => m_OrderColorByState;
            set
            {
                m_OrderColorByState = value;
                OnPropertyChanged(nameof(OrderStateColor));
            }
        }
    }
}
