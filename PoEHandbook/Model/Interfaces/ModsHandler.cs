//  ------------------------------------------------------------------ 
//  PoEHandbook
//  ModsHandler.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PoEHandbook.Model.Interfaces
{
    public class ModsHandler : Handler<IHasMods>
    {
        public ModsHandler(Entity parent) 
            : base(parent)
        {
        }

        public string[] Mods { get; private set; }
        public bool HasImplicitMod { get; private set; }

        public bool IsRelevant
        {
            get { return Mods != null && Mods.Any(); }
        }

        public string ImplicitMod
        {
            get
            {
                if (HasImplicitMod)
                    return Mods[0];
                return null;
            }
        }

        public override void Deserialize(XmlNode node)
        {
            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Mods']");
            if (temp != null)
                Mods = temp.InnerText.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries);

            temp = node.SelectSingleNode(@"HasImplicitMod");
            HasImplicitMod = temp != null;
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = false;
            properties = new List<string>();

            foreach (string mod in Mods)
            {
                if (mod.ContainsInvariant(query))
                {
                    properties.Add(mod);
                    result = true;
                }
            }

            return result;
        }
    }
}