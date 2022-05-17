using System.Collections.ObjectModel;
using System.Drawing;
using CheckerUI.Enums;
using CheckerUI.ViewModels;

namespace CheckerUI.Helpers.Order
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly OrderModel m_Order;
        private string m_RemainItemsString;

        public OrderViewModel(OrderModel i_model)
        {
            m_Order = i_model;
            foreach (var itemModel in i_model.m_Items)
            {
                var itemView = new OrderItemView(itemModel);
                Items.Add(itemView);
            }

            OrderID = i_model.m_OrderID;
            OrderType = i_model.m_OrderType;
            TableNumber = i_model.m_TableNumber;

            RemainingItems = Items.Count.ToString();
            OrderStateColor = new Color();
            setColorState();
        }

        // here we are updating the order items list ,
        // an item should note when the kitchen started to make him 
        // and when done , so by that we can change order state 
        public bool CheckOutItem(OrderItemView i_ToCheck)
        {
            Items.Remove(i_ToCheck);
            int currentSize = Items.Count;
            RemainingItems = currentSize.ToString();
            if (Items.Count == 0)
            {
                AllItemsCheckedOrderID.Add(OrderID);
            }

            return (Items.Count == 0);
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
            set => m_Order.m_OrderID = value;
        }

        public int TableNumber
        {
            get => m_Order.m_TableNumber;
            set => m_Order.m_TableNumber = value;
        }

        public string RemainingItems
        {
            get => m_RemainItemsString;
            set
            {
                m_RemainItemsString = "Remaining : " + value;
                OnPropertyChanged(nameof(RemainingItems));
            }
        }

        public string TableNumberString => "Table : " + m_Order.m_TableNumber.ToString();

        public ObservableCollection<OrderItemView> Items { get; set; } = new ObservableCollection<OrderItemView>();

        public eOrderState State
        {
            get => m_Order.m_OrderState;
            set => m_Order.m_OrderState = value;
        }

        public eOrderType OrderType
        {
            get => m_Order.m_OrderType;
            set => m_Order.m_OrderType = value;
        }

        public ObservableCollection<int> AllItemsCheckedOrderID { get; set; } = new ObservableCollection<int>();

        public Color OrderStateColor { get; set; }
    }
}
