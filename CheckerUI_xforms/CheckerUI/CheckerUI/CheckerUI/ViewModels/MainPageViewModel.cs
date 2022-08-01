using System.ComponentModel;
using System.Threading.Tasks;
using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class MainPageViewModel: TriggerAction<ImageButton>,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string m_UserName;
        private string m_Password;
        private bool m_hidePassword = true;
        public Command LogInCommand { get; }
        public Command SignUpCommand { get; }
        public Command ExitCommand { get; }
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        public MainPageViewModel()
        {
            LogInCommand = new Command(async () =>
            {
                if (checkUserDetails())
                {
                    var user = await loadUserDetails();
                   
                    var loggedPage = new UserMainPage
                    {
                      
                    };
                    await Application.Current.MainPage.Navigation.PushAsync(loggedPage);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Wrong Details", "User name or Password incorrect", "Ok");
                }
            });

            SignUpCommand = new Command(async () =>
            {
                var registrationVm = new RegistrationPageViewModel();
                var regPage = new RegistrationPage
                {
                    BindingContext = registrationVm
                };
                await Application.Current.MainPage.Navigation.PushAsync(regPage);
            });

            ExitCommand = new Command(() =>
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            });
        }

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
        public bool HidePassword
        {
            get => m_hidePassword;
            set
            {
                if (m_hidePassword != value)
                {
                    m_hidePassword = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HidePassword)));
                }
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
        protected override void Invoke(ImageButton sender)
        {
            sender.Source = HidePassword ? ShowIcon : HideIcon;
            HidePassword = !HidePassword;
        }
      
        private bool checkUserDetails()
        {
           // return (UserName == "admin" && Password == "admin");
           return true;
        }
        private async Task<RegistrationPageViewModel> loadUserDetails()
        {
            RegistrationPageViewModel user = new RegistrationPageViewModel
            {
                UserName = UserName,
                Password = Password
            };
            return await Task.FromResult(user);
        }
    }
}
