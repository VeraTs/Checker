using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Helpers.Dishes;
using CheckerWaitersApp.Models;
using CheckerWaitersApp.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace CheckerWaitersApp.ViewModels
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Dish> m_Dishes;
        private DishesManager m_Manager;
        private static int m_CountItemsID = 0;
        private static int m_CountOrdersID = 0;
        private float m_TotalPrice = 0;
        private int restId = 1;
        public OrdersViewModel Orders { get; set; } = new OrdersViewModel();

        public CreateOrderViewModel()
        {
            App.HubConn.On<Order>("ReceiveOrder", (order) =>
            {
                Application.Current.MainPage.DisplayAlert("Order received", "The Order " + order.id + " was successfully added to DB", "OK");

            });
        }
        public CreateOrderViewModel(DishDataStore i_Store)
        {
            m_Manager = new DishesManager(i_Store);
            m_Dishes = new ObservableCollection<Dish>();
            m_Dishes = m_Manager.Dishes;
            DishTypesStrings = new List<string>();
            var typesList = m_Manager.AllDishTypesList();
            foreach (var type in typesList){ DishTypesStrings.Add(m_Manager.OrderTypeToString(type));}
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
            m_TotalPrice += i_ToAdd.Model.price;
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
        public void GenerateOrder()
        {
            var orderItems = new List<OrderItem>();
            foreach (var item in ToOrderCollection)
            {
                item.OrderItemModel.createdDate = DateTime.Now;
                item.OrderItemModel.table = int.Parse(EntryValue);
                orderItems.Add(item.OrderItemModel);
            }

            var newOrder = new Order()
            {
                table = int.Parse(EntryValue),
                id = m_CountOrdersID++,
                status = eOrderStatus.Ordered,
                orderType = PickedOrderType,
                items = orderItems,
                createdDate = DateTime.Now,
                remainsToPay = m_TotalPrice,
                totalCost = m_TotalPrice,
                restaurantId = restId
            };
            UpdateManagerNewOrder(newOrder); // update after succsess
            OrdersCollection.Add(newOrder);
            Orders.AddNewOrder(newOrder);
        }

        private async void UpdateManagerNewOrder(Order ToUpdate)
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server
                  
                }
                catch (System.Exception ex)
                { 
                    await Application.Current.MainPage.DisplayAlert("Exception!", ex.Message, "OK");
                }
            }

            try
            {
                await App.HubConn.InvokeAsync("AddOrder", ToUpdate);
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception!", ex.Message, "OK");
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
