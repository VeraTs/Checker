using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CheckerUI.Enums;
using CheckerUI.Helpers.DishesFolder;
using CheckerUI.Models;
using CheckerUI.ViewModels;

namespace CheckerUI.Helpers
{
    public class Departments : BaseViewModel
    {
        private ObservableCollection<DeptModel> m_DeptsList = new ObservableCollection<DeptModel>();

        private List<Dish_item> m_DishItems = new List<Dish_item>();

        public Departments(ObservableCollection<DeptModel> i_list)
        {
            m_DeptsList = i_list;
            GenerateDepts();
        }
        private async void GenerateDepts()
        {

            AddDishesData data = new AddDishesData();
            string s1 = "Hot Line";
            string s2 = "Cold Line";
            string s3 = "Oven Line";
            await data.GetDishList();
            await data.GetDishList2();
            await data.GetDishList3();
            DeptModel d1 = new DeptModel();
           d1 = DeptBuilder.GenerateDept(s1, 1, 20, data.d1, eDeptState.chill);
            DeptModel d2 = new DeptModel();
           d2= DeptBuilder.GenerateDept(s2, 2, 20, data.d2, eDeptState.busy);
            DeptModel d3 = new DeptModel();
            d3=DeptBuilder.GenerateDept(s3, 3, 20, data.d3, eDeptState.overload);
            m_DeptsList.Add(d1);
            m_DeptsList.Add(d2);
            m_DeptsList.Add(d3);
        }

        public ObservableCollection<DeptModel> DepartmentsList
        {
            get => m_DeptsList;
            private set
            {
                m_DeptsList = value; 
                OnPropertyChanged(nameof(DepartmentsList));
            }
        }
    }
}
