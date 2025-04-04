using DXC.Technology.Caching;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Date conversions
    /// </summary>
    public sealed class Date
    {

        public const string FirstDayOfWeek = "Monday";

        private Date()
        {
        }

        public static string NullDateString
        {
            get
            {
                return "00010101";
            }
        }

        /// <summary>
        /// Writes DateTime to string format yyyyMMddHHmmssfff	
        /// </summary>
        /// <returns></returns>
        public static string NowYYYYMMDDHHMMSSFFFString()
        {
            return ToYYYYMMDDHHMMSSFFFString(DateTime.Now);
        }
        /// <summary>
        /// Writes DateTime to string format yyyyMMddHHmmssfff	
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToYYYYMMDDHHMMSSFFFString(System.DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmssfff", DXC.Technology.Utilities.StringFormatProvider.Default);
        }

        /// <summary>
        /// Writes Time to string format HHmmssfff	
        /// </summary>
        /// <returns></returns>
        public static string NowHHMMSSFFFString()
        {
            return ToHHMMSSFFFString(DateTime.Now);
        }

        /// <summary>
        /// Writes Time to string format HHmmssfff	
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToHHMMSSFFFString(System.DateTime dateTime)
        {
            return dateTime.ToString("HHmmssfff", DXC.Technology.Utilities.StringFormatProvider.Default);
        }

        /// <summary>
        /// Writes Time to string format HHmmssfff	
        /// </summary>
        /// <returns></returns>
        public static string NowHHMMString()
        {
            return ToHHMMString(DateTime.Now);
        }

        /// <summary>
        /// Writes Time to string format HHmmssfff	
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToHHMMString(System.DateTime dateTime)
        {
            return dateTime.ToString("HHmm", DXC.Technology.Utilities.StringFormatProvider.Default);
        }

        /// <summary>
        /// creates DateTime from inputstring yyyyMMddHHmmssfff
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
        public static DateTime FromYYYYMMDDHHMMSSFFFString(string dateOrDateTime)
        {
            if (string.IsNullOrEmpty(dateOrDateTime)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("DateTime String");

            string lDate = dateOrDateTime.PadRight(17, '0');

            try
            {
                int lYear = int.Parse(lDate.Substring(0, 4), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lMonth = int.Parse(lDate.Substring(4, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lDay = int.Parse(lDate.Substring(6, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lHours = int.Parse(lDate.Substring(8, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lMinutes = int.Parse(lDate.Substring(10, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lSeconds = int.Parse(lDate.Substring(12, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lFraction = int.Parse(lDate.Substring(14, 3), DXC.Technology.Utilities.IntFormatProvider.Default);
                return new DateTime(lYear, lMonth, lDay, lHours, lMinutes, lSeconds, lFraction);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", lDate, "Expected Format Is:" + "YYYYMMDDHHMMSSFFF");
            }
            catch (FormatException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", lDate, "Expected Format Is:" + "YYYYMMDDHHMMSSFFF");
            }

        }

        /// <summary>
        /// Writes DateTime to string format yyyyMMdd	
        /// </summary>
        /// <returns></returns>
        public static string NowYYYYMMDDString()
        {
            return ToYYYYMMDDString(DateTime.Now);
        }

        /// <summary>
        /// Writes DateTime to string format yyyyMMdd	
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToYYYYMMDDString(System.DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd", DXC.Technology.Utilities.IntFormatProvider.Default);
        }

        /// <summary>
        /// creates DateTime from inputstring yyyyMMdd
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FromYYYYMMDDString(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) return System.DateTime.MinValue; // throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("DateTime String");
            string[] dateTimeParts = dateTime.Split('/', '-', ' ');

            if (dateTimeParts.Length >= 3)
            {
                try
                {
                    int lYear = int.Parse(dateTimeParts[0], DXC.Technology.Utilities.IntFormatProvider.Default);
                    int lMonth = int.Parse(dateTimeParts[1], DXC.Technology.Utilities.IntFormatProvider.Default);
                    int lDay = int.Parse(dateTimeParts[2], DXC.Technology.Utilities.IntFormatProvider.Default);
                    return new DateTime(lYear, lMonth, lDay, 0, 0, 0, 0);
                }
                catch (FormatException)
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMMDD");
                }
            }
            else
            {
                try
                {
                    int lYear = int.Parse(dateTime.Substring(0, 4), DXC.Technology.Utilities.IntFormatProvider.Default);
                    int lMonth = int.Parse(dateTime.Substring(4, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                    int lDay = int.Parse(dateTime.Substring(6, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                    return new DateTime(lYear, lMonth, lDay, 0, 0, 0, 0);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMMDD");
                }
                catch (FormatException)
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMMDD");
                }
            }
        }
        /// <summary>
        /// Writes DateTime to string format yyyyMMdd	
        /// </summary>
        /// <returns></returns>
        public static string NowYYYYMMString()
        {
            return ToYYYYMMString(DateTime.Now);
        }

        /// <summary>
        /// Writes DateTime to string format yyyyMMdd	
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToYYYYMMString(System.DateTime dateTime)
        {
            return dateTime.ToString("yyyyMM", DXC.Technology.Utilities.IntFormatProvider.Default);
        }

        /// <summary>
        /// creates DateTime from inputstring yyyyMM - 1st day of the month is taken
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FromYYYYMMString(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("DateTime String");
            try
            {
                int lYear = int.Parse(dateTime.Substring(0, 4), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lMonth = int.Parse(dateTime.Substring(4, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                return new DateTime(lYear, lMonth, 1, 0, 0, 0, 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMM");
            }
            catch (FormatException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMM");
            }
        }


        /// <summary>
        /// Writes DateTime to string format yyyy/MM/dd HH:MM:ss	
        /// </summary>
        /// <returns></returns>
        public static string NowYYYYMMDDHHMMSSUserString()
        {
            return ToYYYYMMDDHHMMSSUserString(DateTime.Now);
        }
        /// <summary>
        /// Writes DateTime to string format yyyy/MM/dd HH:mm:ss	
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToYYYYMMDDHHMMSSUserString(System.DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss", DXC.Technology.Utilities.IntFormatProvider.Default);
        }

        public static DateTime ExtractDateFromString(string fileName)
        {
            DateTime dt = DateTime.MinValue;
            string[] parts = fileName.Split('_', '.', '-');

            //take the last part
            string part = parts[parts.GetUpperBound(0) - 1];
            {
                if (!string.IsNullOrEmpty(part) && part.Length >= 8)
                {
                    try
                    {
                        dt = DXC.Technology.Utilities.Date.FromYYYYMMDDString(part);
                    }
                    catch (Exception)
                    {
                        //Ignore
                    }
                }
            }
            if (dt == DateTime.MinValue)
                dt = DateTime.Now.Date;
            return dt;
        }

        /// <summary>
        /// Writes DateTime to string format yyyy/MM/dd HH:MM:ss.fff	
        /// </summary>
        /// <returns></returns>
        public static string ToYYYYMMDDHHMMSSUserString()
        {
            return ToYYYYMMDDHHMMSSUserString(DateTime.Now);
        }
        /// <summary>
        /// Writes DateTime to string format yyyy/MM/dd HH:mm:ss.fff	
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToYYYYMMDDHHMMSSFFFUserString(System.DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss.fff", DXC.Technology.Utilities.IntFormatProvider.Default);
        }

        /// <summary>
        /// creates DateTime from inputstring yyyy/MM/dd HH:mm:ss.fff
        /// Allows:
        /// 2006/11/26, 2006+11+26, 2006-11-26  (any sep char)
        /// 2006/11/26T12:32:16
        /// 2006/11/26T12:32:16.1
        /// 2006/11/26T12:32:16.12
        /// 2006/11/26T12:32:16.13
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FromYYYYMMDDHHMMSSFFFUserString(string dateOrDateTime)
        {
            if (string.IsNullOrEmpty(dateOrDateTime)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("DateTime String");
            string dateTime = dateOrDateTime + " 00:00:00.000"; //Add DDHHMMSSFFF
            try
            {
                int lYear = int.Parse(dateTime.Substring(0, 4), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lMonth = int.Parse(dateTime.Substring(5, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lDay = int.Parse(dateTime.Substring(8, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lHours = int.Parse(dateTime.Substring(11, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lMinutes = int.Parse(dateTime.Substring(14, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lSeconds = int.Parse(dateTime.Substring(17, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lFraction = int.Parse(dateTime.Substring(20, 3), DXC.Technology.Utilities.IntFormatProvider.Default);
                return new DateTime(lYear, lMonth, lDay, lHours, lMinutes, lSeconds, lFraction);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMMDDHHMMSSFFF");
            }
            catch (FormatException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMMDDHHMMSSFFF");
            }
        }

        /// <summary>
        /// creates DateTime from inputstring YYYY-MM-DDTHH:mm:ss.fff
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FromXMLDateTimeString(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("DateTime String");

            string lRegExp = @"^(?'year'[0-9]{4})[^A-Za-z0-9](?'month'[0-9]{2})[^A-Za-z0-9](?'day'[0-9]{2})(T(?'hours'[0-9]{2}):(?'minutes'[0-9]{2}):(?'seconds'[0-9]{2})(.(?'milliseconds'[0-9]{1,3}))?)?$";
            System.Text.RegularExpressions.Match lMatch = System.Text.RegularExpressions.Regex.Match(dateTime, lRegExp);
            if (lMatch.Success)
            {
                int lYear = Convert.ToInt32(lMatch.Groups["year"].Value, DXC.Technology.Utilities.CultureInfoProvider.Default);
                int lMonth = Convert.ToInt32(lMatch.Groups["month"].Value, DXC.Technology.Utilities.CultureInfoProvider.Default);
                int lDay = Convert.ToInt32(lMatch.Groups["day"].Value, DXC.Technology.Utilities.CultureInfoProvider.Default);
                int lHours = 0;
                if (!string.IsNullOrEmpty(lMatch.Groups["hours"].Value)) lHours = Convert.ToInt32(lMatch.Groups["hours"].Value, DXC.Technology.Utilities.CultureInfoProvider.Default);
                int lMinutes = 0;
                if (!string.IsNullOrEmpty(lMatch.Groups["minutes"].Value)) lMinutes = Convert.ToInt32(lMatch.Groups["minutes"].Value, DXC.Technology.Utilities.CultureInfoProvider.Default);
                int lSeconds = 0;
                if (!string.IsNullOrEmpty(lMatch.Groups["seconds"].Value)) lSeconds = Convert.ToInt32(lMatch.Groups["seconds"].Value, DXC.Technology.Utilities.CultureInfoProvider.Default);
                int lMilliSeconds = 0;
                if (!string.IsNullOrEmpty(lMatch.Groups["milliseconds"].Value)) lMilliSeconds = Convert.ToInt32(lMatch.Groups["milliseconds"].Value, DXC.Technology.Utilities.CultureInfoProvider.Default);

                DateTime lResult = new DateTime(lYear, lMonth, lDay, lHours, lMinutes, lSeconds);
                lResult = lResult.AddMilliseconds(lMilliSeconds);
                return lResult;
            }
            else
            {
                // Try the old way, for backward compatibility
                try
                {
                    return DateTime.Parse(dateTime, DXC.Technology.Utilities.CultureInfoProvider.Default);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYY-MM-DDTHH:MM:SS");
                }
                catch (FormatException)
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYY-MM-DDTHH:MM:SS");
                }
            }
        }

        /// <summary>
        /// converts a DateTime to a string YYYY-MM-DDTHH:mm:ss.fff
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToXMLDateTimeString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/ddTHH:mm:ss", DXC.Technology.Utilities.IntFormatProvider.Default);
        }

        /// <summary>
        /// Writes DateTime to string format yyyy/MM/dd	
        /// </summary>
        /// <returns></returns>
        public static string NowYYYYMMDDUserString()
        {
            return ToYYYYMMDDUserString(DateTime.Now);
        }

        /// <summary>
        /// Writes DateTime to string format yyyy/MM/dd	
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToYYYYMMDDUserString(System.DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd", DXC.Technology.Utilities.IntFormatProvider.Default);
        }

        /// <summary>
        /// creates DateTime from inputstring yyyy/MM/dd
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FromYYYYMMDDUserString(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("DateTime String");
            string[] dateTimeParts = dateTime.Split('/', '-', ' ');

            if (dateTimeParts.Length >= 3)
            {
                try
                {
                    int lYear = int.Parse(dateTimeParts[0], DXC.Technology.Utilities.IntFormatProvider.Default);
                    int lMonth = int.Parse(dateTimeParts[1], DXC.Technology.Utilities.IntFormatProvider.Default);
                    int lDay = int.Parse(dateTimeParts[2], DXC.Technology.Utilities.IntFormatProvider.Default);
                    return new DateTime(lYear, lMonth, lDay, 0, 0, 0, 0);
                }
                catch (FormatException)
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMMDD");
                }
            }
            else
            {
                try
                {
                    int lYear = int.Parse(dateTime.Substring(0, 4), DXC.Technology.Utilities.IntFormatProvider.Default);
                    int lMonth = int.Parse(dateTime.Substring(4, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                    int lDay = int.Parse(dateTime.Substring(6, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                    return new DateTime(lYear, lMonth, lDay, 0, 0, 0, 0);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMMDD");
                }
                catch (FormatException)
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMMDD");
                }
            }
        }

        /// <summary>
        /// Writes DateTime to string format yyyy/MM/dd	
        /// </summary>
        /// <returns></returns>
        public static string NowYYYYMMUserString()
        {
            return ToYYYYMMUserString(DateTime.Now);
        }

        /// <summary>
        /// Writes DateTime to string format yyyy/MM/dd	
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToYYYYMMUserString(System.DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM", DXC.Technology.Utilities.IntFormatProvider.Default);
        }


        /// <summary>
        /// creates DateTime from inputstring yyyy/MM - 1st day of the month is taken
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FromYYYYMMUserString(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("DateTime String");
            try
            {
                int lYear = int.Parse(dateTime.Substring(0, 4), DXC.Technology.Utilities.IntFormatProvider.Default);
                int lMonth = int.Parse(dateTime.Substring(5, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                return new DateTime(lYear, lMonth, 1, 0, 0, 0, 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMM");
            }
            catch (FormatException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YYYYMM");
            }
        }

        /// <summary>
        /// Returns a datetime if provided in the SHORT ISO8601 format, example 2008-04-10T06:30:00
        /// else returns DateTime.MinValue
        /// </summary>
        /// <param name="pDateTimeString"></param>
        /// <returns></returns>
        public static DateTime FromISO8601String(string pDateTimeString)
        {
            DateTime lDateTimeValue = DateTime.MinValue;
            if (DateTime.TryParseExact(pDateTimeString, "s", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out lDateTimeValue))
                return lDateTimeValue;
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// Returns a datetime in the SHORT ISO8601 format, example 2008-04-10T06:30:00
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToISO8601String(DateTime dateTime)
        {
            return dateTime.ToString("s", DXC.Technology.Utilities.CultureInfoProvider.Default);
        }

        public static DateTime Now()
        {
            return DateTime.Now;
        }

        public static DateTime MinValue()
        {
            return DateTime.MinValue;
        }

        public static DateTime MaxValue()
        {
            return DateTime.MaxValue;
        }

        public static bool IsValidBusinessDate(DateTime dateTime)
        {
            return ((dateTime.Year > 1950) && (dateTime.Year < 2090));
        }

        /// <summary>
        /// Writes DateTime to string format yy/dd	
        /// </summary>
        /// <returns></returns>
        public static string NowYYUserString()
        {
            return ToYYUserString(DateTime.Now);
        }

        /// <summary>
        /// Writes DateTime to string format yy/dd	
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToYYUserString(System.DateTime dateTime)
        {
            return dateTime.ToString("yy", DXC.Technology.Utilities.IntFormatProvider.Default);
        }


        /// <summary>
        /// creates DateTime from inputstring yy - 1st day of the month is taken
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FromYYUserString(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("DateTime String");
            try
            {
                int lYear = int.Parse("20" + dateTime.Substring(0, 2), DXC.Technology.Utilities.IntFormatProvider.Default);
                return new DateTime(lYear, 1, 1, 0, 0, 0, 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YY");
            }
            catch (FormatException)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time", dateTime, "Expected Format Is:" + "YY");
            }
        }


        /// <remarks>
        /// Returns the first datetime value of the current month
        /// e.g. 2004/10/01 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfCurrentMonth()
        {
            return FirstDateTimeOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        }

        /// <remarks>
        /// Returns the last datetime value of the current month
        /// e.g. 2004/10/31 23:59:59.999 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfCurrentMonth()
        {
            return LastDateTimeOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        }

        /// <remarks>
        /// Returns the first datetime value of the current month
        /// e.g. 2004/09/01 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfPreviousMonth()
        {
            DateTime ldt = FirstDateTimeOfCurrentMonth();
            ldt = ldt.Subtract(new TimeSpan(1, 0, 0, 0, 0));
            return FirstDateTimeOfMonth(ldt.Year, ldt.Month);
        }

        /// <remarks>
        /// Returns the first datetime value of the next month
        /// e.g. 2004/11/01 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfNextMonth()
        {
            DateTime ldt = LastDateTimeOfCurrentMonth();
            ldt = ldt.Add(new TimeSpan(1, 0, 0, 0, 0));
            return FirstDateTimeOfMonth(ldt.Year, ldt.Month);
        }

        /// <remarks>
        /// Returns the last datetime value of the current month
        /// e.g. 2004/09/30 23:59:59.999 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfPreviousMonth()
        {
            DateTime ldt = FirstDateTimeOfCurrentMonth();
            ldt = ldt.Subtract(new TimeSpan(1, 0, 0, 0, 0));
            return LastDateTimeOfMonth(ldt.Year, ldt.Month);
        }

        /// <remarks>
        /// Returns the last datetime value of the next month
        /// e.g. 2004/11/30 23:59:59.999 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfNextMonth()
        {
            DateTime ldt = FirstDateTimeOfNextMonth();

            return LastDateTimeOfMonth(ldt.Year, ldt.Month);
        }


        /// <remarks>
        /// Returns the first datetime value of the current week
        /// e.g. 2004/10/04 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfCurrentWeek()
        {
            return GetDateTimeRangeForWeek(DateTime.Today).From;
        }

        /// <remarks>
        /// Returns the last datetime value of the current week
        /// e.g. 2004/10/10 23:59:59.999 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfCurrentWeek()
        {
            return GetDateTimeRangeForWeek(DateTime.Today).To;
        }

        /// <remarks>
        /// Returns the first datetime value of the current week
        /// e.g. 2004/10/04 00:00:00.000 if called on 2004/10/14
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfPreviousWeek()
        {
            return GetDateTimeRangeForWeek(DateTime.Today.Subtract(new TimeSpan(7, 0, 0, 0, 0))).From;
        }

        /// <remarks>
        /// Returns the first datetime value of the next week
        /// e.g. 2004/10/21 00:00:00.000 if called on 2004/10/14
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfNextWeek()
        {
            return GetDateTimeRangeForWeek(DateTime.Today.Add(new TimeSpan(7, 0, 0, 0, 0))).From;
        }

        /// <remarks>
        /// Returns the last datetime value of the current week
        /// e.g. 2004/10/10 23:59:59.999 if called on 2004/10/14
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfPreviousWeek()
        {
            return GetDateTimeRangeForWeek(DateTime.Today.Subtract(new TimeSpan(7, 0, 0, 0, 0))).To;
        }

        /// <remarks>
        /// Returns the last datetime value of the next week
        /// e.g. 2004/10/27 23:59:59.999 if called on 2004/10/14
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfNextWeek()
        {
            return GetDateTimeRangeForWeek(DateTime.Today.Add(new TimeSpan(7, 0, 0, 0, 0))).To;
        }

        /// <remarks>
        /// Returns the first datetime value of the current month
        /// e.g. 2004/01/01 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfCurrentYear()
        {
            return FirstDateTimeOfMonth(DateTime.Now.Year, 1);
        }

        /// <remarks>
        /// Returns the last datetime value of the current month
        /// e.g. 2004/12/31 23:59:59.999 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfCurrentYear()
        {
            return FirstDateTimeOfMonth(DateTime.Now.Year, 12);
        }

        /// <remarks>
        /// Returns the first datetime value of the current month
        /// e.g. 2003/01/01 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfPreviousYear()
        {
            return FirstDateTimeOfMonth(DateTime.Now.Year - 1, 1);
        }

        /// <remarks>
        /// Returns the first datetime value of the next month
        /// e.g. 2003/01/01 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfNextYear()
        {
            return FirstDateTimeOfMonth(DateTime.Now.Year + 1, 1);
        }

        /// <remarks>
        /// Returns the last datetime value of the current month
        /// e.g. 2003/12/31 23:59:59.999 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfPreviousYear()
        {
            return FirstDateTimeOfMonth(DateTime.Now.Year - 1, 12);
        }

        /// <remarks>
        /// Returns the last datetime value of the next month
        /// e.g. 2003/12/31 23:59:59.999 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfNextYear()
        {
            return FirstDateTimeOfMonth(DateTime.Now.Year + 1, 12);
        }

        /// <remarks>
        /// Returns the first datetime value of the current month
        /// e.g. 2003/01/01 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfYear(int pYear)
        {
            return FirstDateTimeOfMonth(pYear, 1);
        }

        /// <remarks>
        /// Returns the last datetime value of the current month
        /// e.g. 2003/12/31 23:59:59.999 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfYear(int pYear)
        {
            return LastDateTimeOfMonth(pYear, 12);
        }

        /// <remarks>
        /// Returns the last datetime value of the specified month and year
        /// e.g. FirstDateTimeOfMonth(2004,2) = 2004/02/01 00:00:00.000
        /// </remarks>
        /// <param name="pYear">Year e.g. 2004</param>
        /// <param name="pMonth">Month from 1-12</param>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfMonth(int pYear, int pMonth)
        {
            return new DateTime(pYear, pMonth, 1, 0, 0, 0, 0);
        }

        /// <remarks>
        /// Returns the last datetime value of the specified month and year
        /// e.g. LastDateTimeOfMonth(2004,2) = 2004/02/29 23:59:59.998
        /// milliseconds 998 and not 999 as SQL Server DB rounds 999 to the next second: 998 is rounded to 997
        /// </remarks>
        /// <param name="pYear">Year e.g. 2004</param>
        /// <param name="pMonth">Month from 1-12</param>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfMonth(int pYear, int pMonth)
        {
            return new DateTime(pYear, pMonth, DateTime.DaysInMonth(pYear, pMonth), 23, 59, 59, 998);
        }


        /// <remarks>
        /// Returns the first datetime value of the week in which the specified datetime falls
        /// e.g. 2004/10/04 00:00:00.000 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfWeek(DateTime dateTime)
        {
            return GetDateTimeRangeForWeek(dateTime).From;
        }

        /// <remarks>
        /// Returns the last datetime value of the week in which the specified datetime falls
        /// e.g. 2004/10/10 23:59:59.998 if called on 2004/10/07
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfWeek(DateTime dateTime)
        {
            return GetDateTimeRangeForWeek(dateTime).To;
        }

        /// <remarks>
        /// Returns the beginning datetime value of the specified day
        /// e.g. FirstDateTimeOfDay(2004/02/14) = 2004/02/14 00:00:00.000
        /// </remarks>
        /// <param name="dateTime">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime FirstDateTimeOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        /// <remarks>
        /// Returns the beginning datetime value of the specified day
        /// e.g. NoonDateTimeOfDay(2004/02/14) = 2004/02/14 12:00:00.000
        /// </remarks>
        /// <param name="dateTime">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime NoonDateTimeOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 0, 0, 0);
        }


        /// <remarks>
        /// Returns the last datetime value of the specified day
        /// e.g. LastDateTimeOfDay(2004/02/14) = 2004/02/14 23:59:59.998
        /// milliseconds 998 and not 999 as SQL Server DB rounds 999 to the next second: 998 is rounded to 997
        /// </remarks>
        /// <param name="dateTime">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime LastDateTimeOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 998);
        }

        /// <remarks>
        /// Returns the last datetime value usable in SQL for business applications
        /// e.g. SQLMaximalDateTime() = 9998/12/31 23:59:59.998
        /// milliseconds 998 and not 999 as SQL Server DB rounds 999 to the next second: 998 is rounded to 997
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime SQLMaximalDateTime()
        {
            return new DateTime(9999, 12, 31, 23, 59, 59, 998);
        }

        /// <remarks>
        /// Return Minimal datatime value used within SQL Server
        /// SqlMinimalDateTime() = 1753/1/1 23:59:59.998
        /// </remarks>
        /// <returns>DateTime</returns>
        public static DateTime SqlMinimalDateTime()
        {
            return new DateTime(1753, 1, 1, 0, 0, 0, 0);
        }

        public static bool isValidSQLDateTime(DateTime pDataValue)
        {
            if (pDataValue != DXC.Technology.Objects.NullValues.NullDateTime)
            {
                if (pDataValue < DXC.Technology.Utilities.Date.SqlMinimalDateTime() || pDataValue > DXC.Technology.Utilities.Date.SQLMaximalDateTime())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the number of days since the end of the first date and
        /// the beginning of the second date...
        /// </summary>
        /// <param name="pStartDate">Start Date</param>
        /// <param name="pEndDate">End Date</param>
        /// <returns></returns>
        public static int NumberOfDays(DateTime pStartDate, DateTime pEndDate)
        {
            System.TimeSpan lts = DXC.Technology.Utilities.Date.LastDateTimeOfDay(pEndDate).Subtract(
                DXC.Technology.Utilities.Date.FirstDateTimeOfDay(pStartDate));
            return lts.Days;
        }
        /// <remarks>
        /// Returns the number of months between two dates
        /// e.g. NumberOfMonths(10/10/4,20/10/4) = 1
        /// e.g. NumberOfMonths(21/9/4,20/10/4) = 1
        /// e.g. NumberOfMonths(20/9/4,20/10/4) = 2
        /// e.g. NumberOfMonths(21/11/3,20/10/4) = 11
        /// e.g. NumberOfMonths(21/10/3,20/10/4) = 12
        /// e.g. NumberOfMonths(20/10/3,20/10/4) = 13
        /// e.g. NumberOfMonths(20/9/3,20/10/4) = 14
        ///  
        /// </remarks>
        /// <param name="pDateTimeLow">DateTime 1</param>
        /// <param name="pDateTimeHigh">DateTime 2</param>
        /// <returns></returns>
        public static int NumberOfMonths(DateTime pDateTimeLow, DateTime pDateTimeHigh)
        {
            if (pDateTimeLow > pDateTimeHigh) return -NumberOfMonths(pDateTimeHigh, pDateTimeLow);

            int lNumberOfYears = pDateTimeHigh.Year - pDateTimeLow.Year;

            if (pDateTimeLow.Month == pDateTimeHigh.Month)
            {
                if (pDateTimeLow.Day > pDateTimeHigh.Day)
                    return lNumberOfYears * 12;
                else
                    return lNumberOfYears * 12 + 1;
            }

            if (pDateTimeLow.Month < pDateTimeHigh.Month)
            {
                int lNumberOfMonths = pDateTimeHigh.Month - pDateTimeLow.Month;
                if (pDateTimeLow.Day > pDateTimeHigh.Day)
                    return lNumberOfYears * 12 + lNumberOfMonths;
                else
                    return lNumberOfYears * 12 + 1 + lNumberOfMonths;
            }

            if (pDateTimeLow.Month > pDateTimeHigh.Month)
            {
                int lNumberOfMonths = pDateTimeLow.Month - pDateTimeHigh.Month;
                if (pDateTimeLow.Day > pDateTimeHigh.Day)
                    return lNumberOfYears * 12 - lNumberOfMonths;
                else
                    return lNumberOfYears * 12 - lNumberOfMonths + 1;
            }

            throw new DXC.Technology.Exceptions.NamedExceptions.TechnicalException("Should Never Happen");
        }

        /// <remarks>
        /// Returns the number of years between two dates
        /// e.g. NumberOfYears(01/01/2004,01/01/2006) = 2
        /// e.g. NumberOfYears(01/01/2004,02/01/2006) = 2
        /// e.g. NumberOfYears(02/01/2004,01/02/2006) = 1
        ///  
        /// </remarks>
        /// <param name="pDateTimeLow">DateTime 1</param>
        /// <param name="pDateTimeHigh">DateTime 2</param>
        /// <returns></returns>
        public static int NumberOfYears(DateTime pDateTimeLow, DateTime pDateTimeHigh)
        {
            if (pDateTimeLow > pDateTimeHigh) return -NumberOfYears(pDateTimeHigh, pDateTimeLow);

            int lNumberOfYears = pDateTimeHigh.Year - pDateTimeLow.Year;

            if ((pDateTimeLow.Month > pDateTimeHigh.Month)
               || ((pDateTimeLow.Month == pDateTimeHigh.Month) && (pDateTimeLow.Day > pDateTimeHigh.Day)))
                return lNumberOfYears - 1;
            else
                return lNumberOfYears;
        }

        /// <remarks>
        /// Returns the number of years between two dates
        /// e.g. NumberOfYears(15/01/2006,27/01/2004) = 1.9
        /// e.g. NumberOfYears(27/01/2006,27/01/2004) = 2
        ///  
        /// </remarks>
        /// <param name="pDateTimeLow">DateTime 1</param>
        /// <param name="pDateTimeHigh">DateTime 2</param>
        /// <returns></returns>
        public static double NumberOfYearsAsDouble(DateTime pDateTimeLow, DateTime pDateTimeHigh)
        {
            if (pDateTimeLow > pDateTimeHigh) return -NumberOfYearsAsDouble(pDateTimeHigh, pDateTimeLow);

            double lNumberOfYears;
            lNumberOfYears = ((double)new TimeSpan(pDateTimeHigh.Subtract(pDateTimeLow).Ticks).Days / 365.25);

            return lNumberOfYears;
        }

        /// <remarks>
        /// Returns the number of years between two dates
        /// e.g. NumberOfYearsExactAsDouble(15/01/2006,27/01/2004) = 1.9
        /// e.g. NumberOfYearsExactAsDouble(27/01/2006,27/01/2004) = 2
        ///  
        /// </remarks>
        /// <param name="pDateTimeLow">DateTime 1</param>
        /// <param name="pDateTimeHigh">DateTime 2</param>
        /// <returns></returns>
        public static double NumberOfYearsExactAsDouble(DateTime pDateTimeLow, DateTime pDateTimeHigh)
        {
            if (pDateTimeLow > pDateTimeHigh) return -NumberOfYearsExactAsDouble(pDateTimeHigh, pDateTimeLow);


            double years = pDateTimeHigh.Year - pDateTimeLow.Year;
            /** If pDateTimeHigh and pDateTimeLow + round(pDateTimeHigh - pDateTimeLow) are on different sides     
            * * of 29 February, then our partial year is considered to have 366     
            * * days total, otherwise it's 365. Note that 59 is the day number     
            * * of 29 Feb.     */
            double fraction = 365 + (DateTime.IsLeapYear(pDateTimeHigh.Year) && pDateTimeHigh.DayOfYear >= 59 && (pDateTimeLow.DayOfYear < 59 || pDateTimeLow.DayOfYear > pDateTimeHigh.DayOfYear) ? 1 : 0);
            /** The only really nontrivial case is if pDateTimeLow is in a leap year,    
             ** and pDateTimeHigh is not. So let's handle the others first.     */
            if (DateTime.IsLeapYear(pDateTimeHigh.Year) == DateTime.IsLeapYear(pDateTimeLow.Year))
                return years + (pDateTimeHigh.DayOfYear - pDateTimeLow.DayOfYear) / fraction;
            /** If pDateTimeHigh is in a leap year, but pDateTimeLow is not and is March or    
             ** beyond, shift up by a day.     */
            if (DateTime.IsLeapYear(pDateTimeHigh.Year)) { return years + (pDateTimeHigh.DayOfYear - pDateTimeLow.DayOfYear - (pDateTimeLow.DayOfYear >= 59 ? 1 : 0)) / fraction; }
            /** If pDateTimeLow is not on 29 February, shift down pDateTimeLow by a day if     
             ** March or later. Proceed normally.     */
            if (pDateTimeLow.DayOfYear != 59) { return years + (pDateTimeHigh.DayOfYear - pDateTimeLow.DayOfYear + (pDateTimeLow.DayOfYear > 59 ? 1 : 0)) / fraction; }
            /** Okay, here pDateTimeLow is on 29 February, and pDateTimeHigh is not on a leap     
             ** year. What to do now? On 28 Feb in pDateTimeHigh's year, the ``age''     
             ** should be just shy of a whole number, and on 1 Mar should be   
             ** just over. Perhaps the easiest way is to a point halfway    
             ** between those two: 58.5.     */
            return years + (pDateTimeHigh.DayOfYear - 58.5) / fraction;
        }
        /// <summary>
        /// Gets the monday of the first week of that year. This first monday basically can lay in the previous year...
        /// </summary>
        /// <param name="pYear">Year</param>
        /// <returns></returns>
        public static DateTime GetMondayOfFirstWeek(int pYear)
        {
            DateTime lFirstDayOfPhysicalYear = DXC.Technology.Utilities.Date.FirstDateTimeOfYear(pYear);
            DateTime lFirstDayOfLogicalYear;
            System.DayOfWeek lDayOfWeeklFirstDayOfPhysicalYear = lFirstDayOfPhysicalYear.DayOfWeek;
            switch (lDayOfWeeklFirstDayOfPhysicalYear)
            {
                case DayOfWeek.Monday:
                    lFirstDayOfLogicalYear = lFirstDayOfPhysicalYear;
                    break;
                case DayOfWeek.Tuesday:
                    lFirstDayOfLogicalYear = lFirstDayOfPhysicalYear.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    lFirstDayOfLogicalYear = lFirstDayOfPhysicalYear.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    lFirstDayOfLogicalYear = lFirstDayOfPhysicalYear.AddDays(-3);
                    break;
                //Up till here we know the first of the physical year lies in the logical year too
                //From here on we know we the first days of the year are in week 53...
                case DayOfWeek.Friday:
                    lFirstDayOfLogicalYear = lFirstDayOfPhysicalYear.AddDays(3);
                    break;
                case DayOfWeek.Saturday:
                    lFirstDayOfLogicalYear = lFirstDayOfPhysicalYear.AddDays(2);
                    break;
                case DayOfWeek.Sunday:
                    lFirstDayOfLogicalYear = lFirstDayOfPhysicalYear.AddDays(1);
                    break;
                default:
                    throw new DXC.Technology.Exceptions.NamedExceptions.UnexpectedException("Should Never Happen");
            }
            return lFirstDayOfLogicalYear;
        }


        public static DateTimeRange GetDateTimeRangeForWeek(int pYear, int pWeekNr)
        {
            if (pWeekNr <= 0) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Week", pWeekNr, 1, 53);
            //54 is allowed -- you never know -> but you do not want to inform that
            if (pWeekNr > 54) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Week", pWeekNr, 1, 53);
            DateTime lStartingMonday;
            DateTime ldtFirstMonday = DXC.Technology.Utilities.Date.GetMondayOfFirstWeek(pYear);
            lStartingMonday = ldtFirstMonday.AddDays((pWeekNr - 1) * 7);
            DateTime lEndingFriday = lStartingMonday.AddDays(6);
            return new DateTimeRange(lStartingMonday, DXC.Technology.Utilities.Date.LastDateTimeOfDay(lEndingFriday));
        }

        public static DateTimeRange GetDateTimeRangeForWeek(DateTime dateTime)
        {
            YearWeekNumber lywn = GetYearWeekNumber(dateTime);
            return GetDateTimeRangeForWeek(lywn.Year, lywn.Week);
        }

        public static DateTimeRange GetDateTimeRangeForMonth(int pYear, int pMonth)
        {
            return new DateTimeRange(DXC.Technology.Utilities.Date.FirstDateTimeOfMonth(pYear, pMonth), DXC.Technology.Utilities.Date.LastDateTimeOfMonth(pYear, pMonth));
        }

        public static DateTimeRange GetDateTimeRangeForMonth(DateTime dateTime)
        {
            return GetDateTimeRangeForMonth(dateTime.Year, dateTime.Month);
        }


        /// <summary>
        /// Returns the week number of the specified date. Weeks are considered starting on a sunday and the first week is considered
        /// to have at least four days in the specified year. (ISO8601)
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns></returns>
        public static YearWeekNumber GetYearWeekNumber(DateTime dateTime)
        {
            DateTime lFirstDayOfLogicalYear = GetMondayOfFirstWeek(dateTime.Year);
            DateTime lFirstDayOfNextLogicalYear = GetMondayOfFirstWeek(dateTime.Year + 1);
            if (dateTime < lFirstDayOfLogicalYear)
            {
                return new YearWeekNumber(dateTime.Year - 1, 53); //It must be the last week of previous year!
            }
            else
            {
                if (dateTime >= lFirstDayOfNextLogicalYear)
                {
                    return new YearWeekNumber(dateTime.Year + 1, 1); //It must be the first week of next year!
                }
                else
                {
                    TimeSpan lDiff = dateTime.Subtract(lFirstDayOfLogicalYear);
                    return new YearWeekNumber(dateTime.Year, Convert.ToInt16(Math.Floor(lDiff.Days / 7.0) + 1, DXC.Technology.Utilities.IntFormatProvider.Default)); //It must be the datetime of previous year!
                }
            }
        }



        /// <summary>
        /// Returns the week number of the week preceding the specified date. Weeks are considered starting on a sunday and the first week is considered
        /// to have at least four days in the specified year. (ISO8601)
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns></returns>
        public static YearWeekNumber GetPreviousYearWeekNumber(DateTime dateTime)
        {
            return GetYearWeekNumber(dateTime.Subtract(new TimeSpan(7, 0, 0, 0, 0)));
        }

        /// <summary>
        /// Returns the current week number. Weeks are considered starting on a sunday and the first week is considered
        /// to have at least four days in the specified year. (ISO8601)
        /// </summary>
        /// <returns></returns>
        public static YearWeekNumber GetYearWeekNumber()
        {
            return GetYearWeekNumber(DateTime.Now);
        }

        public static YearMonthNumber[] GetLastCompleteYearMonthNumbers(int noOfMonths)
        {

            DateTime startDate = FirstDateTimeOfPreviousMonth().AddMonths(-noOfMonths + 1);
            return GetYearMonthNumbers(startDate, noOfMonths);
        }
        public static YearMonthNumber[] GetYearMonthNumbers(YearMonthNumber startingYearMonthNumber, int noOfMonths)
        {
            return GetYearMonthNumbers(startingYearMonthNumber.ToDateTime(), noOfMonths);
        }
        public static YearMonthNumber[] GetYearMonthNumbers(DateTime startDate, int noOfMonths)
        {
            List<YearMonthNumber> result = new List<YearMonthNumber>();
            for (int i = 0; i < noOfMonths; i++)
            {
                DateTime dateToAdd = startDate.AddMonths(i);
                result.Add(new YearMonthNumber(dateToAdd.Year, dateToAdd.Month));
            }
            return result.ToArray();

        }
        public static string ServerShortDateTimePattern()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern + " " + System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortTimePattern;
        }

        public static DateTime FromParameter(string pParameter)
        {
            if (string.IsNullOrEmpty(pParameter))
                return DXC.Technology.Objects.NullValues.NullDateTime;
            switch (pParameter)
            {
                case "@Today": return DateTime.Today;
                case "@Now": return DateTime.Now;
                case "@FirstDayOfCurrentMonth": return DXC.Technology.Utilities.Date.FirstDateTimeOfCurrentMonth();
                case "@LastDayOfCurrentMonth": return DXC.Technology.Utilities.Date.LastDateTimeOfCurrentMonth();
                case "@FirstDayOfPreviousMonth": return DXC.Technology.Utilities.Date.FirstDateTimeOfPreviousMonth();
                case "@LastDayOfPreviousMonth": return DXC.Technology.Utilities.Date.LastDateTimeOfPreviousMonth();
                case "@FirstDayOfCurrentYear": return DXC.Technology.Utilities.Date.FirstDateTimeOfCurrentYear();
                case "@LastDayOfCurrentYear": return DXC.Technology.Utilities.Date.LastDateTimeOfCurrentYear();
                case "@FirstDayOfPreviousYear": return DXC.Technology.Utilities.Date.FirstDateTimeOfPreviousYear();
                case "@LastDayOfPreviousYear": return DXC.Technology.Utilities.Date.LastDateTimeOfPreviousYear();
                case "@FirstDateTimeOfCurrentMonth": return DXC.Technology.Utilities.Date.FirstDateTimeOfCurrentMonth();
                case "@LastDateTimeOfCurrentMonth": return DXC.Technology.Utilities.Date.LastDateTimeOfCurrentMonth();
                case "@FirstDateTimeOfPreviousMonth": return DXC.Technology.Utilities.Date.FirstDateTimeOfPreviousMonth();
                case "@LastDateTimeOfPreviousMonth": return DXC.Technology.Utilities.Date.LastDateTimeOfPreviousMonth();
                case "@FirstDateTimeOfCurrentYear": return DXC.Technology.Utilities.Date.FirstDateTimeOfCurrentYear();
                case "@LastDateTimeOfCurrentYear": return DXC.Technology.Utilities.Date.LastDateTimeOfCurrentYear();
                case "@FirstDateTimeOfPreviousYear": return DXC.Technology.Utilities.Date.FirstDateTimeOfPreviousYear();
                case "@LastDateTimeOfPreviousYear": return DXC.Technology.Utilities.Date.LastDateTimeOfPreviousYear();
                default:
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Date Time @Parameter", pParameter, "");
            }
        }


        public static DateTime FromParameterOrYYYYMMDDHHMMSSString(string pParameter)
        {
            if (string.IsNullOrEmpty(pParameter))
                return DXC.Technology.Objects.NullValues.NullDateTime;
            if (pParameter.StartsWith("@", Utilities.StringComparisonProvider.Default))
                return FromParameter(pParameter);
            else
                return FromYYYYMMDDHHMMSSFFFString(pParameter);
        }

        public static bool IsNullDateTime(DateTime dateTime)
        {
            if (dateTime.Equals(DXC.Technology.Objects.NullValues.NullDateTime))
            {
                return true;
            }
            else
            {
                //TODO KWA 
                if (dateTime.Year <= 1)
                {
                    System.Diagnostics.Debug.Write("Probalby setting / getting your null date time is not correct");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static System.DateTime AddWorkingDaysToDate(DateTime pDate, int pNumberOfDaysToAdd, params DateTime[] pHolidays)
        {
            DateTime lReturnDate = pDate;
            int lNumberOfDaysToAdd = pNumberOfDaysToAdd;
            if (lNumberOfDaysToAdd < 0)
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Number of days to add", lNumberOfDaysToAdd, "Must be greater than 0");

            while (lNumberOfDaysToAdd > 0)
            {
                lReturnDate = lReturnDate.AddDays(1);
                switch (lReturnDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        if (pHolidays == null || Array.IndexOf(pHolidays, lReturnDate) < 0)
                        {
                            lNumberOfDaysToAdd--;
                        }
                        break;
                }
            }

            return lReturnDate;
        }

        public static System.DateTime SubtractWorkingDaysFromDate(DateTime pDate, int pNumberOfDaysToSubtract, params DateTime[] pHolidays)
        {
            DateTime lReturnDate = pDate;
            int lNumberOfDaysToSubtract = pNumberOfDaysToSubtract;
            if (lNumberOfDaysToSubtract <= 0)
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Number of days to add", lNumberOfDaysToSubtract, "Must be greater than 0");

            while (lNumberOfDaysToSubtract > 0)
            {
                lReturnDate = lReturnDate.AddDays(-1);
                switch (lReturnDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        if (pHolidays == null || Array.IndexOf(pHolidays, lReturnDate) < 0)
                        {
                            lNumberOfDaysToSubtract--;
                        }
                        break;
                }
            }

            return lReturnDate;
        }

        public static DateTime ToSqlServerDbDateTime(DateTime dateTime)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Millisecond
                );
        }
    }


    public class DateTimeRange
    {
        private DateTime iFrom;
        private DateTime iTo;


        public DateTime From
        {
            get
            {
                return iFrom;
            }
            set
            {
                iFrom = value;
            }
        }

        public DateTime To
        {
            get
            {
                return iTo;
            }
            set
            {
                iTo = value;
            }
        }

        public DateTimeRange(DateTime pFrom, DateTime pTo)
        {
            From = DXC.Technology.Utilities.Date.FirstDateTimeOfDay(pFrom);
            To = DXC.Technology.Utilities.Date.LastDateTimeOfDay(pTo);
        }

        public override string ToString()
        {
            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}-{1}", From.ToShortDateString(), To.ToShortDateString());
        }
        public string ToYYYYMMDDHHMMSSFFFDateTimeRangeString()
        {
            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}-{1}", DXC.Technology.Utilities.Date.ToYYYYMMDDHHMMSSFFFString(From), DXC.Technology.Utilities.Date.ToYYYYMMDDHHMMSSFFFString(To));
        }
        public static DateTimeRange FromYYYYMMDDHHMMSSFFFDateTimeRangeString(string pDateTimeRange)
        {
            if (pDateTimeRange == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Date Time Range");
            string[] lDateTimeRangeParts = pDateTimeRange.Split('-');
            return new DateTimeRange(DXC.Technology.Utilities.Date.FromYYYYMMDDHHMMSSFFFString(lDateTimeRangeParts[0]),
                DXC.Technology.Utilities.Date.FromYYYYMMDDHHMMSSFFFString(lDateTimeRangeParts[1]));
        }

        public static bool IsValidYYYYMMDDHHMMSSFFFDateTimeRangeString(string pDateTimeRange)
        {
            try
            {
                FromYYYYMMDDHHMMSSFFFDateTimeRangeString(pDateTimeRange);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public class YearMonthNumber
    {
        private int iYear;
        private int iMonth;
        private string iSelectedFlag;

        public int Year
        {
            get
            {
                return iYear;
            }
            set
            {
                iYear = value;
            }
        }
        public int Month
        {
            get
            {
                return  iMonth;
            }
            set
            {
                iMonth = value;
            }
        }

        public string MonthName
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(iMonth);
            }
        }

        public string YearMonthCode
        {
            get
            {
                return iYear.ToString("0000") + "-" + iMonth.ToString().PadLeft(2, '0');
            }
            set
            {
                iMonth = Convert.ToInt32(value.Split("-")[1]);
                iYear = Convert.ToInt32(value.Split("-")[0]);
            }
        }
        public string SelectedFlag
        {
            get
            {
                return iSelectedFlag;
            }
            set
            {
                iSelectedFlag = value;
            }
        }

        public YearMonthNumber(int pYear, int pMonth)
        {
            this.SelectedFlag = "X";
            Year = pYear;
            Month = pMonth;
            //			if (pYear < 1980) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Year", pYear, 1945, 2100);
            //			if (pYear > 2100) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Year", pYear, 1945, 2100);
            //			if (pWeek < 1) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Week", pWeek, 1, 53);
            //			if (pWeek > 53) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Week", pWeek, 1, 53); 
        }

        public YearMonthNumber(string yearMonthCode)
        {
            this.SelectedFlag = "X";
            this.YearMonthCode = yearMonthCode;
        }

        public YearMonthNumber()
        {
            this.SelectedFlag = "X";
        }



        public override string ToString()
        {
            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}-{1}", Year, Month.ToString().PadLeft(2, '0'));
        }

        public static YearMonthNumber FromString(string pYearMonthNumber)
        {
            if (string.IsNullOrEmpty(pYearMonthNumber)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Year Month Number");

            string[] lYearMonthNumberParts = pYearMonthNumber.Split('-');
            return new YearMonthNumber(int.Parse(lYearMonthNumberParts[0], DXC.Technology.Utilities.IntFormatProvider.Default),
                                      int.Parse(lYearMonthNumberParts[1], DXC.Technology.Utilities.IntFormatProvider.Default));
        }
        public DateTime ToDateTime()
        {
            return new DateTime(iYear, iMonth, 1);
        }

    }

    public class YearWeekNumber
    {
        private int iYear;
        private int iWeek;

        public int Year
        {
            get
            {
                return iYear;
            }
            set
            {
                iYear = value;
            }
        }
        public int Week
        {
            get
            {
                return iWeek;
            }
            set
            {
                iWeek = value;
            }
        }

        public YearWeekNumber(int pYear, int pWeek)
        {
            Year = pYear;
            Week = pWeek;
            //			if (pYear < 1980) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Year", pYear, 1945, 2100);
            //			if (pYear > 2100) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Year", pYear, 1945, 2100);
            //			if (pWeek < 1) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Week", pWeek, 1, 53);
            //			if (pWeek > 53) throw new DXC.Technology.Exceptions.NamedExceptions.ParameterOutOfRangeException("Week", pWeek, 1, 53); 
        }



        public override string ToString()
        {
            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}-{1}", Year, Week);
        }

        public static YearWeekNumber FromString(string pYearWeekNumber)
        {
            if (string.IsNullOrEmpty(pYearWeekNumber)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Year Week Number");

            string[] lYearWeekNumberParts = pYearWeekNumber.Split('-');
            return new YearWeekNumber(int.Parse(lYearWeekNumberParts[0], DXC.Technology.Utilities.IntFormatProvider.Default),
                                      int.Parse(lYearWeekNumberParts[1], DXC.Technology.Utilities.IntFormatProvider.Default));
        }
    }
}