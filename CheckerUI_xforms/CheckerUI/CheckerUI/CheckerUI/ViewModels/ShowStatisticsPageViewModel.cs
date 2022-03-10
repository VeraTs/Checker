using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class ShowStatisticsPageViewModel 
    {
        public Command ReturnCommand { get; }
        public ShowStatisticsPageViewModel()
        {
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}