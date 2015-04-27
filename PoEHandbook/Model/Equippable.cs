//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Equippable.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Xml;

namespace PoEHandbook.Model
{
    /// <summary>
    /// Equippable items: weapons, shields, armour pieces, jewelry and gems
    /// </summary>
    public class Equippable : Entity
    {
        public enum RarityTier
        {
            Normal,
            Magic,
            Rare,
            Unique
        }

        public string Type { get; private set; }
        public RarityTier Rarity { get; private set; }
        public string Base { get; private set; }

        public int Level { get; private set; }
        public int Strength { get; private set; }
        public int Dexterity { get; private set; }
        public int Intelligence { get; private set; }

        public string[] Mods { get; private set; }

        public bool HasRequirements
        {
            get { return Level > 0 || Strength > 0 || Dexterity > 0 || Intelligence > 0; }
        }

        public virtual bool HasStats
        {
            get { return false; }
        }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Type']");
            if (temp != null) Type = temp.InnerText;

            temp = node.SelectSingleNode(@"Property[@id='Rarity']");
            if (temp != null)
                Rarity = (RarityTier) Enum.Parse(typeof (RarityTier), temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Base']");
            if (temp != null) Base = temp.InnerText;

            temp = node.SelectSingleNode(@"Property[@id='Level']");
            if (temp != null) Level = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Strength']");
            if (temp != null) Strength = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Dexterity']");
            if (temp != null) Dexterity = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Intelligence']");
            if (temp != null) Intelligence = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Mods']");
            if (temp != null)
                Mods = temp.InnerText.Split(new[] {" | "}, StringSplitOptions.RemoveEmptyEntries);
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = base.ContainsInProperties(query, out properties);

            if (Type.ToUpperInvariant().Contains(query))
            {
                properties.Add("Item Type");
                result = true;
            }
            if (Base.ToUpperInvariant().Contains(query))
            {
                properties.Add("Base Item");
                result = true;
            }
            foreach (string mod in Mods)
            {
                if (mod.ToUpperInvariant().Contains(query))
                {
                    properties.Add(mod);
                    result = true;
                }
            }

            return result;
        }
    }
}