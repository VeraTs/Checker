using CheckerUI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewLineView : ContentPage
    {
        public LineViewModel m_ViewModel;
        public int LineId { get; set; }
        public string LineName { get; set; }
        public NewLineView(LineViewModel i_VM)
        {
            InitializeComponent();
            BackgroundColor = Color.Transparent;
            m_ViewModel = i_VM;
            BindingContext = m_ViewModel;
            LineId = i_VM.LineId;
            LineName = i_VM.LineName;
            m_ItemsToMakeCardView.ViewModel = m_ViewModel;
            m_ItemsInProgressCardView.ViewModel = m_ViewModel;
            m_ItemsDoneCardView.ViewModel = m_ViewModel;
            m_ItemsLockedCardView.ViewModel = m_ViewModel;
        }

        public int GetLineId()
        {
            return LineId;
        }

        public string GetLineName()
        {
            return LineName;
        }
    }
}