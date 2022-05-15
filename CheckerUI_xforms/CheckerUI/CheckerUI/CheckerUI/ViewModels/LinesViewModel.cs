using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CheckerUI.Helpers;
using CheckerUI.Helpers.Line;
using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class LinesViewModel : BaseViewModel
    {
        private readonly ObservableCollection<LineModel> m_models;
        private ObservableCollection<LineView> m_LinesList = new ObservableCollection<LineView>();
        private Lines m_Lines;
        public LinesViewModel()
        {
            m_models = new ObservableCollection<LineModel>();
            GenerateLines();
            
        }
        private async void GenerateLines()
        {
            m_Lines = new Lines(m_models);
            foreach (var model in m_models)
            {
                var lineView = new LineView(model);
                m_LinesList.Add(lineView);
            }
        }
        public ObservableCollection<LineView> LinesList
        {
            get => m_LinesList;
            set
            {
                m_LinesList = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// /TO DO : right now we are not distinguish between lines
        /// </summary>
        public async Task LineButton_OnClicked(object sender, EventArgs e)
        {
            var s = new NewLineView();
            await Application.Current.MainPage.Navigation.PushAsync(s);
        }
    }
}
