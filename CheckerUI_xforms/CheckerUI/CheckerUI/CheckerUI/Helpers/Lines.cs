using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckerUI.Enums;
using CheckerUI.Helpers.DishesFolder;
using CheckerUI.Helpers.Line;
using CheckerUI.Models;
using CheckerUI.ViewModels;

namespace CheckerUI.Helpers
{
    public class Lines : BaseViewModel
    {
        private ObservableCollection<LineModel> m_LinesList = new ObservableCollection<LineModel>();

        private List<Dish_item> m_DishItems = new List<Dish_item>();

        public Lines(ObservableCollection<LineModel> i_list)
        {
            m_LinesList = i_list;
            GenerateLines();
        }
        private async void GenerateLines()
        {

            var data = new AddDishesData();
            const string s1 = "Hot Line";
            const string s2 = "Cold Line";
            const string s3 = "Oven Line";
            await data.GetDishList();
            await data.GetDishList2();
            await data.GetDishList3();
            var d1 = LineBuilder.GenerateLine(s1, 1, 20, data.d1, eLineState.chill);
           var d2 = LineBuilder.GenerateLine(s2, 2, 20, data.d2, eLineState.busy);
           var d3 = LineBuilder.GenerateLine(s3, 3, 20, data.d3, eLineState.overload);
            m_LinesList.Add(d1);
            m_LinesList.Add(d2);
            m_LinesList.Add(d3);
        }

        public ObservableCollection<LineModel> DepartmentsList
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
