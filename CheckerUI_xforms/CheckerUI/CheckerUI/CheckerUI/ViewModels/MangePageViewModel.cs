using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class MangePageViewModel 
    {
        public Command ReturnCommand { get; }

        public MangePageViewModel()
        {
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}
