using System.Collections.Generic;
using CheckerUI.Enums;
using CheckerUI.Helpers.LinesHelpers;
using CheckerUI.Models;
using CheckerUI.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class LineViewModel : BaseLineViewModel
    {
        private  Line model;
      
        private Color m_BackgroundState;
        private Color m_LabelsColorState;

      
        public LineViewModel(Line i_model) :base()
        {
            base.init();
            m_BackgroundState = new Color();
            model = LineBuilder.GenerateLine(i_model.Name, i_model.id, i_model.Limit, null, i_model.State);
            model.Dishes = new List<Dish>();
            var dishes = App.Repository.Dishes;
            App.HubConn.On<List<LineDTO>>("UpdatedLines", (linesDTO) =>
            {
                var dto = linesDTO.Find(line => line.lineId == model.id);
                if (dto.lineId == model.id)
                {
                    deAllocations();
                    init();
                    foreach (var orderItem in dto.LockedItems)
                    {
                        var currentID = orderItem.dishId;
                        orderItem.dish = dishes.Find(dish => dish.id == currentID);
                        AddOrderItemToLocked(orderItem);
                    }

                    foreach (var orderItem in dto.ToDoItems)
                    {
                        var currentID = orderItem.dishId;
                        orderItem.dish = dishes.Find(dish => dish.id == currentID);
                        AddOrderItemToAvailable(orderItem);
                    }

                    foreach (var orderItem in dto.DoingItems)
                    {
                        var currentID = orderItem.dishId;
                        orderItem.dish = dishes.Find(dish => dish.id == currentID);
                        AddOrderItemToInProgress(orderItem);
                    }
                }
            });
                sendListener();
            setColorState();
        }
        private static async void sendListener()
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
        public void InitLineView(Line i_model)
        {
            base.init();
            m_BackgroundState = new Color();
            model = LineBuilder.GenerateLine(i_model.Name, i_model.id, i_model.Limit, null, i_model.State);
            setColorState();
        }
        private void setColorState()
        {
            switch (model.State)
            {
                case eLineState.Open:
                {
                    LabelsColor = Color.DimGray;
                    LineStateColor = Color.BlanchedAlmond;
                    break;
                }
                case eLineState.Busy:
                {
                    LabelsColor = Color.DarkCyan; 
                    LineStateColor = Color.YellowGreen;
                    break;
                }
                case eLineState.Closed:
                {
                    LineStateColor = Color.White;
                    LineStateColor = Color.Firebrick;
                    break;
                }
            }
        }
        public string LineName
        {
            get => model.Name;
            set
            {
                model.Name = value;
                OnPropertyChanged(nameof(LineName));
            } 
        }

        public int LineID
        {
            get => model.id;
            set
            {
                model.id = value;
                OnPropertyChanged(nameof(LineID));
            } 
        }

        public string MaximumParallelism
        {
            get => model.Limit.ToString();
            set
            {
                model.Limit = int.Parse(value);
                OnPropertyChanged(nameof(MaximumParallelism));
            }
        }

        public eLineState LineStateString
        {
            get => model.State;
            set
            {
                model.State = value;
                OnPropertyChanged(nameof(LineStateString));
            }
        }

        public Color LineStateColor
        {
            get => m_BackgroundState;
            private set
            {
                m_BackgroundState = value; 
                OnPropertyChanged(nameof(LineStateColor));

            }
        }

        public Color LabelsColor
        {
            get => m_LabelsColorState;
            private set
            {
                m_LabelsColorState = value;
                OnPropertyChanged(nameof(LabelsColor));

            }
        }
    }
}
