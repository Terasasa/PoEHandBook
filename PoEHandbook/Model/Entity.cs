//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Entity.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace PoEHandbook.Model
{
    /// <summary>
    /// Generalizes every item in PoE
    /// </summary>
    public class Entity
    {
        public string Name { get; private set; }
        public string ImageName { get; private set; }
        public Uri ImageUri
        {
            get { return new Uri(Path.Combine(Environment.CurrentDirectory, "Data", "Cache", ImageName)); }
        }
        public string Text { get; private set; }
        public string[] BannedKeywords { get; private set; }

        public virtual void Deserialize(XmlNode node)
        {
            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Name']");
            if (temp != null) Name = temp.InnerText;

            temp = node.SelectSingleNode(@"Property[@id='ImageName']");
            ImageName = temp != null ? temp.InnerText : Name + ".png";

            temp = node.SelectSingleNode(@"Property[@id='Text']");
            if (temp != null) Text = temp.InnerText;

            temp = node.SelectSingleNode(@"Property[@id='BannedKeywords']");
            if (temp != null)
                BannedKeywords = temp.InnerText.Split(new[] {" | "}, StringSplitOptions.RemoveEmptyEntries);
        }

        public virtual bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = false;
            properties = new List<string>();

            // Check banned keywords
            if (BannedKeywords != null && BannedKeywords.Any(keyword => keyword.ToUpperInvariant().Contains(query)))
                return false;

            // Process everything else
            if (Name.ToUpperInvariant().Contains(query))
            {
                properties.Add("Name");
                result = true;
            }
            if (Text.ToUpperInvariant().Contains(query))
            {
                properties.Add(Text);
                result = true;
            }

            return result;
        }
    }
}