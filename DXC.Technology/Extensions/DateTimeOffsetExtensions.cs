using System;

namespace DXC.Technology.Extensions
{
    /// <summary>
    /// Provides extension methods for DateTimeOffset.
    /// </summary>
    public static class DatetimeOffsetExtensions
    {
        #region Public Static Methods

        /// <summary>
        /// Returns the value of the nullable DateTimeOffset or the minimum relevant value if null.
        /// </summary>
        /// <param name="value">The nullable DateTimeOffset value.</param>
        /// <returns>The value or the minimum relevant value.</returns>
        public static DateTimeOffset GetValueOrMinRelevantValue(this DateTimeOffset? value)
        {
            return value.GetValueOrDefault(MinRelevantValue());
        }

        /// <summary>
        /// Returns the minimum relevant value for DateTimeOffset.
        /// </summary>
        /// <returns>The minimum relevant DateTimeOffset value.</returns>
        public static DateTimeOffset MinRelevantValue()
        {
            return new DateTimeOffset(new DateTime(1900, 1, 1));
        }

        #endregion
    }
}