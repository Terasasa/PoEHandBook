//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Recipe.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Xml;
using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Recipe : Entity, IHasDescription
    {
        public Recipe()
        {
            DescriptionHandler = new DescriptionHandler(this);
        }

        public DescriptionHandler DescriptionHandler { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            DescriptionHandler.Deserialize(node);
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = base.ContainsInProperties(query, out properties);

            List<string> temp;
            if (DescriptionHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
                result = true;
            }

            return result;
        }
    }
}