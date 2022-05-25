using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Helpers.Dishes;
using CheckerWaitersApp.Models;
using Xamarin.Forms;

namespace CheckerWaitersApp.ViewModels
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private readonly ObservableCollection<DishModel> m_Dishes;
        private DishesManager m_Manager;
        private static int m_CountItemsID = 0;
        private static int m_CountOrdersID = 0;
        public OrdersViewModel Orders { get; set; } = new OrdersViewModel();
        public CreateOrderViewModel()
        {
            m_Manager = new DishesManager();
            m_Dishes = new ObservableCollection<DishModel>();
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

                switch (model.m_DishType)
                {
                    case eOrderItemType.Starter:
                        Starters.Add(dishViewModel);
                        break;
                    case eOrderItemType.Main:
                        Mains.Add(dishViewModel);
                        break;
                    case eOrderItemType.Dessert:
                        Desserts.Add(dishViewModel);
                        break;
                }
            }
        }

        private void dishViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is DishViewModel dishView && dishView.IsOrdered)
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
        public ObservableCollection<OrderItemViewModel> ToOrderCollection { get; private set; } = new ObservableCollection<OrderItemViewModel>();

        public ObservableCollection<OrderModel> OrdersCollection { get; private set; } = new ObservableCollection<OrderModel>();

        public void AddToOrderCollection(DishViewModel i_ToAdd)
        {
            var item = new OrderItemViewModel(i_ToAdd.Model, m_CountItemsID++);
            ToOrderCollection.Add(item);
        }
        public void RemoveFromOrderCollection(OrderItemViewModel i_ToRemove)
        {
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
            var orderItems = new List<OrderItemModel>();
            foreach (var item in ToOrderCollection)
            {
                item.OrderItemModel.m_CreatedDate = DateTime.Now;
                item.OrderItemModel.m_TableNumber = int.Parse(EntryValue);
                orderItems.Add(item.OrderItemModel);
            }

            var newOrder = new OrderModel()
            {
                m_TableNumber = int.Parse(EntryValue),
                m_OrderID = m_CountOrdersID++,
                m_OrderState = eOrderState.Pending,
                m_OrderType = PickedOrderType,
                m_Items = orderItems,
                m_CreatedDate = DateTime.Now
            };
            OrdersCollection.Add(newOrder);
            Orders.AddNewOrder(newOrder);
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
