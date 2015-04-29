//  ------------------------------------------------------------------ 
//  PoEHandbook
//  IHasStats.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

namespace PoEHandbook.Model.Interfaces
{
    /// <summary>
    /// Implemented by items that have stats
    /// </summary>
    public interface IHasStats
    {
        StatsHandler StatsHandler { get; }
    }
}