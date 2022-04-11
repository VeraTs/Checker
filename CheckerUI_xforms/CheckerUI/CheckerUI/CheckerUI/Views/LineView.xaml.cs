using CheckerUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CheckerUI.Helpers;
using CheckerUI.Models;
using CheckerUI.Views.PopupViews;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
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
        private Expander m_lastTappedExpander = null;
        private CollectionView m_LastTappedView = null;

        private int m_CounterToMake = 0;
        private int m_CounterInProgress = 0;

        public LineView(string i_Title)
        {
            InitializeComponent();
            this.Title = i_Title;
            BackgroundColor = Color.Transparent;
            baseVM = new BaseLineViewModel();
            baseVM.init();
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

        private void Expander_OnTapped(object sender, EventArgs e)
        {
            var current_expander = sender as Expander;
            if (m_lastTappedExpander != null && m_lastTappedExpander != current_expander)
            {
                m_lastTappedExpander.IsExpanded = false;
                current_expander.IsExpanded = true;
            }
            m_lastTappedExpander = current_expander;
        }


        private void ToMakeListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection;
            var prev = e.PreviousSelection;
            if (item != prev)
            {
                
            }
        }

        public async Task InvokeNewOrderAni()
        {
            await Navigation.ShowPopupAsync(new PopupPageNewOrder());
        }
      

        private  void M_NoJobYet_OnClicked(object sender, EventArgs e)
        {
            baseVM.refresh();
        }



        private async void InProgressListView_OnChildAdded(object sender, ElementEventArgs e)
        {
            m_LabelInProgress.IsVisible = false; 
            m_FireLottie.IsVisible = true; 
            m_FireLottie.PlayAnimation(); 
            await Task.Delay(3000); 
            m_FireLottie.IsVisible = false; 
            m_LabelInProgress.IsVisible = true;
        }

        private async void ToMakeListView_OnChildAdded(object sender, ElementEventArgs e)
        {
            m_LabelToMake.IsVisible = false;
            m_NewOrderLottie.IsVisible = true;
            m_NewOrderLottie.PlayAnimation();
            await Task.Delay(3000);
            m_NewOrderLottie.IsVisible = false;
            m_LabelToMake.IsVisible = true;
        }
    }
}
