//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Gem.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Gem : Entity, IHasStats, IHasDescription
    {
        public Gem()
        {
            StatsHandler = new StatsHandler(this);
            DescriptionHandler = new DescriptionHandler(this);
        }

        public StatsHandler StatsHandler { get; private set; }
        public DescriptionHandler DescriptionHandler { get; private set; }
    }
}