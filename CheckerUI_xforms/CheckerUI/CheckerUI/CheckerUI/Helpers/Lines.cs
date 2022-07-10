using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerUI.Enums;
using CheckerUI.Helpers.DishesFolder;
using CheckerUI.Helpers.LinesHelpers;
using CheckerUI.Models;
using CheckerUI.ViewModels;

namespace CheckerUI.Helpers
{
    public class Lines : BaseViewModel
    {
        private ObservableCollection<Line> m_LinesList = new ObservableCollection<Line>();

        private List<Dish> m_DishItems = new List<Dish>();

        public Lines(ObservableCollection<Line> i_list)
        {
            m_LinesList = i_list;
            GenerateLines();
        }

         // this needs to be deprecated
        private async void GenerateLines()
        {

            var data = new AddDishesData();
            const string s1 = "Hot Line";
            const string s2 = "Cold Line";
            const string s3 = "Oven Line";
            await data.GetDishList();
            await data.GetDishList2();
            await data.GetDishList3();
            var d1 = LineBuilder.GenerateLine(s1, 6, 20, data.d1, eLineState.Open);
           var d2 = LineBuilder.GenerateLine(s2, 7, 20, data.d2, eLineState.Busy);
           var d3 = LineBuilder.GenerateLine(s3, 8, 20, data.d3, eLineState.Closed);
            m_LinesList.Add(d1);
            m_LinesList.Add(d2);
            m_LinesList.Add(d3);
        }

        public ObservableCollection<Line> DepartmentsList
        {
            get => m_LinesList;
            private set
            {
                m_LinesList = value; 
                OnPropertyChanged(nameof(DepartmentsList));
            }
        }
    }
}
