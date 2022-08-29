﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CheckerUI.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private ObservableCollection<OrderViewModel> m_OrdersViews = new ObservableCollection<OrderViewModel>();
        private readonly Dictionary<int, OrderViewModel> m_Orders = new Dictionary<int, OrderViewModel>();
        private Dictionary<int, Dish> mDishesDictionary;
        public ServingAreaViewModel servingArea { get; set; }
        public ObservableCollection<ServingZone> Zones { get; set; } = new ObservableCollection<ServingZone>();
        public OrdersViewModel()
        {
            mDishesDictionary = App.Repository.DishesDictionary;

            var a = new ServingArea()
            {
                zoneNum = 5,
                restaurantId = 1, lines = null, id = 1, name = "temp"
            };
            servingArea = new ServingAreaViewModel(a);
            foreach (var zone in servingArea.Zones)
            {
                zone.isAvailable = true;
                Zones.Add(zone);
            }
            var orders = App.Repository.Orders;
            foreach (var order in orders)
            {
                var view = new OrderViewModel(order);
                m_OrdersViews.Add(view);
                m_Orders.Add(order.id, view);
                int id = GetFirstAvailableZone();
                if (id >= 0) 
                {
                    SetOrderItemInZone(view.Items[0], id);
                    view.Items[0].Table = order.table;
                    Zones[id].item = view.Items[0];
                    Zones[id].isAvailable = false;
                }
            }

            //App.HubConn.On<Order>("ReceiveOrder", (order) =>
            //{

            //    Application.Current.MainPage.DisplayAlert("Order received", "The Order " + order.id + " was successfully added to DB", "OK");
            //    foreach (var orderItem in order.items)
            //    {
            //        orderItem.dish = mDishesDictionary[orderItem.dishId];
            //    }
            //    var view = new OrderViewModel(order);
            //    m_Orders.Add(order.id, view);
            //    m_OrdersViews.Add(view);
            //});
            //StartListening();
        }
        public int GetFirstAvailableZone()
        {
            var zone = Zones.First(x => x.isAvailable == true);
            return zone.id;
        }

        public bool SetOrderItemInZone(OrderItemViewModel i_Item, int i_zoneId)
        {
            if (i_zoneId <= 0 || i_zoneId >= Zones.Count || !Zones[i_zoneId].isAvailable) return false;
            Zones[i_zoneId].item = i_Item;
            Zones[i_zoneId].isAvailable = false;
            return true;
        }
        private static async void StartListening()
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server

                }
                catch (System.Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception! Disconnected", ex.Message, "OK");
                }
            }

            try
            {
                await App.HubConn.InvokeAsync("RegisterForGroup", 1);
            }
            catch (System.Exception ex)
            {
                // await Application.Current.MainPage.DisplayAlert("Exception!", ex.Message, "OK");
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
