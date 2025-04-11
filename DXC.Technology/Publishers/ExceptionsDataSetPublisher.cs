using DXC.Technology.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Publishers
{
    class ExceptionsDataSetPublisher
    {
        #region Private Methods

        /// <summary>
        /// Creates an ApplicationExceptionBase instance using the first exception in the dataset.
        /// </summary>
        private void CreateApplicationExceptionBase(ExceptionsDataSet exceptionsDataSet)
        {
            throw CreateApplicationExceptionBase(exceptionsDataSet.Exceptions[0], null);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates an ApplicationExceptionBase instance based on the provided exception row and inner exception.
        /// </summary>
        /// <param name="exceptionsRow">The exception row containing exception details.</param>
        /// <param name="innerException">The inner exception, if any.</param>
        /// <returns>An ApplicationExceptionBase instance.</returns>
        /// <exception cref="Exceptions.NamedExceptions.FieldInvalidException">Thrown when the exception type or inner exception is invalid.</exception>
        protected virtual ApplicationExceptionBase CreateApplicationExceptionBase(Publishers.ExceptionsDataSet.ExceptionsRow exceptionsRow, Exception innerException)
        {
            ApplicationExceptionBase applicationExceptionBase;

            if ((innerException != null) && ((exceptionsRow == null) || ((exceptionsRow.ExceptionType == "UnexpectedException") || (exceptionsRow.ExceptionType == "SQLException") || (exceptionsRow.ExceptionType == "DataException"))))
            {
                throw new Exceptions.NamedExceptions.FieldInvalidException("Exception", exceptionsRow.ExceptionType, exceptionsRow.ExceptionType, "Inner Exception Not Supported");
            }

            string[] args = null;
            if (!string.IsNullOrEmpty(exceptionsRow.RawExceptionParmsTSV))
            {
                args = exceptionsRow.RawExceptionParmsTSV.Split(';');
            }

            if (exceptionsRow.ExceptionType.Equals("BrokenRulesException"))
            {
                bool neverThrowAsSingleException = Convert.ToBoolean(args[0]);
                var brokenRulesException = new DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException(neverThrowAsSingleException);

                foreach (var breExceptionRow in exceptionsRow.GetExceptionsRows())
                {
                    try
                    {
                        brokenRulesException.AddException(CreateApplicationExceptionBase(breExceptionRow, null));
                    }
                    catch (Exceptions.NamedExceptions.FieldInvalidException ex)
                    {
                        brokenRulesException.AddException(ex);
                    }
                }
                applicationExceptionBase = brokenRulesException;
            }
            else
            {
                ExceptionDecoder exceptionDecoder = null;
                applicationExceptionBase = exceptionDecoder.Decode(exceptionsRow.ExceptionType, args);
            }

            // Assigning properties to the ApplicationExceptionBase instance
            applicationExceptionBase.AppDomainName = exceptionsRow.AppDomainName;
            applicationExceptionBase.CreationDateTime = exceptionsRow.CreationDateTime;
            applicationExceptionBase.StackTraceInfo = exceptionsRow.StackTrace;
            applicationExceptionBase.ThreadIdentity = exceptionsRow.ThreadIdentityName;
            applicationExceptionBase.WindowsIdentity = exceptionsRow.WindowsIdentityName;

            if (!exceptionsRow.IsReferenceGUIDNull())
            {
                applicationExceptionBase.ReferenceGuid = exceptionsRow.ReferenceGUID;
            }

            applicationExceptionBase.MachineName = exceptionsRow.MachineName;

            if (exceptionsRow.RawExceptionParmsTSV.Length > 0)
            {
                string[] exceptionArgs = exceptionsRow.RawExceptionParmsTSV.Split(';');
                if (exceptionArgs != null)
                {
                    applicationExceptionBase.ExceptionArguments = new List<string>(exceptionArgs.Length);
                    for (int i = 0; i < exceptionArgs.Length; i++)
                    {
                        applicationExceptionBase.ExceptionArguments.Add(Convert.ToString(exceptionArgs[i], DXC.Technology.Utilities.StringFormatProvider.Default));
                    }
                }
            }

            return applicationExceptionBase;
        }

        #endregion
    }
}