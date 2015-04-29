//  ------------------------------------------------------------------ 
//  PoEHandbook
//  StatsHandler.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PoEHandbook.Model.Interfaces
{
    public class StatsHandler : Handler<IHasStats>
    {
        public StatsHandler(Entity parent)
            : base(parent)
        {
        }

        public Range ArmourValue { get; private set; }
        public Range EvasionValue { get; private set; }
        public Range EnergyShieldValue { get; private set; }

        public bool IsRelevant
        {
            get { return ArmourValue.Average > 0 || EvasionValue.Average > 0 || EnergyShieldValue.Average > 0; }
        }

        /// <summary>
        /// Determines whether the item's mods affect Armour value
        /// </summary>
        public bool ArmourAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods
                    .Select(mod => mod.ToUpperInvariant())
                    .Any(modUp => (modUp.Contains("INCREASED") || modUp.Contains("REDUCED")) && modUp.Contains("ARMOUR"));
            }
        }

        /// <summary>
        /// Determines whether the item's mods affect Evasion value
        /// </summary>
        public bool EvasionAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods
                    .Select(mod => mod.ToUpperInvariant())
                    .Any(modUp => (modUp.Contains("INCREASED") || modUp.Contains("REDUCED")) && modUp.Contains("EVASION"));
            }
        }

        /// <summary>
        /// Determines whether the item's mods affect Energy Shield value
        /// </summary>
        public bool EnergyShieldAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods
                    .Select(mod => mod.ToUpperInvariant())
                    .Any(modUp => (modUp.Contains("INCREASED") || modUp.Contains("REDUCED")) && modUp.Contains("ENERGY SHIELD"));
            }
        }

        public override void Deserialize(XmlNode node)
        {
            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='ArmourValue']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) ArmourValue = new Range(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='EvasionValue']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) EvasionValue = new Range(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='EnergyShieldValue']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) EnergyShieldValue = new Range(temp.InnerText);
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            properties = new List<string>();
            return false;
        }
    }
}