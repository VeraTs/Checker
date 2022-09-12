using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CheckerUI.Models;

namespace CheckerUI.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private ObservableCollection<OrderViewModel> m_OrdersViews;
        private readonly Dictionary<int, OrderViewModel> m_Orders;
        private Dictionary<int, Dish> mDishesDictionary;
        public ServingArea servingArea { get; set; } 

        public ObservableCollection<ZoneViewModel> Zones { get; set; }
        public int ViewId { get; set; }

        public string Name => servingArea.name;
        public int Capacity => m_OrdersViews.Count;

        public OrdersViewModel() { }
        public OrdersViewModel(int i_AreaId, ServingArea i_Area)
        {
            m_OrdersViews = new ObservableCollection<OrderViewModel>();
            m_Orders = new Dictionary<int, OrderViewModel>();
            Zones = new ObservableCollection<ZoneViewModel>();
            mDishesDictionary = App.Repository.DishesDictionary;
            ViewId = i_AreaId;

            servingArea = i_Area;

            for (var i = 0; i < servingArea.zoneNum; i++)
            {
                var toAdd = new ServingZone()
                {
                    isAvailable = true,
                    id = i,
                    item = new OrderItemViewModel(),
                };
                var model = new ZoneViewModel(toAdd);
                Zones.Add(model);
            }
        }
        public int GetFirstAvailableZone()
        {
            var zone = Zones.First(x => x.model.isAvailable == true);
            return zone.ZoneId;
        }

        public void AddOrderItem(int i_OrderId, OrderItem i_item, int areaId, Order orderModel)
        {
            if (m_Orders.ContainsKey(i_OrderId))
            {
                m_Orders[i_OrderId].AddOrderItem(i_item);
            }
            else
            {
                var vm = new OrderViewModel(orderModel, areaId);
                vm.AddOrderItem(i_item);
                m_OrdersViews.Add(vm);
                m_Orders.Add(vm.OrderID,vm);
            }
        }

        public void CheckOutItem(OrderItem i_Item)
        {
            m_Orders[i_Item.orderId].RemoveOrderItem(i_Item);
        }
        public bool SetOrderItemInZone(OrderItem i_Item, int i_zoneId, bool i_setFlag)
        {
            if (i_setFlag)
            {
                if (i_zoneId < 0 || !Zones[i_zoneId].isAvailable) return false;
                var itemView = new OrderItemViewModel(i_Item);
                Zones[i_zoneId].SetOrderItem(itemView);
                return true;
            }
            else
            {
                if (i_zoneId < 0 || Zones[i_zoneId].isAvailable) return false;
                var itemVm = Zones[i_zoneId].ItemViewModel;
                Zones[i_zoneId].RemoveOrderItem();
                if (m_OrdersViews[i_Item.orderId].CheckOutItem(itemVm))
                    m_OrdersViews.Remove(m_OrdersViews[i_Item.orderId]);
                return true;
            }
        }

        private void AllItemsCheckedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var observer = sender as ObservableCollection<int>;
            var id = observer[0];
            var viewToRemove = m_Orders[id];
            m_OrdersViews.Remove(viewToRemove);
        }

        public ObservableCollection<OrderViewModel> OrdersViews
        {
            get => m_OrdersViews;
            set
            {
                m_OrdersViews = value;
                OnPropertyChanged(nameof(OrdersViews));
            }
        }
        public ObservableCollection<OrderItemViewModel> itemsLineView { get; set; } = new ObservableCollection<OrderItemViewModel>();
    }
}
