using System;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DXC.Technology.Configuration
{
    public class TelemetryManager
    {
        #region Static Fields

        /// <summary>
        /// Singleton instance of TelemetryManager.
        /// </summary>
        private static TelemetryManager current = null;

        #endregion

        #region Instance Fields

        /// <summary>
        /// Telemetry client used for tracking events.
        /// </summary>
        public TelemetryClient TelemetryClient { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor to initialize the TelemetryManager.
        /// </summary>
        private TelemetryManager()
        {
            TelemetryClient = HttpContext.RequestServices.GetRequiredService<TelemetryClient>();
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the singleton instance of TelemetryManager.
        /// </summary>
        public static TelemetryManager Current
        {
            get
            {
                if (current == null)
                {
                    current = new TelemetryManager();
                }
                return current;
            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Provides access to the current HTTP context.
        /// </summary>
        private static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        #endregion

        #region Public Methods

        /// <summary>
        /// Tracks an event using the telemetry client.
        /// </summary>
        /// <param name="eventName">The name of the event to track.</param>
        public void TrackEvent(string eventName)
        {
            TelemetryClient.TrackEvent(eventName);
        }

        #endregion
    }
}