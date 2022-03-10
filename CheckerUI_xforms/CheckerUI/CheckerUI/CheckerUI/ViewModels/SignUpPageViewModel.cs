
using System.Collections.ObjectModel;
using System.ComponentModel;

using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class SignUpPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<string> UserDetails { get; set; } = new ObservableCollection<string>();
        public Command SignCommand { get; }
        public Command ReturnCommand { get; }
        

        private string m_TempUserName;
        private string m_TempPassword;
        private string m_TempEmail;
        public string TempUserName
        {
            get => m_TempUserName;
            set
            {
                m_TempUserName = value;
                var args = new PropertyChangedEventArgs(nameof(TempUserName));
                PropertyChanged?.Invoke(this, args);
            }
        }
        public string TempEmail
        {
            get => m_TempEmail;
            set
            {
                m_TempEmail = value;
                var args = new PropertyChangedEventArgs(nameof(TempEmail));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string TempPassword
        {
            get => m_TempPassword;
            set
            {
                m_TempPassword = value;
                var args = new PropertyChangedEventArgs(nameof(TempPassword));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public SignUpPageViewModel()
        {
            SignCommand = new Command(() =>
            {
                // check if user already exists
                // save user details
                //login user
                //navigate main page
            });

            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

    }
}