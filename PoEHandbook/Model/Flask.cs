// ------------------------------------------------------------------ 
// PoEHandbook
// Flask.cs by Tyrrrz
// 15/05/2015
// ------------------------------------------------------------------ 

using System.Xml;
using PoEHandbook.Model.Types;

namespace PoEHandbook.Model
{
    public class Flask : Equipment
    {
        public RangeDouble LifeRecovery { get; private set; }
        public RangeDouble ManaRecovery { get; private set; }
        public RangeDouble Duration { get; private set; }
        public RangeDouble Capacity { get; private set; }
        public RangeDouble ChargesUsed { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            XmlNode temp;
            temp = node.SelectSingleNode(@"Property[@id='LifeRecovery']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) LifeRecovery = RangeDouble.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='ManaRecovery']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) ManaRecovery = RangeDouble.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Duration']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) Duration = RangeDouble.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='Capacity']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) Capacity = RangeDouble.Parse(temp.InnerText);

            temp = node.SelectSingleNode(@"Property[@id='ChargesUsed']");
            if (temp != null && !string.IsNullOrEmpty(temp.InnerText)) ChargesUsed = RangeDouble.Parse(temp.InnerText);
        }
    }
}