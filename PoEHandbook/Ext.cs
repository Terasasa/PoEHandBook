//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Ext.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Returns the first occurence of any substrings in enumerable
        /// </summary>
        public static int IndexOfAnyInvariant(this string str, int offset, IEnumerable<string> subStrings, out string match)
        {
            int index = int.MaxValue;
            match = "";

            foreach (string sub in subStrings)
            {
                int curIndex = str.IndexOf(sub, offset, StringComparison.InvariantCultureIgnoreCase);
                if (curIndex == 0)
                {
                    match = str.Substring(curIndex, sub.Length);
                    return 0;
                }
                if (curIndex >= 0 && curIndex < index)
                {
                    match = str.Substring(curIndex, sub.Length);
                    index = curIndex;
                }
            }

            if (index == int.MaxValue) return -1;
            return index;
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

        /// <summary>
        /// Returns the same enumerable, with all its elements trimmed
        /// </summary>
        public static IEnumerable<string> TrimElements(this IEnumerable<string> enumerable)
        {
            return enumerable.Select(str => str.Trim());
        }
    }
}