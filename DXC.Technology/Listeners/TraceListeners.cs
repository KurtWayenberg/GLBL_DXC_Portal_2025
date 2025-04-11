    using System;
    using System.Diagnostics;

namespace DXC.Technology.Listeners
{
    //Kind of [Flags]
    public enum TraceCategoryEnum
    {
        NotSpecified = 0,
        Debug = 0x01,
        Info = 0x02,
        Log = 0x04,
        Audit = 0x08,
        SQL = 0x10,
        DBChange = 0x20,
        Transactions = 0x40,
        Workflow = 0x80,
        Performance = 0x100,
        Configuration = 0x200,
        Deployment = 0x400,
        Translations = 0x800,
        SOAP = 0x1600,
        Custom = 0x3200,
        RuleEngine = 0x6400,
        Transformation = 0x12800,
        Probing = 0x25600,
        UnitTest = 0x51200,
        Batch = 0x102400,
        Dispatching = 0x204800,
        AutoUpdate = 0x409600  //should be removed
    }

    /// <summary>
    /// Listener Detail Level
    /// - High - Highest Level Only
    /// - Medium - Medium Lever or higher only
    /// - Low - All details
    /// </summary>
    public enum TraceDetailLevelEnum
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    [Flags]
    public enum DBChangeActionEnum
    {
        Add = 0x01,
        Update = 0x02,
        Delete = 0x04
    }

    public enum StandardTraceMessageIdEnum
    {
        NotSpecified = 0,
        UseCaseNonRepudiation = 9999,
        DataTierSQLDebugging = 9998,
        SuccessfulLogOn = 9997,
        UnsuccessfulLogOn = 9996,
        UseCaseStartEnd = 9995,
        UseCaseTransaction = 9994,
        DataTierSQLOptimization = 9993,
        IntegrationUseCaseException = 9992,
        IntegrationUseCaseInputContents = 9991,
        IntegrationUseCaseOutputContents = 9990,
        AutoUpdate = 9989,
        DeploymentCleanup = 9988,
        DeploymentMessage = 9987,
        ActivityInvocation = 9986
    }

    /// <summary>
    /// Type defining a structured trace info
    /// </summary>
    /// <remarks>
    /// This type can be used to construct/supply a customized trace info which 
    /// in turn can be supplied to the standard .NET tracing runtime as such.
    /// <seealso cref="System.Diagnostics.Trace"/>
    /// </remarks>
    public class TraceMessage
    {
        private string iInfo = string.Empty;
        private int iNr;
        private string iSource = string.Empty;
        private TraceCategoryEnum iTraceCategory = TraceCategoryEnum.Log;
        private TraceDetailLevelEnum iTraceDetailLevel = TraceDetailLevelEnum.Low;
        private DateTime iTimestamp = DateTime.Now;
        private int iThreadId = System.Threading.Thread.CurrentThread.GetHashCode();
        private string iCustomCategory = string.Empty;
        private System.Collections.Specialized.StringDictionary iCustomProperties = new System.Collections.Specialized.StringDictionary();

        #region Constructors
        /// <summary>
        /// Fast Trace Info Constructor
        /// </summary>
        /// <param name="info">Trace Info</param>
        public TraceMessage(string info)
        {
            Info = info;
        }
        /// <summary>
        /// Fast Trace Info Constructor
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="pNr">Info Number</param>
        public TraceMessage(string info, int pNr)
        {
            Nr = pNr;
            Info = info;
        }
        /// <summary>
        /// Fast Trace Info Constructor
        /// </summary>
        /// <param name="info">Trace Info</param>       
        /// <param name="pTraceCategory">Standard Trace Category</param>
        public TraceMessage(string info, TraceCategoryEnum pTraceCategory)
        {

            Info = info;

            TraceCategory = pTraceCategory;
        }

        /// <summary>
        /// Fast Trace Info Constructor
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="pNr">Info Number</param>
        /// <param name="pSource">Source of tracing</param>
        public TraceMessage(string info, int pNr, string pSource)
        {
            Nr = pNr;
            Info = info;
            Source = pSource;
        }

        /// <summary>
        /// Fast Trace Info Constructor
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="pNr">Info Number</param>
        /// <param name="pSource">Source of tracing</param>
        /// <param name="pTraceCategory">Standard Trace Category</param>
        public TraceMessage(string info, int pNr, string pSource, TraceCategoryEnum pTraceCategory)
        {
            Nr = pNr;
            Info = info;
            Source = pSource;
            TraceCategory = pTraceCategory;
        }

        /// <summary>
        /// Fast Trace Info Constructor
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="pNr">Info Number</param>
        /// <param name="pSource">Source of tracing</param>
        /// <param name="pTraceCategory">Standard Trace Category</param>
        public TraceMessage(string info, int pNr, string pSource, TraceCategoryEnum pTraceCategory, TraceDetailLevelEnum pTraceDetailLevel)
        {
            Nr = pNr;
            Info = info;
            Source = pSource;
            TraceCategory = pTraceCategory;
            TraceDetailLevel = pTraceDetailLevel;
        }
        /// <summary>
        /// Fast Trace Info Constructor
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="pNr">Info Number</param>
        /// <param name="pSource">Source of tracing</param>
        /// <param name="pCustomCategory">Custom Trace Category - ignored if Standard Trace Category != "Custom"</param>
        public TraceMessage(string info, int pNr, string pSource, string pCustomCategory)
        {
            Nr = pNr;
            Info = info;
            Source = pSource;
            TraceCategory = TraceCategoryEnum.Custom;
            CustomCategory = pCustomCategory;
        }

        /// <summary>
        /// Fast Trace Info Constructor
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="pNr">Info Number</param>
        /// <param name="pSource">Source of tracing</param>
        /// <param name="pCustomCategory">Custom Trace Category - ignored if Standard Trace Category != "Custom"</param>
        public TraceMessage(string info, int pNr, string pSource, string pCustomCategory, TraceDetailLevelEnum pTraceDetailLevel)
        {
            Nr = pNr;
            Info = info;
            Source = pSource;
            TraceCategory = TraceCategoryEnum.Custom;
            CustomCategory = pCustomCategory;
            TraceDetailLevel = pTraceDetailLevel;
        }
        #endregion

        #region Properties

        public string TraceCategoryName
        {
            get
            {
                if (this.TraceCategory == TraceCategoryEnum.Custom)
                    return this.CustomCategory;
                else
                    return this.TraceCategory.ToString();
            }
        }
        public string Info
        {
            get
            {
                return iInfo;
            }
            set
            {
                iInfo = value;
            }
        }
        public int Nr
        {
            get
            {
                return iNr;
            }
            set
            {
                iNr = value;
            }
        }
        public string Source
        {
            get
            {
                return iSource;
            }
            set
            {
                iSource = value;
            }
        }
        public TraceCategoryEnum TraceCategory
        {
            get
            {
                return iTraceCategory;
            }
            set
            {
                iTraceCategory = value;
            }
        }
        public TraceDetailLevelEnum TraceDetailLevel
        {
            get
            {
                return iTraceDetailLevel;
            }
            set
            {
                iTraceDetailLevel = value;
            }
        }
        public DateTime Timestamp
        {
            get
            {
                return iTimestamp;
            }
            set
            {
                iTimestamp = value;
            }
        }
        public int ThreadId
        {
            get
            {
                return iThreadId;
            }
            set
            {
                iThreadId = value;
            }
        }
        public string UserName
        {
            get
            {
                return DXC.Technology.Caching.ContextHelper.GetUserName();
            }
        }
        public string CustomCategory
        {
            get
            {
                return iCustomCategory;
            }
            set
            {
                iCustomCategory = value;
            }
        }

        public System.Collections.Specialized.StringDictionary CustomProperties
        {
            get
            {
                return iCustomProperties;
            }
            set
            {
                iCustomProperties = value;
            }
        }

        public string ThreadIdAndCorrelationID
        {
            get
            {
                return DXC.Technology.Caching.ContextHelper.GetUserIp();
            }
        }

        public string GetCustomPropertiesAsString(string pPrefixIfProperties, string pValueSeperator, string pPropertySeperator, string pSuffixIfProperties)
        {
            if (iCustomProperties.Count == 0)
                return string.Empty;
            else
            {
                System.IO.StringWriter lsw = new System.IO.StringWriter(DXC.Technology.Utilities.CultureInfoProvider.Default);
                lsw.Write(pPrefixIfProperties);
                bool lFirst = true;
                foreach (string lCustomProperty in CustomProperties)
                {
                    if (!lFirst)
                        lsw.Write(pPropertySeperator);
                    lFirst = false;
                    lsw.Write(lCustomProperty);
                    lsw.Write(pValueSeperator);
                    lsw.Write(CustomProperties[lCustomProperty]);
                }
                lsw.Write(pSuffixIfProperties);
                return lsw.ToString();
            }
        }
        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}(|{1}|)|{2}|{3}|{4}|{5}|{6}|{7}",
                this.ThreadIdAndCorrelationID, this.UserName, this.Timestamp.ToString("yyyy/MM/dd HH:mm:ss.fff", DXC.Technology.Utilities.StringFormatProvider.Default),
                this.TraceCategoryName, this.Nr.ToString("0000", DXC.Technology.Utilities.StringFormatProvider.Default),
                this.Info, this.Source,
                this.GetCustomPropertiesAsString("--", ":", "-", ""));
        }

        #endregion
    }
}