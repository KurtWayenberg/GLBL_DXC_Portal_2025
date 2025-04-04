using System;
using System.Globalization;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Provides utility methods for converting hexadecimal strings to integers and vice versa.
    /// </summary>
    public sealed class Hexadecimal
    {
        #region Constructors

        /// <summary>
        /// Private constructor to prevent instantiation of this static utility class.
        /// </summary>
        private Hexadecimal()
        {
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Converts a hexadecimal string to a 64-bit integer.
        /// </summary>
        /// <param name="hexString">The hexadecimal string to convert.</param>
        /// <returns>The 64-bit integer representation of the hexadecimal string.</returns>
        public static long FromString(string hexString)
        {
            string upperHexString = hexString.ToUpper(CultureInfo.InvariantCulture);
            string[] hexValues = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            long result = 0;
            long baseValue = 1;

            for (int i = upperHexString.Length; i > 0; i--)
            {
                string currentChar = upperHexString.Substring(i - 1, 1);
                long value = Array.IndexOf(hexValues, currentChar);
                result += value * baseValue;
                baseValue *= 16;
            }

            return result;
        }

        /// <summary>
        /// Converts an integer to its hexadecimal string representation.
        /// </summary>
        /// <param name="integerValue">The integer to convert.</param>
        /// <returns>The hexadecimal string representation of the integer.</returns>
        public static string ToString(int integerValue)
        {
            string[] hexValues = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            int hex = integerValue;
            int baseValue = 16;
            int div;
            int rem;
            string result = string.Empty;

            do
            {
                div = Math.DivRem(hex, baseValue, out rem);
                result = hexValues[rem] + result;
                hex = div;
            }
            while (hex > 0);

            return result;
        }

        #endregion
    }
}