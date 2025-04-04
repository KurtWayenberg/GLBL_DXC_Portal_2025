using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Provides utility methods for working with double values.
    /// </summary>
    public class Double
    {
        #region Public Static Methods

        /// <summary>
        /// Converts a double value to a string using the default format provider.
        /// </summary>
        /// <param name="value">The double value to convert.</param>
        /// <returns>A string representation of the double value.</returns>
        public static string ConvertToString(double value)
        {
            return value.ToString(DoubleFormatProvider.Default);
        }

        /// <summary>
        /// Converts a string to a double value using the default format provider.
        /// </summary>
        /// <param name="value">The string representation of the double value.</param>
        /// <returns>The parsed double value.</returns>
        public static double ConvertFromString(string value)
        {
            return double.Parse(value, DoubleFormatProvider.Default);
        }

        /// <summary>
        /// Converts a string representation of a double to a double value, using a specified decimal separator and default value.
        /// </summary>
        /// <param name="doubleString">The string representation of the double value.</param>
        /// <param name="decimalSeparator">The character used as the decimal separator.</param>
        /// <param name="defaultValue">The default value to return if parsing fails.</param>
        /// <returns>The parsed double value or the default value if parsing fails.</returns>
        public static double AsDouble(string doubleString, char decimalSeparator, double defaultValue)
        {
            double result = 0;
            if (string.IsNullOrEmpty(doubleString))
                return defaultValue;

            string[] doubleParts = doubleString.Split(decimalSeparator);
            if (doubleParts.Length >= 1)
            {
                doubleParts[0] = String.ConvertTo_AZ_09_Blank_String(doubleParts[0]);
                if (!double.TryParse(doubleParts[0], out result))
                    result = 0;
            }

            if (doubleParts.Length >= 2)
            {
                double fraction = 0;
                doubleParts[1] = String.ConvertTo_AZ_09_Blank_String(doubleParts[1]);
                if (!double.TryParse(doubleParts[1], out fraction))
                    fraction = 0;

                result = result + (fraction / Math.Pow(10, doubleParts[1].Length));
            }

            return result;
        }

        /// <summary>
        /// Returns a string describing the value as a file size.
        /// For example, 1.23 MB.
        /// </summary>
        /// <param name="value">The double value representing the file size in bytes.</param>
        /// <returns>A string representation of the file size.</returns>
        public static string ToFileSize(double value)
        {
            string[] suffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            for (int i = 0; i < suffixes.Length; i++)
            {
                if (value <= (Math.Pow(1024, i + 1)))
                {
                    return ThreeNonZeroDigits(value / Math.Pow(1024, i)) + " " + suffixes[i];
                }
            }

            return ThreeNonZeroDigits(value / Math.Pow(1024, suffixes.Length - 1)) + " " + suffixes[suffixes.Length - 1];
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Formats the value to include at most three non-zero digits and at most two digits after the decimal point.
        /// Examples:
        ///         1
        ///       123
        ///        12.3
        ///         1.23
        ///         0.12
        /// </summary>
        /// <param name="value">The double value to format.</param>
        /// <returns>A string representation of the formatted value.</returns>
        private static string ThreeNonZeroDigits(double value)
        {
            if (value >= 100)
            {
                // No digits after the decimal.
                return value.ToString("0,0");
            }
            else if (value >= 10)
            {
                // One digit after the decimal.
                return value.ToString("0.0");
            }
            else
            {
                // Two digits after the decimal.
                return value.ToString("0.00");
            }
        }

        #endregion
    }
}