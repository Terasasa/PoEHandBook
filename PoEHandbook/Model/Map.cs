//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Map.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Xml;
using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Map : Entity, IHasMods, IHasRarity
    {
        public Map()
        {
            ModsHandler = new ModsHandler(this);
            RarityHandler = new RarityHandler(this);
        }

        public ModsHandler ModsHandler { get; private set; }
        public RarityHandler RarityHandler { get; private set; }

        public int Level { get; private set; }
        public int Quantity { get; private set; }

        public bool StatsRelevant { get { return Level > 0 || Quantity > 0; } }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            ModsHandler.Deserialize(node);
            RarityHandler.Deserialize(node);

            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Level']");
            if (temp != null) Level = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Quantity']");
            if (temp != null) Quantity = int.Parse(temp.InnerText);
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = base.ContainsInProperties(query, out properties);

            List<string> temp;
            if (ModsHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
                result = true;
            }
            if (RarityHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
                result = true;
            }

            return result;
        }
    }
}