//  ------------------------------------------------------------------ 
//  PoEHandbook
//  PassiveNode.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Passive : Entity, IHasDescription
    {
        public Passive()
        {
            DescriptionHandler = new DescriptionHandler(this);
        }

        public DescriptionHandler DescriptionHandler { get; private set; }
    }
}