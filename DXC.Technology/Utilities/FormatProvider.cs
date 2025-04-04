using System;
using System.Globalization;
using System.Threading;

namespace DXC.Technology.Utilities
{
    public sealed class CultureInfoProvider
    {
        #region Static Fields

        /// <summary>
        /// Locale for the CultureInfoProvider.
        /// </summary>
        private static readonly CultureInfo cultureInfoProviderLocale = new CultureInfo("en-us");

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureInfoProvider"/> class.
        /// </summary>
        private CultureInfoProvider()
        {
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default culture info for the current thread.
        /// </summary>
        public static CultureInfo Default => Thread.CurrentThread.CurrentUICulture;

        /// <summary>
        /// Gets the system locale for the CultureInfoProvider.
        /// </summary>
        public static CultureInfo SystemLocale => cultureInfoProviderLocale;

        #endregion
    }

    public class StringFormatProvider : IFormatProvider
    {
        #region Static Fields

        /// <summary>
        /// Default instance of StringFormatProvider.
        /// </summary>
        private static readonly StringFormatProvider defaultInstance = new StringFormatProvider();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default instance of StringFormatProvider.
        /// </summary>
        public static StringFormatProvider Default => defaultInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format object for the specified type.
        /// </summary>
        /// <param name="formatType">The type of format object to return.</param>
        /// <returns>The format object, or null if not supported.</returns>
        public object GetFormat(Type formatType)
        {
            return null;
        }

        #endregion
    }

    public class IntFormatProvider : IFormatProvider
    {
        #region Static Fields

        /// <summary>
        /// Default instance of IntFormatProvider.
        /// </summary>
        private static readonly IFormatProvider defaultInstance = CultureInfo.GetCultureInfo("en-US");

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default instance of IntFormatProvider.
        /// </summary>
        public static IFormatProvider Default => defaultInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format object for the specified type.
        /// </summary>
        /// <param name="formatType">The type of format object to return.</param>
        /// <returns>The format object, or null if not supported.</returns>
        public object GetFormat(Type formatType)
        {
            return null;
        }

        #endregion
    }

    public class DateTimeFormatProvider : IFormatProvider
    {
        #region Static Fields

        /// <summary>
        /// Default instance of DateTimeFormatProvider.
        /// </summary>
        private static readonly IFormatProvider defaultInstance = CultureInfo.GetCultureInfo("en-US");

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default instance of DateTimeFormatProvider.
        /// </summary>
        public static IFormatProvider Default => defaultInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format object for the specified type.
        /// </summary>
        /// <param name="formatType">The type of format object to return.</param>
        /// <returns>The format object, or null if not supported.</returns>
        public object GetFormat(Type formatType)
        {
            return null;
        }

        #endregion
    }

    public class DoubleFormatProvider : IFormatProvider
    {
        #region Static Fields

        /// <summary>
        /// Default instance of DoubleFormatProvider.
        /// </summary>
        private static readonly IFormatProvider defaultInstance = CultureInfo.CreateSpecificCulture("en-GB");

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default instance of DoubleFormatProvider.
        /// </summary>
        public static IFormatProvider Default => defaultInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format object for the specified type.
        /// </summary>
        /// <param name="formatType">The type of format object to return.</param>
        /// <returns>The format object, or null if not supported.</returns>
        public object GetFormat(Type formatType)
        {
            return null;
        }

        #endregion
    }

    public class ByteFormatProvider : IFormatProvider
    {
        #region Static Fields

        /// <summary>
        /// Default instance of ByteFormatProvider.
        /// </summary>
        private static readonly IFormatProvider defaultInstance = CultureInfo.GetCultureInfo("en-US");

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default instance of ByteFormatProvider.
        /// </summary>
        public static IFormatProvider Default => defaultInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format object for the specified type.
        /// </summary>
        /// <param name="formatType">The type of format object to return.</param>
        /// <returns>The format object, or null if not supported.</returns>
        public object GetFormat(Type formatType)
        {
            return null;
        }

        #endregion
    }

    public class SingleFormatProvider : IFormatProvider
    {
        #region Static Fields

        /// <summary>
        /// Default instance of SingleFormatProvider.
        /// </summary>
        private static readonly IFormatProvider defaultInstance = CultureInfo.CreateSpecificCulture("en-GB");

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default instance of SingleFormatProvider.
        /// </summary>
        public static IFormatProvider Default => defaultInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format object for the specified type.
        /// </summary>
        /// <param name="formatType">The type of format object to return.</param>
        /// <returns>The format object, or null if not supported.</returns>
        public object GetFormat(Type formatType)
        {
            return null;
        }

        #endregion
    }

    public class DecimalFormatProvider : IFormatProvider
    {
        #region Static Fields

        /// <summary>
        /// Default instance of DecimalFormatProvider.
        /// </summary>
        private static readonly IFormatProvider defaultInstance = CultureInfo.GetCultureInfo("en-US");

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default instance of DecimalFormatProvider.
        /// </summary>
        public static IFormatProvider Default => defaultInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format object for the specified type.
        /// </summary>
        /// <param name="formatType">The type of format object to return.</param>
        /// <returns>The format object, or null if not supported.</returns>
        public object GetFormat(Type formatType)
        {
            return null;
        }

        #endregion
    }

    public class BoolFormatProvider : IFormatProvider
    {
        #region Static Fields

        /// <summary>
        /// Default instance of BoolFormatProvider.
        /// </summary>
        private static readonly BoolFormatProvider defaultInstance = new BoolFormatProvider();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default instance of BoolFormatProvider.
        /// </summary>
        public static BoolFormatProvider Default => defaultInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format object for the specified type.
        /// </summary>
        /// <param name="formatType">The type of format object to return.</param>
        /// <returns>The format object, or null if not supported.</returns>
        public object GetFormat(Type formatType)
        {
            return null;
        }

        #endregion
    }
}