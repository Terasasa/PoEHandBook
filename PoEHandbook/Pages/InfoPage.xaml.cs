// ------------------------------------------------------------------ 
// PoEHandbook
// InfoPage.xaml.cs by Tyrrrz
// 06/05/2015
// ------------------------------------------------------------------ 

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
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
        /// <summary>
        /// Formats an InlineCollection with name of an entity.
        /// If entity has a base, appens that on a new line.
        /// If entity is a recipe, says so on a new line.
        /// </summary>
        /// <returns>False if no information was found</returns>
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

            if (ent is Recipe)
            {
                ic.Add(Environment.NewLine);
                ic.Add("(Recipe)");
            }

            return ic.Count > 0;
        }

        /// <summary>
        /// Formats an InlineCollection with stats of an entity
        /// </summary>
        /// <returns>False if no information was found</returns>
        private static bool FormatEntityStats(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            #region Shield

            if (ent is Shield)
            {
                var shield = (Shield) ent;

                // Block
                if (shield.Block.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Chance to Block: ");
                    var run = new Run(shield.Block + "%")
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
            }

            #endregion

            #region Armour

            if (ent is Armour)
            {
                var armour = (Armour) ent;

                // Armour Value
                if (armour.ArmourValue.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Armour: ");
                    var run = new Run(armour.ArmourValue.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Evasion Value
                if (armour.EvasionValue.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Evasion: ");
                    var run = new Run(armour.EvasionValue.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Energy Shield Value
                if (armour.EnergyShieldValue.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Energy Shield: ");
                    var run = new Run(armour.EnergyShieldValue.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
            }

            #endregion

            #region Flask

            if (ent is Flask)
            {
                var flask = (Flask) ent;

                // Life Recovery
                if (flask.LifeRecovery.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Life Recovered: ");
                    var run = new Run(flask.LifeRecovery.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Mana Recovery
                if (flask.ManaRecovery.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Mana Recovered: ");
                    var run = new Run(flask.ManaRecovery.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Duration
                if (flask.Duration.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Duration: ");
                    var run = new Run(flask.Duration.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Charges Used
                if (flask.ChargesUsed.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Charges Used: ");
                    var run = new Run(flask.ChargesUsed.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
            }

            #endregion

            #region Jewel

            if (ent is Jewel)
            {
                var jewel = (Jewel) ent;

                // Limit
                if (jewel.Limit > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Limited to ");
                    var run = new Run(jewel.Limit.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Radius
                if (!string.IsNullOrEmpty(jewel.Radius))
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Radius: ");
                    var run = new Run(jewel.Radius)
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
            }

            #endregion

            #region Map

            if (ent is Map)
            {
                var map = (Map) ent;

                // Level
                if (map.Level > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Map Level: ");
                    var run = new Run(map.Level.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Quantity
                if (map.Quantity > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Item Quantity: ");
                    var run = new Run(map.Quantity.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
            }

            #endregion

            #region Weapon

            if (ent is Weapon)
            {
                var weapon = (Weapon) ent;

                // Physical Damage
                if (weapon.PhysicalDamage.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Physical Damage: ");
                    var run = new Run(weapon.PhysicalDamage.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Fire Damage
                if (weapon.FireDamage.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Fire Damage: ");
                    var run = new Run(weapon.FireDamage.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#96000e"))
                    };
                    ic.Add(run);
                }

                // Cold Damage
                if (weapon.ColdDamage.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Cold Damage: ");
                    var run = new Run(weapon.ColdDamage.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#30648a"))
                    };
                    ic.Add(run);
                }

                // Lightning Damage
                if (weapon.LightningDamage.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Lightning Damage: ");
                    var run = new Run(weapon.LightningDamage.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#dcd700"))
                    };
                    ic.Add(run);
                }

                // Chaos Damage
                if (weapon.ChaosDamage.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Chaos Damage: ");
                    var run = new Run(weapon.ChaosDamage.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#d02072"))
                    };
                    ic.Add(run);
                }

                // Critical Chance
                if (weapon.CriticalStrikeChance.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Critical Strike Chance: ");
                    var run = new Run(weapon.CriticalStrikeChance + "%")
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // Attacks Per Second
                if (weapon.AttacksPerSecond.Average > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Attacks Per Second: ");
                    var run = new Run(weapon.AttacksPerSecond.ToString())
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
            }

            #endregion

            #region Recipe

            if (ent is Recipe)
            {
                var recipe = (Recipe) ent;

                // Player Offer
                if (!string.IsNullOrEmpty(recipe.PlayerOffer))
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("Player Offer: ");
                    var run = new Run(recipe.PlayerOffer)
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }

                // NPC Offer
                if (!string.IsNullOrEmpty(recipe.NPCOffer))
                {
                    if (ic.Count > 1)
                        ic.Add(Environment.NewLine);
                    ic.Add("NPC Offer: ");
                    var run = new Run(recipe.NPCOffer)
                    {
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ic.Add(run);
                }
            }

            #endregion

            return ic.Count > 0;
        }

        /// <summary>
        /// Formats an InlineCollection with requirements of an entity
        /// </summary>
        /// <returns>False if no information was found</returns>
        private static bool FormatEntityRequirements(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithRequirements = ent as IHasRequirements;
            if (entWithRequirements != null && entWithRequirements.RequirementsHandler.IsRelevant)
            {
                var reqHandler = entWithRequirements.RequirementsHandler;

                ic.Add("Requires ");

                if (reqHandler.Level > 0)
                {
                    if (ic.Count > 1)
                        ic.Add(", ");

                    ic.Add("Level ");
                    ic.Add(new Run
                    {
                        Text = "" + reqHandler.Level,
                        FontWeight = FontWeights.SemiBold,
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
                        FontWeight = FontWeights.SemiBold,
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
                        FontWeight = FontWeights.SemiBold,
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
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Colors.White)
                    });
                    ic.Add(" Int");
                }
            }

            return ic.Count > 0;
        }

        /// <summary>
        /// Formats an InlineCollection with implicit mod of an entity
        /// </summary>
        /// <returns>False if no information was found</returns>
        private static bool FormatEntityImplicitMod(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithMods = ent as IHasMods;
            if (entWithMods != null && entWithMods.ModsHandler.HasImplicitMod)
            {
                ic.Add(entWithMods.ModsHandler.Mods[0]);
            }

            return ic.Count > 0;
        }

        /// <summary>
        /// Formats an InlineCollection with explicit mods of an entity.
        /// If no mods are present, item's description is used instead.
        /// </summary>
        /// <returns>False if no information was found</returns>
        private static bool FormatEntityExplicitMods(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithMods = ent as IHasMods;
            var entWithDescription = ent as IHasDescription;

            // Mods
            if (entWithMods != null && entWithMods.ModsHandler.IsRelevant)
            {
                int start = entWithMods.ModsHandler.HasImplicitMod ? 1 : 0;

                for (int i = start; i < entWithMods.ModsHandler.Mods.Length; i++)
                {
                    string mod = entWithMods.ModsHandler.Mods[i];
                    var run = new Run(mod);

                    // Corrupted mod should be red
                    bool isCorruptedMod = mod.Trim().Equals("Corrupted", StringComparison.InvariantCultureIgnoreCase);
                    if (isCorruptedMod)
                        run.Foreground = new SolidColorBrush(Colors.Red);

                    // New line if not the last mod
                    ic.Add(run);
                    if (i < entWithMods.ModsHandler.Mods.Length - 1)
                        ic.Add(Environment.NewLine);
                }
            }

            // Description
            if (entWithDescription != null && !string.IsNullOrEmpty(entWithDescription.DescriptionHandler.Description))
                ic.Add(entWithDescription.DescriptionHandler.Description);

            return ic.Count > 0;
        }

        private readonly Entity _ent;

        public InfoPage(Entity ent)
        {
            _ent = ent;

            InitializeComponent();

            // Get data
            bool hasName = FormatEntityName(_ent, TbName.Inlines);
            bool hasStats = FormatEntityStats(_ent, TbStats.Inlines);
            bool hasRequirements = FormatEntityRequirements(_ent, TbRequiremenets.Inlines);
            bool hasImplicitMod = FormatEntityImplicitMod(_ent, TbImplicitMod.Inlines);
            bool hasExplicitMods = FormatEntityExplicitMods(_ent, TbMods.Inlines);

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

            if (File.Exists(_ent.ImageUri.LocalPath))
                ImgItem.Source = new BitmapImage(_ent.ImageUri);
            else
            {
                ImgItem.Visibility = Visibility.Collapsed;
                RectSeparator4.Visibility = Visibility.Collapsed;
            }

            // Figure out the colors
            Color fore, back;
            _ent.GetEntityColor(out fore, out back);
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

        private void TbName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_ent is Recipe) return;
            Process.Start("http://pathofexile.gamepedia.com/" + _ent.Name);
        }
    }
}