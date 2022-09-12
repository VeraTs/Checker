using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;


namespace CheckerWaitersApp.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public Order Order { get; private set; }
        public string TableNumber { get; private set; }
        public string OrderID { get; private set; }
        public string OrderHeader { get; private set; }
        public float SumToPay { get; private set; } = 0;

        public Command PayForOrder { get; private set; }
        public Command PayPartialForOrder { get; private set; }

        private string RemainsString;
        public string CreatedTime => Order.items.First().start.ToShortTimeString();

        public ObservableCollection<OrderItem> Items { get; private set; }
        public OrdersViewModel m_MainVm { get; set; }

        public ObservableCollection<OrderItemViewModel> OrderItemsViews { get; private set; } =
            new ObservableCollection<OrderItemViewModel>();
        public eOrderStatus OrderState
        {
            get => Order.status;
            set => Order.status = value;
        }

        public string OrderStateString => Order.status.ToString();

        public eOrderType OrderType
        {
            get => Order.orderType;
            set => Order.orderType = value;
        }

        public string Counter => "Total Items :" + Order.items.Count;
        public string Cost => "Price :" + Order.totalCost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
        public string Remains
        {
            get => RemainsString;
            private set
            {
                RemainsString = value;
                OnPropertyChanged(nameof(Remains));
            }
        }

        public OrderViewModel(Order i_Model, OrdersViewModel i_MainVm)
        {
            m_MainVm = i_MainVm;
            Order = i_Model;
            TableNumber = "Table :" + i_Model.table.ToString();
            OrderID = "Order #" + Order.id.ToString();
            OrderHeader = OrderID + TableNumber;
            OrderState = i_Model.status;
            Remains = "Remain :" + Order.remainsToPay.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            foreach (var card in i_Model.items.Select(item => new OrderItemViewModel(item)))
            {
                OrderItemsViews.Add(card);
            }

            PayForOrder = new Command( async () =>
            {
                SumToPay = Order.totalCost;
                await PayForOrderAction();
                Order.status = eOrderStatus.Done;
                OrderState = eOrderStatus.Done;
            });
            PayPartialForOrder = new Command(async () =>
            {
                getPaymentAmountFromUser();
            });
        }

        public  bool UpdatePayPartialForOrder(float i_sum)
        {
            Order.remainsToPay = i_sum;
            Remains = "Remain :" + Order.remainsToPay.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            return Order.remainsToPay == 0;
        }
        private async void getPaymentAmountFromUser()
        {

            var user_input = await InputBox();
            if (!string.IsNullOrEmpty(user_input))
            {
                SumToPay = float.Parse(user_input);
            }
            await PayForOrderAction();
        }

        public async Task<string> InputBox()
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync("Pay For Order", "How much would you like to pay ?");
            return result;
        }
        public async Task PayForOrderAction()
        {
           
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server

                }
                catch (System.Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Server Close Order Error!", ex.Message, "OK");
                }
            }

            try
            {
                await App.HubConn.InvokeAsync("PayForOrder", this.Order.id, SumToPay);
            }
            catch (System.Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Server Close Order !", ex.Message + Order.id+","+SumToPay, "OK");
            }
        }
    }
}
