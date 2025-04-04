using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DXC.Technology.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Serilog;

namespace DXC.Technology.Logging
{
    #region Public Static Classes
    /// <summary>
    /// Provides extension methods for adding Technology File Logger to the logging builder.
    /// </summary>
    public static class TechnologyFileLoggerExtensions
    {
        #region Public Static Methods
        /// <summary>
        /// Adds the Technology File Logger to the logging builder.
        /// </summary>
        /// <param name="builder">The logging builder.</param>
        /// <param name="configure">The configuration action for the logger options.</param>
        /// <returns>The updated logging builder.</returns>
        public static ILoggingBuilder AddTechnologyFileLogger(this ILoggingBuilder builder, Action<TechnologyFileLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, TechnologyFileLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// Represents a logger that logs messages to a file and Application Insights.
    /// </summary>
    public class TechnologyFileLogger : Microsoft.Extensions.Logging.ILogger
    {
        #region Protected Fields
        /// <summary>
        /// Provider for the Technology File Logger.
        /// </summary>
        protected readonly TechnologyFileLoggerProvider technologyFileLoggerProvider;

        /// <summary>
        /// Logger for Application Insights.
        /// </summary>
        protected readonly Serilog.Core.Logger applicationInsightsLogger;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TechnologyFileLogger"/> class.
        /// </summary>
        /// <param name="technologyFileProvider">The provider for the Technology File Logger.</param>
        public TechnologyFileLogger([NotNull] TechnologyFileLoggerProvider technologyFileProvider)
        {
            technologyFileLoggerProvider = technologyFileProvider;
            try
            {
                applicationInsightsLogger = new Serilog.LoggerConfiguration().MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                                       .Enrich.FromLogContext()
                                       .WriteTo.ApplicationInsights(Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.CreateDefault(), Serilog.TelemetryConverter.Traces)
                                       .CreateLogger();
            }
            catch
            {
                // Ignore
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to associate with the scope.</typeparam>
        /// <param name="state">The state to associate with the scope.</param>
        /// <returns>A disposable object that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Checks if the given log level is enabled.
        /// </summary>
        /// <param name="logLevel">The log level to check.</param>
        /// <returns>True if the log level is enabled; otherwise, false.</returns>
        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return logLevel != Microsoft.Extensions.Logging.LogLevel.None;
        }

        /// <summary>
        /// Logs a message with the specified log level and associated data.
        /// </summary>
        /// <typeparam name="TState">The type of the state object to log.</typeparam>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="state">The state object to log.</param>
        /// <param name="exception">The exception related to this log entry.</param>
        /// <param name="formatter">The function to create a message string.</param>
        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogInApplicationInsights(logLevel, eventId, state, exception, formatter);
            LogInFile(logLevel, eventId, state, exception, formatter);
        }

        /// <summary>
        /// Logs a message to Application Insights.
        /// </summary>
        /// <typeparam name="TState">The type of the state object to log.</typeparam>
        /// <param name="level">The log level.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="state">The state object to log.</param>
        /// <param name="exception">The exception related to this log entry.</param>
        /// <param name="formatter">The function to create a message string.</param>
        public void LogInApplicationInsights<TState>(Microsoft.Extensions.Logging.LogLevel level, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (applicationInsightsLogger == null) return;
            try
            {
                var message = string.Format("{0}|{1}|{2}", level.ToString(), eventId.Name, formatter(state, exception), exception != null ? exception.Message : "");
                switch (level)
                {
                    case Microsoft.Extensions.Logging.LogLevel.Trace:
                    case Microsoft.Extensions.Logging.LogLevel.Information:
                    case Microsoft.Extensions.Logging.LogLevel.None:
                        applicationInsightsLogger.Information($"{eventId}: {message}");
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Debug:
                        applicationInsightsLogger.Debug($"{eventId}: {message}");
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Warning:
                        applicationInsightsLogger.Warning($"{eventId}: {message}");
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Error:
                        applicationInsightsLogger.Error($"{eventId}: {message}");
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Critical:
                        applicationInsightsLogger.Fatal($"{eventId}: {message}");
                        break;
                    default:
                        applicationInsightsLogger.ForContext("Logging", true)
                               .Information($"{eventId}: {message}");
                        break;
                }

                if (exception != null)
                {
                    string logRecord = ExceptionInfoText(SerializableException.CreateSerializableException(exception, VerboseModeEnum.Full), 0, VerboseModeEnum.Full);

                    applicationInsightsLogger.Error(logRecord);
                }
            }
            catch (Exception ex)
            {
                // IGNORE :-(
            }
        }

        /// <summary>
        /// Logs a message to a file.
        /// </summary>
        /// <typeparam name="TState">The type of the state object to log.</typeparam>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="state">The state object to log.</param>
        /// <param name="exception">The exception related to this log entry.</param>
        /// <param name="formatter">The function to create a message string.</param>
        public void LogInFile<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            foreach (string baseFile in BaseFiles(logLevel, eventId))
            {
                var fullFilePath = technologyFileLoggerProvider.Options.FolderPath + "/" + baseFile + technologyFileLoggerProvider.Options.FilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
                var logRecord = string.Format("{0}|{1}|{2}|{3}", DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00"), logLevel.ToString(), eventId.Id.ToString() + "-" + eventId.Name, formatter(state, exception), exception != null ? exception.Message : "");
                int attempts = 0;
                bool done = false;
                while (!done)
                {
                    try
                    {
                        using (var streamWriter = new StreamWriter(fullFilePath, true))
                        {
                            streamWriter.WriteLine(logRecord);
                            done = true;
                            streamWriter.Flush();
                        }
                    }
                    catch
                    {
                        attempts++;
                        if (attempts > 3)  //4 times!
                        {
                            if (attempts < 6)
                                fullFilePath += attempts;
                            else
                                done = true; //give up
                        }
                        if (!done)
                            System.Threading.Thread.Sleep(attempts * 50);
                    }
                }
            }
            if (exception != null)
            {
                var fullFilePath = technologyFileLoggerProvider.Options.FolderPath + "/" + "ERR" + technologyFileLoggerProvider.Options.FilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
                string logRecord = ExceptionInfoText(SerializableException.CreateSerializableException(exception, VerboseModeEnum.Full), 0, VerboseModeEnum.Full);

                int attempts = 0;
                bool done = false;
                while (!done)
                {
                    try
                    {
                        using (var streamWriter = new StreamWriter(fullFilePath, true))
                        {
                            streamWriter.WriteLine(logRecord);
                            done = true;
                        }
                    }
                    catch
                    {
                        attempts++;
                        if (attempts > 3)  //4 times!
                        {
                            if (attempts < 6)
                                fullFilePath += attempts;
                            else
                                done = true; //give up
                        }
                        if (!done)
                            System.Threading.Thread.Sleep(attempts * 50);
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines the base file names based on the log level and event ID.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The event ID.</param>
        /// <returns>An array of base file names.</returns>
        private string[] BaseFiles(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId)
        {
            List<string> files = new List<string>();
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    files.Add("TECH");
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    files.Add("FUNC");
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    files.Add("FUNC");
                    files.Add("CRIT");
                    break;
                default:
                    files.Add("OTH");
                    break;
            }
            if (logLevel == Microsoft.Extensions.Logging.LogLevel.Information)
            {
                if (eventId.Id > 999)
                {
                    Math.DivRem(eventId.Id, 1000, out int result);

                    files.Add("CAT" + result);
                }
            }
            return files.ToArray();
        }

        /// <summary>
        /// Generates a detailed exception information text.
        /// </summary>
        /// <param name="serializableException">The serializable exception.</param>
        /// <param name="indentionLevel">The indentation level.</param>
        /// <param name="verboseMode">The verbosity mode.</param>
        /// <returns>A string containing detailed exception information.</returns>
        private static string ExceptionInfoText(SerializableException serializableException, int indentionLevel, VerboseModeEnum verboseMode)
        {
            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default))
            {
                return ExceptionInfoText(serializableException, indentionLevel, verboseMode, stringWriter);
            }
        }

        /// <summary>
        /// Generates a detailed exception information text with a StringWriter.
        /// </summary>
        /// <param name="serializableException">The serializable exception.</param>
        /// <param name="indentionLevel">The indentation level.</param>
        /// <param name="verboseMode">The verbosity mode.</param>
        /// <param name="stringWriter">The StringWriter to write the exception information to.</param>
        /// <returns>A string containing detailed exception information.</returns>
        private static string ExceptionInfoText(SerializableException serializableException, int indentionLevel, VerboseModeEnum verboseMode, System.IO.StringWriter stringWriter)
        {
            if (verboseMode == VerboseModeEnum.Simple)
            {
                Indent(stringWriter, indentionLevel);
                stringWriter.Write(
                    string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "AppDomain: {0} DateTime: {1} Exception: {4} - {2} Severity: {3}",
                    serializableException, serializableException.CreationDateTime, serializableException.ExceptionMessage, serializableException.SeverityLevel, serializableException.ExceptionNumber)
                    );
            }
            else
            {
                Indent(stringWriter, indentionLevel);
                if (indentionLevel == 0)
                    stringWriter.WriteLine("**-*-* EXCEPTION **********************************************************************");
                else
                    stringWriter.WriteLine("**-*-* INNER EXCEPTION ****************************************************************");

                Indent(stringWriter, indentionLevel);
                stringWriter.WriteLine(
                    string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "AppDomain: {0} DateTime: {1} Exception type: {2} Severity: {3}",
                    serializableException.AppDomainName,
                    serializableException.CreationDateTime.ToString("dd/MM/yyyy HH:mm:ss.fff", DXC.Technology.Utilities.StringFormatProvider.Default),
                    serializableException.ExceptionFullName,
                    serializableException.SeverityLevel)
                    );

                Indent(stringWriter, indentionLevel);
                stringWriter.WriteLine(
                    string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "Message: {1} - {0}", serializableException.ExceptionMessage, serializableException.ExceptionNumber)
                    );

                if (verboseMode == VerboseModeEnum.Full)
                {
                    Indent(stringWriter, indentionLevel);
                    stringWriter.WriteLine(
                        string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "Thrown by: {2} Thread: {1} Machine: {0} Reference: {3}",
                        serializableException.MachineName,
                        serializableException.ThreadIdentityName,
                        serializableException.WindowsIdentityName,
                        serializableException.ReferenceGuid)
                        );

                    Indent(stringWriter, indentionLevel);
                    stringWriter.WriteLine("Stack Trace:");
                    string stackTrace = serializableException.StackTrace;
                    string[] stackTraceLines = stackTrace.Split("\r\n".ToCharArray());
                    foreach (string stackTraceLine in stackTraceLines)
                    {
                        if (!string.IsNullOrEmpty(stackTraceLine))
                        {
                            Indent(stringWriter, indentionLevel + 1);
                            stringWriter.WriteLine(stackTraceLine);
                        }
                    }
                }
                stringWriter.WriteLine();
            }
            foreach (SerializableException innerSerializableException in serializableException.InnerSerializableExceptions)
            {
                ExceptionInfoText(innerSerializableException, indentionLevel + 1, verboseMode, stringWriter);
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Adds indentation to the StringWriter.
        /// </summary>
        /// <param name="stringWriter">The StringWriter to add indentation to.</param>
        /// <param name="indentionLevel">The indentation level.</param>
        private static void Indent(System.IO.StringWriter stringWriter, int indentionLevel)
        {
            for (int i = 1; i <= indentionLevel; i++)
            {
                stringWriter.Write("        ");
            }
        }
        #endregion
    }
}