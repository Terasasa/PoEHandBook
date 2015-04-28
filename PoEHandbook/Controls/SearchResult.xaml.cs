//  ------------------------------------------------------------------ 
//  PoEHandbook
//  SearchResult.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using PoEHandbook.Model;
using PoEHandbook.Pages;

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
            ItemImage.Source = new BitmapImage(_sr.Entity.ImageUri);
            ItemSearchMatches.Text = string.Join(Environment.NewLine, _sr.Matches);

            // Figure out the colors
            var rarity = _sr.Entity is Equippable ? (_sr.Entity as Equippable).Rarity : Equippable.RarityTier.Normal;
            Color fore, back;
            rarity.GetRarityColor(out fore, out back);
            Resources["AccentColor"] = back;
            ItemName.Foreground = new SolidColorBrush(fore);
            InvalidateVisual();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void MainGrid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_ns == null) return;

            if (_sr.Entity is Equippable)
                _ns.Navigate(new EquippableInfoPage(_sr.Entity as Equippable));
        }
    }
}