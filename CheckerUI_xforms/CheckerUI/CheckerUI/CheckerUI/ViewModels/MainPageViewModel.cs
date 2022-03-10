using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class MainPageViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command SaveCommand { get; }
        public Command EraseCommand { get; }
        
        public Command ExitCommand { get; }
        public ObservableCollection<string> AllNotes { get; set; } =  new ObservableCollection<string>();
        string m_UserName;
        private string m_Password;
        public string UserName
        {
            get => m_UserName;
            set
            {
                m_UserName = value;
                var args = new PropertyChangedEventArgs(nameof(UserName));
                PropertyChanged?.Invoke(this,args);
            }
        }

        public string Password
        {
            get => m_Password;
            set
            {
                m_Password = value;
                var args = new PropertyChangedEventArgs(nameof(Password));
                PropertyChanged?.Invoke(this, args);
            }
        }
        public MainPageViewModel()
        {
            SaveCommand = new Command(async () =>
            {
                AllNotes.Add(UserName);
                AllNotes.Add(Password);
                UserName = string.Empty;
                Password = string.Empty;
                //Check if to last notes are good user name and password if so next page?
                // else -Bad

                var LoggedVM = new LoggedMainPageViewModel();
                var LoggedPage = new LoggedMainPage();
                LoggedPage.BindingContext = LoggedVM;
                await Application.Current.MainPage.Navigation.PushAsync(LoggedPage);
            });
            EraseCommand = new Command(async () =>
            {
                var signVM = new SignUpPageViewModel();
                var signPage = new SignUpPage();
                signPage.BindingContext = signVM;
                await Application.Current.MainPage.Navigation.PushAsync(signPage);
                // await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(signPage));
            });
            ExitCommand = new Command(() =>
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            });
        }
         
    }
}
