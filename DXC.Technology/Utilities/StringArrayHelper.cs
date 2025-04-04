using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Utility class to do manipulations on string arrays
    /// </summary>
    public static class StringArrayHelper
    {
        #region Public Static Methods

        /// <summary>
        /// Given an array of strings, creates a Comma Separated value from it
        /// </summary>
        /// <param name="strings">List of strings</param>
        /// <returns>A comma-separated string</returns>
        public static string ToCsvString(string[] strings)
        {
            return ToCsvString(strings, ",");
        }

        /// <summary>
        /// Given an array of strings, creates a 'token' Separated value from it
        /// </summary>
        /// <param name="strings">List of strings</param>
        /// <param name="csvSeparator">Token used to separate the value</param>
        /// <returns>A token-separated string</returns>
        public static string ToCsvString(string[] strings, string csvSeparator)
        {
            if (strings == null) return string.Empty;
            bool atLeastOne = false;
            using StringWriter stringWriter = new StringWriter(StringFormatProvider.Default);
            foreach (string str in strings)
            {
                if (atLeastOne)
                    stringWriter.Write(csvSeparator);
                stringWriter.Write(str);
                atLeastOne = true;
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Given an array of strings, creates a Comma Separated value from it
        /// </summary>
        /// <param name="strings">List of strings</param>
        /// <returns>A comma-separated string</returns>
        public static string ToCsvString(StringCollection strings)
        {
            return ToCsvString(strings, ",");
        }

        /// <summary>
        /// Given an array of strings, creates a 'token' Separated value from it
        /// </summary>
        /// <param name="strings">List of strings</param>
        /// <param name="csvSeparator">Token used to separate the value</param>
        /// <returns>A token-separated string</returns>
        public static string ToCsvString(StringCollection strings, string csvSeparator)
        {
            if (strings == null) return string.Empty;
            bool atLeastOne = false;
            using StringWriter stringWriter = new StringWriter(StringFormatProvider.Default);
            foreach (string str in strings)
            {
                if (atLeastOne)
                    stringWriter.Write(csvSeparator);
                stringWriter.Write(str);
                atLeastOne = true;
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Converts a CSV string to a list of type T
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <param name="csvString">The CSV string</param>
        /// <returns>A list of type T</returns>
        public static List<T> ToList<T>(string csvString)
        {
            List<T> result = new List<T>();
            if (!string.IsNullOrEmpty(csvString))
            {
                string[] strings = csvString.Split(';', ',');
                foreach (string element in strings)
                {
                    result.Add((T)Convert.ChangeType(element, typeof(T)));
                }
            }
            return result;
        }

        /// <summary>
        /// Checks if a value is included in a CSV string
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="csvString">The CSV string</param>
        /// <returns>True if the value is included, otherwise false</returns>
        public static bool Includes(object value, string csvString)
        {
            if (csvString == null) return false;
            string valueString = Convert.ToString(value, CultureInfoProvider.Default);
            string[] possibleValues = csvString.Split(';', ',', '|', '-');
            for (int i = 0; i < possibleValues.Length - 1; i++)
            {
                if (string.Compare(valueString.ToUpper(CultureInfoProvider.Default), possibleValues[i].ToUpper(CultureInfoProvider.Default), StringComparisonProvider.Default) == 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Trims an array of strings, removing empty or null values
        /// </summary>
        /// <param name="stringArray">The array of strings</param>
        /// <returns>A trimmed array of strings</returns>
        public static string[] Trim(string[] stringArray)
        {
            List<string> nonEmptyValues = new List<string>();
            foreach (string element in stringArray)
            {
                if (!string.IsNullOrEmpty(element)) nonEmptyValues.Add(element);
            }
            return nonEmptyValues.ToArray();
        }

        #endregion
    }
}