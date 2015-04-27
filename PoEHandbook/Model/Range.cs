//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Range.cs by Tyrrrz
//  27/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Text.RegularExpressions;

namespace PoEHandbook.Model
{
    /// <summary>
    /// Encapsulates two values that define a range of possible values.
    /// </summary>
    public struct Range
    {
        public readonly int Min;
        public readonly int Max;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public double Average { get { return 0.5*(Min + Max); } }
        public bool SingleValue { get { return Min == Max; } }

        public Range(string str)
        {
            // Single int value
            int value;
            if (int.TryParse(str, out value))
            {
                Min = Max = value;
                return;
            }

            // Range value
            var regex = new Regex(@"(?<min>\d+)\s*((to)|\-)\s*(?<max>\d+)", RegexOptions.IgnoreCase);
            var match = regex.Match(str);

            if (!match.Success) throw new FormatException("Input string does not contain proper data.");

            Min = int.Parse(match.Groups["min"].Value);
            Max = int.Parse(match.Groups["max"].Value);
        }

        public override string ToString()
        {
            if (SingleValue)
                return "" + Min;

            return Min + " to " + Max;
        }
    }
}