//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Ext.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Windows;

namespace PoEHandbook
{
    public static class Ext
    {
        /// <summary>
        /// Determines whether first string contains second
        /// Casing and culture are ignored
        /// </summary>
        public static bool ContainsInvariant(this string str, string sub)
        {
            str = str.ToUpperInvariant().Trim();
            sub = sub.ToUpperInvariant().Trim();
            return str.Contains(sub);
        }

        /// <summary>
        /// Create entity from data, obtained by copying item stats in PoE client
        /// </summary>
        public static string QueryFromPoEClipboard()
        {
            string data = Clipboard.GetText();
            string[] lines = data.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2) return null;

            string rarity = lines[0].Substring(lines[0].IndexOf(':') + 1).Trim();
            string name = lines[1].Trim();

            return rarity + ", " + name;
        }
    }
}