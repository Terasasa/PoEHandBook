//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Handler.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Xml;

namespace PoEHandbook.Model.Interfaces
{
    public abstract class Handler<TInterface>
    {
        public Entity Parent { get; private set; }

        protected Handler(Entity parent)
        {
            Parent = parent;
        }

        public abstract void Deserialize(XmlNode node);
        public abstract bool ContainsInProperties(string query, out List<string> properties);
    }
}