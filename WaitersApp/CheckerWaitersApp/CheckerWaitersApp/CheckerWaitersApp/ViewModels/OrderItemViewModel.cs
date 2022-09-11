using System.Threading.Tasks;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;
using Xamarin.Forms;

namespace CheckerWaitersApp.ViewModels
{
    public class OrderItemViewModel : BaseViewModel
    {
        public string TypeString { get; private set; }
        public string StartItemString { get; private set; }
        public string FinishItemString { get; private set; }
        public string StateString { get; private set; }
        public string Note { get; set; }
        public OrderItem OrderItemModel { get; private set; }

        private Command longPressCommand;
        public OrderItemViewModel(OrderItem i_Model)
        {
            OrderItemModel = new OrderItem
            {
                dish = i_Model.dish,
                start = i_Model.start,
                finish = i_Model.finish,
                orderId = i_Model.id,
                lineStatus = i_Model.lineStatus,
                changes = i_Model.changes,
                table = i_Model.table,
                servingAreaZone = i_Model.servingAreaZone,
                status = i_Model.status
            };
          init();
        }
        public OrderItemViewModel(Dish i_DishModel, int i_CountID)
        {

            OrderItemModel = new OrderItem
            {
                dish = i_DishModel,
                dishId = i_DishModel.id,
                start = default,
                finish = default,
                orderId = i_CountID,
                lineStatus = eLineItemStatus.Locked,
                changes = "None",
                table = 0,
            };
            init();
        }

        private void init()
        {
            StateString = "State :" + OrderItemModel.lineStatus.ToString();
            Note = "Note :" + OrderItemModel.changes;
            TypeString = "Type :" + OrderItemModel.dish.type;
            StartItemString = "Started :" + OrderItemModel.start.ToShortTimeString();
            FinishItemString = "Finished :" + OrderItemModel.finish.ToShortTimeString();
        }
        public string DishName
        {
            get => OrderItemModel.dish.name;
            private set => OrderItemModel.dish.name = value;
        }
        
        public eLineItemStatus State
        {
            get => OrderItemModel.lineStatus;
            set
            { 
                OrderItemModel.lineStatus = value;
                OnPropertyChanged(nameof(State));
            }
        }
        public eDishType OrderItemType
        {
            get => OrderItemModel.dish.type;
            set => OrderItemModel.dish.type = value;
        }

        public Dish OrderItemDish
        {
            get => OrderItemModel.dish;
            set => OrderItemModel.dish = value;
        }
       
        public Command LongPressCommand
        {
            get { return longPressCommand ??= new Command(longPress); }
        }

        private async void longPress()
        {

            var user_input = await InputBox();
            if (!string.IsNullOrEmpty(user_input))
            {
                OrderItemModel.changes = user_input;
            }
        }

        public async Task<string> InputBox()
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync("Enter a note :", "What's would you like to change ?");
            return result;
        }
    }
}
