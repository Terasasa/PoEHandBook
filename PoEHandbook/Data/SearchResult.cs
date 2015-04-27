//  ------------------------------------------------------------------ 
//  PoEHandbook
//  SearchResult.cs by Tyrrrz
//  27/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using PoEHandbook.Model;

namespace PoEHandbook.Data
{
    /// <summary>
    /// Encapsulates an entity, returned after a search query and the list of matches that were triggered
    /// </summary>
    public class SearchResult
    {
        public SearchResult(Entity entity, IEnumerable<string> matches)
        {
            Entity = entity;
            Matches = matches;
        }

        public Entity Entity { get; private set; }
        public IEnumerable<string> Matches { get; private set; }
    }
}