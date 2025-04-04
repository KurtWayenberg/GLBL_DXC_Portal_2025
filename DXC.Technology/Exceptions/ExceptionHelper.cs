using DXC.Technology.Exceptions.NamedExceptions;
using System;
using System.Data.SqlClient;

namespace DXC.Technology.Exceptions
{
    /// <summary>
    /// Forms a decoupling between 'logical DB events' and physical return codes
    /// </summary>
    internal enum SqlServerErrorsEnum
    {
        Locking = 1204,
        DeadLock = 1205,
        Timeout = -2 //To do check out
    }

    internal enum OracleServerErrorsEnum
    {
        DeadLock = 60,
        DtTimeOut = 2049,
        TimeOut = 2800,
        TimeOutPdml = 99,
        LockTimeOut = 4021,
        ResourceBusy = 30006,
        UniqueConstraint = 1
    }

    /// <summary>
    /// Helper class to publish Exceptions. Both normal exceptions as well as DXC.Technology.NamedExceptions can
    /// be published. The latter obviously includes more detail (e.g. Priority)
    /// The publishment of exceptions is done as specified in the Config File
    /// </summary>
    public sealed class ExceptionHelper
    {
        #region Static Fields

        /// <summary>
        /// Logical name for the exceptions database.
        /// </summary>
        public const string ExceptionsDatabaseLogicalName = "ExceptionsDB";

        /// <summary>
        /// Exception decoder instance.
        /// </summary>
        private static ExceptionDecoder exceptionDecoder = new ExceptionDecoder();

        #endregion

        #region Constructors

        /// <summary>
        /// Static Class - No constructors
        /// </summary>
        private ExceptionHelper()
        {
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Translation manager for exception handling.
        /// </summary>
        public static DXC.Technology.Translations.ITranslationProvider TranslationManager { get; set; }

        /// <summary>
        /// Gets or sets the exception decoder instance.
        /// </summary>
        public static ExceptionDecoder ExceptionDecoder
        {
            get => exceptionDecoder;
            set => exceptionDecoder = value;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Publishes an exception through all the publishers specified in the config file. 
        /// </summary>
        /// <param name="exception">The exception to publish. This can be any type of exception, but obviously also a subclass of ApplicationExceptionBase</param>
        public static void Publish(Exception exception)
        {
            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Error,
                new Microsoft.Extensions.Logging.EventId(88888, "Exception Helper Publish"),
                exception.Message + "||" + exception.StackTrace);

            if (exception.InnerException != null)
            {
                DXC.Technology.Configuration.LoggingManager.Current.Log(
                    Microsoft.Extensions.Logging.LogLevel.Error,
                    new Microsoft.Extensions.Logging.EventId(88889, "***** Inner Exception"),
                    exception.InnerException.Message + "||" + exception.InnerException.StackTrace);

                if (exception.InnerException.InnerException != null)
                {
                    DXC.Technology.Configuration.LoggingManager.Current.Log(
                        Microsoft.Extensions.Logging.LogLevel.Error,
                        new Microsoft.Extensions.Logging.EventId(88890, "***** Inner Inner Exception"),
                        exception.InnerException.InnerException.Message + "||" + exception.InnerException.InnerException.StackTrace);
                }
            }
        }

        /// <summary>
        /// Encodes an exception as a string.
        /// </summary>
        /// <param name="exception">The exception to encode.</param>
        /// <returns>The encoded exception message.</returns>
        public static string EncodeAsString(Exception exception)
        {
            // Todo refactor code from Application Exception Publisher etc...
            return exception.Message;
        }

        /// <summary>
        /// Decodes a string into an exception.
        /// </summary>
        /// <param name="exceptionString">The exception string to decode.</param>
        public static void DecodeAsException(string exceptionString)
        {
            throw new DXC.Technology.Exceptions.NamedExceptions.ActionFailedException(exceptionString);
        }

        /// <summary>
        /// Given an exception, recursively checks the inner exceptions until the lowest level exception is found
        /// Returns the top-level exception if it does not contain any inner exceptions
        /// </summary>
        /// <param name="exception">The exception to analyze.</param>
        /// <returns>The most inner exception.</returns>
        public static Exception GetMostInnerException(Exception exception)
        {
            Exception tempException = exception;
            while (tempException.InnerException != null)
            {
                tempException = tempException.InnerException;
            }
            return tempException;
        }

        /// <summary>
        /// Given a dataset with errors, returns a descriptive string explaining the errors
        /// </summary>
        /// <param name="dataSet">Erroneous datasets</param>
        /// <returns>A string describing the dataset errors.</returns>
        public static string GetDataSetErrors(System.Data.DataSet dataSet)
        {
            if (dataSet == null) return "Empty DataSet";
            if (!dataSet.HasErrors) return string.Empty;

            System.IO.StringWriter stringWriter = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default);
            try
            {
                foreach (System.Data.DataTable dataTable in dataSet.Tables)
                {
                    if (dataTable.HasErrors)
                    {
                        stringWriter.Write("Erroneous Table: {0}    # Erroneous Records: {1}\r\n", dataTable.TableName, dataTable.GetErrors().GetLength(0));
                        foreach (System.Data.DataRow dataRow in dataTable.GetErrors())
                        {
                            stringWriter.Write(DXC.Technology.Data.DataSetHelper.GetPrimaryKeyFields(dataRow));
                            stringWriter.WriteLine(dataRow.RowError);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                stringWriter.Write("Error while determining dataset errors:");
                stringWriter.Write(ex.Message);
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Given a SQL Exception, wraps it into an equivalent DXC.Technology.Exceptions.NamedException - exception
        /// </summary>
        /// <param name="exception">SQL exception that was thrown by DB</param>
        /// <returns>An application exception base object.</returns>
        public static ApplicationExceptionBase WrapFromSql(SqlException exception)
        {
            if (exception == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Exception");

            ApplicationExceptionBase appException = null;

            switch (exception.Errors[0].Number)
            {
                case 16947:
                    appException = new NamedExceptions.ItemNotFoundException(exception.Message);
                    break;
                case 11045:
                    appException = new NamedExceptions.NotUniqueException("Literals.SqlException", exception.Message);
                    break;
                case 272:
                case 8102:
                case 273:
                    appException = new NamedExceptions.ParameterTypeInvalidException("Literals.SqlException", exception.Message, null);
                    break;
                case 8152:
                    appException = new NamedExceptions.FieldTruncatedException("Literals.SqlException", exception.Message, "", 0);
                    break;
                case 515:
                    appException = new NamedExceptions.MandatoryFieldNotSpecifiedException("Literals.SqlException", exception.Message);
                    break;
            }
            return appException;
        }

        /// <summary>
        /// Determine if a 'retry' is justified given a (raised) Exception
        /// </summary>
        /// <remarks>
        /// This functionality is used within a context where an evaluation needs to
        /// take place given a certain raised Exception.
        /// Based on the error number, certain checks evaluate to true while other
        /// evaluate to false.
        /// </remarks>
        /// <exception cref="DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException">
        /// When the supplied exception is not of ADO.NET provider related i.e. SqlException,
        /// OleDbException, etc
        /// </exception>
        /// <param name="exception">The exception to evaluate.</param>
        /// <returns>True : Retry is justified, False : Retry not justified</returns>
        public static bool IsRetryJustified(Exception exception)
        {
            bool retryJustified = false; // Assume we don't need to retry

            SqlException sqlException = exception as SqlException;

            if (sqlException != null)
            {
                switch (sqlException.Number)
                {
                    case (int)SqlServerErrorsEnum.Locking:
                    case (int)SqlServerErrorsEnum.DeadLock:
                    case (int)SqlServerErrorsEnum.Timeout:
                        retryJustified = true;
                        break;
                }
            }
            else
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Exception", null, null);
            }
            return retryJustified;
        }

        /// <summary>
        /// Asserts a condition and publishes an exception if the assertion fails.
        /// </summary>
        /// <param name="condition">The condition to assert.</param>
        /// <param name="assertDescription">The description of the assertion.</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Assert(bool condition, string assertDescription)
        {
            Exception ex = new DXC.Technology.Exceptions.NamedExceptions.BusinessLogicException("Assertion Failed:" + assertDescription);
            Publish(ex);
        }

        /// <summary>
        /// Returns a WrapperException with the provided message and Exception
        /// For a BrokenRulesException: returns a BrokenRulesException with every Exception wrapped
        /// </summary>
        /// <param name="exception">The exception to wrap.</param>
        /// <param name="message">The message to include in the wrapper exception.</param>
        /// <returns>A wrapped exception.</returns>
        public static Exception WrapException(Exception exception, string message)
        {
            BrokenRulesException thrownBrokenRulesException = exception as BrokenRulesException;
            if (thrownBrokenRulesException == null)
            {
                return new WrapperException(exception, message);
            }
            else
            {
                BrokenRulesExceptionHelper resultBrokenRulesExceptionHelper = new BrokenRulesExceptionHelper();
                foreach (Exception innerException in thrownBrokenRulesException.BrokenRulesExceptions)
                {
                    resultBrokenRulesExceptionHelper.RegisterException(new WrapperException(innerException, message));
                }
                return resultBrokenRulesExceptionHelper.BrokenRulesException;
            }
        }

        #endregion
    }

    /// <summary>
    /// Simple utility class allowing the server side publishing multiple exceptions at the same time.
    /// Create this helper and register exceptions as you go... 
    /// At any time where you think you should not progress if there are exceptions, you can invoke the ThrowOnErrors message
    /// </summary>
    public class BrokenRulesExceptionHelper
    {
        #region Instance Fields

        private bool neverThrowAsSimpleException;
        private DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException brokenRulesException;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokenRulesExceptionHelper"/> class.
        /// </summary>
        public BrokenRulesExceptionHelper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokenRulesExceptionHelper"/> class with a flag indicating whether to throw as a simple exception.
        /// </summary>
        /// <param name="neverThrowAsSimpleException">Indicates whether to throw as a simple exception.</param>
        public BrokenRulesExceptionHelper(bool neverThrowAsSimpleException)
        {
            this.neverThrowAsSimpleException = neverThrowAsSimpleException;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to never throw as a simple exception.
        /// </summary>
        public bool NeverThrowAsSimpleException
        {
            get => neverThrowAsSimpleException;
            set => neverThrowAsSimpleException = value;
        }

        /// <summary>
        /// Return the broken rules exceptions (containing one or more exceptions) or null if there are no exceptions.
        /// </summary>
        public DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException BrokenRulesException
        {
            get => brokenRulesException;
        }

        /// <summary>
        /// Returns true if there has been registered at least one exception. false otherwise
        /// </summary>
        public bool HasErrors => brokenRulesException != null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Register an exception. The exception will not be thrown
        /// </summary>
        /// <param name="exception">Exception</param>
        public void RegisterException(Exception exception)
        {
            if (brokenRulesException == null)
                brokenRulesException = new DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException(neverThrowAsSimpleException);
            brokenRulesException.AddException(exception);
        }

        /// <summary>
        /// Frequently used exception. Syntactic Sugar
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        public void RegisterMandatoryParameterNotFoundException(string parameterName)
        {
            if (brokenRulesException == null)
                brokenRulesException = new DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException(neverThrowAsSimpleException);
            brokenRulesException.AddException(new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException(parameterName));
        }

        /// <summary>
        /// Wraps the Exception with the provided message and registers it.
        /// For a BrokenRulesException, wraps and registers every exceptions separately
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="message">Message</param>
        public void RegisterWrappedException(Exception exception, string message)
        {
            BrokenRulesException thrownBrokenRulesException = exception as BrokenRulesException;
            if (thrownBrokenRulesException == null)
            {
                RegisterException(new WrapperException(exception, message));
            }
            else
            {
                foreach (Exception innerException in thrownBrokenRulesException.BrokenRulesExceptions)
                {
                    RegisterWrappedException(innerException, message);
                }
            }
        }

        /// <summary>
        /// If there are errors, throw the broken rule exception
        /// </summary>
        public void ThrowOnErrors()
        {
            if (HasErrors)
            {
                if (!neverThrowAsSimpleException)
                {
                    brokenRulesException.ThrowIfSingleException();
                }
                throw brokenRulesException;
            }
        }

        /// <summary>
        /// If there are errors, throw the broken rule exception
        /// </summary>
        public void ThrowOnBlockingErrors()
        {
            if (brokenRulesException != null && HasErrors)
            {
                if (neverThrowAsSimpleException)
                {
                    brokenRulesException.ThrowIfSingleException();
                }
                throw brokenRulesException;
            }
        }

        /// <summary>
        /// Gets the number of errors.
        /// </summary>
        /// <returns>The number of errors.</returns>
        public int NumberOfErrors() => brokenRulesException?.BrokenRulesExceptions.Count ?? 0;

        #endregion
    }
}