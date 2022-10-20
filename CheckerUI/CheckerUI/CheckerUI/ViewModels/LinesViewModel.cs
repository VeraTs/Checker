using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CheckerUI.Models;
using CheckerUI.Views;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;

namespace CheckerUI.ViewModels
{
    public class LinesViewModel : BaseViewModel
    {
        private ObservableCollection<LineViewModel> m_LinesList = new ObservableCollection<LineViewModel>();
        private readonly ObservableCollection<NewLineView> m_LinesViews = new ObservableCollection<NewLineView>();

        public int m_ClickedLineId { get; set; } = 0;
        public string ClickedLineName { get; set; } = "";
        public LinesViewModel()
        {
            initLinesByRepository();

            App.HubConn.On<List<LineDTO>>("UpdatedLines", (linesDTO) =>
            {
                foreach (var dto in linesDTO)
                {
                    var lineVM = m_LinesList.First(line => line.LineId == dto.lineId);
                    if (lineVM.LineId > 0)
                    {
                        foreach (var orderItem in from orderItem in dto.lockedItems let currentID = orderItem.dishId select orderItem)
                        {
                            orderItem.dish = App.Repository.DishesDictionary[orderItem.dishId];
                            lineVM.moveItemViewToRightView(orderItem);
                        }

                        foreach (var orderItem in from orderItem in dto.toDoItems let currentID = orderItem.dishId select orderItem)
                        {
                            orderItem.dish = App.Repository.DishesDictionary[orderItem.dishId];
                            lineVM.moveItemViewToRightView(orderItem);
                        }

                        foreach (var orderItem in from orderItem in dto.doingItems let currentID = orderItem.dishId select orderItem)
                        {
                            orderItem.dish = App.Repository.DishesDictionary[orderItem.dishId];
                            lineVM.moveItemViewToRightView(orderItem);
                        }
                        foreach (var orderItem in from orderItem in dto.doneItems let currentID = orderItem.dishId select orderItem)
                        {
                            orderItem.dish = App.Repository.DishesDictionary[orderItem.dishId];
                            lineVM.moveItemViewToRightView(orderItem);
                        }
                    }
                }
            });
            App.HubConn.On<OrderItem>("ItemMoved", (item) =>
            {
                item.dish = App.Repository.DishesDictionary[item.dishId];
                var lineVm = m_LinesList.First(line => line.LineId == item.dish.lineId);
                lineVm.moveItemViewToRightView(item);
            });

            App.HubConn.On<OrderItem, int>("PlaceItem", (item, spot) =>
            {
                var msg = "Please place the dish in Zone number :" + spot;
                Application.Current.MainPage.DisplayAlert("msg", msg, "OK");
            });
            StartListening();
        }
        private void initLinesByRepository()
        {
            var list = App.Repository.lines;
            foreach (var vm in list.Select(item => new LineViewModel(item)))
            {
                m_LinesList.Add(vm);
                var view = new NewLineView(vm);
                m_LinesViews.Add(view);
            }
        }

        private static async void StartListening()
        {
            if (App.HubConn.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await App.HubConn.StartAsync();     // start async connection to SignalR Hub at server

                }
                catch (System.Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception! Disconnected", ex.Message, "OK");
                }
            }

            try
            {
                await App.HubConn.InvokeAsync("RegisterForGroup", App.RestId);
            }
            catch (System.Exception ex)
            {
               // await Application.Current.MainPage.DisplayAlert("Exception!", ex.Message, "OK");
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
        public async Task LineButton_OnClicked(object sender, EventArgs e)
        {
            var view = m_LinesViews.FirstOrDefault(ll => ll.GetLineId() == m_ClickedLineId);
            
            await Application.Current.MainPage.Navigation.PushAsync(view);
        }
        public async Task LineButton_OnClickedString(object sender, EventArgs e)
        {
            var view = m_LinesViews.FirstOrDefault(ll => ll.GetLineName() == ClickedLineName);

            await Application.Current.MainPage.Navigation.PushAsync(view);
        }
    }
}
