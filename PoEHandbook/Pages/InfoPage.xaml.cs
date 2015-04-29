//  ------------------------------------------------------------------ 
//  PoEHandbook
//  InfoPage.xaml.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
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
        public InfoPage(Entity ent)
        {
            InitializeComponent();

            // Get data
            FormatEntityName(ent, TbName.Inlines);
            FormatEntityStats(ent, TbStats.Inlines);
            FormatEntityRequirements(ent, TbRequiremenets.Inlines);
            FormatEntityMods(ent, TbMods.Inlines);
            ImgItem.Source = new BitmapImage(ent.ImageUri);

            // Figure out the colors
            var rarity = RarityHandler.RarityTier.Normal;

            var entWithRarity = ent as IHasRarity;
            if (entWithRarity != null) rarity = entWithRarity.RarityHandler.Rarity;

            Color fore, back;
            rarity.GetRarityColor(out fore, out back);
            Resources["AccentColor"] = back;
            TbName.Foreground = new SolidColorBrush(fore);
        }

        private static void FormatEntityName(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            ic.Add(ent.Name);

            var entWithRarity = ent as IHasRarity;
            if (entWithRarity != null)
            {
                ic.Add(Environment.NewLine);
                ic.Add(entWithRarity.RarityHandler.Base);
            }
        }

        private static void FormatEntityStats(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithStats = ent as IHasStats;

            if (entWithStats == null || !entWithStats.StatsHandler.IsRelevant)
            {
                ic.Add("No stats");
                return;
            }

            bool first = true;

            var statsHandler = entWithStats.StatsHandler;
            if (statsHandler.ArmourValue.Average > 0)
            {
                first = false;

                ic.Add("Armour: ");
                var run = new Run(statsHandler.ArmourValue.ToString()) {FontWeight = FontWeights.Bold};
                if (statsHandler.ArmourAffected)
                    run.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"));
                ic.Add(run);
            }
            if (statsHandler.EvasionValue.Average > 0)
            {
                if (!first)
                    ic.Add(Environment.NewLine);
                first = false;

                ic.Add("Evasion: ");
                var run = new Run(statsHandler.EvasionValue.ToString()) {FontWeight = FontWeights.Bold};
                if (statsHandler.EvasionAffected)
                    run.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"));
                ic.Add(run);
            }
            if (statsHandler.EnergyShieldValue.Average > 0)
            {
                if (!first)
                    ic.Add(Environment.NewLine);

                ic.Add("Energy Shield: ");
                var run = new Run(statsHandler.EnergyShieldValue.ToString()) {FontWeight = FontWeights.Bold};
                if (statsHandler.EnergyShieldAffected)
                    run.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"));
                ic.Add(run);
            }
        }

        private static void FormatEntityRequirements(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithRequirements = ent as IHasRequirements;

            if (entWithRequirements == null || !entWithRequirements.RequirementsHandler.IsRelevant)
            {
                ic.Add("No requirements");
                return;
            }

            bool first = true;
            ic.Add("Requires ");

            var reqHandler = entWithRequirements.RequirementsHandler;
            if (reqHandler.Level > 0)
            {
                first = false;

                ic.Add("Level ");
                ic.Add(new Run {Text = "" + reqHandler.Level, FontWeight = FontWeights.Bold});
            }
            if (reqHandler.Strength > 0)
            {
                if (!first)
                    ic.Add(", ");
                first = false;

                ic.Add(new Run {Text = "" + reqHandler.Strength, FontWeight = FontWeights.Bold});
                ic.Add(" Str");
            }
            if (reqHandler.Dexterity > 0)
            {
                if (!first)
                    ic.Add(", ");
                first = false;

                ic.Add(new Run {Text = "" + reqHandler.Dexterity, FontWeight = FontWeights.Bold});
                ic.Add(" Dex");
            }
            if (reqHandler.Intelligence > 0)
            {
                if (!first)
                    ic.Add(", ");

                ic.Add(new Run {Text = "" + reqHandler.Intelligence, FontWeight = FontWeights.Bold});
                ic.Add(" Int");
            }
        }

        private static void FormatEntityMods(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            var entWithMods = ent as IHasMods;
            var entWithDescription = ent as IHasDescription;

            if ((entWithMods == null || !entWithMods.ModsHandler.IsRelevant) && entWithDescription == null)
            {
                ic.Add("No mods or description");
                return;
            }

            ic.Add(entWithMods != null
                ? string.Join(Environment.NewLine, entWithMods.ModsHandler.Mods)
                : entWithDescription.DescriptionHandler.Description);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService == null || !NavigationService.CanGoBack) return;

            NavigationService.GoBack();
        }
    }
}