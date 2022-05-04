using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CheckerUI.Helpers.CustomControls
{
    internal class AutocompleteSearchBar : StackLayout
    {
        private readonly SearchBar m_searchBar = new SearchBar();
        private readonly ListView m_Results = new ListView();
        private readonly List<string> m_Options = new List<string>();

        public AutocompleteSearchBar()
        {
            m_searchBar.Placeholder = "Type here...";
            m_Results.BackgroundColor = Color.White;
            m_Results.Footer = "";
            Children.Add(m_searchBar);
            Children.Add(m_Results);
        }

        public void SetOptionsData(List<string> i_AllOptions)
        {
            foreach (var opt in i_AllOptions)
            {
                m_Options.Add(opt);
            }
            m_Options.Sort(string.Compare);
            m_Results.ItemsSource = m_Options;
            m_searchBar.TextChanged += OnSearchBarOnTextChanged;
        }
        public void ResultsListIsVisible()
        {
            m_Results.IsVisible = !m_Results.IsVisible;
        }
        private void OnSearchBarOnTextChanged(object sender, TextChangedEventArgs e)
        {
            IEnumerable<string> search_results;
            var text_val = e.NewTextValue;
            if (string.IsNullOrEmpty(text_val))
            {
                search_results = m_Options;
                m_searchBar.Placeholder = "Type here...";
            }
            else 
            {
                var cap = char.ToUpper(text_val[0]).ToString();
                var list = m_Options.Where(c => c.StartsWith(cap)).ToList();
                var more = m_Options.Where(c => c.Contains(text_val.ToLower()) && !c.StartsWith(cap)).ToList();
                list.AddRange(more);
                search_results = (IEnumerable<string>) list;
            }
            m_Results.ItemsSource = search_results;
        }
    }
}
