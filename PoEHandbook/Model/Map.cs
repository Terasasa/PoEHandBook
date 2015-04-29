//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Map.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Map : Entity, IHasMods, IHasDescription, IHasRarity
    {
        public Map()
        {
            ModsHandler = new ModsHandler(this);
            DescriptionHandler = new DescriptionHandler(this);
            RarityHandler = new RarityHandler(this);
        }

        public ModsHandler ModsHandler { get; private set; }
        public DescriptionHandler DescriptionHandler { get; private set; }
        public RarityHandler RarityHandler { get; private set; }
    }
}