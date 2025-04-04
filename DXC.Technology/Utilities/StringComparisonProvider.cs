using System;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Provides string comparison utilities.
    /// </summary>
    public sealed class StringComparisonProvider
    {
        #region Static Fields

        /// <summary>
        /// Gets the default string comparison method.
        /// </summary>
        public static StringComparison Default => StringComparison.CurrentCultureIgnoreCase;

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents instantiation of the class.
        /// </summary>
        private StringComparisonProvider()
        {
        }

        #endregion
    }
}