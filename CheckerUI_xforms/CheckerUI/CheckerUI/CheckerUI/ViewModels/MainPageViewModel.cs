using System.ComponentModel;
using System.Threading.Tasks;
using CheckerUI.Views;
using Xamarin.Forms;
using CheckerUI.Models;

namespace CheckerUI.ViewModels
{
    public class MainPageViewModel: TriggerAction<ImageButton>,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string m_UserName;
        private string m_Password;
        private bool m_hidePassword = true;
        public Command LogInCommand { get; }
        public Command ExitCommand { get; }
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        public MainPageViewModel()
        {
            LogInCommand = new Command(async () =>
            {
                var res = await checkUserDetails();
                if (res)
                {
                    App.RestId = App.restaurant.id;
                    App.Repository.LoadData();
                    await Application.Current.MainPage.Navigation.PushAsync(new UserMainPage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Wrong Details", "User name or Password incorrect", "Ok");
                }
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
      
        private async Task<bool> checkUserDetails()
        {
            var user = new User()
            {
                userEmail = m_UserName, userPassword = Password
            };
            var res = await App.UserStore.LoginAsync(user);
            return res;
        }
    }
}
