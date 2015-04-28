//  ------------------------------------------------------------------ 
//  PoEHandbook
//  SearchResult.cs by Tyrrrz
//  27/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<SearchResult> Intersect(IEnumerable<SearchResult> a, IEnumerable<SearchResult> b)
        {
            var results1 = a as IList<SearchResult> ?? a.ToList();
            var results2 = b as IList<SearchResult> ?? b.ToList();

            var output = new List<SearchResult>();

            foreach (var sr1 in results1)
            {
                // Get entity from first list in the second list
                var mutualResult = results2.FirstOrDefault(sr2 => sr2.Entity.Name == sr1.Entity.Name);
                if (mutualResult == null) continue;

                // Combine matches
                var matches = sr1.Matches.ToList();
                matches.AddRange(mutualResult.Matches);
                matches = matches.Distinct().ToList();

                // Create new result
                output.Add(new SearchResult(sr1.Entity, matches));
            }

            return output;
        }
    }
}