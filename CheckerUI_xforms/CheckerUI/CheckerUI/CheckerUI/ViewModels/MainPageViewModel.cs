using System.Collections.ObjectModel;
using System.ComponentModel;
using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class MainPageViewModel: TriggerAction<ImageButton>,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command EnterCommand { get; }
        public Command SignUpCommand { get; }
        
        public Command ExitCommand { get; }
        public ObservableCollection<string> AllNotes { get; set; } =  new ObservableCollection<string>();
        string _mUserName;
        private string _mPassword;

        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        bool _hidePassword = true;
        public string UserName
        {
            get => _mUserName;
            set
            {
                _mUserName = value;
                var args = new PropertyChangedEventArgs(nameof(UserName));
                PropertyChanged?.Invoke(this,args);
            }
        }
        public bool HidePassword
        {
            set
            {
                if (_hidePassword != value)
                {
                    _hidePassword = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HidePassword)));
                }
            }
            get => _hidePassword;
        }
        public string Password
        {
            get => _mPassword;
            set
            {
                _mPassword = value;
                var args = new PropertyChangedEventArgs(nameof(Password));
                PropertyChanged?.Invoke(this, args);
            }
        }
        protected override void Invoke(ImageButton sender)
        {
            sender.Source = HidePassword ? ShowIcon : HideIcon;
            HidePassword = !HidePassword;
        }
        public MainPageViewModel()
        {
            EnterCommand = new Command(async () =>
            {
                AllNotes.Add(UserName);
                AllNotes.Add(Password);
                UserName = string.Empty;
                Password = string.Empty;
                //Check if to last notes are good user name and password if so next page?
                // else -Bad

                var loggedVm = new LoggedMainPageViewModel();
                var loggedPage = new LoggedMainPage();
                loggedPage.BindingContext = loggedVm;
                await Application.Current.MainPage.Navigation.PushAsync(loggedPage);
            });
            SignUpCommand = new Command(async () =>
            {
                var signVm = new SignUpPageViewModel();
                var signPage = new SignUpPage();
                signPage.BindingContext = signVm;
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
