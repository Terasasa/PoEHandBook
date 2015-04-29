//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Jewel.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Jewel : Entity, IHasMods, IHasRarity
    {
        public Jewel()
        {
            ModsHandler = new ModsHandler(this);
            RarityHandler = new RarityHandler(this);
        }

        public ModsHandler ModsHandler { get; private set; }
        public RarityHandler RarityHandler { get; private set; }
    }
}