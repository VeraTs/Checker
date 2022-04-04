using CheckerUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using CheckerUI.Helpers;
using CheckerUI.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineView : ContentPage
    {
        private List<OrderItemView> OrdersViews = new List<OrderItemView>(10);

        private readonly BaseLineViewModel baseVM;
        private ListView m_LastListWithItemSelected = null;

        public LineView(string i_Title)
        {
            InitializeComponent();
            this.Title = i_Title;
            BackgroundColor = Color.Transparent;
            baseVM = new BaseLineViewModel();
            baseVM.init(m_OrdersLayout);
            BindingContext = baseVM;
            m_GetOrdersButton.Command = baseVM.FeelOrdersCommand;
            m_ReturnButton.Command = baseVM.ReturnCommand;
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
                    await App.HubConn.InvokeAsync("InitialToDos");  // invoke initial event - to get all current listings
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            baseVM.LastSelectedItem = lv.SelectedItem as OrderItemView;
            m_LastListWithItemSelected = lv;
        }
        private void Cell_OnTapped(object sender, EventArgs e)
        {
            ViewCell vc = sender as ViewCell;
            baseVM.LastTappedCell = vc;
            baseVM.Button_Clicked(sender, EventArgs.Empty);
            m_LastListWithItemSelected.SelectedItem = null;
            
        }
        private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           Grid g = sender as Grid;
           baseVM.m_LastGridInCell = g;
        }
    }
}