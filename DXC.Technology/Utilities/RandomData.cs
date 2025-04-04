using System;
using System.Collections.Generic;
using System.IO;
using DXC.Technology.Enumerations;

namespace DXC.Technology.Utilities
{
    public static class RandomData
    {
        #region Static Fields

        /// <summary>
        /// Random instance used for generating random values.
        /// </summary>
        private static readonly Random random = new Random();

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets a random user code from the provided list, possibly blank.
        /// </summary>
        /// <param name="codes">List of codes to choose from.</param>
        /// <returns>A random code from the list.</returns>
        public static string GetRandomUserCodePossiblyBlank(List<string> codes)
        {
            int codeIndex = random.Next(0, codes.Count - 1);
            return codes[codeIndex];
        }

        /// <summary>
        /// Generates a random string with the specified maximum length.
        /// </summary>
        /// <param name="maxLength">Maximum length of the string.</param>
        /// <returns>A random string.</returns>
        public static string GetRandomString(int maxLength)
        {
            return GetRandomString(maxLength, false, false, false);
        }

        /// <summary>
        /// Generates a random name by appending "son" to a random first name.
        /// </summary>
        /// <returns>A random name.</returns>
        public static string GetRandomName()
        {
            return GetRandomFirstName() + "son";
        }

        /// <summary>
        /// Generates a random first name from a predefined list.
        /// </summary>
        /// <returns>A random first name.</returns>
        public static string GetRandomFirstName()
        {
            string[] names = { "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen", "Nancy", "Lisa", "Betty", "Margaret", "Sandra", "Ashley", "Kimberly", "Emily", "Donna", "Michelle", "Dorothy", "Carol", "Amanda", "Melissa", "Deborah", "Stephanie", "James", "Robert", "John", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony", "Mark", "Donald", "Steven", "Paul", "Andrew", "Joshua", "Kenneth", "Kevin", "Brian", "George", "Edward", "Ronald" };
            int rnd = GetRandomNumber(0, names.Length);
            return names[rnd];
        }

        /// <summary>
        /// Generates a random string with specified options.
        /// </summary>
        /// <param name="length">Length of the string.</param>
        /// <param name="includeBlanks">Include blank spaces.</param>
        /// <param name="includeNumbers">Include numbers.</param>
        /// <param name="includeForeignChars">Include foreign characters.</param>
        /// <returns>A random string.</returns>
        public static string GetRandomString(int length, bool includeBlanks, bool includeNumbers, bool includeForeignChars)
        {
            string charsAZ = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string foreignCharsAZ = "ü?èéêìòïöû";
            string numbers = "0123456789";
            var stringWriter = new StringWriter();

            int soFar = 0;
            for (int charsToGo = length; charsToGo > 0; charsToGo--)
            {
                soFar++;
                char charToWrite = charsAZ[random.Next(0, 51)]; // default
                if (((soFar % 4) == 0) && includeBlanks)
                    charToWrite = ' '; // Overwrite with a blank
                if (((soFar % 5) == 0) && includeNumbers)
                    charToWrite = numbers[random.Next(0, 9)]; // Overwrite with a number
                if (((soFar % 7) == 0) && includeForeignChars)
                    charToWrite = foreignCharsAZ[random.Next(0, 9)]; // Overwrite with a foreign char
                stringWriter.Write(charToWrite);
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Generates a random number string with specified options.
        /// </summary>
        /// <param name="length">Length of the number string.</param>
        /// <param name="includeBlanks">Include blank spaces.</param>
        /// <returns>A random number string.</returns>
        public static string GetRandomNumberString(int length, bool includeBlanks)
        {
            string numbers = "0123456789";
            var stringWriter = new StringWriter();

            int soFar = 0;
            for (int charsToGo = length; charsToGo > 0; charsToGo--)
            {
                soFar++;
                char charToWrite = numbers[random.Next(0, 9)]; // default
                if (((soFar % 3) == 0) && includeBlanks)
                    charToWrite = ' '; // Overwrite with a blank

                stringWriter.Write(charToWrite);
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Generates a random IP number with the specified prefix.
        /// </summary>
        /// <param name="prefix">Prefix for the IP number.</param>
        /// <returns>A random IP number.</returns>
        public static string GetRandomIpNumber(string prefix)
        {
            string part1 = prefix;
            if (string.IsNullOrEmpty(part1)) part1 = GetRandomNumberString(3, false);
            return string.Format("{0}.{1}.{2}.{3}", part1, GetRandomNumberString(3, false), GetRandomNumberString(3, false), GetRandomNumberString(3, false));
        }

        /// <summary>
        /// Generates a random number within the specified range.
        /// </summary>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>A random number.</returns>
        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Generates a random decimal within the specified range.
        /// </summary>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>A random decimal.</returns>
        public static decimal GetRandomDecimal(int minValue, int maxValue)
        {
            return Convert.ToDecimal(GetRandomNumber(minValue, maxValue) + 0.56);
        }

        #endregion
    }
}