// ------------------------------------------------------------------ 
// PoEHandbook
// RangeRange.cs by Tyrrrz
// 06/06/2015
// ------------------------------------------------------------------ 

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PoEHandbook.Model.Types
{
    /// <summary>
    /// Encapsulates two values that define a range of possible values.
    /// </summary>
    public struct RangeRange
    {
        public static RangeRange Parse(string s)
        {
            // Single value
            double value;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return new RangeRange(new RangeDouble(value));

            // Range
            var regex = new Regex(@"(?<min>[\d\.\,]+)\s*((to)|\-|\–)\s*(?<max>[\d\.\,]+)", RegexOptions.IgnoreCase);
            var matches = regex.Matches(s);
            if (matches.Count <= 0 || matches.Count > 2) throw new FormatException("Input string does not contain proper data.");

            // Single range
            if (matches.Count == 1)
                return new RangeRange(RangeDouble.Parse(matches[0].Value));

            // Range of range
            var min = RangeDouble.Parse(matches[0].Value);
            var max = RangeDouble.Parse(matches[1].Value);
            return new RangeRange(min, max);
        }

        public static bool TryParse(string s, out RangeRange result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch (Exception)
            {
                result = default(RangeRange);
                return false;
            }
        }

        public readonly RangeDouble Max;
        public readonly RangeDouble Min;
        public readonly bool SingleValue;

        public double Average
        {
            get { return 0.5*(Max.Average + Min.Average); }
        }

        public RangeRange(RangeDouble value)
        {
            SingleValue = true;
            Min = Max = value;
        }

        public RangeRange(RangeDouble min, RangeDouble max)
        {
            SingleValue = false;
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            if (SingleValue)
                return "" + Min;

            return Min + " to " + Max;
        }
    }
}