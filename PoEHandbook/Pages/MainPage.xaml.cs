//  ------------------------------------------------------------------ 
//  PoEHandbook
//  MainPage.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using PoEHandbook.Data;
using PoEHandbook.Model;

namespace PoEHandbook.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage
    {
        private readonly DispatcherTimer _queryTimer;

        private string _lastQuery;
        private IEnumerable<SearchResult> _lastResults;

        public MainPage()
        {
            InitializeComponent();

            // Timer, which will serve as a delay before search initiation
            _queryTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.5)};
            _queryTimer.Tick += (sender, args) => GetSearchResults();
            _queryTimer.IsEnabled = false;

            // Make an empty placeholder
            _lastResults = new SearchResult[] {};
        }

        /// <summary>
        /// Update the status text in the bottom
        /// </summary>
        private void UpdateStatusText()
        {
            var searchResults = _lastResults as SearchResult[] ?? _lastResults.ToArray();

            TbStatus.Inlines.Clear();

            int equipCount = searchResults.Count(sr => sr.Entity is Equipment);
            int gemCount = searchResults.Count(sr => sr.Entity is Gem);
            int jewelCount = searchResults.Count(sr => sr.Entity is Jewel);
            int mapCount = searchResults.Count(sr => sr.Entity is Map);
            int passiveCount = searchResults.Count(sr => sr.Entity is Passive);
            int recipeCount = searchResults.Count(sr => sr.Entity is Recipe);
            int miscCount = searchResults.Length - equipCount - gemCount - jewelCount - mapCount - passiveCount -
                            recipeCount;

            TbStatus.Inlines.Add("Found ");

            if (equipCount > 0)
            {
                if (TbStatus.Inlines.Count > 1)
                    TbStatus.Inlines.Add(", ");

                var run = new Run("" + equipCount) {FontWeight = FontWeights.Bold};
                TbStatus.Inlines.Add(run);
                TbStatus.Inlines.Add(" equipment item(s)");
            }
            if (gemCount > 0)
            {
                if (TbStatus.Inlines.Count > 1)
                    TbStatus.Inlines.Add(", ");

                var run = new Run("" + gemCount) { FontWeight = FontWeights.Bold };
                TbStatus.Inlines.Add(run);
                TbStatus.Inlines.Add(" gem(s)");
            }
            if (jewelCount > 0)
            {
                if (TbStatus.Inlines.Count > 1)
                    TbStatus.Inlines.Add(", ");

                var run = new Run("" + jewelCount) { FontWeight = FontWeights.Bold };
                TbStatus.Inlines.Add(run);
                TbStatus.Inlines.Add(" jewel(s)");
            }
            if (mapCount > 0)
            {
                if (TbStatus.Inlines.Count > 1)
                    TbStatus.Inlines.Add(", ");

                var run = new Run("" + mapCount) { FontWeight = FontWeights.Bold };
                TbStatus.Inlines.Add(run);
                TbStatus.Inlines.Add(" map(s)");
            }
            if (passiveCount > 0)
            {
                if (TbStatus.Inlines.Count > 1)
                    TbStatus.Inlines.Add(", ");

                var run = new Run("" + passiveCount) { FontWeight = FontWeights.Bold };
                TbStatus.Inlines.Add(run);
                TbStatus.Inlines.Add(" passive(s)");
            }
            if (recipeCount > 0)
            {
                if (TbStatus.Inlines.Count > 1)
                    TbStatus.Inlines.Add(", ");

                var run = new Run("" + recipeCount) { FontWeight = FontWeights.Bold };
                TbStatus.Inlines.Add(run);
                TbStatus.Inlines.Add(" recipe(s)");
            }
            if (miscCount > 0)
            {
                if (TbStatus.Inlines.Count > 1)
                    TbStatus.Inlines.Add(", ");

                var run = new Run("" + miscCount) { FontWeight = FontWeights.Bold };
                TbStatus.Inlines.Add(run);
                TbStatus.Inlines.Add(" misc item(s)");
            }
            if (TbStatus.Inlines.Count == 1)
                TbStatus.Inlines.Add("nothing");
        }

        /// <summary>
        /// Submit a query and get entities that satisfy it
        /// </summary>
        private void GetSearchResults()
        {
            PnlResults.Children.Clear();

            var queries = _lastQuery.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var entities = DataAccess.PerformSearchQuery(queries);

            _lastResults = entities
                .OrderBy(sr => sr.Entity.GetType())
                .ThenBy(sr => sr.Entity.Name);
            foreach (var sr in _lastResults)
            {
                var result = new Controls.SearchResult(sr, NavigationService);
                PnlResults.Children.Add(result);
            }

            UpdateStatusText();
            _queryTimer.Stop();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Focus the query text box
            TbQuery.Focus();

            // Default the status text
            UpdateStatusText();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void TbQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            _lastQuery = TbQuery.Text.Trim();

            // Stop the timer if one is already running
            _queryTimer.Stop();

            // If query is empty - just clear the panel
            if (string.IsNullOrEmpty(_lastQuery))
            {
                PnlResults.Children.Clear();
                UpdateStatusText();
            }
            // If not - start the timer
            else
                _queryTimer.Start();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbQuery.Text = "";
            TbQuery.Focus();
        }

        private void BtnOptions_Click(object sender, RoutedEventArgs e)
        {
            MenuOptions.Visibility = MenuOptions.Visibility == Visibility.Hidden
                ? Visibility.Visible
                : Visibility.Hidden;
            if (MenuOptions.Visibility == Visibility.Hidden)
                TbQuery.Focus();
        }

        private void MenuOptions_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MenuOptions.Visibility = Visibility.Hidden;
            TbQuery.Focus();
        }

        private void CbShowImages_Click(object sender, RoutedEventArgs e)
        {
            // Re-load the data with new settings
            GetSearchResults();
        }

        private void TbCredits_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("http://www.tyrrrz.me");
        }
    }
}