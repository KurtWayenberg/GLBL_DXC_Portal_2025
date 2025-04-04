using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DXC.Technology.Enumerations;
using DXC.Technology.Exceptions.NamedExceptions;

namespace DXC.Technology.Exceptions
{
    public class ExceptionDecoder
    {
        #region Static Fields

        /// <summary>
        /// Contains all exception names.
        /// </summary>
        private static string[] allExceptionNames;

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets all exception names.
        /// </summary>
        public static string[] AllExceptionNames
        {
            get
            {
                if (allExceptionNames == null)
                {
                    allExceptionNames = Assembly.GetAssembly(typeof(ApplicationExceptionBase)).GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(ApplicationExceptionBase)) && !t.IsAbstract)
                        .OrderBy(t => t.Name)
                        .Select(t => t.Name).ToArray();
                }
                return allExceptionNames;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets exception patterns.
        /// </summary>
        /// <returns>A list of exception patterns.</returns>
        public virtual ArrayList GetExceptionPatterns()
        {
            ArrayList result = new ArrayList();
            foreach (string exceptionName in ExceptionDecoder.AllExceptionNames)
            {
                string[] args = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                try
                {
                    ApplicationExceptionBase applicationException = this.Decode(exceptionName, args);
                    result.Add(applicationException.ExceptionPatternString());
                }
                catch (ApplicationExceptionBase)
                {
                    result.Add(exceptionName);
                    // Do nothing
                }
            }
            return result;
        }

        /// <summary>
        /// Decodes an exception type and arguments into an ApplicationExceptionBase.
        /// </summary>
        /// <param name="exceptionType">The type of the exception.</param>
        /// <param name="args">The arguments for the exception.</param>
        /// <returns>An instance of ApplicationExceptionBase.</returns>
        public virtual ApplicationExceptionBase Decode(string exceptionType, string[] args)
        {
            ApplicationExceptionBase applicationExceptionBase = null;
            switch (exceptionType)
            {
                case "ActionDeniedException":
                    applicationExceptionBase = new ActionDeniedException(args[0], args[1]);
                    break;
                case "ActionImpossibleException":
                    applicationExceptionBase = new ActionImpossibleException(args[0], args[1]);
                    break;
                case "ClientDisconnectedException":
                    applicationExceptionBase = new ClientDisconnectedException(args[0]);
                    break;
                case "MandatoryFieldNotSpecifiedException":
                    applicationExceptionBase = new MandatoryFieldNotSpecifiedException(args[0], args[1]);
                    break;
                case "FieldOutOfRangeException":
                    applicationExceptionBase = new FieldOutOfRangeException(args[0], args[1], args[2], args[3], args[4]);
                    break;
                case "ParameterOutOfRangeException":
                    applicationExceptionBase = new ParameterOutOfRangeException(args[0], args[1], args[2], args[3]);
                    break;
                case "SelectedValueInvalidValueException":
                    applicationExceptionBase = new SelectedValueInvalidValueException(args[0], args[1], args[2]);
                    break;
                case "FieldInvalidValueException":
                    applicationExceptionBase = new FieldInvalidValueException(args[0], args[1], args[2], args[3]);
                    break;
                case "IllegalInterfacedCustomTypeException":
                    applicationExceptionBase = new IllegalInterfacedCustomTypeException(args[0], args[1]);
                    break;
                case "DuplicateInfoException":
                    applicationExceptionBase = new DuplicateInfoException(args[0], args[1]);
                    break;
                case "ParameterInvalidValueException":
                    applicationExceptionBase = new ParameterInvalidValueException(args[0], args[1], args[2]);
                    break;
                case "ParameterInvalidException":
                    applicationExceptionBase = new ParameterInvalidException(args[0], args[1], args[2]);
                    break;
                case "ParameterInconsistentException":
                    applicationExceptionBase = new ParameterInconsistentException(args[0], args[1], args[2]);
                    break;
                case "FieldInvalidException":
                    applicationExceptionBase = new FieldInvalidException(args[0], args[1], args[2], args[3]);
                    break;
                case "RangeOverlapsException":
                    applicationExceptionBase = new RangeOverlapsException(args[0]);
                    break;
                case "ObjectNotFoundException":
                    applicationExceptionBase = new ObjectNotFoundException(args[0], args[1], null);
                    break;
                case "DataSetMergeException":
                    applicationExceptionBase = new UnexpectedException(string.Format(CultureInfo.InvariantCulture, "DataSetMergeException: {0}", args[0]));
                    break;
                case "RecordNotFoundException":
                    applicationExceptionBase = new RecordNotFoundException(args[0], args[1], args[2]);
                    break;
                case "ConcurrencyException":
                    applicationExceptionBase = new ConcurrencyException();
                    break;
                case "ConcurrencyViolationException":
                    applicationExceptionBase = new ConcurrencyViolationException(args[0], args[1]);
                    break;
                case "DeadlockException":
                    applicationExceptionBase = new DeadlockException();
                    break;
                case "NoRecordsFoundException":
                    applicationExceptionBase = new NoRecordsFoundException(args[0], args[1], args[2]);
                    break;
                case "FieldTruncatedException":
                    applicationExceptionBase = new FieldTruncatedException(args[0], args[1], args[2], Convert.ToInt32(args[3], CultureInfo.InvariantCulture));
                    break;
                case "WebServiceNotAvailableException":
                    applicationExceptionBase = new WebServiceNotAvailableException(args[0], args[1]);
                    break;
                case "VersionExpiredException":
                    applicationExceptionBase = new VersionExpiredException(args[0]);
                    break;
                case "FieldTypeInvalidException":
                    applicationExceptionBase = new FieldTypeInvalidException(args[0], args[1], args[2], args[3]);
                    break;
                case "FieldLengthInvalidException":
                    applicationExceptionBase = new FieldLengthInvalidException(args[0], args[1], args[2], Convert.ToInt64(args[3], CultureInfo.InvariantCulture));
                    break;
                case "ParameterTypeInvalidException":
                    applicationExceptionBase = new ParameterTypeInvalidException(args[0], args[1], args[2]);
                    break;
                case "CannotCompareException":
                    applicationExceptionBase = new CannotCompareException(args[0], args[1]);
                    break;
                case "FileTypeInvalidException":
                    applicationExceptionBase = new FileTypeInvalidException(args[0]);
                    break;
                case "FileNotFoundException":
                    applicationExceptionBase = new NamedExceptions.FileNotFoundException(args[0]);
                    break;
                case "DatabaseNotFoundException":
                    applicationExceptionBase = new DatabaseNotFoundException(args[0]);
                    break;
                case "QueueNotFoundException":
                    applicationExceptionBase = new QueueNotFoundException(args[0]);
                    break;
                case "ServiceDoesNotRespondException":
                    applicationExceptionBase = new ServiceDoesNotRespondException(args[0], args[1]);
                    break;
                case "ServiceNotFoundException":
                    applicationExceptionBase = new ServiceNotFoundException(args[0], args[1]);
                    break;
                case "CompareException":
                    applicationExceptionBase = new CompareFieldException(args[0], args[1], EnumerationHelper.NameToEnum<CompareEnum>(args[2]), args[3], args[4]);
                    break;
                case "ActionFailedException":
                    applicationExceptionBase = new ActionFailedException(args[0]);
                    break;
                case "UpdateFailedException":
                    applicationExceptionBase = new UpdateFailedException(args[0], args[1], args[2]);
                    break;
                case "InsertFailedException":
                    applicationExceptionBase = new InsertFailedException(args[0], args[1], args[2]);
                    break;
                case "DuplicateKeyException":
                    applicationExceptionBase = new DuplicateKeyException(args[0], args[1], args[2]);
                    break;
                case "CannotDeleteChildrenRecordsExistException":
                    applicationExceptionBase = new CannotDeleteChildrenRecordsExistException(args[0], args[1], args[2], args[3]);
                    break;
                case "DeleteFailedException":
                    applicationExceptionBase = new DeleteFailedException(args[0]);
                    break;
                case "PrintFailedException":
                    applicationExceptionBase = new PrintFailedException();
                    break;
                case "ImportFailedException":
                    applicationExceptionBase = new ImportFailedException();
                    break;
                case "ExportFailedException":
                    applicationExceptionBase = new ExportFailedException();
                    break;
                case "IntegrityViolationException":
                    applicationExceptionBase = new IntegrityViolationException(args[0], args[1], args[2], args[3]);
                    break;
                case "CustomInformationException":
                    applicationExceptionBase = new CustomInformationException(args[0]);
                    break;
                case "CustomWarningException":
                    applicationExceptionBase = new CustomWarningException(args[0]);
                    break;
                case "CustomLowSeverityException":
                    applicationExceptionBase = new CustomLowSeverityException(args[0]);
                    break;
                case "CustomMediumSeverityException":
                    applicationExceptionBase = new CustomMediumSeverityException(args[0]);
                    break;
                case "CustomHighSeverityException":
                    applicationExceptionBase = new CustomHighSeverityException(args[0]);
                    break;
                case "CustomCriticalSeverityException":
                    applicationExceptionBase = new CustomCriticalSeverityException(args[0]);
                    break;
                case "InvalidLogonIdException":
                    applicationExceptionBase = new InvalidLogOnIdException(args[0]);
                    break;
                case "NewPasswordsDoNotMatchException":
                    applicationExceptionBase = new NewPasswordsDoNotMatchException();
                    break;
                case "InvalidPasswordException":
                    applicationExceptionBase = new InvalidPasswordException(args[0]);
                    break;
                case "DataAccessDeniedException":
                    applicationExceptionBase = new DataAccessDeniedException(args[0], args[1], args[2]);
                    break;
                case "IllegalApplicationAccessException":
                    applicationExceptionBase = new IllegalApplicationAccessException(args[0]);
                    break;
                case "AccessDeniedException":
                    applicationExceptionBase = args.Length == 2 ? new AccessDeniedException(args[0], args[1]) : new AccessDeniedException(args[0], args[1], args.Skip(2).ToArray());
                    break;
                case "ApplicationNotAvailableException":
                    applicationExceptionBase = new ApplicationNotAvailableException(args[0]);
                    break;
                case "PasswordExpiredException":
                    applicationExceptionBase = new PasswordExpiredException();
                    break;
                case "NewPasswordInvalidException":
                    applicationExceptionBase = new NewPasswordInvalidException(args[0]);
                    break;
                case "NotUniqueException":
                    applicationExceptionBase = new NotUniqueException(args[0], args[1]);
                    break;
                case "MandatoryParameterNotSpecifiedException":
                    applicationExceptionBase = new MandatoryParameterNotSpecifiedException(args[0], args[1]);
                    break;
                case "ServiceBusyException":
                    applicationExceptionBase = new ServiceBusyException(args[0], args[1]);
                    break;
                case "ValidationException":
                    if (ValidationException.ValidationMessageTemplateDictionary.ContainsKey(args[0]))
                    {
                        string[] arguments = new string[args.Length - 3];
                        Array.Copy(args, 2, arguments, 0, args.Length - 3);
                        string argumentsCSV = Utilities.StringArrayHelper.ToCsvString(arguments);
                        applicationExceptionBase = new ValidationException(
                            args[0],
                            ValidationException.ValidationMessageTemplateDictionary[args[0]],
                            argumentsCSV,
                            Convert.ToInt32(args[^1], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        applicationExceptionBase = new ValidationException(args[0], args[1], args[2], Convert.ToInt32(args[3], CultureInfo.InvariantCulture));
                    }
                    break;
                case "AnomalyException":
                    applicationExceptionBase = new AnomalyException(args[0], Convert.ToInt32(args[1], CultureInfo.InvariantCulture), args[2], args[3], args[4], args[5], args[6]);
                    break;
                case "DataInconsistencyException":
                    applicationExceptionBase = new DataInconsistencyException(args[0], args[1], args[2]);
                    break;
                case "UnexpectedException":
                    applicationExceptionBase = new UnexpectedException(args[0], args[1]);
                    break;
                case "DataException":
                    applicationExceptionBase = args == null ? new UnexpectedException("DataException") : new DataException(args[0], args[1]);
                    break;
                case "SQLCommandExecutionException":
                    applicationExceptionBase = new SQLCommandExecutionException(null, args[0], args[1]);
                    break;
                case "MissingConfigurationException":
                    applicationExceptionBase = new MissingConfigurationException(args[0]);
                    break;
                case "ItemNotFoundException":
                    applicationExceptionBase = new ItemNotFoundException(args[0]);
                    break;
                case "UseCaseSimulationException":
                    applicationExceptionBase = new UseCaseSimulationException();
                    break;
                case "SimpleException":
                    applicationExceptionBase = new SimpleException(args[0]);
                    break;
                case "WrapperException":
                    applicationExceptionBase = new WrapperException(args);
                    break;
                case "BusinessLogicException":
                    applicationExceptionBase = new BusinessLogicException(args[0]);
                    break;
                case "TechnicalException":
                    applicationExceptionBase = new TechnicalException(args[0]);
                    break;
                case "TimespanLimitExceededException":
                    applicationExceptionBase = new TimespanLimitExceededException(Convert.ToDateTime(args[0], CultureInfo.InvariantCulture), Convert.ToDateTime(args[1], CultureInfo.InvariantCulture), Convert.ToDateTime(args[2], CultureInfo.InvariantCulture));
                    break;
                case "FlexibleReportException":
                    applicationExceptionBase = new FlexibleReportException();
                    break;
                case "FunctionalityNotSupportedException":
                    applicationExceptionBase = new FunctionalityNotSupportedException(args[0]);
                    break;
                case "FunctionalityDiscontinuedException":
                    applicationExceptionBase = new FunctionalityDiscontinuedException(args[0]);
                    break;
                case "MaliciousInputException":
                    applicationExceptionBase = new MaliciousInputException(args[0]);
                    break;
                case "ConfirmationCodeRequiredException":
                    applicationExceptionBase = new ConfirmationCodeRequiredException();
                    break;
                case "ConfirmationCodeNotGeneratedException":
                    applicationExceptionBase = new ConfirmationCodeNotGeneratedException();
                    break;
                case "ConfirmationCodeInvalidException":
                    applicationExceptionBase = new ConfirmationCodeInvalidException();
                    break;
                case "NonConvergenceException":
                    applicationExceptionBase = new NonConvergenceException(args[0]);
                    break;
                case "ToBeImplementedException":
                    applicationExceptionBase = new ToBeImplementedException(args[0], args[1]);
                    break;
                case "ConditionNotSatisfiedException":
                    applicationExceptionBase = new ConditionNotSatisfiedException(args[0], args[1]);
                    break;
                case "Exception":
                    applicationExceptionBase = new UnexpectedException(args[0]);
                    break;
                case "PeriodExpiredException":
                    applicationExceptionBase = new PeriodExpiredException(args[0], Convert.ToDateTime(args[1], CultureInfo.InvariantCulture), Convert.ToDateTime(args[2], CultureInfo.InvariantCulture), Convert.ToDateTime(args[3], CultureInfo.InvariantCulture));
                    break;
                case "SoapException":
                    applicationExceptionBase = new UnexpectedException(args[0]);
                    break;
                case "RecordLockedException":
                    applicationExceptionBase = new RecordLockedException(args[0], args[1]);
                    break;
                case "RecordLockedByOtherUserException":
                    applicationExceptionBase = new RecordLockedByOtherUserException(args[0], args[1], args[2]);
                    break;
                case "RecordUnLockedException":
                    applicationExceptionBase = new RecordUnlockedException(args[0], args[1]);
                    break;
                case "CountLimitExceededException":
                    applicationExceptionBase = new CountLimitExceededException(int.Parse(args[0], CultureInfo.InvariantCulture), args[1], args[2]);
                    break;
                case "ResultListTooLongException":
                    applicationExceptionBase = new ResultListTooLongException();
                    break;
                case "SearchCountLimitExceededException":
                    applicationExceptionBase = new SearchCountLimitExceededException(int.Parse(args[0], CultureInfo.InvariantCulture), args[1], args[2]);
                    break;
                case "NoResultFoundException":
                    applicationExceptionBase = new NoResultFoundException(args[0], args[1], args[2]);
                    break;
                case "PrintException":
                    applicationExceptionBase = new PrintException();
                    break;
                case "DataIncompleteException":
                    applicationExceptionBase = new DataIncompleteException(args[0]);
                    break;
                case "DataBindingFromControlException":
                    applicationExceptionBase = new DataBindingFromControlException(args[0], args[1], args[2], args[3]);
                    break;
                case "DataBindingToControlException":
                    applicationExceptionBase = new DataBindingToControlException(args[0], args[1], args[2], args[3]);
                    break;
                case "CompareValueException":
                    applicationExceptionBase = new CompareValueException(args[0], args[1], args[2], args[3]);
                    break;
                case "AddEntityNotAllowedException":
                    applicationExceptionBase = new AddEntityNotAllowedException(args[0], EnumerationHelper.CodeToEnum<ExceptionPatternEnum>(args[1]), args.Skip(2).ToArray());
                    break;
                case "ExternalPatternBusinessLogicException":
                    applicationExceptionBase = new ExternalPatternBusinessLogicException(EnumerationHelper.CodeToEnum<ExceptionPatternEnum>(args[0]), args.Skip(1).ToArray());
                    break;
                default:
                    applicationExceptionBase = new UnexpectedException(exceptionType);
                    break;
            }
            return applicationExceptionBase;
        }

        /// <summary>
        /// Decodes a serializable exception into an ApplicationExceptionBase.
        /// </summary>
        /// <param name="serializableException">The serializable exception to decode.</param>
        /// <returns>An instance of ApplicationExceptionBase.</returns>
        public ApplicationExceptionBase Decode(SerializableException serializableException)
        {
            if (serializableException == null)
                throw new MandatoryParameterNotSpecifiedException("Exception", "Serializable Exception");

            string[] args = null;
            if (!string.IsNullOrEmpty(serializableException.RawExceptionParametersCsv))
                args = serializableException.RawExceptionParametersCsv.Split(';', ',');

            ApplicationExceptionBase applicationExceptionBase;
            if (serializableException.ExceptionType.Equals("BrokenRulesException"))
            {
                BrokenRulesException brokenRulesException = new BrokenRulesException(true);
                foreach (var innerSerializableException in serializableException.InnerSerializableExceptions)
                {
                    try
                    {
                        brokenRulesException.AddException(Decode(innerSerializableException));
                    }
                    catch (FieldInvalidException ex)
                    {
                        brokenRulesException.AddException(ex);
                    }
                }
                applicationExceptionBase = brokenRulesException;
            }
            else
            {
                applicationExceptionBase = Decode(serializableException.ExceptionType, args);
            }

            applicationExceptionBase.AppDomainName = serializableException.AppDomainName;
            applicationExceptionBase.CreationDateTime = serializableException.CreationDateTime;
            applicationExceptionBase.StackTraceInfo = serializableException.StackTrace;
            applicationExceptionBase.ThreadIdentity = serializableException.ThreadIdentityName;
            applicationExceptionBase.WindowsIdentity = serializableException.WindowsIdentityName;
            if (!string.IsNullOrEmpty(serializableException.ReferenceGuid))
                applicationExceptionBase.ReferenceGuid = serializableException.ReferenceGuid;
            applicationExceptionBase.MachineName = serializableException.MachineName;

            return applicationExceptionBase;
        }

        #endregion
    }
}