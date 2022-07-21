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
        public Command NextPageCommand { get; private set; }
        private List<Dish> m_Dishes { get; set; }

        public int m_ClickedLineId { get; set; } = 0;
        public string ClickedLineName { get; set; }
        public LinesViewModel()
        {

            m_Dishes = App.Repository.Dishes;
            initLinesByRepository();


            App.HubConn.On<List<LineDTO>>("UpdatedLines", (linesDTO) =>
            {

                foreach (var dto in linesDTO)
                {
                    var lineVM = m_LinesList.First(line => line.LineID == dto.lineId);
                    if (lineVM.LineID > 0)
                    {
                        foreach (var orderItem in dto.LockedItems)
                        {
                            var currentID = orderItem.dishId;
                            orderItem.dish = m_Dishes.Find(dish => dish.id == currentID);
                            lineVM.moveItemViewToRightView(orderItem);
                        }

                        foreach (var orderItem in dto.ToDoItems)
                        {
                            var currentID = orderItem.dishId;
                            orderItem.dish = m_Dishes.Find(dish => dish.id == currentID);
                            lineVM.moveItemViewToRightView(orderItem);
                        }

                        foreach (var orderItem in dto.DoingItems)
                        {
                            var currentID = orderItem.dishId;
                            orderItem.dish = m_Dishes.Find(dish => dish.id == currentID);
                            lineVM.moveItemViewToRightView(orderItem);
                        }
                    }
                }
            });
            App.HubConn.On<OrderItem>("ItemMoved", (item) =>
            {
                item.dish = m_Dishes.Find(dish => dish.id == item.dishId);
                var lineVm = m_LinesList.First(line => line.LineID == item.dish.lineId);
                lineVm.moveItemViewToRightView(item);
            });
            StartListening();
            NextPageCommand = new Command(() =>
            {
                
            });
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
        //private void initUsingRepository()
        //{
        //   // var items = App.Repository.OrderedItems;
        //    foreach (var orderItem in items)
        //    {
        //        var lineId = orderItem.dish.lineId;
        //        var lineView = m_LinesViews.First(view => view.m_ViewModel.LineID == lineId);
        //        var vm = lineView.m_ViewModel;
        //        switch (orderItem.lineStatus)
        //        {
        //            case eLineItemStatus.Locked:
        //                {
        //                    vm.AddOrderItemToLocked(orderItem);
        //                    break;
        //                }
        //            case eLineItemStatus.ToDo:
        //                {
        //                    vm.AddOrderItemToAvailable(orderItem);

        //                    break;
        //                }
        //            case eLineItemStatus.Doing:
        //                {
        //                    vm.AddOrderItemToInProgress(orderItem);
        //                    break;
        //                }
        //            case eLineItemStatus.Done:
        //                {
        //                    vm.AddOrderItemToDone(orderItem);
        //                    break;
        //                }
        //        }
        //    }
        //}
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
                await App.HubConn.InvokeAsync("RegisterForGroup", 1);
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
