// ------------------------------------------------------------------ 
// PoEHandbook
// Recipe.cs by Tyrrrz
// 06/05/2015
// ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Xml;
using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Recipe : Entity, IHasDescription
    {
        public DescriptionHandler DescriptionHandler { get; private set; }
        public string PlayerOffer { get; private set; }
        public string NPCOffer { get; private set; }

        public Recipe()
        {
            DescriptionHandler = new DescriptionHandler(this);
        }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            DescriptionHandler.Deserialize(node);

            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='PlayerOffer']");
            if (temp != null) PlayerOffer = temp.InnerText;

            temp = node.SelectSingleNode(@"Property[@id='NPCOffer']");
            if (temp != null) NPCOffer = temp.InnerText;
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = base.ContainsInProperties(query, out properties);

            List<string> temp;
            if (PlayerOffer.ContainsInvariant(query))
            {
                properties.Add("Player Offer");
                result = true;
            }
            if (NPCOffer.ContainsInvariant(query))
            {
                properties.Add("NPC Offer");
                result = true;
            }
            if (DescriptionHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
                result = true;
            }

            return result;
        }
    }
}