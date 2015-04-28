//  ------------------------------------------------------------------ 
//  PoEHandbook
//  MainPage.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        private string _query;

        public MainPage()
        {
            InitializeComponent();

            // Timer, which will serve as a delay before search initiation
            _queryTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.5)};
            _queryTimer.Tick += (sender, args) => GetSearchResults();
            _queryTimer.IsEnabled = false;
        }

        /// <summary>
        /// Submit a query and get entities that satisfy it
        /// </summary>
        private void GetSearchResults()
        {
            PnlResults.Children.Clear();

            var queries = _query.Split(new[] {','});
            var entities = DataAccess.PerformSearchQuery(queries);

            foreach (var sr in entities.OrderBy(sr => sr.Entity.Name))
            {
                var result = new Controls.SearchResult(sr, NavigationService);
                PnlResults.Children.Add(result);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Focus the query text box
            TbQuery.Focus();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void TbQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            _query = TbQuery.Text.Trim();

            // Stop the timer if one is already running
            _queryTimer.Stop();

            // If query is empty - just clear the panel
            if (string.IsNullOrEmpty(_query))
                PnlResults.Children.Clear();
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
    }
}