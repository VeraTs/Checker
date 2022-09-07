﻿using System.Collections.ObjectModel;
using CheckerUI.Models;
using CheckerUI.Views;
using Xamarin.Forms;

namespace CheckerUI.ViewModels
{
    public class UserMainPageViewModel
    {
        public Command ReturnCommand { get; }
        public Command MyKitchenCommand { get; }
        public Command OrdersViewCommand { get; }
        public Command UpdateKitchenCommand { get; }
        public Command MangeKitchenCommand { get; }
        public LinesPage linesPage { get; private set; }
        public UpdateKitchenViewModel updateVm { get; private set; }
        public UpdateKitchenPage updatePage { get; private set; }
        public MangePage mangeKitchenPage { get; private set; }
        public MangePageViewModel mangeKitchenVm { get; private set; }
        public OrdersView ordersView { get; private set; }

        public ObservableCollection<PageModel> Pages { get; private set; }

        public UserMainPageViewModel()
        {
            
            updateVm = new UpdateKitchenViewModel();
            updatePage = new UpdateKitchenPage
            {
                BindingContext = updateVm
            };
            mangeKitchenVm = new MangePageViewModel();
            mangeKitchenPage = new MangePage
            {
                BindingContext = mangeKitchenVm
            };
            ordersView = new OrdersView();
            MyKitchenCommand = new Command(async () =>
            {
                linesPage = new LinesPage();
                await Application.Current.MainPage.Navigation.PushAsync(linesPage);
            });
            UpdateKitchenCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(updatePage);
            });
            MangeKitchenCommand = new Command(async () =>
            {
               
                await Application.Current.MainPage.Navigation.PushAsync(mangeKitchenPage);
            });
            OrdersViewCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(ordersView);
            });
            ReturnCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
            init();
        }

        private void init()
        {
            var p1 = new PageModel
            {
                Name = "Kitchen Lines",
                Id = 1,
                Description = "View your kitchen Lines, start Working ",
                BackgroundImageURL = "KitchenBackground.PNG",
                NextPageCommand = MyKitchenCommand
            };
            var p2 = new PageModel
            {
                Name = "Update System",
                Id = 2,
                Description = "Update Application with new Information",
                BackgroundImageURL = "ManageBackground.PNG",
                NextPageCommand = UpdateKitchenCommand
            };
            var p3 = new PageModel
            {
                Name = "Orders Window",
                Id = 3,
                Description = "Open a new station for issuing and marking orders ",
                BackgroundImageURL = "CheckoutBackground.PNG",
                NextPageCommand = OrdersViewCommand
            };
            Pages = new ObservableCollection<PageModel>
            {
                p1,
                p2,
                p3
            };
        }
    }
}