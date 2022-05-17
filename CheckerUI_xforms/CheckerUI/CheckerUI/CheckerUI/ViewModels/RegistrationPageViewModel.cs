using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using CheckerUI.Models;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class RegistrationPageViewModel : TriggerAction<ImageButton>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RegistrationPageModel m_Model;

        public Command SignCommand { get; }
        public Command ReturnCommand { get; }


        public RegistrationPageViewModel()
        {
            m_Model = new RegistrationPageModel();
            SignCommand = new Command(async () =>
            {
               await checkSignUpUser();
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
        public string UserName
        {
            get => m_Model.m_UserName;
            set
            {
                m_Model.m_UserName = value;
                var args = new PropertyChangedEventArgs(nameof(UserName));
                PropertyChanged?.Invoke(this, args);
            }
        }
        public string Email
        {
            get => m_Model.m_Email;
            set
            {
                m_Model.m_Email = value;
                var args = new PropertyChangedEventArgs(nameof(Email));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string EmailConfirmed
        {
            get => m_Model.m_EmailConfirmed;
            set
            {
                m_Model.m_EmailConfirmed = value;
                var args = new PropertyChangedEventArgs(nameof(EmailConfirmed));
                PropertyChanged?.Invoke(this, args);
            }
        }

       
        public string PhoneNumber
        {
            get => m_Model.m_PhoneNumber;
            set
            {
                m_Model.m_PhoneNumber = value;
                var args = new PropertyChangedEventArgs(nameof(PhoneNumber));
                PropertyChanged?.Invoke(this, args);
            }
        }
        public string Password
        {
            get => m_Model.m_Password;
            set
            {
                m_Model.m_Password = value;
                var args = new PropertyChangedEventArgs(nameof(Password));
                PropertyChanged?.Invoke(this, args);
            }
        }
        public string PasswordConfirmed
        {
            get => m_Model.m_PasswordConfirmed;
            set
            {
                m_Model.m_PasswordConfirmed = value;
                var args = new PropertyChangedEventArgs(nameof(PasswordConfirmed));
                PropertyChanged?.Invoke(this, args);
            }
        }
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        bool m_HidePassword = true;
      
        
        public bool HidePassword
        {
            set
            {
                if (m_HidePassword != value)
                {
                    m_HidePassword = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HidePassword)));
                }
            }
            get => m_HidePassword;
        }
       
        protected override void Invoke(ImageButton sender)
        {
            sender.Source = HidePassword ? ShowIcon : HideIcon;
            HidePassword = !HidePassword;
        }

        private bool compareEmails()
        {
            return m_Model.m_Email == m_Model.m_EmailConfirmed;
        }

        private bool comparePasswords()
        {
            return m_Model.m_Password == m_Model.m_PasswordConfirmed;
        }

        private bool checkUserName()
        {
            return true;
        }

        private bool checkPhoneNumber()
        {
            return true;
        }

        private async Task<bool> checkSignUpUser()
        {
            string message="";
            string title ="";
            bool res = false;
            if (!checkUserName())
            {
                title = "Wrong User name";
                message = "This user name already exists in the system";
            }
            else if (!checkPhoneNumber())
            {
                title = "Invalid Phone Number";
                message = "The phone number entered is invalid";
            }
            else if (!compareEmails())
            {
                title = "Incompatible Emails";
                message = "Entered Emails addresses do not match";
            }
            else if (!comparePasswords())
            {
                title = "Incompatible Passwords";
                message = "Entered Passwords do not match";
            }
            else
            {
                message = "All details have been verified !";
                title = "Signed Up Completed";
                saveUserDetails();
                res = true;
            }
            await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
            return await Task.FromResult(res);
        }

        private void saveUserDetails()
        {

        }
    }
}
