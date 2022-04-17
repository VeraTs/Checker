using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers.Order;
using CheckerUI.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewLineView : ContentPage
    {
        private BaseLineViewModel m_ViewModel;
        public NewLineView()
        {
            InitializeComponent();
            BackgroundColor = Color.Transparent;
            m_ViewModel = new BaseLineViewModel();
            m_ViewModel.init();
            BindingContext = m_ViewModel;
            m_ItemsToMakeCardView.ViewModel = m_ViewModel;
            m_ItemsInProgressCardView.ViewModel = m_ViewModel;
            m_ItemsDoneCardView.ViewModel = m_ViewModel;
            m_ItemsLockedCardView.ViewModel = m_ViewModel;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // this is the only place of initiating the connection
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

    }
}