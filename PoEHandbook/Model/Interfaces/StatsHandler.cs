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

        // Armour
        public Range ArmourValue { get; private set; }
        public Range EvasionValue { get; private set; }
        public Range EnergyShieldValue { get; private set; }

        // Shields
        public Range Block { get; private set; }

        // Weapons
        public Range Damage { get; private set; }
        public Range CriticalStrikeChance { get; private set; }
        public Range AttacksPerSecond { get; private set; }
        public Range DamagePerSecond { get; private set; }

        // Maps
        public int Quantity { get; private set; }

        // Jewels
        public string Radius { get; private set; }

        public bool IsRelevant
        {
            get
            {
                return ArmourValue.Average > 0 || EvasionValue.Average > 0 || EnergyShieldValue.Average > 0 ||
                       Block.Average > 0 || Damage.Average > 0 || CriticalStrikeChance.Average > 0 ||
                       AttacksPerSecond.Average > 0 || DamagePerSecond.Average > 0 || Quantity > 0 ||
                       !string.IsNullOrEmpty(Radius);
            }
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
                return parentWithMods.ModsHandler.Mods.Any(mod => (
                    mod.ContainsInvariant("INCREASED") ||
                    mod.ContainsInvariant("REDUCED")) &&
                    mod.ContainsInvariant("ARMOUR"));
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
                return parentWithMods.ModsHandler.Mods.Any(mod => (
                    mod.ContainsInvariant("INCREASED") ||
                    mod.ContainsInvariant("REDUCED")) &&
                    mod.ContainsInvariant("EVASION"));
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
                return parentWithMods.ModsHandler.Mods.Any(mod => (
                    mod.ContainsInvariant("INCREASED") ||
                    mod.ContainsInvariant("REDUCED")) &&
                    mod.ContainsInvariant("ENERGY SHIELD"));
            }
        }

        /// <summary>
        /// Determines whether the item's mods affect Block value
        /// </summary>
        public bool BlockAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods.Any(mod => (
                    mod.ContainsInvariant("ADDITIONAL") ||
                    mod.ContainsInvariant("REDUCED")) &&
                    mod.ContainsInvariant("BLOCK") &&
                    mod.ContainsInvariant("CHANCE"));
            }
        }

        /// <summary>
        /// Determines whether the item's mods affect Damage value
        /// </summary>
        public bool DamageAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods.Any(mod => (
                    mod.ContainsInvariant("INCREASED") ||
                    mod.ContainsInvariant("REDUCED")) &&
                    mod.ContainsInvariant("DAMAGE"));
            }
        }

        /// <summary>
        /// Determines whether the item's mods affect Critical Strike Chance value
        /// </summary>
        public bool CriticalStrikeChanceAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods.Any(mod => (
                    mod.ContainsInvariant("INCREASED") ||
                    mod.ContainsInvariant("REDUCED")) &&
                    mod.ContainsInvariant("CRITICAL STRIKE CHANCE"));
            }
        }

        /// <summary>
        /// Determines whether the item's mods affect Attacks Per Second value
        /// </summary>
        public bool AttacksPerSecondAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods.Any(mod => (
                    mod.ContainsInvariant("INCREASED") ||
                    mod.ContainsInvariant("REDUCED")) &&
                    mod.ContainsInvariant("ATTACK SPEED"));
            }
        }

        /// <summary>
        /// Determines whether the item's mods affect DPS value
        /// </summary>
        public bool DamagePerSecondAffected
        {
            get { return DamageAffected || AttacksPerSecondAffected; }
        }

        /// <summary>
        /// Determines whether the item's mods affect Quantity value
        /// </summary>
        public bool QuantityAffected
        {
            get
            {
                // No item mods that affect quantity exist
                return false;
            }
        }

        /// <summary>
        /// Determines whether the item's mods affect Radius value
        /// </summary>
        public bool RadiusAffected
        {
            get
            {
                // No item mods that affect radius exist
                return false;
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

            temp = node.SelectSingleNode(@"Property[@id='Block']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText))
                Block = new Range(temp.InnerText.TrimEnd('%'));

            temp = node.SelectSingleNode(@"Property[@id='Damage']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) Damage = new Range(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='CriticalStrikeChance']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText))
                CriticalStrikeChance = new Range(temp.InnerText.TrimEnd('%'));

            temp = node.SelectSingleNode(@"Property[@id='AttacksPerSecond']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) AttacksPerSecond = new Range(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='DamagePerSecond']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) DamagePerSecond = new Range(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Quantity']");
            if (temp != null) Quantity = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Radius']");
            if (temp != null && temp.InnerText != "0") Radius = temp.InnerText;
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            properties = new List<string>();
            return false;
        }
    }
}