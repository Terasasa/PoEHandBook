//  ------------------------------------------------------------------ 
//  PoEHandbook
//  RarityHandler.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Xml;

namespace PoEHandbook.Model.Interfaces
{
    public class RarityHandler : Handler<IHasRarity>
    {
        public enum RarityTier
        {
            Normal,
            Magic,
            Rare,
            Unique
        }

        public RarityHandler(Entity parent) 
            : base(parent)
        {
        }

        public RarityTier Rarity { get; private set; }
        public string Base { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Rarity']");
            if (temp != null)
                Rarity = (RarityTier) Enum.Parse(typeof (RarityTier), temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Base']");
            if (temp != null) Base = temp.InnerText;
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = false;
            properties = new List<string>();

            if (Rarity.ToString().ContainsInvariant(query))
            {
                properties.Add("Rarity");
                result = true;
            }
            if (Base.ContainsInvariant(query))
            {
                properties.Add("Base Item");
                result = true;
            }

            return result;
        }
    }
}