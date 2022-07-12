using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CheckerUI.Helpers;
using CheckerUI.Helpers.LinesHelpers;
using CheckerUI.Helpers.OrdersHelpers;
using CheckerUI.Models;
using CheckerUI.Views;
using Xamarin.Forms;

using Microsoft.AspNetCore.SignalR.Client;
namespace CheckerUI.ViewModels
{
    public class LinesViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Line> m_models;
        private ObservableCollection<LineViewModel> m_LinesList = new ObservableCollection<LineViewModel>();
        private ObservableCollection<NewLineView> m_LinesViews = new ObservableCollection<NewLineView>();
        private Lines m_Lines;

        private List<Dish> m_Dishes { get; set; } = new List<Dish>();

        public int m_ClickedLineId { get; set; } = 0;
        public LinesViewModel()
        {
            m_Dishes = App.Store.dishes;
            m_models = new ObservableCollection<Line>();
            init();
            App.HubConn.On<List<LineDTO>>("UpdatedLines", (linesDtos) =>
            {
               
                foreach (var lineDTO in linesDtos)
                {
                    
                    var id = lineDTO.line.id;
                    var lineVM = m_LinesList.FirstOrDefault(ll => ll.LineID == id);
                    if (lineVM != null)
                    {
                        //  lineVM.deAllocations();
                        foreach (var orderItem in lineDTO.LockedItems)
                        {
                            var currentID = orderItem.dishId;
                            var dish = m_Dishes.Find(dish => dish.id == currentID);
                            orderItem.dish = new Dish();
                            orderItem.dish = dish;
                            var itemView = new OrderItemView(orderItem);
                            lineVM.m_Orders.Add(itemView);
                            // lineVM.addItemToLinePlace(cardView);
                        }
                        foreach (var orderItem in lineDTO.ToDoItems)
                        {
                            var currentID = orderItem.dishId;
                            var dish = m_Dishes.Find(dish => dish.id == currentID);
                            orderItem.dish = new Dish();
                            orderItem.dish = dish;
                            var itemView = new OrderItemView(orderItem);
                            lineVM.m_Orders.Add(itemView);
                            // lineVM.addItemToLinePlace(cardView);
                        }
                        foreach (var orderItem in lineDTO.DoingItems)
                        {
                            var currentID = orderItem.dishId;
                            var dish = m_Dishes.Find(dish => dish.id == currentID);
                            orderItem.dish = new Dish();
                            orderItem.dish = dish;
                            var itemView = new OrderItemView(orderItem);
                            lineVM.m_Orders.Add(itemView);
                            // lineVM.addItemToLinePlace(cardView);
                        }
                    }
                }
            });
            SignalR();
        }

        private async void init()
        {
           
            var linesStore = App.linesStore;
            await linesStore.GetItemsAsync();
            var list = linesStore.lines;
            foreach (var item in list)
            {
                m_models.Add(item);
                var vm = new LineViewModel(item);
                m_LinesList.Add(vm);
                var view = new NewLineView(vm);
                m_LinesViews.Add(view);
            }
        }
        private async void SignalR()
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server

                }
                catch (System.Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception!", ex.Message, "OK");
                }
            }

            try
            {
                await App.HubConn.InvokeAsync("RegisterForGroup", 1);
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception!", ex.Message, "OK");
            }
        }
        private async void GenerateLines()
        {
            m_Lines = new Lines(m_models);
            foreach (var model in m_models)
            {
                var lineView = new LineViewModel(model);
                m_LinesList.Add(lineView);
            }
        }
        public ObservableCollection<LineViewModel> LinesList
        {
            get => m_LinesList;
            set
            {
                m_LinesList = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// /TO DO : right now we are not distinguish between lines
        /// </summary>
        public async Task LineButton_OnClicked(object sender, EventArgs e)
        {
            var view = m_LinesViews.FirstOrDefault(ll => ll.GetLineId() == m_ClickedLineId);
            await Application.Current.MainPage.Navigation.PushAsync(view);
        }
    }
}
