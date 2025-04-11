using System;
using System.Collections.Specialized;
using System.Diagnostics;
using DXC.Technology.Exceptions;
using DXC.Technology.Enumerations;
using System.Xml;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using DXC.Technology.Exceptions.NamedExceptions;
using System.ServiceModel;

namespace DXC.Technology.Publishers
{
    /// <summary>
    /// Enumeration which 'steers' the publication algorithm in terms of 'what to log' (weighted by Severity)
    /// </summary>
    /// <remarks>
    /// PublishMode is used in combination with SeverityLevelEnum in publishing
    /// to determine which exceptions will be published by which publishers. 
    /// A certain tracefile publishes the "TracingOnly", so only "Information" and 
    /// "Warnings" errors will be publised
    /// The ErrorEventLog publishes the "ErrorsOnly", so levels "Low", "Medium", 
    /// "High", "Critical"
    /// The Management Console publishes "ImportantErrorsOnly", so only severity's 
    /// "High" and "Critical"
    /// Full publishes all exceptions, FullExcludeInformation all exceptions except 
    /// "Information" exceptions
    /// Unspecified exceptions are allways published
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
    /// Enumeration which 'steers' the publication algorithm in terms of 'which format to use'
    /// </summary>
    /// <remarks>
    /// Use PublisherFormat to specify in what format the Exception shoudl be published.
    /// XML has the adventage that it can be parsed into a dataset, whereas Text is somewhat 
    /// easier to read, especially for inner exceptions
    /// </remarks>
    public enum PublishFormatEnum
    {
        /// <summary>
        /// XML - parsable format
        /// </summary>
        XML = 0,
        /// <summary>
        /// Readable, indented format
        /// </summary>
        Text = 1
    }


    /// <summary>
    /// Enumeration which 'steers' the publication algorithm in terms of 'what format to log' (weighted by Verbosity)
    /// </summary>
    /// <remarks>
    /// Use VerboseMode to specify the amount of information to be published.
    /// </remarks>
    //public enum VerboseModeEnum
    //{
    //    /// <summary>
    //    /// No verbose mode specified
    //    /// </summary>
    //    Unspecified = 0,
    //    /// <summary>
    //    /// Only essential information wil be shown
    //    /// </summary>
    //    Simple = 1,
    //    /// <summary>
    //    /// Essential information and the stacktrace will be shown
    //    /// </summary>
    //    Medium = 2,
    //    /// <summary>
    //    /// Full information will be shown, including local variables
    //    /// </summary>
    //    Full = 3
    //}

    /// <summary>
    /// Enumeration which 'steers' the publication algorithm in terms of 'where to log'
    /// </summary>
    /// <remarks>
    /// Use this enumeration to specify the publishing destination
    /// </remarks>
    public enum PublisherTypeEnum
    {
        /// <summary>
        /// None specified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// Publish towards System Eventlog 
        /// </summary>
        EventLog = 1,
        /// <summary>
        /// Publish towards File
        /// </summary>
        File = 2,
        /// <summary>
        /// Publish towards mail provider
        /// </summary>
        Mail = 3,
        /// <summary>
        /// Publish towards a (predefined) WebService
        /// </summary>
        WebService = 4,
        /// <summary>
        /// Publish towards a MSMQ queue
        /// </summary>
        Queue = 5,
        /// <summary>
        /// Publish towards the screen (popup)
        /// </summary>
        Screen = 6,
        /// <summary>
        /// Publish towards WMI subsystem
        /// </summary>
        WMI = 7,
        /// <summary>
        /// Publish towards the database
        /// </summary>
        Database = 8,
        /// <summary>
        /// Publish towards the (developer) debugging context
        /// </summary>
        Debug = 9,
        /// <summary>
        /// Publish towards SMS service
        /// </summary>
        SMS = 10,
        /// <summary>
        /// Publish towards (Smart Client) Isolated Storage
        /// </summary>
        IsolatedStorage = 11
    }

    /// <summary>
    /// Enumeration which 'steers' the publication algorithm in when a FilePublisher is used
    /// </summary>
    /// <remarks>
    /// This applies only to FilePublishers. Results in a Yeat/Month/Week suffix of the log files 
    /// </remarks>
    public enum RolloverEnum
    {
        /// <summary>
        /// The same publishing media is used (no rollover)
        /// </summary>
        Never,
        /// <summary>
        /// Perform a daily rollover
        /// </summary>
        Daily,
        /// <summary>
        /// Perform a weekly rollover
        /// </summary>
        Weekly,
        /// <summary>
        /// Perform a monthly rollover
        /// </summary>
        Monthly,
        /// <summary>
        /// Perform a yearly rollover
        /// </summary>
        Yearly
    }


    /// <summary>
    /// Type which serves as a base type for all publishers
    /// </summary>
    /// <remarks>
    /// Use this class to publish exceptions. Supports a broad range of publishers, and 
    /// default fetches its publish instructions out of the configurable setup.
    /// A few methods exist to retrieve the "Content" of an exception, so if needed e.g. 
    /// for debugging purposes you can publish yourself too
    /// </remarks>
    public abstract class PublisherBase
    {

        #region Instance Variables
        private string iApplicationNameOverride = "";

        #endregion

        #region Abstact Methods

        /// <summary>
        /// Override this method to realize the publishing of an exception with the current verbosity mode and the requested publish format
        /// </summary>
        /// <param name="exception">Exception to publish</param>
        /// <param name="verboseMode">Verbosity level (High-medium-low)</param>
        /// <param name="publishFormat">Publish Format XML or Text</param>
        protected abstract void DoPublish(Exception exception, VerboseModeEnum verboseMode, PublishFormatEnum publishFormat);

        /// <summary>
        /// Override this method to realize the publishing of a DXC.Technology exception with the current verbosity mode and the requested publish format
        /// </summary>
        /// <param name="exception">Exception to publish</param>
        /// <param name="verboseMode">Verbosity level (High-medium-low)</param>
        /// <param name="publishFormat">Publish Format XML or Text</param>
        protected abstract void DoPublish(ApplicationExceptionBase exception, VerboseModeEnum verboseMode, PublishFormatEnum publishFormat);

        /// <summary>
        /// Override this method to realize the publishing of a single string
        /// </summary>
        /// <param name="message">String to publish</param>
        protected abstract void DoPublish(String message);

        #endregion

        #region Constructors
        protected PublisherBase()
            : base()
        {
        }

        protected PublisherBase(string pApplicationNameOverride)
            : base()
        {
            this.iApplicationNameOverride = pApplicationNameOverride;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Invoke this method to publish aan exception. This will publish the exception if full verbosity and in text format
        /// </summary>
        /// <param name="exception"></param>
        public void Publish(Exception exception)
        {
            Publish(exception, VerboseModeEnum.Full, PublishFormatEnum.Text);
        }

        /// <summary>
        /// Call this method to realize the publishing of an exception with the specified verbosity mode and the specified publish format
        /// This method will inspect the exception and forward it based on whether it is a simple exception or an HP exception
        /// </summary>
        /// <param name="exception">Exception to publish</param>
        /// <param name="verboseMode">Verbosity level (High-medium-low)</param>
        /// <param name="publishFormat">Publish Format XML or Text</param>
        public void Publish(Exception exception, VerboseModeEnum verboseMode, PublishFormatEnum publishFormat)
        {
            if (exception == null) return;
            if (exception.GetType().IsSubclassOf(typeof(ApplicationExceptionBase)))
            {
                this.DoPublish((ApplicationExceptionBase)exception, verboseMode, publishFormat);
            }
            else
            {
                this.DoPublish(exception, verboseMode, publishFormat);
            }
        }

        /// <summary>
        /// Call this method to publish a simple string
        /// </summary>
        /// <param name="message"></param>
        public void Publish(String information)
        {
            this.DoPublish(information);
        }


        /// <summary>
        /// Returns a dataset-representation of the exception. Use the pIncludeInnerExceptions-flag to indicate whether also the internal exceptions need to be published
        /// </summary>
        /// <param name="applicationException">Application exception to 'encapsulate'</param>
        /// <param name="verboseMode">Verbosity level</param>
        //protected SerializableException ExceptionInfoAsSerializableException(Exception exception, VerboseModeEnum verboseMode)
        //{
        //    bool lIncludeInnerExceptions = verboseMode == VerboseModeEnum.Medium || verboseMode == VerboseModeEnum.Full;
        //    bool lIncludeStackTrace = verboseMode == VerboseModeEnum.Full;
        //    return SerializableException.CreateSerializableException(exception, lIncludeInnerExceptions, DateTime.Now, lIncludeStackTrace, iMaxNumberOfInnerExceptions + 1);
        //}

        /// <summary>
        /// Crucial method in deserializing an exception into a dataset. The dataset will have a record for each exception, but the inner exceptions will be 
        /// linked to their parent exception. When publishing an [inner] exception recursively calls itself for the inner exceptions
        /// </summary>
        /// <param name="exception">The exception to publish (possibly an inner exception</param>
        /// <param name="pExceptionsDataSet">The dataset in which the exception needs to be serialized / added</param>
        /// <param name="pIncludeInnerExceptions">Boolean to indicate whether the routine should recursively call itself</param>
        /// <param name="pParentException">Default nothing or the ParentExceptionRow from the DataSet for inner exceptions. This will be used to link inner exceptions to their parent exceptions.</param>
        /// <param name="creationDateTime"></param>
        /// <param name="pIncludeStackTrace">Include stack trace information</param>
        /// <param name="pMaxNumberOfInnerExceptions"></param>
        //private SerializableException CreateSerializableException(Exception exception, bool includeInnerExceptions, DateTime creationDateTime, bool includeStackTrace, int maxNumberOfInnerExceptions)
        //{           
        //    // Check if we did not exceed the max number
        //    if (maxNumberOfInnerExceptions <= 0)
        //    {
        //        SerializableException lMaxNumberOfInnerExceptions = new SerializableException();
        //        lMaxNumberOfInnerExceptions.ExceptionType = "BusinessLogicException";
        //        lMaxNumberOfInnerExceptions.CreationDateTime = creationDateTime;
        //        lMaxNumberOfInnerExceptions.RawExceptionParametersCsv = "";
        //        lMaxNumberOfInnerExceptions.RawExceptionPatternString = "";
        //        lMaxNumberOfInnerExceptions.ExceptionMessage = "The maximum number of exceptions has been exceeded";
        //        return lMaxNumberOfInnerExceptions;
        //    }


        //    SerializableException lSerializableException = new SerializableException();

        //    ApplicationExceptionBase lBrokenRulesException = exception as DXC.Technology.Exceptions.NamedExceptions.BrokenRulesException;
        //    ApplicationExceptionBase lApplicationExceptionBase = exception as ApplicationExceptionBase;
        //    if (lBrokenRulesException != null)
        //    {
        //        lSerializableException.ExceptionFullName = "BrokenRulesException";
        //        lSerializableException.ExceptionNumber = lBrokenRulesException.ExceptionNumber();
        //        lSerializableException.RawExceptionParametersCsv = "";
        //        lSerializableException.RawExceptionPatternString ="";

        //        foreach (SerializableException serializableexception in lBrokenRulesException.AsListOfSerializableExceptions())
        //        {
        //            lSerializableException.InnerSerializableExceptions.Add(serializableexception);
        //        }
        //    }
        //    else
        //    {
        //        if (lApplicationExceptionBase != null)
        //        {
        //            lSerializableException.ExceptionFullName = lApplicationExceptionBase.GetType().ToString(); ;
        //            lSerializableException.ExceptionNumber = lApplicationExceptionBase.ExceptionNumber();
        //            lSerializableException.RawExceptionParametersCsv = DXC.Technology.Utilities.StringArrayHelper.ToCSVString(lApplicationExceptionBase.iExceptionArguments.ToArray());
        //            lSerializableException.RawExceptionPatternString = lApplicationExceptionBase.ExceptionPatternString();
        //        }
        //        else
        //        {

        //            lSerializableException.ExceptionFullName = exception.GetType().ToString();
        //            lSerializableException.ExceptionNumber = -1;
        //            lSerializableException.RawExceptionParametersCsv = "";
        //            lSerializableException.RawExceptionPatternString = "";
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(this.iApplicationNameOverride))
        //    {
        //        lSerializableException.AppDomainName = this.iApplicationNameOverride;
        //    }
        //    else
        //    {
        //        if (exception.Source != null)
        //            lSerializableException.AppDomainName = exception.Source;
        //        else
        //            lSerializableException.AppDomainName = AppDomain.CurrentDomain.FriendlyName;
        //    }
        //    if (exception.StackTrace != null && includeStackTrace)
        //    {
        //        lSerializableException.StackTrace = exception.StackTrace;
        //    }
        //    else
        //    {
        //        lSerializableException.StackTrace = "";
        //    }
        //    lSerializableException.MachineName = Environment.MachineName;
        //    lSerializableException.ExceptionMessage = exception.Message;
        //    lSerializableException.CreationDateTime = creationDateTime;
        //    lSerializableException.SeverityLevel = SeverityLevelEnum.None.ToString();
        //    lSerializableException.HelpLink = exception.HelpLink;
        //    string lUserName;
        //    if (DXC.Technology.Caching.TechnologyContext.Current != null)
        //        lUserName = DXC.Technology.Caching.TechnologyContext.Current.BasicContextInfo.UserName;
        //    else
        //        lUserName = DXC.Technology.Caching.TechnologyContext.Current.BasicContextInfo.UserName;

        //    lSerializableException.ThreadIdentityName = string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "{0} ({1})", System.Threading.Thread.CurrentPrincipal.Identity.Name, lUserName);
        //    lSerializableException.WindowsIdentityName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        //    lSerializableException.ExceptionType = exception.GetType().Name;

        //    if (includeInnerExceptions && exception.InnerException != null)
        //    {
        //        SerializableException lInnerException = CreateSerializableException(exception.InnerException, includeInnerExceptions , creationDateTime, includeStackTrace, --maxNumberOfInnerExceptions);
        //        lSerializableException.InnerSerializableExceptions.Add(lInnerException);
        //    }

        //    //if (pMaxNumberOfInnerExceptions <= 0) break;
        //    //        iCounter += 1000;
        //    //        ApplicationExceptionBase laex = lBrokenRule as ApplicationExceptionBase;
        //    //        if (laex != null)
        //    //        {
        //    //            if (laex.GetType().Name == "AnomalyException" && laex.SeverityLevel() != SeverityLevelEnum.Critical && laex.SeverityLevel() != SeverityLevelEnum.High)
        //    //            {
        //    //                if (pMaxNumberOfInnerExceptions > 2)
        //    //                {
        //    //                    AddExceptionInfo(laex, pExceptionsDataSet, pIncludeInnerExceptions, lSerializableException, exception.CreationDateTime.AddMilliseconds(iCounter), lIncludeStackTrace, --pMaxNumberOfInnerExceptions);
        //    //                }
        //    //                else
        //    //                {
        //    //                    if (pMaxNumberOfInnerExceptions == 2)
        //    //                    {
        //    //                        //Ok there are only two spots left ... take one spot to indicate that not all warnings are shown and ignore all consecutive warning
        //    //                        DXC.Technology.Exceptions.NamedExceptions.AnomalyException lNotAllWarningsShownException
        //    //                            = new DXC.Technology.Exceptions.NamedExceptions.AnomalyException("NAWS", 5, "", "", "", "", "");
        //    //                        AddExceptionInfo(lNotAllWarningsShownException, pExceptionsDataSet, pIncludeInnerExceptions, lSerializableException, exception.CreationDateTime.AddMilliseconds(iCounter), lIncludeStackTrace, --pMaxNumberOfInnerExceptions);
        //    //                    }




        //    return lSerializableException;
        //}

        //private void AddMaxNumberOfExceptionsExceeded(ExceptionsDataSet pExceptionsDataSet, ExceptionsDataSet.ExceptionsRow pParentException, DateTime creationDateTime)
        //{
        //    DXC.Technology.Exceptions.NamedExceptions.BusinessLogicException lMaxNbrExceededException = new DXC.Technology.Exceptions.NamedExceptions.BusinessLogicException("The maximum number of exceptions has been exceeded");
        //    AddExceptionInfo(lMaxNbrExceededException, pExceptionsDataSet, false, pParentException, creationDateTime, false, 1);
        //}

        //private void AddNotAllWarningsShownException(ExceptionsDataSet pExceptionsDataSet, ExceptionsDataSet.ExceptionsRow pParentException, DateTime creationDateTime)
        //{
        //    DXC.Technology.Exceptions.NamedExceptions.AnomalyException lNotAllWarningsShownException
        //        = new DXC.Technology.Exceptions.NamedExceptions.AnomalyException("NAWS", 3, "", "", "", "", "");
        //    AddExceptionInfo(lNotAllWarningsShownException, pExceptionsDataSet, false, pParentException, creationDateTime, false, 1);
        //}

        /// <summary>
        /// Returns a dataset-representation of the exception. Use the pIncludeInnerExceptions-flag to indicate whether also the internal exceptions need to be published
        /// </summary>
        /// <param name="applicationException">Application exception to 'encapsulate'</param>
        /// <param name="verboseMode">Verbosity level</param>
        /// <param name="publishFormat">Publish format</param>
        protected string ExceptionInfo(Exception exception, VerboseModeEnum verboseMode, PublishFormatEnum publishFormat)
        {
            SerializableException lSerializableException = SerializableException.CreateSerializableException(exception, verboseMode);

            if (publishFormat == PublishFormatEnum.XML)
            {
                return DXC.Technology.Objects.SerializationHelper.XmlSerializeToBasicXml(lSerializableException);
            }
            else
            {
                return ExceptionInfoText(lSerializableException, 0, verboseMode);
            }
        }

        /// <summary>
        /// Creates the Exception Info for one row based on verbosity and publish format. Indention level is used to get nice aligned exception files
        /// </summary>
        /// <param name="pExceptionsRow">ExceptionRow to publish</param>
        /// <param name="pIndentionLevel">indent level to indent all lines with</param>
        /// <param name="verboseMode">Verbosity level (Full, medium, low)</param>
        private string ExceptionInfoText(SerializableException serializableException, int pIndentionLevel, VerboseModeEnum verboseMode)
        {
            using (System.IO.StringWriter lStringWriter = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default))
            {
                return ExceptionInfoText(serializableException, pIndentionLevel, verboseMode, lStringWriter);
            }
        }
        /// <summary>
        /// Creates the Exception Info for one row based on verbosity and publish format. Indention level is used to get nice aligned exception files
        /// </summary>
        /// <param name="pExceptionsRow">ExceptionRow to publish</param>
        /// <param name="pIndentionLevel">indent level to indent all lines with</param>
        /// <param name="verboseMode">Verbosity level (Full, medium, low)</param>
        /// <param name="pStringWriter">Target write stream</param>
        /// <returns></returns>
        private string ExceptionInfoText(SerializableException serializableException, int pIndentionLevel, VerboseModeEnum verboseMode, System.IO.StringWriter pStringWriter)
        {

            if (verboseMode == VerboseModeEnum.Simple)
            {
                Indent(pStringWriter, pIndentionLevel);
                pStringWriter.Write(
                    string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "AppDomain: {0} DateTime: {1} Exception: {4} - {2} Severity: {3}",
                    serializableException, serializableException.CreationDateTime, serializableException.ExceptionMessage, serializableException.SeverityLevel, serializableException.ExceptionNumber)
                    );
            }
            else
            {
                Indent(pStringWriter, pIndentionLevel);
                if (pIndentionLevel == 0)
                    pStringWriter.WriteLine("***** EXCEPTION **********************************************************************");
                else
                    pStringWriter.WriteLine("***** INNER EXCEPTION ****************************************************************");

                Indent(pStringWriter, pIndentionLevel);
                pStringWriter.WriteLine(
                    string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "AppDomain: {0} DateTime: {1} Exception type: {2} Severity: {3} AdditionalContext: {4}",
                    serializableException.AppDomainName,
                    serializableException.CreationDateTime.ToString("dd/MM/yyyy HH:mm:ss.fff", DXC.Technology.Utilities.StringFormatProvider.Default),
                    serializableException.ExceptionFullName,
                    serializableException.SeverityLevel,
                    DXC.Technology.Caching.ContextHelper.GetExecutingUseCaseDescription())
                    );

                Indent(pStringWriter, pIndentionLevel);
                pStringWriter.WriteLine(
                    string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "Message: {1} - {0}", serializableException.ExceptionMessage, serializableException.ExceptionNumber)
                    );

                if (verboseMode == VerboseModeEnum.Full)
                {
                    Indent(pStringWriter, pIndentionLevel);
                    pStringWriter.WriteLine(
                        string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "Thrown by: {2} Thread: {1} Machine: {0} Reference: {3}",
                        serializableException.MachineName,
                        serializableException.ThreadIdentityName,
                        serializableException.WindowsIdentityName,
                        serializableException.ReferenceGuid)
                        );

                    Indent(pStringWriter, pIndentionLevel);
                    pStringWriter.WriteLine("Stack Trace:");
                    string lStackTrace = serializableException.StackTrace;
                    string[] lStackTraceLines = lStackTrace.Split("\r\n".ToCharArray());
                    foreach (string lStackTraceLine in lStackTraceLines)
                    {
                        if (!string.IsNullOrEmpty(lStackTraceLine))
                        {
                            Indent(pStringWriter, pIndentionLevel + 1);
                            pStringWriter.WriteLine(lStackTraceLine);
                        }
                    }
                }
                pStringWriter.WriteLine();
            }
            foreach (SerializableException lSerializableException in serializableException.InnerSerializableExceptions)
            {
                ExceptionInfoText(lSerializableException, pIndentionLevel + 1, verboseMode, pStringWriter);
            }
            return pStringWriter.ToString();
        }

        /// <summary>
        /// indents a number of times 5 spaces
        /// </summary>
        /// <param name="pStringWriter">Target write stream</param>
        /// <param name="pIndentionLevel">Indent level =  number of times 5 spaces to indent</param>
        private static void Indent(System.IO.StringWriter pStringWriter, int pIndentionLevel)
        {
            for (int i = 1; i <= pIndentionLevel; i++)
            {
                pStringWriter.Write("        ");
            }
        }


        #endregion
    }

    public class StringPublisher : PublisherBase
    {
        private string iResult = string.Empty;

        protected override void DoPublish(ApplicationExceptionBase exception, VerboseModeEnum verboseMode, PublishFormatEnum publishFormat)
        {
            this.DoPublish(ExceptionInfo(exception, verboseMode, publishFormat));
        }

        protected override void DoPublish(Exception exception, VerboseModeEnum verboseMode, PublishFormatEnum publishFormat)
        {
            this.DoPublish(ExceptionInfo(exception, verboseMode, publishFormat));
        }

        protected override void DoPublish(String message)
        {
            this.iResult = message;
        }

        public string Result
        {
            get
            {
                return iResult;
            }
        }
    }

    /// <summary>
    /// Publishes to a File
    /// </summary>
    public class FilePublisher : PublisherBase
    {
        private string iFilename;
        private RolloverEnum iRollover = RolloverEnum.Never;

        public FilePublisher(string pFilename)
            : base()
        {
            this.iFilename = pFilename;
        }
        public FilePublisher(string pFilename, RolloverEnum pRollover)
            : base()
        {
            this.iFilename = pFilename;
            this.iRollover = pRollover;
        }

        protected override void DoPublish(ApplicationExceptionBase exception, VerboseModeEnum verboseMode, PublishFormatEnum publishFormat)
        {
            this.DoPublish(ExceptionInfo(exception, verboseMode, publishFormat));
        }


        protected override void DoPublish(Exception exception, VerboseModeEnum verboseMode, PublishFormatEnum publishFormat)
        {
            this.DoPublish(ExceptionInfo(exception, verboseMode, publishFormat));
        }

        protected override void DoPublish(String message)
        {
            string lFilename = iFilename;
            string lRolloverIdentifier;
            switch (iRollover)
            {
                case RolloverEnum.Daily:
                    lRolloverIdentifier = DateTime.Now.ToString("yyyyMMdd", DXC.Technology.Utilities.StringFormatProvider.Default);
                    break;
                case RolloverEnum.Weekly:
                    TimeSpan ts = DateTime.Now.Subtract(new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 1));
                    lRolloverIdentifier = Convert.ToString(Math.Floor(Convert.ToDouble((ts.Days - 1) / 7, DXC.Technology.Utilities.DoubleFormatProvider.Default)) + 1, DXC.Technology.Utilities.StringFormatProvider.Default);
                    break;
                case RolloverEnum.Monthly:
                    lRolloverIdentifier = DateTime.Now.ToString("yyyyMM", DXC.Technology.Utilities.StringFormatProvider.Default);
                    break;
                case RolloverEnum.Yearly:
                    lRolloverIdentifier = DateTime.Now.ToString("yyyy", DXC.Technology.Utilities.StringFormatProvider.Default);
                    break;
                default:
                    lRolloverIdentifier = "";
                    break;
            }

            int lPathEndPoint = lFilename.LastIndexOf("\\", Utilities.StringComparisonProvider.Default);
            if (lPathEndPoint > 0)
            {
                System.IO.DirectoryInfo ldi = new System.IO.DirectoryInfo(lFilename.Substring(0, lPathEndPoint));
                if (!ldi.Exists)
                {
                    ldi.Create();
                }
            }

            int lInsertpoint = lFilename.LastIndexOf(".", Utilities.StringComparisonProvider.Default);
            if (lInsertpoint > 0)
            {
                System.Text.StringBuilder lStrBldr = new System.Text.StringBuilder(1024);

                lStrBldr.Append(lFilename.Substring(0, lInsertpoint));
                lStrBldr.Append(lRolloverIdentifier);
                lStrBldr.Append(lFilename.Substring(lInsertpoint));

                lFilename = lStrBldr.ToString();
            }


            System.IO.FileInfo lfi = new System.IO.FileInfo(lFilename);
            if (lfi.Exists)
            {
                if (lfi.Length > 20000000)
                {
                    lfi.MoveTo(String.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "{0}_OLD_{1}", lFilename, DXC.Technology.Utilities.Date.NowHHMMSSFFFString()));
                }
            }


            int RetryCount = 20;
            while (RetryCount > 0)
            {
                try
                {
                    using (System.IO.StreamWriter lsw = new System.IO.StreamWriter(lFilename, true))
                    {

                        lsw.WriteLine(message);
                        return;
                    }
                }
                catch (System.IO.IOException)
                {
                    System.Threading.Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                finally
                {
                    RetryCount -= 1;
                }
            }
            try
            {
                //OK just retry and log it in your own file
                int lNewInsertpoint = lFilename.LastIndexOf(".", Utilities.StringComparisonProvider.Default);
                if (lNewInsertpoint > 0)
                {
                    System.Text.StringBuilder lStrBldr = new System.Text.StringBuilder(1024);

                    lStrBldr.Append(lFilename.Substring(0, lNewInsertpoint));
                    lStrBldr.Append(DXC.Technology.Utilities.Date.NowHHMMSSFFFString());
                    lStrBldr.Append(lFilename.Substring(lNewInsertpoint));

                    lFilename = lStrBldr.ToString();
                }

                using (System.IO.StreamWriter lsw = new System.IO.StreamWriter(lFilename, true))
                {
                    lsw.WriteLine(message);
                }
                return;
            }
            catch
            {
                //While finished un usccessfully! Throw an exception
                throw new DXC.Technology.Exceptions.NamedExceptions.CannotLogException();
            }
        }
    }

}