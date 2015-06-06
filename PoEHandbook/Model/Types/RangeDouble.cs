// ------------------------------------------------------------------ 
// PoEHandbook
// RangeDouble.cs by Tyrrrz
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
    public struct RangeDouble
    {
        public static RangeDouble Parse(string s)
        {
            // Single value
            double value;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return new RangeDouble(value);

            // Range value
            var regex = new Regex(@"(?<min>[\d\.\,]+)\s*((to)|\-|\–)\s*(?<max>[\d\.\,]+)", RegexOptions.IgnoreCase);
            var match = regex.Match(s);

            if (!match.Success) throw new FormatException("Input string does not contain proper data.");

            double min = double.Parse(match.Groups["min"].Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            double max = double.Parse(match.Groups["max"].Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            return new RangeDouble(min, max);
        }

        public static bool TryParse(string s, out RangeDouble result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch (Exception)
            {
                result = default(RangeDouble);
                return false;
            }
        }

        public readonly double Max;
        public readonly double Min;
        public readonly bool SingleValue;

        public double Average
        {
            get { return 0.5*(Min + Max); }
        }

        public RangeDouble(double value)
        {
            SingleValue = true;
            Min = Max = value;
        }

        public RangeDouble(double min, double max)
        {
            SingleValue = false;
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            if (SingleValue)
                return "" + Min;

            return "(" + Min + " to " + Max + ")";
        }
    }
}