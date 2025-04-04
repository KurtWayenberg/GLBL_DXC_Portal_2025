using System.Text;
using DXC.Technology.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DXC.Technology.Configuration
{
    #region SomeOtherClass

    public class SomeOtherClass
    {
        #region Instance Fields

        /// <summary>
        /// Provides access to the current HTTP context.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SomeOtherClass"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
        public SomeOtherClass(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        #endregion
    }

    #endregion

    #region LoggingManager

    public class LoggingManager
    {
        #region Static Fields

        /// <summary>
        /// Provides access to the current HTTP context.
        /// </summary>
        private static HttpContext httpContext => new HttpContextAccessor().HttpContext;

        #endregion

        #region Instance Fields

        /// <summary>
        /// Logger instance for file logging.
        /// </summary>
        public ILogger<TraceMessage> FileLogger { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingManager"/> class.
        /// </summary>
        /// <param name="fileLogger">Logger instance for file logging.</param>
        public LoggingManager(ILogger<TraceMessage> fileLogger)
        {
            FileLogger = fileLogger;
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the current instance of <see cref="LoggingManager"/> from the service provider.
        /// </summary>
        public static LoggingManager Current
        {
            get
            {
                return httpContext.RequestServices.GetRequiredService<LoggingManager>();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs a message with the specified log level and event ID.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="eventId">The event ID associated with the log message.</param>
        /// <param name="message">The message to log.</param>
        public void Log(LogLevel level, EventId eventId, string message)
        {
            FileLogger.Log(level, eventId, message);
        }

        #endregion
    }

    #endregion
}