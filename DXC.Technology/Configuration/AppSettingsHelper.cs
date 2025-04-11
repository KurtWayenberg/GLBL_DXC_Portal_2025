using System;
using Microsoft.Extensions.Configuration;

namespace DXC.Technology.Configuration
{
    /// <summary>
    /// Helper class for managing application settings using IConfiguration.
    /// </summary>
    public sealed class AppSettingsHelper
    {
        #region Static Fields

        /// <summary>
        /// The configuration instance used to retrieve application settings.
        /// </summary>
        private static IConfiguration? configuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor to prevent instantiation of the class.
        /// </summary>
        private AppSettingsHelper()
        {
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Initializes the AppSettingsHelper with a configuration instance.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        public static void Initialize(IConfiguration configuration)
        {
            AppSettingsHelper.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Reads an app setting as a boolean value.
        /// </summary>
        /// <param name="appSettingKey">The key of the app setting.</param>
        /// <returns>Boolean value of the app setting.</returns>
        public static bool GetAsBoolean(string appSettingKey)
        {
            if (configuration == null)
            {
                throw new InvalidOperationException("AppSettingsHelper is not initialized. Call Initialize() with a valid IConfiguration instance.");
            }

            var value = configuration[appSettingKey];
            if (!string.IsNullOrEmpty(value))
            {
                switch (value.ToUpper())
                {
                    case "1":
                    case "T":
                    case "TRUE":
                    case "ON":
                    case "Y":
                    case "YES":
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Reads an app setting as an integer value.
        /// </summary>
        /// <param name="appSettingKey">The key of the app setting.</param>
        /// <param name="defaultValue">The default value to return if the key is not found or invalid.</param>
        /// <returns>Integer value of the app setting.</returns>
        public static int GetAsInteger(string appSettingKey, int defaultValue = 0)
        {
            if (configuration == null)
            {
                throw new InvalidOperationException("AppSettingsHelper is not initialized. Call Initialize() with a valid IConfiguration instance.");
            }

            var value = configuration[appSettingKey];
            if (int.TryParse(value, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Reads an app setting as a string value.
        /// </summary>
        /// <param name="appSettingKey">The key of the app setting.</param>
        /// <returns>String value of the app setting.</returns>
        public static string GetAsString(string appSettingKey)
        {
            if (configuration == null)
            {
                throw new InvalidOperationException("AppSettingsHelper is not initialized. Call Initialize() with a valid IConfiguration instance.");
            }

            return configuration[appSettingKey] ?? string.Empty;
        }

        #endregion
    }
}