using System;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Provides utility methods for converting between hexadecimal strings and byte arrays.
    /// </summary>
    public static class Hex
    {
        #region Public Static Methods

        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// </summary>
        /// <param name="hexString">The hexadecimal string to convert.</param>
        /// <returns>A byte array representing the hexadecimal string.</returns>
        public static byte[] HexStringToBytes(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hexString));
            }

            return Convert.FromHexString(hexString);
        }

        /// <summary>
        /// Converts a byte array to a hexadecimal string.
        /// </summary>
        /// <param name="byteArray">The byte array to convert.</param>
        /// <returns>A hexadecimal string representing the byte array.</returns>
        public static string BytesToHexString(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(byteArray));
            }

            return Convert.ToHexString(byteArray);
        }

        #endregion
    }
}