using CheckerDTOs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTest2.Models;

namespace XamarinTest2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignalRViewPage : ContentPage
    {
        public ObservableCollection<Order> Orders { get; private set; }
        //public ObservableCollection<ToDo> Items { get; private set; }


        public SignalRViewPage()
        {
            InitializeComponent();
            //Items = new ObservableCollection<ToDo>();
            Orders = new ObservableCollection<Order>();
            MyListView.ItemsSource = Orders;

            App.HubConn.On<List<Order>>("ReceiveOrders", (orders) =>
            {
                Orders.Clear();
                // ObservableCollection doesn't support AddRange
                foreach(Order order in orders)
                {
                    Orders.Add(order);
                }

            });

            /*App.HubConn.On<String, DateTime>("ReceiveList", (desc, date) =>
            {
                ToDo todo = new ToDo() { createdDate = date, description = desc };
                Items.Add(todo);

            });*/

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            // this is the only place of initiating the connection
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server
                    //await App.HubConn.InvokeAsync("InitialToDos");  // invoke initial event - to get all current listings
                    await App.HubConn.InvokeAsync("GetAllOrders");
                }
                catch (System.Exception ex) {
                    Console.WriteLine("------------ERROR-----ERROR------ERROR------");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
