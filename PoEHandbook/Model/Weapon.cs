// ------------------------------------------------------------------ 
// PoEHandbook
// Weapon.cs by Tyrrrz
// 14/05/2015
// ------------------------------------------------------------------ 

using System.Xml;
using PoEHandbook.Model.Types;

namespace PoEHandbook.Model
{
    public class Weapon : Equipment
    {
        public RangeRange PhysicalDamage { get; private set; }
        public RangeRange FireDamage { get; private set; }
        public RangeRange ColdDamage { get; private set; }
        public RangeRange LightningDamage { get; private set; }
        public RangeRange ChaosDamage { get; private set; }
        public RangeDouble CriticalStrikeChance { get; private set; }
        public RangeDouble AttacksPerSecond { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            XmlNode temp;

            temp = node.SelectSingleNode(@"Property[@id='PhysicalDamage']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText))
                PhysicalDamage = RangeRange.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='FireDamage']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) FireDamage = RangeRange.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='ColdDamage']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) ColdDamage = RangeRange.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='LightningDamage']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText))
                LightningDamage = RangeRange.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='ChaosDamage']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) ChaosDamage = RangeRange.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='CriticalStrikeChance']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText))
                CriticalStrikeChance = RangeDouble.Parse(temp.InnerText.TrimEnd('%'));

            temp = node.SelectSingleNode(@"Property[@id='AttacksPerSecond']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText))
                AttacksPerSecond = RangeDouble.Parse(temp.InnerText);
        }
    }
}