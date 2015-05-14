//  ------------------------------------------------------------------ 
//  PoEHandbook
//  RequirementsHandler.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PoEHandbook.Model.Interfaces
{
    public class RequirementsHandler : Handler<IHasRequirements>
    {
        public RequirementsHandler(Entity parent)
            : base(parent)
        {
        }

        public int Level { get; private set; }
        public int Strength { get; private set; }
        public int Dexterity { get; private set; }
        public int Intelligence { get; private set; }

        public bool IsRelevant
        {
            get { return Level > 0 || Strength > 0 || Dexterity > 0 || Intelligence > 0; }
        }

        /// <summary>
        /// Determines whether the item's mods affect Level value
        /// </summary>
        public bool LevelAffected
        {
            get
            {
                // Currently no items have level requirement affecting mods
                return false;
            }
        }
        /// <summary>
        /// Determines whether the item's mods affect Strength value
        /// </summary>
        public bool StrengthAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods.Any(mod =>
                    (mod.ContainsInvariant("STRENGTH") || mod.ContainsInvariant("ATTRIBUTE")));
            }
        }
        /// <summary>
        /// Determines whether the item's mods affect Dexterity value
        /// </summary>
        public bool DexterityAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods.Any(mod =>
                    (mod.ContainsInvariant("DEXTERITY") || mod.ContainsInvariant("ATTRIBUTE")));
            }
        }
        /// <summary>
        /// Determines whether the item's mods affect Intelligence value
        /// </summary>
        public bool IntelligenceAffected
        {
            get
            {
                var parentWithMods = Parent as IHasMods;
                if (parentWithMods == null) return false;
                return parentWithMods.ModsHandler.Mods.Any(mod =>
                    (mod.ContainsInvariant("INTELLIGENCE") || mod.ContainsInvariant("ATTRIBUTE")));
            }
        }

        public override void Deserialize(XmlNode node)
        {
            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Level']");
            if (temp != null) Level = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Strength']");
            if (temp != null) Strength = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Dexterity']");
            if (temp != null) Dexterity = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Intelligence']");
            if (temp != null) Intelligence = int.Parse(temp.InnerText);
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            properties = new List<string>();
            return false;
        }
    }
}