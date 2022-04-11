using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Enums;
using CheckerUI.Helpers;
using CheckerUI.Helpers.Deparment;
using CheckerUI.Helpers.DishesFolder;
using CheckerUI.Models;
using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class DeptsViewModel : BaseViewModel
    {
        private ObservableCollection<DeptModel> m_models;
        private ObservableCollection<DeptView> m_DeptsList = new ObservableCollection<DeptView>();

        private List<Dish_item> m_DishItems = new List<Dish_item>();

        private Departments m_Depts;
      

        public DeptsViewModel()
        {
            m_models = new ObservableCollection<DeptModel>();
            GenerateDepts();
            
        }
        private async void GenerateDepts()
        {
            m_Depts = new Departments(m_models);
            foreach (var model in m_models)
            {
                var deptView = new DeptView(model);
                m_DeptsList.Add(deptView);
            }
        }

        public ObservableCollection<DeptView> DepartmentsList
        {
            get => m_DeptsList;
            set
            {
                m_DeptsList = value;
                OnPropertyChanged();
            }
        }
        public async Task DeptButton_OnClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string deptName = button.Text;


            switch (deptName)
            {
                case "Hot Line":
                {
                    var baseLinePage = new LineView("Hot Line");
                    await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
                    break;
                }
                case "Cold Line":
                {
                    var baseLinePage = new LineView("Cold Line");
                    await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
                    break;
                }
                case "Oven Line":
                {
                    var baseLinePage = new LineView("Oven Line");
                    await Application.Current.MainPage.Navigation.PushAsync(baseLinePage);
                    break;
                }
            }
        }

    }
}
