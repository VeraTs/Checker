using System;
using System.Collections.Generic;
using System.Text;
using CheckerUI.Enums;
using CheckerUI.Helpers.DishesFolder;
using CheckerUI.Models;
using CheckerUI.ViewModels;
using Lottie.Forms;
using Xamarin.Forms;

namespace CheckerUI.Helpers.Deparment
{
    public class DeptView : BaseViewModel
    {
        private DeptModel model;
        private List<Dish_item> m_DishItems = new List<Dish_item>();
        private Color m_BackgroundState;
        private Color m_LabelsColorState;
        
        public DeptView(DeptModel i_model)
        {
            m_BackgroundState = new Color();
            model = DeptBuilder.GenerateDept(i_model.m_DeptName, i_model.m_DeptID, i_model.m_MaximumParallelism, i_model.m_DishesList, i_model.m_DeptState);
            setColorState();
        }

        private void setColorState()
        {
            switch (model.m_DeptState)
            {
                case eDeptState.chill:
                {
                    LabelsColor = Color.DimGray;
                    DeptStateColor = Color.BlanchedAlmond;
                    break;
                }
                case eDeptState.busy:
                {
                    LabelsColor = Color.DarkCyan; 
                    DeptStateColor = Color.YellowGreen;
                    break;
                }
                case eDeptState.overload:
                {
                    DeptStateColor = Color.White;
                    DeptStateColor = Color.Firebrick;
                    break;
                }
            }
        }
        public string DeptName
        {
            get => model.m_DeptName;
            set
            {
                model.m_DeptName = value;
                OnPropertyChanged(nameof(DeptName));
            } 
        }

        public int DeptID
        {
            get => model.m_DeptID;
            set
            {
                model.m_DeptID = value;
                OnPropertyChanged(nameof(DeptID));
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

        public eDeptState DeptStateString
        {
            get => model.m_DeptState;
            set
            {
                model.m_DeptState = value;
                OnPropertyChanged(nameof(DeptStateString));
            }
        }

        public Color DeptStateColor
        {
            get => m_BackgroundState;
            private set
            {
                m_BackgroundState = value; 
                OnPropertyChanged(nameof(DeptStateColor));

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
