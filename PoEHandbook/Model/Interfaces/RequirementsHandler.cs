// ------------------------------------------------------------------ 
// PoEHandbook
// RequirementsHandler.cs by Tyrrrz
// 06/05/2015
// ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Xml;

namespace PoEHandbook.Model.Interfaces
{
    public class RequirementsHandler : Handler<IHasRequirements>
    {
        public int Level { get; private set; }
        public int Strength { get; private set; }
        public int Dexterity { get; private set; }
        public int Intelligence { get; private set; }

        public bool IsRelevant
        {
            get { return Level > 0 || Strength > 0 || Dexterity > 0 || Intelligence > 0; }
        }

        public RequirementsHandler(Entity parent)
            : base(parent)
        {
        }

        public override void Deserialize(XmlNode node)
        {
            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Level']");
            if (temp != null) Level = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Strength']");
            if (temp != null) Strength = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Dexterity']");
            if (temp != null) Dexterity = int.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Intelligence']");
            if (temp != null) Intelligence = int.Parse(temp.InnerText);
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            properties = new List<string>();
            return false;
        }
    }
}