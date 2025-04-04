using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace DXC.Technology.Exceptions
{
    /// <summary>
    /// SeverityLevel is used in combination with PublishModeEnum in publishingserial
    /// to determine which exceptions will be published by which publishers. e.g. 
    /// A certain tracefile publishes the "TracingOnly", so only "Information" and "Warnings"
    /// The ErrorEventLog publishes the "ErrorsOnly", so levels "Low", "Medium", "High", "Critical"
    /// The Management Console publishes "ImportantErrorsOnly", so only severity's "High" and "Critical"
    /// Unspecified exceptions are always published
    /// Publishing modi are specified in the config file
    /// </summary>
    [Flags]
    public enum SeverityLevelEnum
    {
        None = 0,
        Information = 1,
        Warning = 2,
        Low = 4,
        Medium = 8,
        High = 16,
        Critical = 32
    }

    /// <summary>
    /// Enumeration which 'steers' the publication algorithm in terms of 'what format to log' (weighted by Verbosity)
    /// </summary>
    /// <remarks>
    /// Use VerboseMode to specify the amount of information to be published.
    /// </remarks>
    public enum VerboseModeEnum
    {
        /// <summary>
        /// No verbose mode specified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// Only essential information will be shown
        /// </summary>
        Simple = 1,
        /// <summary>
        /// Essential information and the stack trace will be shown
        /// </summary>
        Medium = 2,
        /// <summary>
        /// Full information will be shown, including local variables
        /// </summary>
        Full = 3
    }

    /// <summary>
    /// Enumeration which 'steers' the publication algorithm in terms of 'what to log' (weighted by Severity)
    /// </summary>
    /// <remarks>
    /// PublishMode is used in combination with SeverityLevelEnum in publishing
    /// to determine which exceptions will be published by which publishers. 
    /// A certain tracefile publishes the "TracingOnly", so only "Information" and 
    /// "Warnings" errors will be published
    /// The ErrorEventLog publishes the "ErrorsOnly", so levels "Low", "Medium", 
    /// "High", "Critical"
    /// The Management Console publishes "ImportantErrorsOnly", so only severity's 
    /// "High" and "Critical"
    /// Full publishes all exceptions, FullExcludeInformation all exceptions except 
    /// "Information" exceptions
    /// Unspecified exceptions are always published
    /// Publishing modi are specified in the 'configurable setup'
    /// </remarks>
    public enum PublishModeEnum
    {
        AllExceptions = 0,
        TracingOnly = 1,
        ErrorsOnly = 2,
        ImportantExceptionsOnly = 3,
        SecurityExceptionsOnly = 4,
        AllButInformationExceptions = 5
    }

    /// <summary>
    /// Use this class as the root of your application-exceptions rather than 
    /// directly using the ApplicationException. It has extended functionality 
    /// to nicely publish exceptions
    /// </summary>
    public abstract class ApplicationExceptionBase : Exception
    {
        #region Static Fields

        // No static fields in this class.

        #endregion

        #region Public Properties

        /// <summary>
        /// Name of the machine where the exception occurred.
        /// </summary>
        public string MachineName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the application domain where the exception occurred.
        /// </summary>
        public string AppDomainName { get; set; } = string.Empty;

        /// <summary>
        /// Identity of the thread where the exception occurred.
        /// </summary>
        public string ThreadIdentity { get; set; } = string.Empty;

        /// <summary>
        /// Windows identity of the user where the exception occurred.
        /// </summary>
        public string WindowsIdentity { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the exception was created.
        /// </summary>
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Arguments associated with the exception.
        /// </summary>
        public List<string> ExceptionArguments { get; set; } = new List<string>();

        /// <summary>
        /// Stack trace information for the exception.
        /// </summary>
        public string StackTraceInfo { get; set; } = string.Empty;

        /// <summary>
        /// Reference GUID for linking client exceptions to server exceptions.
        /// </summary>
        public string ReferenceGuid { get; set; } = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Pass the arguments for this exception. These will be "substituted" in the (possibly translated) message pattern when publishing.
        /// </summary>
        /// <param name="exceptionArguments">Param array of exception arguments. Can be of any type, but will be converted to a string</param>
        protected ApplicationExceptionBase(params object[] exceptionArguments) : base()
        {
            if (exceptionArguments == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Exception Arguments");

            InitializeApplicationException();
            this.ExceptionArguments = new List<string>();
            foreach (var argument in exceptionArguments)
            {
                if (argument is object[] nestedArguments)
                {
                    foreach (var nestedArgument in nestedArguments)
                    {
                        AddExceptionArgument(nestedArgument);
                    }
                }
                else
                {
                    AddExceptionArgument(argument);
                }
            }
        }

        /// <summary>
        /// Pass the arguments for this exception. These will be "substituted" in the (possibly translated) message pattern when publishing.
        /// </summary>
        /// <param name="innerException">Inner exception</param>
        /// <param name="exceptionArguments">Param array of exception arguments. Can be of any type, but will be converted to a string</param>
        protected ApplicationExceptionBase(Exception innerException, params object[] exceptionArguments) : base(string.Empty, innerException)
        {
            if (exceptionArguments == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Exception Arguments");
            InitializeApplicationException();
            foreach (var argument in exceptionArguments)
            {
                this.ExceptionArguments.Add(Convert.ToString(argument, DXC.Technology.Utilities.StringFormatProvider.Default));
            }
        }

        private void InitializeApplicationException()
        {
            try
            {
                MachineName = Environment.MachineName;
            }
            catch (System.Security.SecurityException)
            {
                MachineName = "Not Accessible";
            }
            try
            {
                AppDomainName = AppDomain.CurrentDomain.FriendlyName;
            }
            catch (System.Security.SecurityException)
            {
                AppDomainName = "Not Accessible";
            }
            try
            {
                ThreadIdentity = DXC.Technology.Caching.ContextHelper.GetUserName();
            }
            catch (System.Security.SecurityException)
            {
                ThreadIdentity = "Not Accessible";
            }
            try
            {
                WindowsIdentity = Environment.UserName;
            }
            catch (System.Security.SecurityException)
            {
                WindowsIdentity = "Not Accessible";
            }
            try
            {
                StackTraceInfo = Environment.StackTrace;
            }
            catch (System.Security.SecurityException)
            {
                StackTraceInfo = "Not Accessible";
            }
            ReferenceGuid = Guid.NewGuid().ToString();
        }

        private void AddExceptionArgument(object argument)
        {
            if (argument is DateTime dateTimeArgument)
            {
                ExceptionArguments.Add(DXC.Technology.Utilities.Date.ToISO8601String(dateTimeArgument));
            }
            else
            {
                ExceptionArguments.Add(Convert.ToString(argument, DXC.Technology.Utilities.StringFormatProvider.Default));
            }
        }

        #endregion

        #region Public Properties


     


        public virtual string ExceptionArgumentsAsCSV => DXC.Technology.Utilities.StringArrayHelper.ToCsvString(ExceptionArguments.ToArray());


        /// <summary>
        /// Binds the exception message pattern with the exception arguments to have a nice explanation of the exception message
        /// </summary>
        public virtual string FullMessage
        {
            get
            {
                try
                {
                    if (ExceptionHelper.TranslationManager != null)
                    {
                        var localizedArguments = ExceptionArguments.Select(arg =>
                        {
                            var dateTimeValue = DXC.Technology.Utilities.Date.FromISO8601String(arg);
                            return dateTimeValue == DateTime.MinValue
                                ? ExceptionHelper.TranslationManager.Localize(arg)
                                : Convert.ToDateTime(arg, DXC.Technology.Utilities.DateTimeFormatProvider.Default).ToString();
                        }).ToArray();

                        var localizedPattern = ExceptionHelper.TranslationManager.Localize(ExceptionPatternString());

                        if (localizedArguments.Length > 0 && !localizedPattern.Contains("{"))
                        {
                            return string.Join(" ", new[] { localizedPattern }.Concat(localizedArguments));
                        }

                        return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, localizedPattern, localizedArguments);
                    }

                    return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, ExceptionPatternString(), ExceptionArguments.ToArray());
                }
                catch (FormatException)
                {
                    var message = new System.Text.StringBuilder();
                    message.AppendLine($"(ApplicationExceptionBase - Error getting FullMessage of Exception: Pattern '{ExceptionPatternString()}' not compatible with number of ExceptionArguments (= {ExceptionArguments.Count}))");
                    message.AppendLine("=> Your Exception Message:");
                    foreach (var arg in ExceptionArguments)
                    {
                        message.AppendLine(arg);
                    }
                    return message.ToString();
                }
            }
        }

        /// <summary>
        /// Override this property to indicate this is also to be flagged as security exception
        /// </summary>
        public virtual bool IsSecurityException() => false;

        /// <summary>
        /// Return the formatted exception string as message
        /// </summary>
        public override string Message => FullMessage;

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the exception into a list of serializable exceptions.
        /// </summary>
        /// <returns>A list of serializable exceptions.</returns>
        public virtual IEnumerable<object> AsListOfSerializableExceptions()
        {
            var result = new List<SerializableException>
            {
                SerializableException.CreateSerializableException(this, VerboseModeEnum.Full)
            };
            return result;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Override this function to return the Exception Message (pattern). e.g. "Element {0} not found")
        /// </summary>
        public abstract string ExceptionPatternString();

        /// <summary>
        /// Override this function to return the SeverityLevel e.g .High, Medium, Low, Warning, InformationOnly, ...
        /// </summary>
        public abstract SeverityLevelEnum SeverityLevel();

        /// <summary>
        /// Override this function to return the Error Number
        /// </summary>
        public abstract int ExceptionNumber();

        #endregion
    }
}