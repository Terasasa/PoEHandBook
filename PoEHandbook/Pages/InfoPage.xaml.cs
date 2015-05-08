// //  ------------------------------------------------------------------ 
// //  PoEHandbook
// //  InfoPage.xaml.cs by 
// //  06/05/2015
// //  ------------------------------------------------------------------ 

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
            Color fore, back;
            ent.GetEntityColor(out fore, out back);
            Resources["AccentColor"] = back;
            TbName.Foreground = new SolidColorBrush(fore);
        }

        static private void FormatEntityName(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            ic.Add(ent.Name);

            IHasRarity entWithRarity = ent as IHasRarity;
            if (entWithRarity != null)
            {
                ic.Add(Environment.NewLine);
                ic.Add(entWithRarity.RarityHandler.Base);
            }
        }

        static private void FormatEntityStats(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            IHasStats entWithStats = ent as IHasStats;

            if (entWithStats == null || !entWithStats.StatsHandler.IsRelevant)
            {
                ic.Add("No stats");
                return;
            }

            bool first = true;

            StatsHandler statsHandler = entWithStats.StatsHandler;
            if (statsHandler.Block.Average > 0)
            {
                first = false;

                ic.Add("Chance to Block: ");
                Run run = new Run(statsHandler.Block + "%");
                if (statsHandler.BlockAffected)
                    run.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"));
                ic.Add(run);
            }
            if (statsHandler.ArmourValue.Average > 0)
            {
                if (!first)
                    ic.Add(Environment.NewLine);
                first = false;

                ic.Add("Armour: ");
                Run run = new Run(statsHandler.ArmourValue.ToString());
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
                Run run = new Run(statsHandler.EvasionValue.ToString());
                if (statsHandler.EvasionAffected)
                    run.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"));
                ic.Add(run);
            }
            if (statsHandler.EnergyShieldValue.Average > 0)
            {
                if (!first)
                    ic.Add(Environment.NewLine);

                ic.Add("Energy Shield: ");
                Run run = new Run(statsHandler.EnergyShieldValue.ToString());
                if (statsHandler.EnergyShieldAffected)
                    run.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6a88ef"));
                ic.Add(run);
            }
        }

        static private void FormatEntityRequirements(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            IHasRequirements entWithRequirements = ent as IHasRequirements;

            if (entWithRequirements == null || !entWithRequirements.RequirementsHandler.IsRelevant)
            {
                ic.Add("No requirements");
                return;
            }

            bool first = true;
            ic.Add("Requires ");

            RequirementsHandler reqHandler = entWithRequirements.RequirementsHandler;
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

        static private void FormatEntityMods(Entity ent, InlineCollection ic)
        {
            ic.Clear();

            IHasMods entWithMods = ent as IHasMods;
            IHasDescription entWithDescription = ent as IHasDescription;

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