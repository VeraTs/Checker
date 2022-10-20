using CheckerUI.Models;

namespace CheckerUI.ViewModels
{
    public class ZoneViewModel : BaseViewModel
    {
        public ServingZone model;
        public ZoneViewModel(ServingZone i_model)
        {
            model = i_model;
        }

        public void SetOrderItem(OrderItemViewModel i_Item)
        {
            model.item = i_Item;
            model.isAvailable = false;
        }
        public OrderItemViewModel ItemViewModel
        {
            get => model.item;
            set=> model.item = value;
        }
        public bool isAvailable => model.isAvailable;

        public void RemoveOrderItem()
        {
            model.item.Model.dish.name = "Empty";
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

        public int ItemId()
        {
            
            if (model.item == null) return -1;
            else return model.item.OderItemID;
        
        }
        public string ItemName => model.item != null ? model.item.OrderItemName : "Empty";
    }
}
