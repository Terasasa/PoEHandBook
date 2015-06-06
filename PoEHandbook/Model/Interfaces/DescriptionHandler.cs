// ------------------------------------------------------------------ 
// PoEHandbook
// DescriptionHandler.cs by Tyrrrz
// 06/05/2015
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PoEHandbook.Model.Interfaces
{
    public class DescriptionHandler : Handler<IHasDescription>
    {
        public string Description { get; private set; }

        public string[] DescriptionLines
        {
            get { return Description.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries); }
        }

        public DescriptionHandler(Entity parent)
            : base(parent)
        {
        }

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
                properties.Add(DescriptionLines.FirstOrDefault(line => line.ContainsInvariant(query)));
                result = true;
            }

            return result;
        }
    }
}