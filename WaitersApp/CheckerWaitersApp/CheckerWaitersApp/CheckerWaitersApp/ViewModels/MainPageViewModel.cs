using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using CheckerWaitersApp.Views.CreateOrderView;
using Xamarin.Forms;

namespace CheckerWaitersApp.ViewModels
{
    public class MainPageViewModel : TriggerAction<ImageButton>, INotifyPropertyChanged
    {
        private CreateOrderView waitersPage;
      
        public Command LogInCommand { get; set; }
      
        public Command ExitCommand { get; private set; }

        public ObservableCollection<string> AllNotes { get; set; } = new ObservableCollection<string>();
        string _mUserName;
        private string _mPassword;
        public MainPageViewModel()
        {
            LogInCommand = new Command(async () =>
            {
                waitersPage = new CreateOrderView();
                await Application.Current.MainPage.Navigation.PushAsync(waitersPage);
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


    }
}

