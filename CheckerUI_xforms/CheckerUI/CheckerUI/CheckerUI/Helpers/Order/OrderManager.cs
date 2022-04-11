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
            for (int i = 10; i < 20; i++) feel_layout(i);
        }

        private void sortOrders() // each time a new order entered
                                  // need to sort it to her right line -
                                  // only the one added
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
                "Pizza", "Burger", "Fish"
            };
            int status = 0;
            int id = m_TotalOrdersCounterID++;
           

            OrderIDNotifier m = new OrderIDNotifier(id, status);
            OrderItemView order = new OrderItemView(nmList[m_TotalOrdersCounterID%3], m);
            m_Orders.Add(id, order);
            m_HotLineOrders.Add(order);
        }
        public void feel_layout(int i_id) // make it private
        {
            List<string> nmList = new List<string>()
            {
                "Pizza", "Burger", "Fish"
            };
            int status = 0;
            int id = i_id;
           
            OrderIDNotifier m = new OrderIDNotifier(id, status);
            OrderItemView order = new OrderItemView(nmList[i_id%3], m);
            m_Orders.Add(id, order);
            m_HotLineOrders.Add(order);
        }
       
    }
}
