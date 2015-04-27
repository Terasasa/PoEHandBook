//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Alias.cs by Tyrrrz
//  27/04/2015
//  ------------------------------------------------------------------ 

using System.Xml;

namespace PoEHandbook.Data
{
    public class Alias
    {
        public string Input { get; private set; }
        public string Output { get; private set; }

        public void Deserialize(XmlNode node)
        {
            // ReSharper disable PossibleNullReferenceException
            Input = node.Attributes["Input"].Value;
            Output = node.Attributes["Output"].Value;
            // ReSharper restore PossibleNullReferenceException
        }
    }
}