//  ------------------------------------------------------------------ 
//  PoEHandbook
//  SearchResult.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using PoEHandbook.Model;
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

        private static void FormatSearchResultMatches(Data.SearchResult sr, IEnumerable<string> highlights,
            InlineCollection ic)
        {
            ic.Clear();

            var subStrings = highlights as string[] ?? highlights.ToArray();

            // Loop through all highlights
            foreach (string line in sr.Matches)
            {
                if (ic.Count > 0)
                    ic.Add(Environment.NewLine);

                // Loop to split matches and the rest of the string
                int offset = 0;
                int matchIndex;
                do
                {
                    // Find the match
                    string match;
                    matchIndex = line.IndexOfAnyInvariant(offset, subStrings, out match);

                    // If found - substring everything before
                    // .. if not - substring the rest of the string
                    string sub = matchIndex >= 0
                        ? line.Substring(offset, matchIndex - offset)
                        : line.Substring(offset);
                    offset = matchIndex + match.Length;

                    // Add substring
                    ic.Add(sub);
                    // Add match
                    if (matchIndex >= 0)
                        ic.Add(new Run(match) { TextDecorations = TextDecorations.Underline, FontWeight = FontWeights.Bold });
                } while (matchIndex >= 0);
            }
        }

        public SearchResult(Data.SearchResult sr, IEnumerable<string> highlights, NavigationService ns = null)
        {
            InitializeComponent();

            _sr = sr;
            _ns = ns;

            // Name
            ItemName.Text = _sr.Entity.Name;
            if (sr.Entity is Recipe)
                ItemName.Text += " (Recipe)";

            // Image
            if (Settings.Default.ShowImages && File.Exists(_sr.Entity.ImageUri.LocalPath))
                ItemImage.Source = new BitmapImage(_sr.Entity.ImageUri);
            else
                MainGrid.RowDefinitions[1].Height = new GridLength(0);

            // Search Matches
            FormatSearchResultMatches(_sr, highlights, ItemSearchMatches.Inlines);

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