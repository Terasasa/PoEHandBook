//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Armor.cs by Tyrrrz
//  27/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PoEHandbook.Model
{
    /// <summary>
    /// An armour piece: body armour, gloves, helmet, boots
    /// </summary>
    public class Armour : Equippable
    {
        public Range ArmourValue { get; private set; }
        public Range EvasionValue { get; private set; }
        public Range EnergyShieldValue { get; private set; }

        public override bool HasStats
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
                return Mods
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
                return Mods
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
                return Mods
                    .Select(mod => mod.ToUpperInvariant())
                    .Any(modUp => (modUp.Contains("INCREASED") || modUp.Contains("REDUCED")) && modUp.Contains("ENERGY SHIELD"));
            }
        }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

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
            bool result = base.ContainsInProperties(query, out properties);

            return result;
        }
    }
}