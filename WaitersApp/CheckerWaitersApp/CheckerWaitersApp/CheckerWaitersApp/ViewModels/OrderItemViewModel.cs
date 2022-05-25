using System;
using System.Threading.Tasks;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;
using Xamarin.Forms;

namespace CheckerWaitersApp.ViewModels
{
    public class OrderItemViewModel : BaseViewModel
    {
        public OrderItemViewModel(OrderItemModel i_Model)
        {

            OrderItemModel = new OrderItemModel
            {
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = i_Model.m_OrderID,
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = i_Model.m_OrderItemName,
                m_Description = i_Model.m_Description,
                m_ItemType = i_Model.m_ItemType,
                m_LineID = i_Model.m_LineID,
                m_StartDate = DateTime.MinValue,
                m_TableNumber = i_Model.m_TableNumber,
            };
          init();
        }
        public OrderItemViewModel(DishModel i_DishModel, int i_CountID)
        {

            OrderItemModel = new OrderItemModel
            {
                m_CreatedDate = DateTime.Now,
                m_DoneDate = DateTime.MinValue,
                m_OrderID = i_CountID,
                m_State = eOrderItemState.Waiting,
                m_OrderItemName = i_DishModel.m_DishName,
                m_Description = "None",
                m_ItemType = i_DishModel.m_DishType,
                m_LineID = i_DishModel.m_LineID,
                m_StartDate = DateTime.MinValue,
                m_TableNumber = 0,
            };
            init();
        }

        private void init()
        {
            StateString = "State :" + OrderItemModel.m_State.ToString();
            Description = "Description :" + OrderItemModel.m_Description;
            TypeString = "Type :" + OrderItemModel.m_ItemType;
            CreatedItemString = "Created :" + OrderItemModel.m_CreatedDate.ToShortTimeString();
        }
        public string TypeString { get; private set; }
        public string CreatedItemString { get; private set; }
        public string StateString { get; private set; }
        public string Description { get;  set; }
        public string DishName
        {
            get => OrderItemModel.m_OrderItemName;
            private set => OrderItemModel.m_OrderItemName = value;
        }
       

        public eOrderItemState State
        {
            get => OrderItemModel.m_State;
            set
            { 
                OrderItemModel.m_State = value;
                OnPropertyChanged(nameof(State));
            }
        }
        public eOrderItemType OrderItemType
        {
            get => OrderItemModel.m_ItemType;
            set => OrderItemModel.m_ItemType = value;
        }

        public OrderItemModel OrderItemModel { get; private set; }

        private Command longPressCommand;
        public Command LongPressCommand
        {
            get
            {
                if (longPressCommand == null)
                {
                    longPressCommand = new Command(longPress);
                }

                return longPressCommand;
            }
        }

        private async void longPress()
        {

            var user_input = await InputBox();
            if (!string.IsNullOrEmpty(user_input))
            {
                OrderItemModel.m_Description = user_input;
            }
        }

        public async Task<string> InputBox()
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync("Enter a note :", "What's would you like to change ?");
            return result;
        }
    }
}
