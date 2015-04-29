//  ------------------------------------------------------------------ 
//  PoEHandbook
//  IHasRarity.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

namespace PoEHandbook.Model.Interfaces
{
    /// <summary>
    /// Implemented by items that come with different rarity tiers
    /// </summary>
    public interface IHasRarity
    {
        RarityHandler RarityHandler { get; }
    }
}