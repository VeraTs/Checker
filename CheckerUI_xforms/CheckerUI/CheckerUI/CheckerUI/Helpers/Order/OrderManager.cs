using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerUI.Models;
using CheckerUI.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;


//This Class is responsible for scattering an order item to its correct cooker line,
//every time a new order enters the server it updates (currently)
//the todo list and from there in response we create
//a new order model and update the list of the correct line in a new order

namespace CheckerUI.Helpers.Order
{
    public class OrderManager : BaseViewModel
    {
        private static int m_TotalOrdersCounterID = 0;
        public Dictionary<int, OrderItemView> m_Orders { get; set; } = new Dictionary<int, OrderItemView>();

        private ObservableCollection<ToDo> m_ToDoList;
        private ObservableCollection<OrderItemView> m_HotLineOrders { get; set; } = new ObservableCollection<OrderItemView>();
        public OrderManager(ObservableCollection<OrderItemView> i_OrdersListOfLine)
        {
            CreateSignal();
            m_ToDoList = new ObservableCollection<ToDo>();
            m_HotLineOrders = i_OrdersListOfLine;
            App.HubConn.On<String, DateTime>("ReceiveList", (desc, date) =>
            {
                ToDo todo = new ToDo() { createdDate = date, description = desc };
                m_ToDoList.Add(todo);
                addToOrdersList(todo);
            });
        }

        private void sortOrders() // each time a new order entered
                                  // need to sort it to her right line - only the one added
        {
            getHotLineOrders();
        }

        private void getHotLineOrders()
        {
            foreach (var key in m_Orders.Keys)
            {
                if (m_Orders[key].OderID % 2 == 0)
                {
                    m_HotLineOrders.Add(m_Orders[key]);
                }
            }
        }
        public async void CreateSignal()
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server
                    await App.HubConn.InvokeAsync("InitialToDos");  // invoke initial event - to get all current listings
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void addToOrdersList(ToDo i_ToDo)
        {
            List<string> nmList = new List<string>()
            {
                "pizza", "burger", "fish"
            };
            int status = 0;
            int id = m_TotalOrdersCounterID++;
            Button buttonToMake = generateButton(id);

            OrderIDNotifier m = new OrderIDNotifier(id, status);
            OrderButtonModel model = new OrderButtonModel(id, buttonToMake);
            OrderItemView order = new OrderItemView(nmList[0], model, m);
            m_Orders.Add(id, order);
            m_HotLineOrders.Add(order);
        }
        private Button generateButton(int i_ID)
        {
            Button buttonToMake = new Button()
            {
                Padding = new Thickness(20, 20, 20, 20),
                Text = i_ID.ToString(),
                CornerRadius = 10,
                BackgroundColor = Color.Gold,
                IsVisible = true,
            };
            return buttonToMake;
        }
    }
}
