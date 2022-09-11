using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using CheckerUI.Models;

namespace CheckerUI.ViewModels
{
    public class ZoneViewModel : BaseViewModel
    {
        public ServingZone model;

        private Color m_BackgroundState;
        private Color m_LabelsColorState;


        public ZoneViewModel(ServingZone i_model) : base()
        {
            m_BackgroundState = new Color();
            model = i_model;
        }

        public void SetOrderItem(OrderItemViewModel i_Item)
        {
            model.item = i_Item;
            model.isAvailable = false;
        }

        public void RemoveOrderItem()
        {
            model.item = null;
            model.isAvailable = true;
        }
        public int ZoneId
        {
            get => model.id;
            set
            {
                model.id = value;
                OnPropertyChanged(nameof(ZoneId));
            }
        }

        public string ItemName => model.item != null ? model.item.OrderItemName : "Empty";


    }
}
