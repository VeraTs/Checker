using System.Collections.Generic;
using CheckerUI.Enums;
using CheckerUI.Models;
using CheckerUI.ViewModels;
using Xamarin.Forms;

namespace CheckerUI.Helpers.Line
{
    public class LineView : BaseViewModel
    {
        private readonly LineModel model;
        private List<Dish_item> m_DishItems = new List<Dish_item>();
        private Color m_BackgroundState;
        private Color m_LabelsColorState;
        
        public LineView(LineModel i_model)
        {
            m_BackgroundState = new Color();
            model = LineBuilder.GenerateLine(i_model.m_LineName, i_model.m_LineID, i_model.m_MaximumParallelism, i_model.m_DishesList, i_model.m_LineState);
            setColorState();
        }

        private void setColorState()
        {
            switch (model.m_LineState)
            {
                case eLineState.chill:
                {
                    LabelsColor = Color.DimGray;
                    LineStateColor = Color.BlanchedAlmond;
                    break;
                }
                case eLineState.busy:
                {
                    LabelsColor = Color.DarkCyan; 
                    LineStateColor = Color.YellowGreen;
                    break;
                }
                case eLineState.overload:
                {
                    LineStateColor = Color.White;
                    LineStateColor = Color.Firebrick;
                    break;
                }
            }
        }
        public string LineName
        {
            get => model.m_LineName;
            set
            {
                model.m_LineName = value;
                OnPropertyChanged(nameof(LineName));
            } 
        }

        public int LineID
        {
            get => model.m_LineID;
            set
            {
                model.m_LineID = value;
                OnPropertyChanged(nameof(LineID));
            } 
        }

        public string MaximumParallelism
        {
            get => model.m_MaximumParallelism.ToString();
            set
            {
                model.m_MaximumParallelism = int.Parse(value);
                OnPropertyChanged(nameof(MaximumParallelism));
            }
        }

        public eLineState LineStateString
        {
            get => model.m_LineState;
            set
            {
                model.m_LineState = value;
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
