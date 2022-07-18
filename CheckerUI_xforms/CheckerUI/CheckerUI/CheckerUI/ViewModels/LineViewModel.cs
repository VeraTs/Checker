using System.Collections.Generic;
using CheckerUI.Enums;
using CheckerUI.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class LineViewModel : BaseLineViewModel
    {
        private readonly Line model;
      
        private Color m_BackgroundState;
        private Color m_LabelsColorState;

      
        public LineViewModel(Line i_model) :base()
        {
            base.init();
            m_BackgroundState = new Color();
            
            model = i_model;
            model.Dishes = new List<Dish>();
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
