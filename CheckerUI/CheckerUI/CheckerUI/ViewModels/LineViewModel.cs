using System.Collections.Generic;
using CheckerUI.Enums;
using CheckerUI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Xamarin.Forms;
//The lines all inherit from the base line
//Each line represents a work area of ​​a cook such as a hot line
//and so later we can make personal adjustments according to the needs of a line (polymorphism)
//The line is divided into 4 windows that display the dishes in the line
//and their status using the appropriate window
namespace CheckerUI.ViewModels
{
    public class LineViewModel : BaseLineViewModel
    {
        private readonly Line model;

        private Color m_BackgroundState;
        private Color m_LabelsColorState;
        public int LineId { get; set; }

        public LineViewModel(Line i_model) : base()
        {
            base.init();
            m_BackgroundState = new Color();
            model = i_model;
            LineId = i_model.id;
            LineName = i_model.name;
            model.dishes = new List<Dish>();
            model.dishes.AddRange(i_model.dishes);
            setColorState();
        }

        private void setColorState()
        {
            switch (model.state)
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
            get => model.name;
            set
            {
                model.name = value;
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
            get => model.limit.ToString();
            set
            {
                model.limit = int.Parse(value);
                OnPropertyChanged(nameof(MaximumParallelism));
            }
        }

        public string LineCapacity => "Current Capacity :" + this.m_OrderItemsViewsList.Count;

        public eLineState LineStateString
        {
            get => model.state;
            set
            {
                model.state = value;
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
