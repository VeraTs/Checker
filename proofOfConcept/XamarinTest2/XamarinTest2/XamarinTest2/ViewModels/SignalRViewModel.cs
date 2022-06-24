using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinTest2.ViewModels
{
    internal class SignalRViewModel
    {
        //public String NewToDoDesc { get; set; } = "";
        public Command Init { get; private set; }
        public SignalRViewModel()
        {
            Init = new Command(async () =>
            {
                // if for some reason, connection is inactive, activate
                if (App.HubConn.State == HubConnectionState.Disconnected)
                {
                    try
                    {
                        await App.HubConn.StartAsync();
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // if connection is active, invoke an event to add new listing
                /*if (App.HubConn.State == HubConnectionState.Connected)
                {
                    await App.HubConn.InvokeAsync("AddToDo", NewToDoDesc, DateTime.Now);
                }*/
            });
        }
    }
}
