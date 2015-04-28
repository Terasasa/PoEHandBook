//  ------------------------------------------------------------------ 
//  PoEHandbook
//  DataAccess.cs by Tyrrrz
//  26/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using PoEHandbook.Model;

namespace PoEHandbook.Data
{
    public static class DataAccess
    {
        private static readonly HashSet<Entity> Entities = new HashSet<Entity>();
        private static readonly Dictionary<string, string> Aliases = new Dictionary<string, string>();

        private static void LoadEntities()
        {
            Entities.Clear();
            var resources = new Dictionary<string, Type>
            {
                {"uniques_armors.xml", typeof (Armour)},
                {"uniques_shields.xml", typeof (Equippable)},
                {"misc.xml", typeof (Entity)}
            };

            foreach (var resource in resources)
            {
                string path = Path.Combine(Environment.CurrentDirectory, "Data", resource.Key);

                if (!File.Exists(path)) continue;

                var doc = new XmlDocument();
                doc.Load(path);

                var entityNodes = doc.SelectNodes("Root/Entity");
                if (entityNodes == null) continue;

                foreach (XmlNode node in entityNodes)
                {
                    var item = (Entity)Activator.CreateInstance(resource.Value);
                    item.Deserialize(node);
                    Entities.Add(item);
                }
            }
            
        }

        private static void LoadAliases()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Data", "Aliases.xml");

            if (!File.Exists(path)) return;

            var doc = new XmlDocument();
            doc.Load(path);

            var aliasNodes = doc.SelectNodes("Root/Alias");
            if (aliasNodes == null) return;

            foreach (XmlNode node in aliasNodes)
            {
                var alias = new Alias();
                alias.Deserialize(node);
                Aliases.Add(alias.Input.ToUpperInvariant(), alias.Output.ToUpperInvariant());
            }
        }

        public static void LoadData()
        {
            LoadEntities();   
            LoadAliases();
        }

        /// <summary>
        /// Searches through entities using a query, but sets all matches to Alias matches
        /// </summary>
        private static IEnumerable<SearchResult> PerformAliasSearchQuery(string aliasQuery)
        {
            aliasQuery = aliasQuery.Trim().ToUpperInvariant();

            var result = new List<SearchResult>();

            foreach (var ent in Entities)
            {
                List<string> properties;
                if (ent.ContainsInProperties(aliasQuery, out properties))
                    result.Add(new SearchResult(ent, new[] { "Alias" }));
            }

            return result;
        }

        /// <summary>
        /// Search entities' properties with the given query
        /// </summary>
        /// <param name="query">Single query</param>
        public static IEnumerable<SearchResult> PerformSearchQuery(string query)
        {
            query = query.Trim().ToUpperInvariant();

            var output = new List<SearchResult>();

            // Basic search
            foreach (var ent in Entities)
            {
                List<string> properties;
                if (ent.ContainsInProperties(query, out properties))
                    output.Add(new SearchResult(ent, properties));
            }

            // Search with aliases
            string aliasOutput;
            if (Aliases.TryGetValue(query, out aliasOutput))
                output.AddRange(PerformAliasSearchQuery(aliasOutput)
                        .Where(sr1 => output.All(sr2 => sr2.Entity.Name != sr1.Entity.Name)));

            return output;
        }

        /// <summary>
        /// Search entities' properties with the given queries
        /// </summary>
        /// <param name="queries">Multiple queries</param>
        public static IEnumerable<SearchResult> PerformSearchQuery(IEnumerable<string> queries)
        {
            IEnumerable<SearchResult> output = null;

            foreach (string query in queries)
            {
                // First query ----v
                if (output == null)
                {
                    output = PerformSearchQuery(query);
                    continue;
                }

                // Other queries --v
                var newResults = PerformSearchQuery(query);
                output = SearchResult.Intersect(output, newResults);
            }

            return output;
        }
    }
}