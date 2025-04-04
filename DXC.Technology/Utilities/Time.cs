using System;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Provides utility methods for time manipulation and formatting.
    /// </summary>
    public static class Time
    {
        #region Public Static Methods

        /// <summary>
        /// Returns the current time in HH:mm:ss format.
        /// </summary>
        /// <returns>A string representation of the current time in HH:mm:ss format.</returns>
        public static string FromNow()
        {
            return FromDateTime(DateTime.Now);
        }

        /// <summary>
        /// Converts a DateTime object to a string in HH:mm:ss format.
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert.</param>
        /// <returns>A string representation of the time.</returns>
        public static string FromDateTime(DateTime dateTime)
        {
            return string.Format("{0}:{1}:{2}", dateTime.Hour.ToString().PadLeft(2, '0'), dateTime.Minute.ToString().PadLeft(2, '0'), dateTime.Second.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// Converts a DateTime object to a string in HHmm format.
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert.</param>
        /// <returns>A string representation of the time in HHmm format.</returns>
        public static string HourMinuteSystemStringFromDateTime(DateTime dateTime)
        {
            return string.Format("{0}{1}", dateTime.Hour.ToString().PadLeft(2, '0'), dateTime.Minute.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// Converts a TimeSpan object to a string in HH:mm:ss format.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan object to convert.</param>
        /// <returns>A string representation of the time.</returns>
        public static string FromTimespan(TimeSpan timeSpan)
        {
            return string.Format("{0}:{1}:{2}", timeSpan.Hours.ToString().PadLeft(2, '0'), timeSpan.Minutes.ToString().PadLeft(2, '0'), timeSpan.Seconds.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// Converts a time string in HH:mm:ss:fff format to a DateTime object.
        /// </summary>
        /// <param name="time">The time string to convert.</param>
        /// <returns>A DateTime object representing the time.</returns>
        public static DateTime ToDateTime(string time)
        {
            if (string.IsNullOrEmpty(time))
                return DateTime.Now.Date;

            string[] timeParts = time.Split(':');
            if (timeParts.Length > 4)
                Array.Copy(timeParts, timeParts.Length - 4, timeParts, 0, 4);

            int[] timePartValues = new int[4];
            for (int i = 0; i < timeParts.Length; i++)
                int.TryParse(timeParts[i], out timePartValues[i]);

            return DateTime.Now.Date
                .AddHours(timePartValues[0])
                .AddMinutes(timePartValues[1])
                .AddSeconds(timePartValues[2])
                .AddMilliseconds(timePartValues[3]);
        }

        /// <summary>
        /// Converts a time string in HH:mm:ss:fff format to a nullable TimeSpan object.
        /// </summary>
        /// <param name="timeString">The time string to convert.</param>
        /// <returns>A nullable TimeSpan object representing the time.</returns>
        public static TimeSpan? ToTimeSpan(string timeString)
        {
            int hours = timeString.Length >= 2 ? Convert.ToInt32(timeString.Substring(0, 2)) : 0;
            int minutes = timeString.Length >= 5 ? Convert.ToInt32(timeString.Substring(3, 2)) : 0;
            int seconds = timeString.Length >= 8 ? Convert.ToInt32(timeString.Substring(6, 2)) : 0;
            int milliseconds = timeString.Length >= 11 ? Convert.ToInt32(timeString.Substring(9, 2)) : 0;

            return new TimeSpan(0, hours, minutes, seconds, milliseconds);
        }

        #endregion
    }
}