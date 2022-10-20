using System.ComponentModel;
using System.Threading.Tasks;
using CheckerWaitersApp.Models;
using CheckerWaitersApp.Views.CreateOrderView;
using Xamarin.Forms;

namespace CheckerWaitersApp.ViewModels
{
    public class MainPageViewModel : TriggerAction<ImageButton>, INotifyPropertyChanged
    {
        private CreateOrderView waitersPage;
      
        public Command LogInCommand { get; set; }
      
        public Command ExitCommand { get; private set; }

        
        string _mUserName;
        private string _mPassword;
        public MainPageViewModel()
        {
            LogInCommand = new Command(async () =>
            {
                var res = await checkUserDetails();
                if (res)
                {
                    App.RestId = App.restaurant.id; 
                    App.Repository.LoadData();
                    await Application.Current.MainPage.Navigation.PushAsync(new CreateOrderView());
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

        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        bool _hidePassword = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserName
        {
            get => _mUserName;
            set
            {
                _mUserName = value;
                var args = new PropertyChangedEventArgs(nameof(UserName));
                PropertyChanged?.Invoke(this, args);
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

        private async Task<bool> checkUserDetails()
        {
            var user = new User(UserName, Password);
            var res = await App.UserStore.LoginAsync(user);
            return res;
        }
    }
}

