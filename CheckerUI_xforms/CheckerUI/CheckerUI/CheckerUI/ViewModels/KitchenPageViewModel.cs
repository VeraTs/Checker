﻿using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class KitchenPageViewModel 
    {
        public Command ReturnCommand { get; }
        public Command HotStripeCommand { get; }
        public Command ColdStripeCommand { get; }
        public Command OvenStripeCommand { get; }
        
        public KitchenPageViewModel()
        {
            
            ReturnCommand = new Command(async () =>
            {
                
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            HotStripeCommand = new Command(async () =>
            {
                var hotStripeVm = new HotStripePageViewModel();
                var hotStripePage = new HotStripePage();
                hotStripePage.BindingContext = hotStripeVm;
                await Application.Current.MainPage.Navigation.PushAsync(hotStripePage);
            });
            ColdStripeCommand = new Command(async () =>
            {
                var coldStripeVm = new ColdStripePageViewModel();
                var coldStripePage = new ColdStripePage();
                coldStripePage.BindingContext = coldStripeVm;
                await Application.Current.MainPage.Navigation.PushAsync(coldStripePage);
            });
            OvenStripeCommand = new Command(async () =>
            {
                var ovenStripeVm = new OvenStripePageViewModel();
                var ovenStripePage = new OvenStripePage();
                ovenStripePage.BindingContext = ovenStripeVm;
                await Application.Current.MainPage.Navigation.PushAsync(ovenStripePage);
            });
        }


    }
}