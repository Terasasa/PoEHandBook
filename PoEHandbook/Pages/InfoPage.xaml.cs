// //  ------------------------------------------------------------------ 
// //  PoEHandbook
// //  InfoPage.xaml.cs by 
// //  06/05/2015
// //  ------------------------------------------------------------------ 

using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using PoEHandbook.Model;
using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Pages
{
    /// <summary>
    /// Interaction logic for InfoPage.xaml
    /// </summary>
    public partial class InfoPage
    {
        private static bool FormatEntityName(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            ic.Add(ent.Name);

            var entWithRarity = ent as IHasRarity;
            if (entWithRarity != null)
            {
                ic.Add(Environment.NewLine);
                ic.Add(entWithRarity.RarityHandler.Base);
            }

            return true;
        }

        private static bool FormatEntityStats(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithStats = ent as IHasStats;

            if (entWithStats != null && entWithStats.StatsHandler.IsRelevant)
            {
                var statsHandler = entWithStats.StatsHandler;
                if (statsHandler.Block.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Chance to Block: ");
                    var run = new Run(statsHandler.Block + "%")
                    {
                        Foreground = statsHandler.BlockAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.ArmourValue.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Armour: ");
                    var run = new Run(statsHandler.ArmourValue.ToString())
                    {
                        Foreground = statsHandler.ArmourAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.EvasionValue.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Evasion: ");
                    var run = new Run(statsHandler.EvasionValue.ToString())
                    {
                        Foreground = statsHandler.EvasionAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.EnergyShieldValue.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Energy Shield: ");
                    var run = new Run(statsHandler.EnergyShieldValue.ToString())
                    {
                        Foreground = statsHandler.EnergyShieldAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                return true;
            }

            var entAsMap = ent as Map;
            if (entAsMap != null && entAsMap.Quantity > 0)
            {
                ic.Add("Map Quantity: ");
                ic.Add(new Run
                {
                    Text = "" + entAsMap.Quantity,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White)
                });

                return true;
            }

            return false;
        }

        private static bool FormatEntityRequirements(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithRequirements = ent as IHasRequirements;
            if (entWithRequirements != null && entWithRequirements.RequirementsHandler.IsRelevant)
            {

                ic.Add("Requires ");

                var reqHandler = entWithRequirements.RequirementsHandler;
                if (reqHandler.Level > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add("Level ");
                    ic.Add(new Run
                    {
                        Text = "" + reqHandler.Level,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White)
                    });
                }
                if (reqHandler.Strength > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add(new Run
                    {
                        Text = "" + reqHandler.Strength,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White)
                    });
                    ic.Add(" Str");
                }
                if (reqHandler.Dexterity > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add(new Run
                    {
                        Text = "" + reqHandler.Dexterity,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White)
                    });
                    ic.Add(" Dex");
                }
                if (reqHandler.Intelligence > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add(new Run
                    {
                        Text = "" + reqHandler.Intelligence,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White)
                    });
                    ic.Add(" Int");
                }

                return true;
            }

            var entAsMap = ent as Map;
            if (entAsMap != null)
            {
                ic.Add("Map Level: ");
                ic.Add(new Run
                {
                    Text = "" + entAsMap.Level,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White)
                });

                return true;
            }

            var entAsJewel = ent as Jewel;
            if (entAsJewel != null)
            {
                if (entAsJewel.Limit > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add("Limit: ");
                    ic.Add(new Run
                    {
                        Text = "" + entAsJewel.Limit,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White)
                    });
                }
                if (!string.IsNullOrEmpty(entAsJewel.Radius))
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add("Radius: ");
                    ic.Add(new Run
                    {
                        Text = "" + entAsJewel.Radius,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White)
                    });
                }

                return entAsJewel.Limit > 0 && !string.IsNullOrEmpty(entAsJewel.Radius);
            }

            return false;
        }

        private static bool FormatEntityImplicitMod(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithMods = ent as IHasMods;
            if (entWithMods != null && entWithMods.ModsHandler.HasImplicitMod)
            {
                ic.Add(entWithMods.ModsHandler.Mods[0]);
                return true;
            }

            return false;
        }

        private static bool FormatEntityExplicitMods(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithMods = ent as IHasMods;
            var entWithDescription = ent as IHasDescription;

            if ((entWithMods == null || !entWithMods.ModsHandler.IsRelevant) && entWithDescription == null)
                return false;

            if (entWithMods != null)
            {
                int start = entWithMods.ModsHandler.HasImplicitMod ? 1 : 0;

                for (int i = start; i < entWithMods.ModsHandler.Mods.Length; i++)
                {
                    string mod = entWithMods.ModsHandler.Mods[i];
                    var run = new Run(mod);

                    bool isCorruptedMod = mod.Trim().Equals("Corrupted", StringComparison.InvariantCultureIgnoreCase);
                    if (isCorruptedMod)
                        run.Foreground = new SolidColorBrush(Colors.Red);

                    ic.Add(run);
                    ic.Add(Environment.NewLine);
                }
                return true;
            }

            ic.Add(entWithDescription.DescriptionHandler.Description);
            return true;
        }

        public InfoPage(Entity ent)
        {
            InitializeComponent();

            // Get data
            bool hasName = FormatEntityName(ent, TbName.Inlines);
            bool hasStats = FormatEntityStats(ent, TbStats.Inlines);
            bool hasRequirements = FormatEntityRequirements(ent, TbRequiremenets.Inlines);
            bool hasImplicitMod = FormatEntityImplicitMod(ent, TbImplicitMod.Inlines);
            bool hasExplicitMods = FormatEntityExplicitMods(ent, TbMods.Inlines);

            // Hide unused stuff
            if (!hasName) ((dynamic)TbName).Parent.Visibility = Visibility.Collapsed;
            if (!hasStats)
            {
                TbStats.Visibility = Visibility.Collapsed;
                RectSeparator1.Visibility = Visibility.Collapsed;
            }
            if (!hasRequirements)
            {
                TbRequiremenets.Visibility = Visibility.Collapsed;
                RectSeparator2.Visibility = Visibility.Collapsed;
            }
            if (!hasImplicitMod)
            {
                TbImplicitMod.Visibility = Visibility.Collapsed;
                RectSeparator3.Visibility = Visibility.Collapsed;
            }
            if (!hasExplicitMods)
            {
                TbMods.Visibility = Visibility.Collapsed;
                RectSeparator4.Visibility = Visibility.Collapsed;
            }

            if (File.Exists(ent.ImageUri.LocalPath))
                ImgItem.Source = new BitmapImage(ent.ImageUri);
            else
            {
                ImgItem.Visibility = Visibility.Collapsed;
                RectSeparator4.Visibility = Visibility.Collapsed;
            }

            // Figure out the colors
            Color fore, back;
            ent.GetEntityColor(out fore, out back);
            Resources["AccentColor"] = back;
            TbName.Foreground = new SolidColorBrush(fore);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade in
            GrdMain.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25)));
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService == null || !NavigationService.CanGoBack) return;

            // Fade out and go back
            var anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.25));
            anim.Completed += (o, args) => NavigationService.GoBack();
            GrdMain.BeginAnimation(OpacityProperty, anim);
        }
    }
}