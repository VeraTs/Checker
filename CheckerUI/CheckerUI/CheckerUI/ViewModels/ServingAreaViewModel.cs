using System.Collections.ObjectModel;
using System.Linq;
using CheckerUI.Models;

namespace CheckerUI.ViewModels
{
    public class ServingAreaViewModel
    {
        private ServingArea model;
        public readonly ObservableCollection<ServingZone> Zones = new ObservableCollection<ServingZone>();
        public ServingAreaViewModel(ServingArea i_model)
        {
            model = i_model;
            for (var i = 0; i < model.zoneNum; i++)
            {
                var zone = new ServingZone()
                {
                    id = i,
                    isAvailable = true,
                    item = null
                };
                Zones.Add(zone);
            }
        }

        public int GetFirstAvailableZone()
        {
            var zone = Zones.First(x => x.isAvailable == true);
            return zone.id;
        }

        //public bool SetOrderItemInZone(OrderItem i_Item, int i_zoneId)
        //{
        //    if (i_zoneId <= 0 || i_zoneId >= Zones.Count || !Zones[i_zoneId].isAvailable) return false;
        //    Zones[i_zoneId].item = i_Item;
        //    Zones[i_zoneId].isAvailable = false;
        //    return true;
        //}
        public int NumberOfZones
        {
            get => model.zoneNum;
            private set => model.zoneNum = value;
        }
    }
}
