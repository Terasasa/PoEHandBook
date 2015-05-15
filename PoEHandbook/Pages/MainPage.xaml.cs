//  ------------------------------------------------------------------ 
//  PoEHandbook
//  MainPage.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using PoEHandbook.Data;

namespace PoEHandbook.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage
    {
        private readonly DispatcherTimer _queryTimer;

        private string _lastQuery = "";
        private SearchResult[] _lastResults;

        public MainPage()
        {
            InitializeComponent();

            // Timer, which will serve as a delay before search initiation
            _queryTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.5)};
            _queryTimer.Tick += (sender, args) =>
            {
                GetSearchResults();
                _queryTimer.Stop();
            };
            _queryTimer.IsEnabled = false;

            // Make an empty placeholder
            _lastResults = new SearchResult[] {};
        }

        /// <summary>
        /// Update the status text in the bottom
        /// </summary>
        private void UpdateStatusText()
        {
            TbStatus.Inlines.Clear();

            // No results - no worries
            if (!_lastResults.Any())
            {
                TbStatus.Inlines.Add("Nothing found");
                return;
            }

            // Get all the entity types
            var entityTypes = _lastResults.Select(sr => sr.Entity.GetType()).Distinct();

            // Output
            TbStatus.Inlines.Add("Total: ");
            TbStatus.Inlines.Add(new Run { Text = "" + _lastResults.Length, FontWeight = FontWeights.SemiBold });

            foreach (var entityType in entityTypes)
            {
                TbStatus.Inlines.Add(", ");

                int typeCount = _lastResults.Count(sr => sr.Entity.GetType() == entityType);

                TbStatus.Inlines.Add(entityType.Name + ": ");
                TbStatus.Inlines.Add(new Run {Text = "" + typeCount, FontWeight = FontWeights.SemiBold});
            }
        }

        /// <summary>
        /// Submit a query and get entities that satisfy it
        /// </summary>
        private void GetSearchResults()
        {
            // Clear previous results
            PnlResults.Children.Clear();
            _lastResults = new SearchResult[] {};

            // Format query
            var queries = _lastQuery.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).TrimElements().ToArray();

            // Get results
            var entities = DataAccess.PerformSearchQuery(queries);
            if (entities == null) return;

            // Order results and save them
            _lastResults = entities
                .OrderBy(sr => sr.Entity.GetType().Name)
                .ThenBy(sr => sr.Entity.Name)
                .ToArray();

            // Create controls for results and popualte the panel
            foreach (var sr in _lastResults)
            {
                var result = new Controls.SearchResult(sr, queries, NavigationService);
                PnlResults.Children.Add(result);
            }

            // Update the text and stop the timer
            UpdateStatusText();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade in
            GrdMain.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25)));

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
                _lastResults = new SearchResult[] {};
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

        private void CbShowImages_Click(object sender, RoutedEventArgs e)
        {
            // Re-load the data with new settings
            GetSearchResults();
        }

        private void BtnIdentifyItem_OnClick(object sender, RoutedEventArgs e)
        {
            string query = Ext.QueryFromPoEClipboard();
            if (query == null)
                MessageBox.Show(
                    "Item identification failed. Please mouse over the item in-game and press Ctrl+C and then try to identify the item.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                TbQuery.Text = query;
        }

        private void TbCredits_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("http://www.tyrrrz.me");
        }
    }
}