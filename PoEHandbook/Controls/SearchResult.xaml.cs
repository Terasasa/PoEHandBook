//  ------------------------------------------------------------------ 
//  PoEHandbook
//  SearchResult.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using PoEHandbook.Model;
using PoEHandbook.Model.Interfaces;
using PoEHandbook.Pages;
using PoEHandbook.Properties;

namespace PoEHandbook.Controls
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    public partial class SearchResult
    {
        private readonly Data.SearchResult _sr;
        private readonly NavigationService _ns;

        public SearchResult(Data.SearchResult sr, NavigationService ns = null)
        {
            InitializeComponent();

            _sr = sr;
            _ns = ns;

            // Fill the data
            ItemName.Text = _sr.Entity.Name;
            if (Settings.Default.ShowImages)
                ItemImage.Source = new BitmapImage(_sr.Entity.ImageUri);
            else
                MainGrid.RowDefinitions[1].Height = new GridLength(0);
            ItemSearchMatches.Text = string.Join(Environment.NewLine, _sr.Matches);

            // Figure out the colors
            Color fore, back;
            _sr.Entity.GetEntityColor(out fore, out back);
            Resources["AccentColor"] = back;
            ItemName.Foreground = new SolidColorBrush(fore);
            InvalidateVisual();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void MainGrid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_ns == null) return;
            _ns.Navigate(new InfoPage(_sr.Entity));
        }
    }
}