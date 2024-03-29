﻿using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using CheckerUI.Enums;
using CheckerUI.Models;
//A checkout window shows the total of the orders as well as the items in it in the upper part
//In its lower part, it shows a division into areas of the same window,
//in order to differentiate between several identical packages that are at the same time on the window,
//thus avoiding confusion between the item and its correct destination
namespace CheckerUI.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly Models.Order m_Order;
        private string m_RemainItemsString;
        public int AreaId = -1;
        public OrderViewModel(Models.Order i_model, int i_areaId)
        {
            AreaId = i_areaId; 
            m_Order = i_model;
            foreach (var itemView in from itemModel in i_model.items where itemModel.servingAreaZone == i_areaId select new OrderItemViewModel(itemModel))
            {
                Items.Add(itemView);
            }

            OrderID = i_model.id;
            OrderType = i_model.orderType;
            TableNumber = i_model.table;

            RemainingItems = Items.Count.ToString();
            OrderStateColor = new Color();
            setColorState();
        }

        // here we are updating the order items list ,
        // an item should note when the kitchen started to make him 
        // and when done , so by that we can change order state 
        public bool CheckOutItem(OrderItemViewModel i_ToCheck)
        {
            Items.Remove(i_ToCheck);
            RemainingItems = Items.Count.ToString();
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
                case eOrderStatus.Ordered:
                    {
                        OrderStateColor = Color.Brown;
                        break;
                    }
                case eOrderStatus.InProgress:
                    {
                        OrderStateColor = Color.DarkOrange;
                        break;
                    }
                case eOrderStatus.Done:
                    {
                        OrderStateColor = Color.YellowGreen;
                        break;
                    }
            }
        }

        public int OrderID
        {
            get => m_Order.id;
            set => m_Order.id = value;
        }

        public int TableNumber
        {
            get => m_Order.table;
            set => m_Order.table = value;
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

        public string TableNumberString => "Table : " + m_Order.table.ToString();

        public ObservableCollection<OrderItemViewModel> Items { get; set; } = new ObservableCollection<OrderItemViewModel>();

        public void AddOrderItem(OrderItem i_item)
        {
            var itemView = new OrderItemViewModel(i_item); 
            Items.Add(itemView);
            RemainingItems = Items.Count.ToString();
        }

        public void RemoveOrderItem(OrderItem i_item)
        {
            Items.Remove(Items.First(t=>t.OderItemID == i_item.id));
            RemainingItems = Items.Count.ToString();
        }
        public eOrderStatus State
        {
            get => m_Order.status;
            set => m_Order.status = value;
        }

        public eOrderType OrderType
        {
            get => m_Order.orderType;
            set => m_Order.orderType = value;
        }

        public ObservableCollection<int> AllItemsCheckedOrderID { get; set; } = new ObservableCollection<int>();

        public Color OrderStateColor { get; set; }
    }
}
