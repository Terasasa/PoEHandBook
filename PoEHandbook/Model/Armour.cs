// ------------------------------------------------------------------ 
// PoEHandbook
// Armour.cs by Tyrrrz
// 14/05/2015
// ------------------------------------------------------------------ 

using System.Xml;
using PoEHandbook.Model.Types;

namespace PoEHandbook.Model
{
    public class Armour : Equipment
    {
        public RangeDouble ArmourValue { get; private set; }
        public RangeDouble EvasionValue { get; private set; }
        public RangeDouble EnergyShieldValue { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='ArmourValue']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) ArmourValue = RangeDouble.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='EvasionValue']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) EvasionValue = RangeDouble.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='EnergyShieldValue']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText))
                EnergyShieldValue = RangeDouble.Parse(temp.InnerText);
        }
    }
}