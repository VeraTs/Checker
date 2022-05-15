using CheckerUI.ViewModels;
using CheckerUI.Views.KitchenLineCardsViews;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace CheckerUI.Helpers
{
    internal class ItemCardHelper
    {
        private Expander m_LastTappedExpander;
        private Frame m_LastTappedFrame;

        public void OnSingleTap(Frame frame, Expander expander)
        {
            if (m_LastTappedExpander != null && m_LastTappedFrame != null)
            {
                m_LastTappedFrame.BackgroundColor = Color.White;
                m_LastTappedExpander.IsExpanded = false;
            }

            if (m_LastTappedExpander != expander)
            {
                m_LastTappedFrame = frame;
                m_LastTappedFrame.BackgroundColor = Color.BurlyWood;
                m_LastTappedExpander = expander;
                m_LastTappedExpander.IsExpanded = true;
            }
            else
            {
                m_LastTappedExpander = null;
            }
        }
    }


}
