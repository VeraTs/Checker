﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace CheckerWaitersApp.ViewModels
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Dish> m_Dishes;
        private static int m_CountItemsID = 0;
        
        private float m_TotalPrice = 0;
        public OrdersViewModel Orders { get; set; }
        
     
        public CreateOrderViewModel()
        {
            m_Dishes = new ObservableCollection<Dish>();

            initUsingRepository();
            Orders ??= new OrdersViewModel();
            App.HubConn.On<Order>("ReceiveOrder", (order) =>
            {
                foreach (var item in order.items)
                {
                    item.dish = App.Repository.DishesDictionary[item.dishId];
                }
                if (order.restaurantId == App.RestId) { Orders.AddNewOrder(order); }
               
            });
            App.HubConn.On<List<Order>>("ReceiveOrders", (orders) =>
            {
                foreach (var order in orders.Where(o=>o.restaurantId == App.RestId))
                {
                    foreach (var item in order.items)
                    {
                        item.dish = App.Repository.DishesDictionary[item.dishId];
                    }
                    Orders.AddNewOrder(order);
                }
            });

            App.HubConn.On<Order, float>("PaymentMadeFull" ,(order , sum) =>
            {
                if(order.restaurantId == App.RestId){ Orders.RemovePaidOrder(order); }
            });

            App.HubConn.On<Order, float>("PartialPaymentMade", (order, sum) =>
            {
                if (order.restaurantId == App.RestId)
                {
                    Orders.UpdatePartialPay(order, order.remainsToPay);
                }
            });


            DishTypesStrings = new List<string>();
            var typesList = AllDishTypesList();
            foreach (var type in typesList){ DishTypesStrings.Add(OrderTypeToString(type));}
            foreach (var model in m_Dishes)
            {
                var dishViewModel = new DishViewModel(model);
                dishViewModel.PropertyChanged += dishViewModel_PropertyChanged;
                DishesOnView.Add(dishViewModel);
               Dishes.Add(dishViewModel);

                switch (model.type)
                {
                    case eDishType.Starter:
                        Starters.Add(dishViewModel);
                        break;
                    case eDishType.Main:
                        Mains.Add(dishViewModel);
                        break;
                    case eDishType.Dessert:
                        Desserts.Add(dishViewModel);
                        break;
                    case eDishType.Drink:
                        Drinks.Add(dishViewModel);
                        break;
                }
            }

            initOrdersHub();
        }
        private static async void initOrdersHub()
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();
                }
                catch (System.Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception! Disconnected orders", ex.Message, "OK");
                }
            }

            try
            {
                await App.HubConn.InvokeAsync("RegisterForGroup", App.RestId);
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception!", ex.Message, "OK");
            }
        }

        private void initUsingRepository()
        {
            foreach (var dish in App.Repository.Dishes)
            {
                m_Dishes.Add(dish);
            }
        }
        public List<eOrderType> AllDishTypesList()
        {
            var list = new List<eOrderType>
            {
                eOrderType.AllTogether,
                eOrderType.Staggered,
                eOrderType.FIFO
            };
            return list;
        }
        public string OrderTypeToString(eOrderType i_Type)
        {
            string output;
            switch (i_Type)
            {
                case eOrderType.FIFO:
                {
                    output = "FIFO";
                    break;
                }
                case eOrderType.AllTogether:
                {
                    output = "All Together";
                    break;
                }
                case eOrderType.Staggered:
                {
                    output = "Staggered";
                    break;
                }
                default:
                {
                    output = "Staggered";
                    break;
                }
            }
            return output;
        }
        private void dishViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is DishViewModel {IsOrdered: true} dishView)
            {
                AddToOrderCollection(dishView);
            }
        }
     
        public string EntryValue { get; set; }

        public ObservableCollection<DishViewModel> DishesOnView { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Dishes { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Starters { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Mains { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Desserts { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<DishViewModel> Drinks { get; private set; } = new ObservableCollection<DishViewModel>();
        public ObservableCollection<OrderItemViewModel> ToOrderCollection { get; private set; } = new ObservableCollection<OrderItemViewModel>();

        public ObservableCollection<Order> OrdersCollection { get; private set; } = new ObservableCollection<Order>();

        public void AddToOrderCollection(DishViewModel i_ToAdd)
        {
            var item = new OrderItemViewModel(i_ToAdd.Model, m_CountItemsID++);
            m_TotalPrice += i_ToAdd.DishPrice;
            ToOrderCollection.Add(item);
        }
        public void RemoveFromOrderCollection(OrderItemViewModel i_ToRemove)
        {
            m_TotalPrice -= i_ToRemove.OrderItemDish.price;
            ToOrderCollection.Remove(i_ToRemove);
        }

        // need to compare to tables list 

        public bool CheckTableNumber(int i_num)
        {
            return i_num > 0;
        }
        // should be async
        public async void GenerateOrder()
        {
            var orderItems = ToOrderCollection.Select(item => item.OrderItemModel).ToList();

            var newOrder = new Order()
            {
                table = int.Parse(EntryValue),
                orderType = PickedOrderType,
                items = orderItems,
                remainsToPay = m_TotalPrice,
                totalCost = m_TotalPrice,
                restaurantId = App.RestId,
            };
           await UpdateManagerNewOrder(newOrder);
            m_CountItemsID++;
        }

        private static async Task UpdateManagerNewOrder(Order ToUpdate)
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server
                  
                }
                catch (System.Exception ex)
                { 
                    await Application.Current.MainPage.DisplayAlert("Server Error!", ex.Message, "OK");
                }
            }

            try
            {
                await App.HubConn.InvokeAsync("AddOrder", ToUpdate);
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Server Error!", ex.Message, "OK");
            }
        }
        public List<string> DishTypesStrings { get; private set; }
        public void ClearOrderCollection()
        {
            ToOrderCollection.Clear();
        }

        private Command longPressOrderCommand;
        private Command longPressClearCommand;
        public int PickedTableNumber { get; set; }
        public eOrderType PickedOrderType { get; set; }
        public Command LongPressOrderCommand => longPressOrderCommand ?? (longPressOrderCommand = new Command(longPress));

        public Command LongPressClearCommand =>
            longPressClearCommand ?? (longPressClearCommand = new Command(ClearOrderCollection));
        public async void longPress()
        {
            if (!await CheckFiledDetails()) return;
            GenerateOrder();
            ClearOrderCollection();
        }

        private async Task<bool> CheckFiledDetails()
        {
            var res = false;
            if (string.IsNullOrEmpty(EntryValue) || !CheckTableNumber(int.Parse(EntryValue)))
            {
                await Application.Current.MainPage.DisplayAlert("Wrong Table Number", "Enter Correct Table number", "OK");
            }
            else if (ToOrderCollection.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Empty Order", "Enter Order Details", "OK");
            }
            else
            {
                res = true;
            }
            return res;
        }
    }
}
