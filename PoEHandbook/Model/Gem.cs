//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Gem.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Xml;
using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Gem : Entity, IHasStats, IHasDescription
    {
        public Gem()
        {
            StatsHandler = new StatsHandler(this);
            DescriptionHandler = new DescriptionHandler(this);
        }

        public StatsHandler StatsHandler { get; private set; }
        public DescriptionHandler DescriptionHandler { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            StatsHandler.Deserialize(node);
            DescriptionHandler.Deserialize(node);
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = base.ContainsInProperties(query, out properties);

            List<string> temp;
            if (StatsHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
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