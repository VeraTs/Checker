using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class BaseLineViewModel : BaseViewModel
    {
        public Command ReturnCommand { get; }

        public BaseLineViewModel() : base()
        {
            
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}
