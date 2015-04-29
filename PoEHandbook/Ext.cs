//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Ext.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

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
    }
}