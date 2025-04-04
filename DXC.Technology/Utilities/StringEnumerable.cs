using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Provides utility methods for working with string enumerables.
    /// </summary>
    public sealed class StringEnumerable
    {
        #region Constructors

        /// <summary>
        /// Private constructor to prevent instantiation of the static class.
        /// </summary>
        private StringEnumerable()
        {
            // Static class only
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Given an array of strings, creates a Comma Separated value from it.
        /// </summary>
        /// <param name="strings">List of strings.</param>
        /// <returns>A CSV string.</returns>
        public static string ToCsvString(IEnumerable<string> strings)
        {
            return ToCsvString(strings, ",");
        }

        /// <summary>
        /// Given an array of strings, creates a 'token' Separated value from it.
        /// </summary>
        /// <param name="strings">List of strings.</param>
        /// <param name="csvSeparator">Token used to separate the value.</param>
        /// <returns>A token-separated string.</returns>
        public static string ToCsvString(IEnumerable<string> strings, string csvSeparator)
        {
            if (strings == null) return string.Empty;
            bool atLeastOne = false;
            using var stringWriter = new System.IO.StringWriter(StringFormatProvider.Default);
            foreach (string str in strings.OrderBy(p => p).ToArray())
            {
                if (atLeastOne)
                    stringWriter.Write(csvSeparator);
                if (!string.IsNullOrEmpty(str))
                {
                    stringWriter.Write(str);
                    atLeastOne = true;
                }
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Converts a CSV string into an enumerable of strings.
        /// </summary>
        /// <param name="csvString">The CSV string.</param>
        /// <returns>An enumerable of strings.</returns>
        public static IEnumerable<string> FromCsvString(string csvString)
        {
            if (string.IsNullOrWhiteSpace(csvString)) return new List<string>();
            return csvString.Split(';', ',', '|').ToList();
        }

        /// <summary>
        /// Trims and removes empty strings from an enumerable of strings.
        /// </summary>
        /// <param name="strings">The enumerable of strings.</param>
        /// <returns>An array of trimmed non-empty strings.</returns>
        public static string[] Trim(IEnumerable<string> strings)
        {
            List<string> nonEmptyValues = new();
            foreach (string element in strings)
            {
                if (!string.IsNullOrEmpty(element)) nonEmptyValues.Add(element);
            }
            return nonEmptyValues.ToArray();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if a value is included in a CSV string.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="stringCsv">The CSV string.</param>
        /// <returns>True if the value is included; otherwise, false.</returns>
        public bool Includes(object value, string stringCsv)
        {
            if (string.IsNullOrWhiteSpace(stringCsv)) return false;
            string valueString = Convert.ToString(value, CultureInfoProvider.Default);
            string[] possibleValues = stringCsv.Split(';', ',', '|', '-');
            foreach (string possibleValue in possibleValues)
            {
                if (string.Compare(valueString.ToUpper(CultureInfoProvider.Default), possibleValue.ToUpper(CultureInfoProvider.Default), StringComparisonProvider.Default) == 0)
                    return true;
            }
            return false;
        }

        #endregion
    }
}