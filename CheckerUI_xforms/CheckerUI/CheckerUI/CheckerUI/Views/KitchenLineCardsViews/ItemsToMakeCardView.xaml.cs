﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckerUI.Helpers;
using CheckerUI.Helpers.Order;
using CheckerUI.ViewModels;
using Lottie.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views.KitchenLineCardsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsToMakeCardView : ContentView
    {
        public CollectionView m_ToMakeView = new CollectionView();
        private readonly ItemCardHelper r_ItemCardHelper = new ItemCardHelper();

        public BaseLineViewModel ViewModel { get; set; } = new BaseLineViewModel();

        public ItemsToMakeCardView()
        {
            InitializeComponent();
            m_ToMakeView = m_ToMakeListView;
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            //m_ToMakeListView.SelectedItem = stackLayout.BindingContext;
            KitchenOrderItemCardView card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
           

            var frame = card.Children[0] as Frame;
            var expander = frame.Children[0] as Expander;

            r_ItemCardHelper.TapGestureRecognizer_OnTapped(frame, expander);
        }

        private void TapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            //m_ToMakeListView.SelectedItem = stackLayout.BindingContext;
            KitchenOrderItemCardView card = stackLayout.LogicalChildren[0] as KitchenOrderItemCardView;
            var item = card.BindingContext as OrderItemView;
            ViewModel.ItemToMakeOnDoubleClicked(item);
        }

        private async void M_ToMakeListView_OnChildAdded(object sender, ElementEventArgs e)
        {
            
            m_StackForAni.IsVisible = true;
            m_NewItemLottie.IsVisible = true;
            m_NewItemLottie.PlayAnimation();
            await Task.Delay(3000);
            m_NewItemLottie.IsVisible = false;
            m_StackForAni.IsVisible = false;

        }
    }
}