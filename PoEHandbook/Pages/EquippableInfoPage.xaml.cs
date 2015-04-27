//  ------------------------------------------------------------------ 
//  PoEHandbook
//  ItemInfoPage.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PoEHandbook.Model;

namespace PoEHandbook.Pages
{
    /// <summary>
    /// Interaction logic for EquippableInfoPage.xaml
    /// </summary>
    public partial class EquippableInfoPage
    {
        private static void FormatItemStats(Equippable item, InlineCollection ic)
        {
            ic.Clear();

            if (!item.HasStats)
            {
                ic.Add("No stats");
                return;
            }

            var asArmor = item as Armour;
            if (asArmor != null)
            {
                bool first = true;

                if (asArmor.ArmourValue.Average > 0)
                {
                    first = false;

                    ic.Add("Armour: ");
                    var run = new Run(asArmor.ArmourValue.ToString());
                    if (asArmor.ArmourAffected)
                        run.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"));
                    ic.Add(run);
                }
                if (asArmor.EvasionValue.Average > 0)
                {
                    if (!first)
                        ic.Add(Environment.NewLine);
                    first = false;

                    ic.Add("Evasion: ");
                    var run = new Run(asArmor.EvasionValue.ToString());
                    if (asArmor.EvasionAffected)
                        run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a88ef"));
                    ic.Add(run);
                }
                if (asArmor.EnergyShieldValue.Average > 0)
                {
                    if (!first)
                        ic.Add(Environment.NewLine);

                    ic.Add("Energy Shield: ");
                    var run = new Run(asArmor.EnergyShieldValue.ToString());
                    if (asArmor.EnergyShieldAffected)
                        run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a88ef"));
                    ic.Add(run);
                }
            }
        }

        private static void FormatItemRequirements(Equippable item, InlineCollection ic)
        {
            ic.Clear();
            if (!item.HasRequirements)
            {
                ic.Add("No requirements");
                return;
            }

            bool first = true;
            ic.Add("Requires ");

            if (item.Level > 0)
            {
                first = false;

                ic.Add("Level ");
                ic.Add(new Run {Text = "" + item.Level, FontWeight = FontWeights.Bold});
            }
            if (item.Strength > 0)
            {
                if (!first)
                    ic.Add(", ");
                first = false;

                ic.Add(new Run { Text = "" + item.Strength, FontWeight = FontWeights.Bold });
                ic.Add(" Str");
            }
            if (item.Dexterity > 0)
            {
                if (!first)
                    ic.Add(", ");
                first = false;

                ic.Add(new Run { Text = "" + item.Dexterity, FontWeight = FontWeights.Bold });
                ic.Add(" Dex");
            }
            if (item.Intelligence > 0)
            {
                if (!first)
                    ic.Add(", ");

                ic.Add(new Run { Text = "" + item.Intelligence, FontWeight = FontWeights.Bold });
                ic.Add(" Int");
            }
        }

        private static void FormatItemMods(Equippable item, InlineCollection ic)
        {
            ic.Clear();
            if (string.IsNullOrEmpty(item.Text) && !item.Mods.Any())
            {
                ic.Add("No mods");
                return;
            }

            ic.Add(string.Join(Environment.NewLine, item.Mods));
        }

        public EquippableInfoPage(Equippable item)
        {
            InitializeComponent();

            // Get data
            TbName.Text = item.Name + Environment.NewLine + item.Base;
            FormatItemStats(item, TbStats.Inlines);
            FormatItemRequirements(item, TbRequiremenets.Inlines);
            FormatItemMods(item, TbMods.Inlines);
            ImgItem.Source = new BitmapImage(item.ImageUri);

            // Figure out the colors
            Color fore, back;
            item.Rarity.GetRarityColor(out fore, out back);
            AccentColor = back;
            TbName.Foreground = new SolidColorBrush(fore);
        }

        private Color AccentColor
        {
            set { ((SolidColorBrush)Resources["AccentColor"]).Color = value; }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService == null || !NavigationService.CanGoBack) return;

            NavigationService.GoBack();
        }
    }
}