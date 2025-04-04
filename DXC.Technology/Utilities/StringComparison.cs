using System;
using System.Text;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Non-standard string comparison techniques. Options are LongestStringsFirst, ShortestStringsFirst, and CaseInsensitive.
    /// LongestStringsFirst and ShortestStringsFirst purely check on the length of a string and are useful in string replacement
    /// algorithms e.g. To ensure PARM11 is handled before PARM1 because a PARM1 replacement might also affect PARM11
    /// </summary>
    public enum StringComparisonEnum
    {
        None,
        ShortestStringsFirst = 1,
        LongestStringsFirst = 2,
        CaseInsensitive = 3
    }

    /// <summary>
    /// Compares strings, dependent on certain strategies 
    /// </summary>
    public class StringComparer : System.Collections.IComparer
    {
        #region Instance Fields

        /// <summary>
        /// Strategy for string comparison
        /// </summary>
        private StringComparisonEnum stringComparison { get; set; } = StringComparisonEnum.None;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StringComparer"/> class.
        /// </summary>
        /// <param name="stringComparison">The string comparison strategy.</param>
        public StringComparer(StringComparisonEnum stringComparison)
        {
            this.stringComparison = stringComparison;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares two objects based on the specified strategy.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>An integer indicating the relative order of the objects.</returns>
        public int Compare(object x, object y)
        {
            string firstString = (string)x;
            string secondString = (string)y;

            switch (stringComparison)
            {
                case StringComparisonEnum.LongestStringsFirst:
                    return secondString.Length.CompareTo(firstString.Length);
                case StringComparisonEnum.ShortestStringsFirst:
                    return firstString.Length.CompareTo(secondString.Length);
                case StringComparisonEnum.CaseInsensitive:
                    return string.Compare(secondString, firstString, Utilities.StringComparisonProvider.Default);
                default:
                    return 0;
            }
        }

        #endregion
    }

    /// <summary>
    /// Utility class containing .NET AppDomain related logic
    /// </summary>
    public sealed class DotNetAppDomainHelper
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DotNetAppDomainHelper"/> class from being created.
        /// </summary>
        private DotNetAppDomainHelper() { }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Substitutes a range of pre-established 'variants' within the supplied string.
        /// </summary>
        /// <remarks>
        /// The following 'logical variants' will be taken into account:
        /// - @BASEDIR -> Mapping to AppDomain.BaseDirectory
        /// </remarks>
        /// <param name="input">Input string to do the substitution within.</param>
        /// <returns>Input string with 'variant values' replaced.</returns>
        public static string SubstituteVariants(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("InputString");

            string[] variants = { "@BASEDIR" };

            StringBuilder buffer = new StringBuilder((int)(input.Length * 1.25));
            buffer.Append(input);

            foreach (string variant in variants)
            {
                switch (variant)
                {
                    case "@BASEDIR":
                        buffer.Replace(variant, AppDomain.CurrentDomain.BaseDirectory);
                        break;
                }
            }

            return buffer.ToString();
        }

        #endregion
    }
}