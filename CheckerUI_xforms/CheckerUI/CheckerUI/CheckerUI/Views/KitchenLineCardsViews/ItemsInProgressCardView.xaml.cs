﻿using System;
using CheckerUI.Helpers;
using CheckerUI.Helpers.OrdersHelpers;
using CheckerUI.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views.KitchenLineCardsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsInProgressCardView : ContentView
    {
        private readonly ItemCardHelper r_ItemCardHelper = new ItemCardHelper();

        public LineViewModel ViewModel { get; set; }
        public ItemsInProgressCardView()
        {
            InitializeComponent();
        }
        
      //   private Frame m_LastFrameTapped;
        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            //m_InProgressCollection.SelectedItem = stackLayout.BindingContext;
            var card = stackLayout.LogicalChildren[0] as KitchenOrderItemInProgressCardView;
            var frame = card.Children[0] as Frame;
            var expander = frame.Children[0] as Expander;

            r_ItemCardHelper.OnSingleTap(frame, expander);

        }

        private async void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            InProgressCollection.SelectedItem = stackLayout.BindingContext;
            KitchenOrderItemInProgressCardView card = stackLayout.LogicalChildren[0] as KitchenOrderItemInProgressCardView;
            var item = card.BindingContext as OrderItemView;
           await ViewModel.ItemInProgressOnDoubleClick(item);
        }
    }
}