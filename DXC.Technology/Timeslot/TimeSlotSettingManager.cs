using System;
using System.Collections.Generic;
using DXC.Technology.Configuration;

namespace DXC.Technology.Timeslot
{
    /// <summary>
    /// Specifies whether a certain TimeslotSetting is active or not
    /// </summary>
    public enum TimeslotSettingModeEnum
    {
        On = 0,
        Off = 1
    }

    /// <summary>
    /// Specifies whether a certain TimeslotSetting is active or not
    /// </summary>
    [Flags]
    public enum TimeslotSettingTypeEnum
    {
        None = 0,
        Search = 1,
        PrintOut = 2,
        OverviewReport = 4,
        ManagementReport = 8,
        FlexibleReport = 16,
        Import = 32,
        Export = 64,
        Report = PrintOut | OverviewReport | ManagementReport | FlexibleReport,
        XML = Import | Export,
        All = Search | Report | XML
    }

    public class TimeslotSettingTypeManager
    {
        #region Instance Fields

        /// <summary>
        /// The type of the timeslot setting.
        /// </summary>
        private TimeslotSettingTypeEnum timeslotSettingType;

        /// <summary>
        /// The date and time associated with the timeslot setting.
        /// </summary>
        private DateTime dateTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeslotSettingTypeManager"/> class.
        /// </summary>
        /// <param name="timeslotSettingType">The type of the timeslot setting.</param>
        /// <param name="dateTime">The date and time associated with the timeslot setting.</param>
        public TimeslotSettingTypeManager(TimeslotSettingTypeEnum timeslotSettingType, DateTime dateTime)
        {
            this.timeslotSettingType = timeslotSettingType;
            this.dateTime = dateTime;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines the setting for a specific timeslot.
        /// </summary>
        /// <param name="timeSpan">The time span of the timeslot.</param>
        /// <param name="identifierCsv">A CSV string of identifiers.</param>
        /// <returns>The setting for the timeslot.</returns>
        public int SettingForTimeslot(TimeSpan timeSpan, string identifierCsv)
        {
            return DXC.Technology.Objects.NullValues.NullInteger;
        }

        #endregion
    }

    public class TimeslotSettingManager
    {
        #region Static Fields

        /// <summary>
        /// The next cache refresh date and time.
        /// </summary>
        private static DateTime nextCacheRefreshDateTime = DateTime.MinValue;

        /// <summary>
        /// Lock for initializing time settings.
        /// </summary>
        private static DXC.Technology.Objects.Lock timeSettingsInitializationLock = new DXC.Technology.Objects.Lock();

        /// <summary>
        /// Cached timeslot managers.
        /// </summary>
        private static Dictionary<TimeslotSettingTypeEnum, TimeslotSettingTypeManager> cachedTimeslotManagers = null;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Determines the setting for a specific timeslot.
        /// </summary>
        /// <param name="timeslotSettingType">The type of the timeslot setting.</param>
        /// <param name="identifierCsv">A CSV string of identifiers.</param>
        /// <returns>The setting for the timeslot.</returns>
        public static int SettingForTimeslot(TimeslotSettingTypeEnum timeslotSettingType, string identifierCsv)
        {
            DateTime now = DateTime.Now;
            return SettingForTimeslot(timeslotSettingType, identifierCsv, now.Date, now.TimeOfDay);
        }

        /// <summary>
        /// Determines the setting for a specific timeslot.
        /// </summary>
        /// <param name="timeslotSettingType">The type of the timeslot setting.</param>
        /// <param name="identifierCsv">A CSV string of identifiers.</param>
        /// <param name="timeSpan">The time span of the timeslot.</param>
        /// <returns>The setting for the timeslot.</returns>
        public static int SettingForTimeslot(TimeslotSettingTypeEnum timeslotSettingType, string identifierCsv, TimeSpan timeSpan)
        {
            return SettingForTimeslot(timeslotSettingType, identifierCsv, DateTime.Now.Date, timeSpan);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines the setting for a specific timeslot.
        /// </summary>
        /// <param name="timeslotSettingType">The type of the timeslot setting.</param>
        /// <param name="identifierCsv">A CSV string of identifiers.</param>
        /// <param name="dateToday">The current date.</param>
        /// <param name="timeSpan">The time span of the timeslot.</param>
        /// <returns>The setting for the timeslot.</returns>
        private static int SettingForTimeslot(TimeslotSettingTypeEnum timeslotSettingType, string identifierCsv, DateTime dateToday, TimeSpan timeSpan)
        {
            lock (timeSettingsInitializationLock)
            {
                if ((dateToday >= nextCacheRefreshDateTime) || (cachedTimeslotManagers == null))
                {
                    cachedTimeslotManagers = new Dictionary<TimeslotSettingTypeEnum, TimeslotSettingTypeManager>();
                    nextCacheRefreshDateTime = dateToday.AddDays(1);
                }
                if (!cachedTimeslotManagers.ContainsKey(timeslotSettingType))
                {
                    cachedTimeslotManagers.Add(timeslotSettingType, new TimeslotSettingTypeManager(timeslotSettingType, dateToday));
                }
            }
            return cachedTimeslotManagers[timeslotSettingType].SettingForTimeslot(timeSpan, identifierCsv);
        }

        #endregion
    }
}