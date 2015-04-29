//  ------------------------------------------------------------------ 
//  PoEHandbook
//  DescriptionHandler.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Xml;

namespace PoEHandbook.Model.Interfaces
{
    public class DescriptionHandler : Handler<IHasDescription>
    {
        public DescriptionHandler(Entity parent) 
            : base(parent)
        {
        }

        public string Description { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Description']");
            if (temp != null) Description = temp.InnerText;
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = false;
            properties = new List<string>();

            if (Description.ContainsInvariant(query))
            {
                properties.Add("Description");
                result = true;
            }

            return result;
        }
    }
}