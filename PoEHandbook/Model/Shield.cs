// ------------------------------------------------------------------ 
// PoEHandbook
// Shield.cs by Tyrrrz
// 14/05/2015
// ------------------------------------------------------------------ 

using System.Xml;
using PoEHandbook.Model.Types;

namespace PoEHandbook.Model
{
    public class Shield : Equipment
    {
        public RangeDouble Block { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='Block']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText))
                Block = RangeDouble.Parse(temp.InnerText.TrimEnd('%'));
        }
    }
}