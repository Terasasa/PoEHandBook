//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Range.cs by Tyrrrz
//  27/04/2015
//  ------------------------------------------------------------------ 

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PoEHandbook.Model
{
    /// <summary>
    /// Encapsulates two values that define a range of possible values.
    /// </summary>
    public struct Range
    {
        public readonly bool SingleValue;
        public readonly double Min;
        public readonly double Max;

        public Range(double value)
        {
            SingleValue = true;
            Min = Max = value;
        }

        public Range(double min, double max)
        {
            SingleValue = false;
            Min = min;
            Max = max;
        }

        public double Average { get { return 0.5*(Min + Max); } }

        public Range(string str)
        {
            // Single int value
            double value;
            if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                Min = Max = value;
                SingleValue = true;
                return;
            }

            // Range value
            var regex = new Regex(@"(?<min>[\d\.\,]+)\s*((to)|\-|\–)\s*(?<max>[\d\.\,]+)", RegexOptions.IgnoreCase);
            var match = regex.Match(str);

            if (!match.Success) throw new FormatException("Input string does not contain proper data.");

            SingleValue = false;
            Min = double.Parse(match.Groups["min"].Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            Max = double.Parse(match.Groups["max"].Value, NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            if (SingleValue)
                return "" + Min;

            return "(" + Min + " to " + Max + ")";
        }
    }
}