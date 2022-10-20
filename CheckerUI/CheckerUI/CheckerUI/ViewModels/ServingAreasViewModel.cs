using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CheckerUI.Enums;
using CheckerUI.Models;
using CheckerUI.Views;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;
namespace CheckerUI.ViewModels
{
    public class ServingAreasViewModel : BaseViewModel
    {
       // private ObservableCollection<ServingArea> m_AreasList = new ObservableCollection<ServingArea>();
        public ObservableCollection<OrdersViewModel> m_OrdersViewModels = new ObservableCollection<OrdersViewModel>();
        private ObservableCollection<OrdersView> m_Views { get; set; } = new ObservableCollection<OrdersView>();
        public int ClickedAreaId { get; set; } = 0;
        public Dictionary<int, Order> OrdersModels { get; set; } = new Dictionary<int, Order>();
        public string ClickedAreaName { get; set; }
        private readonly Dictionary<int, Dish> mDishesDictionary;
        public ServingAreasViewModel()
        {
            mDishesDictionary = App.Repository.DishesDictionary;
            initServingAreasByRepository();
            initEvents();
            initOrdersHub();
        }
        private void initServingAreasByRepository()
        {
            var list = App.Repository.ServingAreas;
            foreach (var vm in list.Select(servingArea => new OrdersViewModel(servingArea.id, servingArea)))
            {
                m_OrdersViewModels.Add(vm);
                var view = new OrdersView(vm);
                view.SetBindingToMainVM(this);
                m_Views.Add(view);
            }
        }

        private void initEvents()
        {
            App.OrderHubConnection.On<Order>("ReceiveOrder", (order) =>
            {
                // Application.Current.MainPage.DisplayAlert("Order received", "The Order " + order.id + " was successfully added to DB", "OK");
                if (order.status == eOrderStatus.Done) return;
                foreach (var orderItem in order.items)
                {
                    orderItem.dish = mDishesDictionary[orderItem.dishId];
                }

                foreach (var orderItem in order.items)
                {
                    if (orderItem.status == eItemStatus.Served) continue;
                    var lineId = orderItem.dish.lineId;
                    var line = App.restaurant.lines.First(l => l.id == lineId);
                    var areaId = line.servingAreaId;
                    var windowVM = m_OrdersViewModels.First(vm => vm.ViewId == areaId);
                    windowVM.AddOrderItem(order.id, orderItem, areaId, order);
                    if (orderItem.servingAreaZone > -1)
                    {
                        windowVM.SetOrderItemInZone(orderItem, orderItem.servingAreaZone, true);
                    }
                }
            });
            App.OrderHubConnection.On<List<Order>>("ReceiveOrders", (orders) =>
            {
                foreach (var order in orders.Where(order => order.status != eOrderStatus.Done))
                {
                    foreach (var orderItem in order.items)
                    {
                        orderItem.dish = mDishesDictionary[orderItem.dishId];
                    }

                    foreach (var orderItem in order.items)
                    {
                        if (orderItem.status == eItemStatus.Served) continue;
                        var lineId = orderItem.dish.lineId;
                        var line = App.restaurant.lines.First(l => l.id == lineId);
                        var areaId = line.servingAreaId;
                        var windowVM = m_OrdersViewModels.First(vm => vm.ViewId == areaId);
                        windowVM.AddOrderItem(order.id, orderItem, areaId, order);
                        if (orderItem.servingAreaZone > -1)
                        {
                            windowVM.SetOrderItemInZone(orderItem, orderItem.servingAreaZone, true);
                        }
                    }
                }
            });

            App.OrderHubConnection.On<OrderItem>("ItemServed", (item) =>
            {
                item.dish = mDishesDictionary[item.dishId];
                var lineId = item.dish.lineId;
                var line = App.restaurant.lines.First(l => l.id == lineId);
                var zone = line.servingAreaId;
                var vm = m_OrdersViewModels.First(o => o.ViewId == zone);
                vm.CheckOutItem(item);

                vm.SetOrderItemInZone(item, item.servingAreaZone, false);
            });
            App.OrderHubConnection.On<int, OrderItem>("ItemToBeServed", (areaId, item) =>
            {

                item.dish = mDishesDictionary[item.dishId];

                var vm = m_OrdersViewModels.First(o => o.ViewId == areaId);
                vm.SetOrderItemInZone(item, item.servingAreaZone, true);
            });
        }
        private static async void initOrdersHub()
        {
            if (App.OrderHubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.OrderHubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception! Disconnected orders", ex.Message, "OK");
                }
            }

            try
            {
                await App.OrderHubConnection.InvokeAsync("RegisterForGroup", App.RestId);
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception!", ex.Message, "OK");
            }
        }
        public ObservableCollection<OrdersViewModel> OrdersWindow
        {
            get => m_OrdersViewModels;
            set
            {
                m_OrdersViewModels = value;
                OnPropertyChanged(nameof(OrdersWindow));
            }
        }
        public async Task AreaButton_OnClicked(object sender, EventArgs e)
        {
            var view = m_Views.FirstOrDefault(ll => ll.AreaId == ClickedAreaId);

            await Application.Current.MainPage.Navigation.PushAsync(view);
        }
        public async Task AreaButton_OnClickedString(object sender, EventArgs e)
        {
            var view = m_Views.FirstOrDefault(ll => ll.name == ClickedAreaName);
            await Application.Current.MainPage.Navigation.PushAsync(view);
        }
        public async Task<bool> PickUpItemForServing(int i_ItemId)
        {
            if (App.OrderHubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.OrderHubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception! Disconnected orders", ex.Message, "OK");
                }
            }
            try
            {
                 App.OrderHubConnection.InvokeAsync("PickUpItemForServing", i_ItemId);
                return true;
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("failed -  PickUpItemForServing", ex.Message, "OK");
                return false;
            }
        }
    }
}
