using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DXC.Technology.Exceptions
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class SerializableException
    {
        #region Instance Fields

        /// <summary>
        /// The list of inner serializable exceptions.
        /// </summary>
        private List<SerializableException> innerSerializableExceptions = new List<SerializableException>();

        #endregion

        #region Public Properties

        /// <summary>
        /// The type of the exception.
        /// </summary>
        [DataMember]
        public string ExceptionType { get; set; }

        /// <summary>
        /// The creation date and time of the exception.
        /// </summary>
        [DataMember]
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// The name of the machine where the exception occurred.
        /// </summary>
        [DataMember]
        public string MachineName { get; set; }

        /// <summary>
        /// The full name of the exception.
        /// </summary>
        [DataMember]
        public string ExceptionFullName { get; set; }

        /// <summary>
        /// The exception number.
        /// </summary>
        [DataMember]
        public int ExceptionNumber { get; set; }

        /// <summary>
        /// The severity level of the exception.
        /// </summary>
        [DataMember]
        public string SeverityLevel { get; set; }

        /// <summary>
        /// The timestamp of the exception.
        /// </summary>
        [DataMember]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// The name of the application domain where the exception occurred.
        /// </summary>
        [DataMember]
        public string AppDomainName { get; set; }

        /// <summary>
        /// The name of the thread identity.
        /// </summary>
        [DataMember]
        public string ThreadIdentityName { get; set; }

        /// <summary>
        /// The name of the Windows identity.
        /// </summary>
        [DataMember]
        public string WindowsIdentityName { get; set; }

        /// <summary>
        /// The help link associated with the exception.
        /// </summary>
        [DataMember]
        public string HelpLink { get; set; }

        /// <summary>
        /// The stack trace of the exception.
        /// </summary>
        [DataMember]
        public string StackTrace { get; set; }

        /// <summary>
        /// The raw exception pattern string.
        /// </summary>
        [DataMember]
        public string RawExceptionPatternString { get; set; }

        /// <summary>
        /// The raw exception parameters in CSV format.
        /// </summary>
        [DataMember]
        public string RawExceptionParametersCsv { get; set; }

        /// <summary>
        /// The message of the exception.
        /// </summary>
        [DataMember]
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// The reference GUID of the exception.
        /// </summary>
        [DataMember]
        public string ReferenceGuid { get; set; }

        /// <summary>
        /// Indicates whether the exception has been acknowledged.
        /// </summary>
        [DataMember]
        public bool Acknowledged { get; set; }

        /// <summary>
        /// The list of inner serializable exceptions.
        /// </summary>
        [DataMember]
        public List<SerializableException> InnerSerializableExceptions
        {
            get => innerSerializableExceptions;
            set => innerSerializableExceptions = value;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates a serializable exception from a standard exception and verbose mode.
        /// </summary>
        /// <param name="exception">The original exception.</param>
        /// <param name="verboseMode">The verbosity level for the exception.</param>
        /// <returns>A serializable exception.</returns>
        public static SerializableException CreateSerializableException(Exception exception, VerboseModeEnum verboseMode)
        {
            bool includeInnerExceptions = verboseMode == VerboseModeEnum.Medium || verboseMode == VerboseModeEnum.Full;
            bool includeStackTrace = verboseMode == VerboseModeEnum.Full;
            return CreateSerializableException(exception, includeInnerExceptions, DateTime.Now, includeStackTrace, 15);
        }

        /// <summary>
        /// Creates a serializable exception with detailed parameters.
        /// </summary>
        /// <param name="exception">The original exception.</param>
        /// <param name="includeInnerExceptions">Whether to include inner exceptions.</param>
        /// <param name="creationDateTime">The creation date and time of the exception.</param>
        /// <param name="includeStackTrace">Whether to include the stack trace.</param>
        /// <param name="maxNumberOfInnerExceptions">The maximum number of inner exceptions to include.</param>
        /// <returns>A serializable exception.</returns>
        public static SerializableException CreateSerializableException(Exception exception, bool includeInnerExceptions, DateTime creationDateTime, bool includeStackTrace, int maxNumberOfInnerExceptions)
        {
            if (maxNumberOfInnerExceptions <= 0)
            {
                return new SerializableException
                {
                    ExceptionType = "BusinessLogicException",
                    CreationDateTime = creationDateTime,
                    RawExceptionParametersCsv = "",
                    RawExceptionPatternString = "",
                    ExceptionMessage = "The maximum number of exceptions has been exceeded"
                };
            }

            var serializableException = new SerializableException();

            if (exception is DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException brokenRulesException)
            {
                serializableException.ExceptionFullName = "BrokenRulesException";
                serializableException.ExceptionNumber = brokenRulesException.ExceptionNumber();
                serializableException.RawExceptionParametersCsv = "";
                serializableException.RawExceptionPatternString = "";

                foreach (var innerException in brokenRulesException.AsListOfSerializableExceptions())
                {
                    serializableException.InnerSerializableExceptions.Add(innerException);
                }
            }
            else if (exception is ApplicationExceptionBase applicationExceptionBase)
            {
                serializableException.ExceptionFullName = applicationExceptionBase.GetType().ToString();
                serializableException.ExceptionNumber = applicationExceptionBase.ExceptionNumber();
                serializableException.RawExceptionParametersCsv = DXC.Technology.Utilities.StringArrayHelper.ToCsvString(applicationExceptionBase.ExceptionArguments.ToArray());
                serializableException.RawExceptionPatternString = applicationExceptionBase.ExceptionPatternString();
            }
            else
            {
                serializableException.ExceptionFullName = exception.GetType().ToString();
                serializableException.ExceptionNumber = -1;
                serializableException.RawExceptionParametersCsv = "";
                serializableException.RawExceptionPatternString = "";
            }

            serializableException.AppDomainName = exception.Source ?? AppDomain.CurrentDomain.FriendlyName;
            serializableException.StackTrace = includeStackTrace && exception.StackTrace != null ? exception.StackTrace : "";
            serializableException.MachineName = Environment.MachineName;
            serializableException.ExceptionMessage = exception.Message;
            serializableException.CreationDateTime = creationDateTime;
            serializableException.SeverityLevel = SeverityLevelEnum.None.ToString();
            serializableException.HelpLink = exception.HelpLink;

            string userName = DXC.Technology.Caching.ContextHelper.GetUserName();
            userName = DXC.Technology.Security.SensitiveInformationHelper.Obfuscate(userName);

            serializableException.ThreadIdentityName = string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "{0} ({1})", DXC.Technology.Caching.ContextHelper.GetUserName(), userName);
            serializableException.WindowsIdentityName = DXC.Technology.Caching.ContextHelper.GetUserName();
            serializableException.ExceptionType = exception.GetType().Name;

            if (includeInnerExceptions && exception.InnerException != null)
            {
                var innerException = CreateSerializableException(exception.InnerException, includeInnerExceptions, creationDateTime, includeStackTrace, --maxNumberOfInnerExceptions);
                serializableException.InnerSerializableExceptions.Add(innerException);
            }

            return serializableException;
        }

        #endregion
    }
}