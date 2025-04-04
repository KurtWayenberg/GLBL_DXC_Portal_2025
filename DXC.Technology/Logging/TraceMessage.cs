using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Logging
{
    /// <summary>
    /// Enum representing various trace categories.
    /// </summary>
    [Flags]
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
        Dispatching = 0x204800
    }

    /// <summary>
    /// Enum representing listener detail levels.
    /// </summary>
    public enum TraceDetailLevelEnum
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    /// <summary>
    /// Enum representing database change actions.
    /// </summary>
    [Flags]
    public enum DBChangeActionEnum
    {
        Add = 0x01,
        Update = 0x02,
        Delete = 0x04
    }

    /// <summary>
    /// Enum representing standard trace message IDs.
    /// </summary>
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
    /// Type defining a structured trace message.
    /// </summary>
    /// <remarks>
    /// This type can be used to construct/supply a customized trace info which 
    /// in turn can be supplied to the standard .NET tracing runtime as such.
    /// <seealso cref="System.Diagnostics.Trace"/>
    /// </remarks>
    public class TraceMessage
    {
        #region Instance Fields

        /// <summary>
        /// Thread ID associated with the trace message.
        /// </summary>
        private int threadId = System.Threading.Thread.CurrentThread.GetHashCode();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessage"/> class with trace info.
        /// </summary>
        /// <param name="info">Trace Info</param>
        public TraceMessage(string info)
        {
            Info = info;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessage"/> class with trace info and number.
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="nr">Info Number</param>
        public TraceMessage(string info, int nr)
        {
            Nr = nr;
            Info = info;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessage"/> class with trace info and category.
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="traceCategory">Standard Trace Category</param>
        public TraceMessage(string info, TraceCategoryEnum traceCategory)
        {
            Info = info;
            TraceCategory = traceCategory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessage"/> class with trace info, number, and source.
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="nr">Info Number</param>
        /// <param name="source">Source of tracing</param>
        public TraceMessage(string info, int nr, string source)
        {
            Nr = nr;
            Info = info;
            Source = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessage"/> class with trace info, number, source, and category.
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="nr">Info Number</param>
        /// <param name="source">Source of tracing</param>
        /// <param name="traceCategory">Standard Trace Category</param>
        public TraceMessage(string info, int nr, string source, TraceCategoryEnum traceCategory)
        {
            Nr = nr;
            Info = info;
            Source = source;
            TraceCategory = traceCategory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessage"/> class with trace info, number, source, category, and detail level.
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="nr">Info Number</param>
        /// <param name="source">Source of tracing</param>
        /// <param name="traceCategory">Standard Trace Category</param>
        /// <param name="traceDetailLevel">Trace Detail Level</param>
        public TraceMessage(string info, int nr, string source, TraceCategoryEnum traceCategory, TraceDetailLevelEnum traceDetailLevel)
        {
            Nr = nr;
            Info = info;
            Source = source;
            TraceCategory = traceCategory;
            TraceDetailLevel = traceDetailLevel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessage"/> class with trace info, number, source, and custom category.
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="nr">Info Number</param>
        /// <param name="source">Source of tracing</param>
        /// <param name="customCategory">Custom Trace Category - ignored if Standard Trace Category != "Custom"</param>
        public TraceMessage(string info, int nr, string source, string customCategory)
        {
            Nr = nr;
            Info = info;
            Source = source;
            TraceCategory = TraceCategoryEnum.Custom;
            CustomCategory = customCategory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceMessage"/> class with trace info, number, source, custom category, and detail level.
        /// </summary>
        /// <param name="info">Trace Info</param>
        /// <param name="nr">Info Number</param>
        /// <param name="source">Source of tracing</param>
        /// <param name="customCategory">Custom Trace Category - ignored if Standard Trace Category != "Custom"</param>
        /// <param name="traceDetailLevel">Trace Detail Level</param>
        public TraceMessage(string info, int nr, string source, string customCategory, TraceDetailLevelEnum traceDetailLevel)
        {
            Nr = nr;
            Info = info;
            Source = source;
            TraceCategory = TraceCategoryEnum.Custom;
            CustomCategory = customCategory;
            TraceDetailLevel = traceDetailLevel;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the trace category.
        /// </summary>
        public string TraceCategoryName => TraceCategory == TraceCategoryEnum.Custom ? CustomCategory : TraceCategory.ToString();

        /// <summary>
        /// Gets or sets the trace info.
        /// </summary>
        public string Info { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the trace number.
        /// </summary>
        public int Nr { get; set; }

        /// <summary>
        /// Gets or sets the source of the trace.
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the trace category.
        /// </summary>
        public TraceCategoryEnum TraceCategory { get; set; } = TraceCategoryEnum.Log;

        /// <summary>
        /// Gets or sets the trace detail level.
        /// </summary>
        public TraceDetailLevelEnum TraceDetailLevel { get; set; } = TraceDetailLevelEnum.Low;

        /// <summary>
        /// Gets or sets the timestamp of the trace.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the thread ID associated with the trace.
        /// </summary>
        public int ThreadId
        {
            get => threadId;
            set => threadId = value;
        }

        /// <summary>
        /// Gets the username associated with the trace.
        /// </summary>
        public string UserName => DXC.Technology.Caching.ContextHelper.GetUserName();

        /// <summary>
        /// Gets or sets the custom trace category.
        /// </summary>
        public string CustomCategory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the custom properties associated with the trace.
        /// </summary>
        public StringDictionary CustomProperties { get; set; } = new();

        /// <summary>
        /// Gets the thread ID and correlation ID.
        /// </summary>
        public string ThreadIdAndCorrelationID => string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "{0}:{1}", ThreadId, DXC.Technology.Caching.ContextHelper.GetServiceGuid());

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts custom properties to a formatted string.
        /// </summary>
        /// <param name="prefixIfProperties">Prefix to use if properties exist.</param>
        /// <param name="valueSeparator">Separator between property name and value.</param>
        /// <param name="propertySeparator">Separator between properties.</param>
        /// <param name="suffixIfProperties">Suffix to use if properties exist.</param>
        /// <returns>Formatted string of custom properties.</returns>
        public string GetCustomPropertiesAsString(string prefixIfProperties, string valueSeparator, string propertySeparator, string suffixIfProperties)
        {
            if (CustomProperties.Count == 0)
                return string.Empty;

            using StringWriter stringWriter = new(DXC.Technology.Utilities.CultureInfoProvider.Default);
            stringWriter.Write(prefixIfProperties);
            bool first = true;
            foreach (string customProperty in CustomProperties.Keys)
            {
                if (!first)
                    stringWriter.Write(propertySeparator);
                first = false;
                stringWriter.Write(customProperty);
                stringWriter.Write(valueSeparator);
                stringWriter.Write(CustomProperties[customProperty]);
            }
            stringWriter.Write(suffixIfProperties);
            return stringWriter.ToString();
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Converts the trace message to a string representation.
        /// </summary>
        /// <returns>String representation of the trace message.</returns>
        public override string ToString()
        {
            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}(|{1}|)|{2}|{3}|{4}|{5}|{6}|{7}",
                ThreadIdAndCorrelationID, UserName, Timestamp.ToString("yyyy/MM/dd HH:mm:ss.fff", DXC.Technology.Utilities.StringFormatProvider.Default),
                TraceCategoryName, Nr.ToString("0000", DXC.Technology.Utilities.StringFormatProvider.Default),
                Info, Source,
                GetCustomPropertiesAsString("--", ":", "-", ""));
        }

        #endregion
    }

    /// <summary>
    /// Type defining a structured database change message.
    /// </summary>
    public class DBChangeMessage
    {
        #region Instance Fields

        /// <summary>
        /// Thread ID associated with the database change message.
        /// </summary>
        private int threadId = System.Threading.Thread.CurrentThread.GetHashCode();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DBChangeMessage"/> class with database change action, table name, and key.
        /// </summary>
        /// <param name="dbChangeAction">Database change action.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="key">Key associated with the change.</param>
        public DBChangeMessage(DBChangeActionEnum dbChangeAction, string tableName, string key)
        {
            DBChangeAction = dbChangeAction;
            TableName = tableName;
            Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBChangeMessage"/> class with database change action, table name, key, and parent key.
        /// </summary>
        /// <param name="dbChangeAction">Database change action.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="key">Key associated with the change.</param>
        /// <param name="keyParent">Parent key associated with the change.</param>
        public DBChangeMessage(DBChangeActionEnum dbChangeAction, string tableName, string key, string keyParent)
        {
            DBChangeAction = dbChangeAction;
            TableName = tableName;
            Key = key;
            KeyParent = keyParent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBChangeMessage"/> class with database change action, table name, key, field name, old value, and new value.
        /// </summary>
        /// <param name="dbChangeAction">Database change action.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="key">Key associated with the change.</param>
        /// <param name="field">Field name associated with the change.</param>
        /// <param name="oldValue">Old value of the field.</param>
        /// <param name="newValue">New value of the field.</param>
        public DBChangeMessage(DBChangeActionEnum dbChangeAction, string tableName, string key, string field, string oldValue, string newValue)
        {
            DBChangeAction = dbChangeAction;
            TableName = tableName;
            Key = key;
            FieldName = field;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBChangeMessage"/> class with database change action, table name, key, parent key, field name, old value, and new value.
        /// </summary>
        /// <param name="dbChangeAction">Database change action.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="key">Key associated with the change.</param>
        /// <param name="keyParent">Parent key associated with the change.</param>
        /// <param name="field">Field name associated with the change.</param>
        /// <param name="oldValue">Old value of the field.</param>
        /// <param name="newValue">New value of the field.</param>
        public DBChangeMessage(DBChangeActionEnum dbChangeAction, string tableName, string key, string keyParent, string field, string oldValue, string newValue)
        {
            DBChangeAction = dbChangeAction;
            TableName = tableName;
            Key = key;
            KeyParent = keyParent;
            FieldName = field;
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the trace category.
        /// </summary>
        public string TraceCategoryName => TraceCategory.ToString();

        /// <summary>
        /// Gets or sets the database change action.
        /// </summary>
        public DBChangeActionEnum DBChangeAction { get; set; } = DBChangeActionEnum.Add;

        /// <summary>
        /// Gets or sets the trace category.
        /// </summary>
        public TraceCategoryEnum TraceCategory { get; set; } = TraceCategoryEnum.DBChange;

        /// <summary>
        /// Gets or sets the timestamp of the database change.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the thread ID associated with the database change.
        /// </summary>
        public int ThreadId
        {
            get => threadId;
            set => threadId = value;
        }

        /// <summary>
        /// Gets or sets the username associated with the database change.
        /// </summary>
        public string UserName { get; set; } = DXC.Technology.Caching.ContextHelper.GetUserName();

        /// <summary>
        /// Gets or sets the name of the table associated with the database change.
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the key associated with the database change.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parent key associated with the database change.
        /// </summary>
        public string KeyParent { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field name associated with the database change.
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the old value of the field associated with the database change.
        /// </summary>
        public string OldValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the new value of the field associated with the database change.
        /// </summary>
        public string NewValue { get; set; } = string.Empty;

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Converts the database change message to a string representation.
        /// </summary>
        /// <returns>String representation of the database change message.</returns>
        public override string ToString()
        {
            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9} {10}", ThreadId, UserName, Timestamp.ToString("yyyy/MM/dd HH:mm:ss.fff", DXC.Technology.Utilities.StringFormatProvider.Default), TraceCategoryName, DBChangeAction, TableName, Key, KeyParent, FieldName, OldValue, NewValue);
        }

        #endregion
    }
}