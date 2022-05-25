using Xamarin.CommunityToolkit.UI.Views;

namespace CheckerWaitersApp.Helpers
{
    internal class ItemCardHelper
    {
        private Expander m_LastTappedExpander;
        
        public void OnSingleTap(Expander expander)
        {
            if (m_LastTappedExpander != null )
            {
                m_LastTappedExpander.IsExpanded = false;
            }

            if (m_LastTappedExpander != expander)
            {
              
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

