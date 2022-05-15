﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers;
using CheckerUI.Helpers.Order;
using CheckerUI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views.KitchenLineCardsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsLockedCardView : ContentView
    {
        private Expander m_LastTappedExpander;
        private Frame m_LastFrameTapped;
        
        public BaseLineViewModel ViewModel { get; set; } = new BaseLineViewModel();

        private readonly ItemCardHelper r_ItemCardHelper = new ItemCardHelper();  

        public ItemsLockedCardView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
           // m_LockedCollection.SelectedItem = stackLayout.BindingContext;
            KitchenOrderItemCardView card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;

            var frame = card.Children[0] as Frame;
            var expander = frame.Children[0] as Expander;
            r_ItemCardHelper.OnSingleTap(frame, expander);
        }
        private async void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            //m_LockedCollection.SelectedItem = stackLayout.BindingContext;
            KitchenOrderItemCardView card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var item = card.BindingContext as OrderItemView;
            bool answer = await Application.Current.MainPage.DisplayAlert("Order Locked", "Are You Sure ?", "Yes", "No");
            if (answer)
            {
                ViewModel.ItemLockedOnDoubleClicked(item);
            }
        }
    }
}