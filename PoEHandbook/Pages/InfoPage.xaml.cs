// ------------------------------------------------------------------ 
// PoEHandbook
// InfoPage.xaml.cs by 
// 06/05/2015
// ------------------------------------------------------------------ 

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

                // Shields
                if (statsHandler.Block.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Chance to Block: ");
                    var run = new Run(statsHandler.Block + "%")
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.BlockAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Armour
                if (statsHandler.ArmourValue.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Armour: ");
                    var run = new Run(statsHandler.ArmourValue.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
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
                        FontWeight = FontWeights.SemiBold,
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
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.EnergyShieldAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Weapons
                if (statsHandler.PhysicalDamage.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Physical Damage: ");
                    var run = new Run(statsHandler.PhysicalDamage.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.PhysicalDamageAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.CriticalStrikeChance.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Critical Strike Chance: ");
                    var run = new Run(statsHandler.CriticalStrikeChance + "%")
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.CriticalStrikeChanceAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.AttacksPerSecond.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Attacks Per Second: ");
                    var run = new Run(statsHandler.AttacksPerSecond.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.AttacksPerSecondAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.DamagePerSecond.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Damage Per Second: ");
                    var run = new Run(statsHandler.DamagePerSecond.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.DamagePerSecondAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Maps
                if (statsHandler.Quantity > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Quantity: ");
                    var run = new Run(statsHandler.Quantity.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.QuantityAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Jewels
                if (!string.IsNullOrEmpty(statsHandler.Radius))
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Radius: ");
                    var run = new Run(statsHandler.Radius)
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.RadiusAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Flasks
                if (statsHandler.LifeRecovery.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Life Recovered: ");
                    var run = new Run(statsHandler.LifeRecovery.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.LifeRecoveryAffected
                            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.ManaRecovery.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Mana Recovered: ");
                    var run = new Run(statsHandler.ManaRecovery.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.ManaRecoveryAffected
                            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.Duration.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Duration: ");
                    var run = new Run(statsHandler.Duration.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.DurationAffected
                            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                    ic.Add(" seconds");
                }
                if (statsHandler.Capacity.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Capacity: ");
                    var run = new Run(statsHandler.Capacity.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.CapacityAffected
                            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
                if (statsHandler.ChargesUsed.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);

                    ic.Add("Charges Used: ");
                    var run = new Run(statsHandler.ChargesUsed.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = statsHandler.ChargesUsedAffected
                            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

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
                var reqHandler = entWithRequirements.RequirementsHandler;

                if (reqHandler.Level > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add("Level ");
                    ic.Add(new Run
                    {
                        Text = "" + reqHandler.Level,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = reqHandler.LevelAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    });
                }
                if (reqHandler.Strength > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add(new Run
                    {
                        Text = "" + reqHandler.Strength,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = reqHandler.StrengthAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
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
                        FontWeight = FontWeights.SemiBold,
                        Foreground = reqHandler.DexterityAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
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
                        FontWeight = FontWeights.SemiBold,
                        Foreground = reqHandler.IntelligenceAffected
                            ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    });
                    ic.Add(" Int");
                }
                if (reqHandler.Limit > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add("Limit: ");
                    ic.Add(new Run
                    {
                        Text = "" + reqHandler.Limit,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = reqHandler.LimitAffected
                            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a88ef"))
                            : new SolidColorBrush(Colors.White)
                    });
                }
                return true;
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

            // Mods
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
                    if (i < entWithMods.ModsHandler.Mods.Length - 1)
                        ic.Add(Environment.NewLine);
                }
                return true;
            }

            // Description
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
            if (!hasName) ((dynamic) TbName).Parent.Visibility = Visibility.Collapsed;
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
            GrdMain.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5)));
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