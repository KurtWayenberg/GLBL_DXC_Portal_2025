using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using DXC.Technology.Enumerations;
using Microsoft.Data.SqlClient;

namespace DXC.Technology.Exceptions.NamedExceptions
{

    public enum CompareEnum
    {
        SmallerThan = -2,
        SmallerThanOrEqualTo = -1,
        EqualTo = 0,
        LargeThanOrEqualTo = 1,
        LargerThan = 2
    }

    public enum ExceptionPatternEnum
    {
        P0001_TextContent_1BasedOnStandardText_2CanNotBeAddedToTextContent_3WithContentBasedOnStandardText_4,
        P0002_User_1HasNoPermissionForConfidentialDocument_2,
        P0003_UserHasNo_1PermissionForTextContent_2,
        P0004_UserHasNoPermissionForConfidentialDocument_1,
        P0005_UserHasNo_1PermissionForDocumentsOfType_2,
        P0006_UserHasNo_1PermissionForNonPublishedDocumentsOfType_2ForLegalEntity_3,
        P0007_UserOfLegalEntity_1HasNo_2PermissionForDocumentsOfLegalEntity_3,
        P0008_AddingA_1TextcontentToA_2IsNotAllowed,
        P0009_UserHasNo_1PermissionForNonApprovedCataloguesForLegalEntity_2,
        P0010_UserHasNo_1PermissionForCatalogues,
        P0011_UserOfLegalEntity_1HasNo_2PermissionForCataloguesOfLegalEntity_3,
        P0012_DesignNumber_1AlreadyExistsChangeNumberAndSaveAgain,
        P0013_DesignNumbers_1AlreadyExistChangeNumbersAndSaveAgain,
        P0014_DesignNumber_1MayNotBeChangedInto_2BecauseItIsBasedOnAStandardDocument,
        P0015_DesignNumber_1MayNotBeChangedInto_2Because_3AlreadyExistsInBaseDocument_4,
        P0016_TextContentMayNotBeDeletedBecauseItIsStillReferencedInTextCont_1,
        P0017_TextContentMayNotBeDeletedBecauseItIsStillReferencedInTextContents_1,
        P0018_TheStandardDocumentWasNotApprovedYet,
        P0019_TextContent_1WasAlreadyChangedByUser_2,
        P0020_TextContents_1WereAlreadyChangedByUser_2,
        P0021_TextContents_1WereAlreadyChangedByUsers_2,
        P0022_TextContentDocument_1WasAlreadyChangedByUser_2,
        P0023_Catalogue_1WasAlreadyChangedByUser_2,
        P0024_DesignNumber_1AlreadyExistsInDocumentAndMayNotBeCopiedTwice,
        P0025_StatusChangedToPublishedConfirmWithTheSaveButtonOrHitCancel,
        P0026_UserHasNoPermissionForDocument_1,
        P0027_Field_1IsMandatory,
        P0028_TheCurrentStatusOfTheCatalogueDoesNotAllowToDelete,
        P0029_CatalogueCanNotBeDeletedItIsStillLinkedToDocuments,
        P0030_TheCurrentStatusOfTheDocumentDoesNotAllowToDelete,
        P0031_DocumentCanNotBeDeletedItHasLinkedDocuments,
        P0032_InvalidExcelFormatForCatalogue,
        P0033_PleaseSelectAFileFirst,
        P0034_Version_1AlreadyExistsForDocument,
        P0035_TextContent_1CanNotBeAddedTo_2InDocument_3,
        P0036_TextContents_1CanNotBeAddedTo_2InDocument_3,
        P0037_PleaseSelectAtLeastOneTextContentToAddTo_1InDocument_2,
        P0038_PleaseReduceTheSelectionOf_1TextContentsToAMaximumOf_2TextContents,
        P0039_Template_1WasAlreadyChangedByUser_2,
        P0040_OnlyFilesWithExtension_1CanBeUploaded,
        P0041_DownloadTemplateForDocumentIsNotDefinedYet,
        P0042_User_1HasNo_2PermissionForTask_3_4,
        P0043_Field_1IsTooLongLengthOfValue_2Is_3MaximumLengthIs_4,
        P0044_TextContent_1MayNotBeMovedBecauseItIsBasedOnStandardDocument_2
    }

    /// <summary>
    /// Client Disconnected exception
    /// </summary>

    public class ClientDisconnectedException : ApplicationExceptionBase
    {
        public ClientDisconnectedException(string pActionTakenMessage)
            : base(pActionTakenMessage)
        { }
        public override String ExceptionPatternString()
        {
            return "Client was disconnected. {0} .";
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
        public override int ExceptionNumber()
        {
            return -488;
        }
        public string FieldName
        {
            get { return this.ExceptionArguments[0]; }
        }
        public string FieldValue
        {
            get { return this.ExceptionArguments[1]; }
        }
    }

    /// <summary>
    /// Duplicate info exception
    /// </summary>

    public class DuplicateInfoException : ApplicationExceptionBase
    {
        public DuplicateInfoException(string fieldName, string fieldValue)
            : base(fieldName, fieldValue)
        { }
        public override String ExceptionPatternString()
        {
            return "Duplicate value found in {0} with value {1}.";
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
        public override int ExceptionNumber()
        {
            return -138;
        }
        public string FieldName
        {
            get { return this.ExceptionArguments[0]; }
        }
        public string FieldValue
        {
            get { return this.ExceptionArguments[1]; }
        }
    }

    /// <summary>
    /// Illegal Interfaced Exception
    /// </summary>

    public class IllegalInterfacedCustomTypeException : ApplicationExceptionBase
    {
        public IllegalInterfacedCustomTypeException(string typeName, string interfaceName)
            : base(typeName, interfaceName)
        { }
        public override String ExceptionPatternString()
        {
            return "Custom type '{0}' cannot be instantiated or it does not implement interface '{1}'";
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
        public override bool IsSecurityException()
        {
            return true;
        }
        public override int ExceptionNumber()
        {
            return -272;
        }

        public string TypeName
        {
            get { return this.ExceptionArguments[0]; }
        }
        public string InterfaceName
        {
            get { return this.ExceptionArguments[1]; }
        }
    }

    /// <summary>
    /// Throw this exception to indicate a method still shuld be implemented
    /// </summary>

    public class ToBeImplementedException : ApplicationExceptionBase
    {
        public ToBeImplementedException(string className, string methodName)
            : base(className, methodName)
        {
        }
        public ToBeImplementedException()
            : base("", "")
        {
        }
        public override String ExceptionPatternString()
        {
            return "Method {1} in Class {0} is not implemented yet";
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
        public override int ExceptionNumber()
        {
            return -137;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain field in a table in a dataset should be specified but is null
    /// </summary>

    public class MandatoryFieldNotSpecifiedException : ApplicationExceptionBase
    {
        public MandatoryFieldNotSpecifiedException(string tableName, string fieldName)
            : base(tableName, fieldName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Field {1} in table {0} is mandatory";
        }
        public override int ExceptionNumber()
        {
            return -131;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain field in a table in a dataset is out of a specified range
    /// </summary>

    public class FieldOutOfRangeException : ApplicationExceptionBase
    {
        public FieldOutOfRangeException(string tableName, string fieldName, object value, object rangeFrom, object rangeTo)
            : base(tableName, fieldName, value, rangeFrom, rangeTo)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Field {1} in tabel {0} with value {2} is out of range. Valid range is from {3} to {4}";
        }
        public override int ExceptionNumber()
        {
            return -156;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a parameter is out of a specified range
    /// </summary>

    public class ParameterOutOfRangeException : ApplicationExceptionBase
    {
        public ParameterOutOfRangeException(string parameterName, object value, object rangeFrom, object rangeTo)
            : base(parameterName, value, rangeFrom, rangeTo)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Parameter {0} with value {1} is out of range. Valid range is from {2} to {3}";
        }
        public override int ExceptionNumber()
        {
            return -183;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a parameter is out of a specified range
    /// </summary>

    public class PeriodExpiredException : ApplicationExceptionBase
    {
        public PeriodExpiredException(string periodDescription, DateTime periodStart, DateTime periodEnd, DateTime expiredDate)
            : base(periodDescription, periodStart, periodEnd, expiredDate)
        {
        }
        public override String ExceptionPatternString()
        {
            return "The given date {3} is outside the {0} period {1}{2}.";
        }
        public override int ExceptionNumber()
        {
            return -184;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain selected value is not one of the expected values for a certain context. 
    /// </summary>

    public class SelectedValueInvalidValueException : ApplicationExceptionBase
    {
        public SelectedValueInvalidValueException(string selectionType, string selectedValue, string csvPossibleValues)
            : base(selectionType, selectedValue, csvPossibleValues)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0} with value {1} is not allowed in this context. Valid values that apply here are: {2}";
        }
        public override int ExceptionNumber()
        {
            return -185;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }


    }

    /// <summary>
    /// Throw this exception to indicate a certain field in a certain table is not one of the predetermined values
    /// </summary>

    public class FieldInvalidValueException : ApplicationExceptionBase
    {
        public FieldInvalidValueException(string tableName, string fieldName, object value, string csvPossibleValues)
            : base(tableName, fieldName, value, csvPossibleValues)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Field {1} in table {0} with value {2} is out of range. Valid values are: {3}";
        }
        public override int ExceptionNumber()
        {
            return -203;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain parameter is not one of the oredetermined values
    /// </summary>

    public class ParameterInvalidValueException : ApplicationExceptionBase
    {
        public ParameterInvalidValueException(string parameterName, object value, string csvPossibleValues)
            : base(parameterName, value, csvPossibleValues)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Parameter {0} with value {1} is out of range. Valid values are: {2}";
        }
        public override int ExceptionNumber()
        {
            return -207;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a parameter is invalid
    /// </summary>

    public class ParameterInvalidException : ApplicationExceptionBase
    {
        public ParameterInvalidException(string parameterName, object value, string additionalInformation)
            : base(parameterName, value, additionalInformation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Parameter {0} with value {1} is invalid: {2}";
        }
        public override int ExceptionNumber()
        {
            return -220;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a parameter is invalid
    /// </summary>

    public class ParameterInconsistentException : ApplicationExceptionBase
    {
        public ParameterInconsistentException(string parameterName, object value, string additionalInformation)
            : base(parameterName, value, additionalInformation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Parameter {0} with value {1} is inconsistent: {2}";
        }
        public override int ExceptionNumber()
        {
            return -219;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a record is (already) locked
    /// </summary>

    public class RecordLockedException : ApplicationExceptionBase
    {
        public RecordLockedException(string table, object key)
            : base(table, key)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Record with key {1} of table {0} is Locked";
        }
        public override int ExceptionNumber()
        {
            return -265;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a record is (already) locked
    /// </summary>

    public class RecordLockedByOtherUserException : ApplicationExceptionBase
    {
        public RecordLockedByOtherUserException(string table, object key, string user)
            : base(table, key, user)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Record with key {1} of table {0} is Locked";
        }
        public override int ExceptionNumber()
        {
            return -262;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a record is (already) unlocked
    /// </summary>
    public class RecordUnlockedException : ApplicationExceptionBase
    {
        public RecordUnlockedException(string table, object key)
            : base(table, key)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Record with key {1} of table {0} is unlocked";
        }
        public override int ExceptionNumber()
        {
            return -267;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain field from a certain table is invalid
    /// </summary>

    public class FieldInvalidException : ApplicationExceptionBase
    {
        public FieldInvalidException(string tableName, string fieldName, object value, string additionalInformation)
            : base(tableName, fieldName, value, additionalInformation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Field {1} with value {2} in table {0} is invalid: {3}";
        }
        public override int ExceptionNumber()
        {
            return -221;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }
    /// <summary>
    /// Throw this exception to indicate the specified range overlaps with existing ranges
    /// </summary>

    public class RangeOverlapsException : ApplicationExceptionBase
    {
        public RangeOverlapsException(string pRangeItem)
            : base(pRangeItem)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Range for {0} contains duplicates or overlaps.";
        }
        public override int ExceptionNumber()
        {
            return -255;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Warning;
        }
    }
    /// <summary>
    /// Throw this exception to indicate an object of a certain type with a certain value/key is not found in a certain collection 
    /// </summary>

    public class ObjectNotFoundException : ApplicationExceptionBase
    {
        public ObjectNotFoundException(string objectType, object searchValue, System.Array array)
            : base(objectType, searchValue, array)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Object of type {0} with searchValue {1} not found in array with values {2}";
        }
        public override int ExceptionNumber()
        {
            return -224;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain record was not found in the table using the specified search criterium 
    /// </summary>

    public class RecordNotFoundException : ApplicationExceptionBase
    {
        public RecordNotFoundException(string tableName, string searchCriterium, object searchValue)
            : base(tableName, searchCriterium, searchValue)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Record not found in table {0} for search criterium {1} with value {2}";
        }
        public override int ExceptionNumber()
        {
            return -266;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }


    /// <summary>
    /// Throw this exception to indicate you (or SQL) detected a concurrency exception
    /// </summary>

    public class ConcurrencyViolationException : ApplicationExceptionBase
    {
        public ConcurrencyViolationException(string message, string commandText)
            : base(message, commandText)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Concurrency Exception - {0} - Executing Command: {1}";
        }
        public override int ExceptionNumber()
        {
            return -275;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate you (or SQL) detected a concurrency exception
    /// </summary>

    public class ConcurrencyException : ApplicationExceptionBase
    {
        private string iGuiltyFields = string.Empty;
        public ConcurrencyException()
            : base()
        {
        }
        //public ConcurrencyException(System.Data.OracleClient.OracleDataAdapter pDataAdapter, System.Data.DataRow[] dataRows)
        //    : base()
        //{
        //    iDataAdapter = pDataAdapter;
        //    iApdataRows = dataRows;

        //    try
        //    {
        //        if (iApdataRows.Length > 0)
        //        {
        //            FindGuiltyFields();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Text.StringBuilder lMsg = new System.Text.StringBuilder();
        //        lMsg.AppendLine("Exception in 'FindGuiltyFields()'");
        //        lMsg.AppendLine("Message:" + ex.Message);
        //        lMsg.AppendLine("StackTrace:" + ex.StackTrace);
        //        iGuiltyFields = lMsg.ToString();
        //    }
        //}
        public override String ExceptionPatternString()
        {
            return "Concurrency Exception" + " " + iGuiltyFields;
        }
        public override int ExceptionNumber()
        {
            return -278;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
        //private void FindGuiltyFields()
        //{
        //    string lSelectStatement;
        //    System.Data.DataRow lDatabaseRow;
        //    System.Data.DataTable lDatabaseTable;
        //    DXC.Technology.Data.DynamicOracleDataHelper lddhOracle;
        //    DXC.Technology.Data.OracleDataHelper lOracleDataHelper;
        //    System.Data.DataSet lDataSet;
        //    System.Text.StringBuilder lGuiltyFields = new System.Text.StringBuilder();

        //    lGuiltyFields.AppendLine("");
        //    if (iApdataRows.Length > 0)
        //    {
        //        lGuiltyFields.AppendLine("TableName: '" + iApdataRows[0].Table.TableName + "'");
        //    }
        //    lGuiltyFields.AppendLine("Concurrency Problems:");

        //    lOracleDataHelper = new DXC.Technology.Data.OracleDataHelper();

        //    foreach (System.Data.DataRow lAppRow in iApdataRows)
        //    {
        //        lddhOracle = new DXC.Technology.Data.DynamicOracleDataHelper(iDataAdapter, iApdataRows[0].Table);

        //        lGuiltyFields.AppendLine("--- Row ---");

        //        lSelectStatement = lddhOracle.ResultDataAdapter.SelectCommand.CommandText;
        //        if (lSelectStatement.IndexOf("WHERE", Utilities.StringComparisonProvider.Default) > -1)
        //        {
        //            lSelectStatement = lSelectStatement.Substring(0, lSelectStatement.IndexOf("WHERE", Utilities.StringComparisonProvider.Default));
        //        }
        //        lddhOracle.ResultDataAdapter.SelectCommand.CommandText = lSelectStatement;

        //        // Compose SelectStatement
        //        foreach (System.Data.DataColumn lColumn in lAppRow.Table.PrimaryKey)
        //        {
        //            switch (lColumn.DataType.FullName)
        //            {
        //                case "System.Int64":
        //                case "System.Int32":
        //                case "System.Int16":
        //                    lddhOracle.AddSelectionNumericColumn(
        //                        lColumn.ColumnName,
        //                        DXC.Technology.Data.NumericComparisonOperationEnum.EqualTo,
        //                        lAppRow[lColumn.ColumnName].ToString());
        //                    break;
        //                default:
        //                    lddhOracle.AddSelectionTextColumn(
        //                        lColumn.ColumnName,
        //                        DXC.Technology.Data.StringComparisonOperationEnum.EqualTo,
        //                        lAppRow[lColumn.ColumnName].ToString());
        //                    break;
        //            }
        //            lGuiltyFields.AppendLine(string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "Key: {0} = '{1}' ", lColumn.ColumnName, lAppRow[lColumn.ColumnName].ToString()));
        //        }

        //        lDataSet = new System.Data.DataSet();
        //        lDataSet.Locale = DXC.Technology.Utilities.CultureInfoProvider.Default;
        //        lOracleDataHelper.Fill(lDataSet, lddhOracle.ResultDataAdapter);

        //        lDatabaseTable = lDataSet.Tables[0];
        //        if (lDatabaseTable.Rows.Count == 0)
        //        {
        //            lGuiltyFields.AppendLine("=> Row not found in database");
        //            continue;
        //        }

        //        lDatabaseRow = lDatabaseTable.Rows[0];

        //        // Compare values
        //        foreach (System.Data.DataColumn lColumn in lAppRow.Table.Columns)
        //        {
        //            //// Log all field values
        //            //if (lDatabaseTable.Columns.Contains(lColumn.ColumnName))
        //            //{
        //            //    lGuiltyFields.AppendLine(string.Format("- {0}: '{1}'  - '{2}' - '{3}'",
        //            //        lColumn.ColumnName,                            
        //            //        FormatFieldValue(lDatabaseRow, lColumn.ColumnName, System.Data.DataRowVersion.Current),
        //            //        FormatFieldValue(lAppRow, lColumn.ColumnName, System.Data.DataRowVersion.Original),
        //            //        FormatFieldValue(lAppRow, lColumn.ColumnName, System.Data.DataRowVersion.Current)));
        //            //}
        //            //else
        //            //{
        //            //    lGuiltyFields.AppendLine(string.Format("- {0}: <NotPresent>  - '{1}' - '{2}'",
        //            //        lColumn.ColumnName,                            
        //            //        FormatFieldValue(lAppRow, lColumn.ColumnName, System.Data.DataRowVersion.Original),
        //            //        FormatFieldValue(lAppRow, lColumn.ColumnName, System.Data.DataRowVersion.Current)));
        //            //}

        //            if (lDatabaseTable.Columns.Contains(lColumn.ColumnName))
        //            {
        //                if (FormatFieldValue(lAppRow, lColumn.ColumnName, System.Data.DataRowVersion.Original) != FormatFieldValue(lDatabaseRow, lColumn.ColumnName, System.Data.DataRowVersion.Original))
        //                {
        //                    lGuiltyFields.AppendLine(string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "=> Field '{0}': DB = '{1}' - App = '{2}'",
        //                        lColumn.ColumnName,
        //                        lDatabaseRow[lColumn.ColumnName],
        //                        lAppRow[lColumn.ColumnName, System.Data.DataRowVersion.Original]));
        //                }
        //            }
        //            else
        //            {
        //                lGuiltyFields.AppendLine(string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "=> Missing in DB '{0}': App = '{1}'",
        //                        lColumn.ColumnName,
        //                        lAppRow[lColumn.ColumnName, System.Data.DataRowVersion.Original]));

        //            }
        //        }
        //    }
        //    iGuiltyFields = lGuiltyFields.ToString();
        //}

        private static string FormatFieldValue(System.Data.DataRow row, string columnName, System.Data.DataRowVersion dataRowVerion)
        {
            string lColumnType;

            lColumnType = row.Table.Columns[columnName].DataType.ToString();
            if (row.IsNull(row.Table.Columns[columnName], dataRowVerion))
            {
                return "DBNull";
            }
            else
            {
                switch (lColumnType)
                {
                    case "System.Boolean":
                        if (row[columnName, dataRowVerion].ToString() == "False") return "0";
                        else return "1";
                    default:
                        return row[columnName, dataRowVerion].ToString();
                }
            }
        }
    }


    /// <summary>
    /// Throw this exception to indicate SQL detected a deadlock
    /// </summary>

    public class DeadlockException : ApplicationExceptionBase
    {
        public DeadlockException()
            : base()
        {
        }
        public override String ExceptionPatternString()
        {
            return "Deadlock Exception";
        }
        public override int ExceptionNumber()
        {
            return -281;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain search criteria dit nog match any record in the datatable (i.e. 0 records are returned) 
    /// </summary>

    public class NoRecordsFoundException : ApplicationExceptionBase
    {
        private enum ExceptionContext
        {
            None,
            TableName,
            TableName_Criteria,
            TableName_Criteria_Value
        }

        private ExceptionContext iContext = ExceptionContext.None;

        public NoRecordsFoundException(string tableName)
            : base(tableName, "", "")
        {
            iContext = ExceptionContext.TableName;
        }

        public NoRecordsFoundException(string tableName, string searchCriterium)
            : base(tableName, searchCriterium, "")
        {
            iContext = ExceptionContext.TableName_Criteria;
        }

        public NoRecordsFoundException(string tableName, string searchCriterium, object searchValue)
            : base(tableName, searchCriterium, searchValue)
        {
            iContext = ExceptionContext.TableName_Criteria_Value;
        }

        public override String ExceptionPatternString()
        {
            switch (iContext)
            {
                case ExceptionContext.TableName_Criteria_Value:

                    return "No records found in table {0} for search criterium {1} with value {2}";

                case ExceptionContext.TableName_Criteria:

                    return "No records found in table {0} for {1}";

                default:

                    return "No records found in table {0}";
            }
        }

        public override int ExceptionNumber()
        {
            return -308;
        }

        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Warning;
        }

    }

    /// <summary>
    /// Throw this exception to indicate a certain field in a certain table was truncated
    /// </summary>

    public class FieldTruncatedException : ApplicationExceptionBase
    {
        public FieldTruncatedException(string tableName, string fieldName, string value, int length)
            : base(tableName, fieldName, value, length)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Field {1} with value {2} was truncated in table {0} to length {3}";
        }
        public override int ExceptionNumber()
        {
            return -325;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain webservice does not respond
    /// </summary>
    public class WebServiceNotAvailableException : ApplicationExceptionBase
    {
        public WebServiceNotAvailableException(string webServiceLogicalName, string webServicePhysicalName)
            : base(webServiceLogicalName, webServicePhysicalName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "WebService {0}  {1} not available";
        }
        public override int ExceptionNumber()
        {
            return -342;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain webservice does not respond
    /// </summary>
    public class VersionExpiredException : ApplicationExceptionBase
    {
        public VersionExpiredException(string pSubject)
            : base(pSubject)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0} Version Expired   Please Upgrade";
        }
        public override int ExceptionNumber()
        {
            return -327;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }


    /// <summary>
    /// Throw this exception to indicate a certain field of a certain table got passed a value of a non compatible type
    /// </summary>

    public class FieldTypeInvalidException : ApplicationExceptionBase
    {
        public FieldTypeInvalidException(string tableName, string fieldName, object value, string expectedType)
            : base(tableName, fieldName, value, expectedType)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Field {1} with value {2} in table {0} is not of type {3}";
        }
        public override int ExceptionNumber()
        {
            return -360;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain field of a certain table got passed a value of a non compatible Length
    /// </summary>

    public class FieldLengthInvalidException : ApplicationExceptionBase
    {
        public FieldLengthInvalidException(string tableName, string fieldName, object value, long expectedLength)
            : base(tableName, fieldName, value, expectedLength)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Field {1} with value {2} in table {0} is too long. Expected length = {3}";
        }
        public override int ExceptionNumber()
        {
            return -357;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary> 
    /// Throw this exception to indicate a certain parameter got passed a value of a non compatible type
    /// </summary>

    public class ParameterTypeInvalidException : ApplicationExceptionBase
    {
        public ParameterTypeInvalidException(string parameterName, object value, string expectedType)
            : base(parameterName, value, expectedType)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Parameter {0} with value {1} is not of type {2}";
        }
        public override int ExceptionNumber()
        {
            return -415;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an ICompare implementation was called with invalid object types
    /// </summary>

    public class CannotCompareException : ApplicationExceptionBase
    {
        public CannotCompareException(string type1, string type2)
            : base(type1, type2, "")
        {
        }
        public CannotCompareException(string type1, string type2, string comment)
            : base(type1, type2, comment)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Cannot compare objects of type {0} to object of type {1} {2}";
        }
        public override int ExceptionNumber()
        {
            return -501;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a file was not found of the expected type
    /// </summary>

    public class FileTypeInvalidException : ApplicationExceptionBase
    {
        public FileTypeInvalidException(string fileType)
            : base(fileType)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Unsupported file type {0}";
        }
        public override int ExceptionNumber()
        {
            return -503;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }
    /// <summary>
    /// Throw this exception to indicate a file was not found as expected
    /// </summary>

    public class FileNotFoundException : ApplicationExceptionBase
    {
        public FileNotFoundException(string fileName)
            : base(fileName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "File {0} not found";
        }
        public override int ExceptionNumber()
        {
            return -504;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a Database is not found as expected
    /// </summary>

    public class DatabaseNotFoundException : ApplicationExceptionBase
    {
        public DatabaseNotFoundException(string databaseName)
            : base(databaseName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Database {0} not found";
        }
        public override int ExceptionNumber()
        {
            return -549;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a Queue is not found as expected
    /// </summary>

    public class QueueNotFoundException : ApplicationExceptionBase
    {
        public QueueNotFoundException(string queueName)
            : base(queueName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Queue {0} not found";
        }
        public override int ExceptionNumber()
        {
            return -570;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a service does not respond. Use it for Web Services, Databases, Queues,...
    /// </summary>

    public class ServiceDoesNotRespondException : ApplicationExceptionBase
    {
        public ServiceDoesNotRespondException(string serviceType, string serviceName)
            : base(serviceType, serviceName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Service {1} of type {0} does not respond correctly. \r\nPlease contact the Application Responsible";
        }
        public override int ExceptionNumber()
        {
            return -611;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a Service (Web/Windows/Whatever) is not found as expected
    /// </summary>

    public class ServiceNotFoundException : ApplicationExceptionBase
    {
        public ServiceNotFoundException(string serviceType, string serviceName)
            : base(serviceType, serviceName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Service {1} of type {0} is not found";
        }
        public override int ExceptionNumber()
        {
            return -614;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    ///<summary>
    ///Throw this exception to indicate a value is not smaller/larger/equal to/than another value as expected
    ///</summary>

    public class CompareValueException : ApplicationExceptionBase
    {
        public CompareValueException(string field, object value1, string compare, object value2)
            : base(field, value1, compare, value2)
        {
        }
        public override string ExceptionPatternString()
        {
            return "Value of {0} ({1}) is expected to be {2} {3}";
        }
        public override int ExceptionNumber()
        {
            return -618;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }

    }
    /// <summary>
    /// Throw this exception to indicate a field is not smaller/larger than another field as expected
    /// </summary>

    public class CompareFieldException : ApplicationExceptionBase
    {
        public CompareFieldException(string field1Name, object field1Value, CompareEnum compare, string field2Name, object field2Value)
            : base(field1Name, field1Value, compare, field2Name, field2Value)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Field {0} {1} is expected to be {2} than field {3} {4}";
        }
        public override int ExceptionNumber()
        {
            return -616;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an action failed
    /// </summary>

    public class ActionFailedException : ApplicationExceptionBase
    {
        public ActionFailedException(string action)
            : base(action)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Action {0} failed";
        }
        public override int ExceptionNumber()
        {
            return -639;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an action failed
    /// </summary>

    public class ActionDeniedException : ApplicationExceptionBase
    {
        public ActionDeniedException(string action, string additionalInformation)
            : base(action, additionalInformation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "You have insufficient permissions to perform this action: {0} {1}";
        }
        public override bool IsSecurityException()
        {
            return true;
        }
        public override int ExceptionNumber()
        {
            return -636;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }


    /// <summary>
    /// Throw this exception to indicate an action failed
    /// </summary>

    public class ActionImpossibleException : ApplicationExceptionBase
    {
        public ActionImpossibleException(string action, string additionalInformation)
            : base(action, additionalInformation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "You cannot perform this action: {0} {1}";
        }
        public override int ExceptionNumber()
        {
            return -634;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }


    /// <summary>
    /// Throw this exception to indicate an update failed
    /// </summary>

    public class UpdateFailedException : ApplicationExceptionBase
    {
        public UpdateFailedException(string tableName, string keyName, object keyValue)
            : base(tableName, keyName, keyValue)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Update in table {0} failed for record with key {1} and value {2}";
        }
        public override int ExceptionNumber()
        {
            return -641;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an insert failed
    /// </summary>


    public class FileReadException : ApplicationExceptionBase
    {
        public string FileName { get; set; }
        public FileReadException()
            : base()
        {
        }
        public FileReadException(string message)
            : base(message)
        {
        }
        public FileReadException(string message, string fileName)
            : base(message)
        {
            FileName = fileName;
        }
        public FileReadException(string message, string fileName, Exception ex)
            : base(message, ex)
        {
            FileName = fileName;
        }
        public FileReadException(string message, Exception ex)
            : base(message, ex)
        {
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("FileName", FileName);
        }

        public override string Message
        {
            get
            {
                return base.Message + "(" + FileName + ")";
            }
        }

        public override string ExceptionPatternString()
        {
            return "File Read Exception: {0} ({1})";
        }

        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }

        public override int ExceptionNumber()
        {
            return 394;
        }
    }
    public class InsertFailedException : ApplicationExceptionBase
    {
        public InsertFailedException(string tableName, string keyName, object keyValue)
            : base(tableName, keyName, keyValue)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Insert in table {0} failed for record with key {1} and value {2}";
        }
        public override int ExceptionNumber()
        {
            return -684;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a Duplicate key was found
    /// </summary>

    public class DuplicateKeyException : ApplicationExceptionBase
    {
        public DuplicateKeyException(string tableName, string keyName, object keyValue)
            : base(tableName, keyName, keyValue)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Duplicate key in table {0} failed for record with key {1} and value {2}";
        }
        public override int ExceptionNumber()
        {
            return -692;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Throw this exception to indicate the record cannot be deleted because of Database Constraints 
    /// </summary>

    public class CannotDeleteChildrenRecordsExistException : ApplicationExceptionBase
    {
        public CannotDeleteChildrenRecordsExistException(string tableName, string keyName, object keyValue, string childrenTableName)
            : base(tableName, keyName, keyValue, childrenTableName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Delete in table {0} failed for record with key {1} and value {2}. Children exist in table {3}";
        }
        public override int ExceptionNumber()
        {
            return -723;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a Delete failed
    /// </summary>

    public class DeleteFailedException : ApplicationExceptionBase
    {
        public DeleteFailedException(string tableName)
            : base(tableName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Delete in table {0} failed";
        }
        public override int ExceptionNumber()
        {
            return -737;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a Print action failed 
    /// </summary>

    public class PrintFailedException : ApplicationExceptionBase
    {
        public PrintFailedException()
            : base()
        {
        }
        public override String ExceptionPatternString()
        {
            return "Print failed";
        }
        public override int ExceptionNumber()
        {
            return -744;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an Import failed
    /// </summary>

    public class ImportFailedException : ApplicationExceptionBase
    {
        public ImportFailedException()
            : base()
        {
        }
        public override String ExceptionPatternString()
        {
            return "Import failed";
        }
        public override int ExceptionNumber()
        {
            return -771;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an Export failed
    /// </summary>

    public class ExportFailedException : ApplicationExceptionBase
    {
        public ExportFailedException()
            : base()
        {
        }
        public ExportFailedException(string exception)
            : base(exception)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Export failed: {0}";
        }
        public override int ExceptionNumber()
        {
            return -810;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an integrity violation is detected
    /// </summary>

    public class IntegrityViolationException : ApplicationExceptionBase
    {
        public IntegrityViolationException(string tableName, string foreignKeyName, object foreignKeyValue, string parentTable)
            : base(tableName, foreignKeyName, foreignKeyValue, parentTable)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Integrity violation: Table {0} with foreign key {1} has a record with value {2} which is not in table {3}";
        }
        public override int ExceptionNumber()
        {
            return -836;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to pass some custom information
    /// </summary>

    public class CustomInformationException : ApplicationExceptionBase
    {
        public CustomInformationException(string customMessage)
            : base(customMessage)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -860;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Throw this exception to pass some warning information 
    /// </summary>

    public class CustomWarningException : ApplicationExceptionBase
    {
        public CustomWarningException(string customMessage)
            : base(customMessage)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -870;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Warning;
        }
    }

    /// <summary>
    /// Throw this exception to pass some custom low severity exception
    /// </summary>

    public class CustomLowSeverityException : ApplicationExceptionBase
    {
        public CustomLowSeverityException(string customMessage)
            : base(customMessage)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -894;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to pass some custom low severity exception
    /// </summary>

    public class CustomMediumSeverityException : ApplicationExceptionBase
    {
        public CustomMediumSeverityException(string customMessage)
            : base(customMessage)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -895;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to pass some custom high severity exception
    /// </summary>

    public class CustomHighSeverityException : ApplicationExceptionBase
    {
        public CustomHighSeverityException(string customMessage)
            : base(customMessage)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -914;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a SQL exception occurred
    /// </summary>

    public class DataIncompleteException : ApplicationExceptionBase
    {
        public DataIncompleteException(string message)
            : base(message)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Data incomplete: {0}";
        }
        public override int ExceptionNumber()
        {
            return -908;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }
    /// <summary>
    /// Throw this exception to pass some custom critical severity exception
    /// </summary>

    public class CustomCriticalSeverityException : ApplicationExceptionBase
    {
        public CustomCriticalSeverityException(string customMessage)
            : base(customMessage)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -953;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an invalid logon entry
    /// </summary>

    public class InvalidLogOnIdException : ApplicationExceptionBase
    {
        public InvalidLogOnIdException(string logOnId)
            : base(logOnId)
        {
        }
        public override String ExceptionPatternString()
        {
            return "User with LoginId {0} has not been defined";
        }
        public override bool IsSecurityException()
        {
            return true;
        }
        public override int ExceptionNumber()
        {
            return -365;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate an invalid logon entry
    /// </summary>

    public class NewPasswordsDoNotMatchException : ApplicationExceptionBase
    {
        public NewPasswordsDoNotMatchException()
            : base()
        {
        }
        public override String ExceptionPatternString()
        {
            return "New Passwords Do Not Match Exception";
        }
        public override int ExceptionNumber()
        {
            return -356;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }


    /// <summary>
    /// Throw this exception to indicate an invalid logon entry
    /// </summary>

    public class InvalidMappingException : ApplicationExceptionBase
    {
        public InvalidMappingException(Exception innerException, string columnName)
            : base(innerException, columnName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Could not map column {0}";
        }
        public override int ExceptionNumber()
        {
            return -348;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }


    /// <summary>
    /// Throw this exception to indicate an invalid password entry
    /// </summary>

    public class InvalidPasswordException : ApplicationExceptionBase
    {
        public InvalidPasswordException(string logOnId)
            : base(logOnId)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Password for user with LoginId {0} does not match";
        }
        public override bool IsSecurityException()
        {
            return true;
        }
        public override int ExceptionNumber()
        {
            return -359;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a user has no access to the requested service
    /// </summary>

    public sealed class AccessDeniedException : ApplicationExceptionBase
    {
        public AccessDeniedException(string logOnId, string reason)
            : base(logOnId, reason)
        {
        }

        public AccessDeniedException(string lonOnId, string externalPatternString, params object[] externalPatternArgs)
            : base(lonOnId, externalPatternString, externalPatternArgs)
        {
        }

        private ExternalPatternException iExternalPatternException;
        private ExternalPatternException ExternalPatternException
        {
            get
            {
                if (iExternalPatternException == null)
                {
                    if (ExceptionArguments.Count == 1)
                    {
                        ExceptionArguments = ExceptionArguments.First().Split(',').ToList();
                    }
                    iExternalPatternException = new ExternalPatternException(EnumerationHelper.CodeToEnum<ExceptionPatternEnum>(ExceptionArguments[1]), ExceptionArguments.Skip(2).ToArray());
                }
                return iExternalPatternException;
            }
        }

        public override String ExceptionPatternString()
        {
            if (ExceptionArguments.Count == 2) return "Access to user with LoginId {0} has been denied {1}";
            return "Access to user with LoginId {0} has been denied";
        }
        public override bool IsSecurityException()
        {
            return true;
        }
        public override int ExceptionNumber()
        {
            return -368;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
        public override string FullMessage
        {
            get
            {
                if (ExceptionArguments.Count == 2) return base.FullMessage;
                if (ExceptionArguments.Count == 1)
                {
                    ExceptionArguments = ExceptionArguments.First().Split(',').ToList();
                }
                string lExceptionPattern = this.ExceptionPatternString();
                if (ExceptionHelper.TranslationManager != null)
                {
                    lExceptionPattern = ExceptionHelper.TranslationManager.Localize(this.ExceptionPatternString());
                }
                string lBaseMessage = string.Format(
                    DXC.Technology.Utilities.StringFormatProvider.Default,
                    lExceptionPattern,
                    this.ExceptionArguments[0]);
                return lBaseMessage + ": " + ExternalPatternException.FullMessage;
            }
        }
    }



    /// <summary>
    /// Throw this exception to indicate a user has no access to the requested service
    /// </summary>

    public sealed class ApplicationNotAvailableException : ApplicationExceptionBase
    {
        public ApplicationNotAvailableException(string reason)
            : base(reason)
        {
        }
        public override String ExceptionPatternString()
        {
            return "The application is currently not available. {0}";
        }
        public override int ExceptionNumber()
        {
            return -846;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }
    /// <summary>
    /// Throw this exception if the user's password is expired or has an initial password
    /// </summary>

    public class PasswordExpiredException : ApplicationExceptionBase
    {
        public PasswordExpiredException()
            : base()
        {
        }
        public override String ExceptionPatternString()
        {
            return "Your password is expired. Please change it first...";
        }
        public override int ExceptionNumber()
        {
            return -364;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception if the user tries to change his password to a non-compliant one
    /// </summary>

    public class NewPasswordInvalidException : ApplicationExceptionBase
    {
        public NewPasswordInvalidException(string goodPasswordRule)
            : base(goodPasswordRule)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Your new password is not compliant with the password policy. {0}";
        }
        public override int ExceptionNumber()
        {
            return -394;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }


    /// <summary>
    /// Throw this exception to indicate an attempt is made to access the application on a non-conventional way...
    /// </summary>

    public class IllegalApplicationAccessException : ApplicationExceptionBase
    {
        public IllegalApplicationAccessException(string comment)
            : base(comment)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Illegal application access. {0}";
        }
        public override bool IsSecurityException()
        {
            return true;
        }
        public override int ExceptionNumber()
        {
            return -345;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }


    /// <summary>
    /// Throw this exception to indicate a user has no access to the requested service
    /// </summary>

    public class DataAccessDeniedException : ApplicationExceptionBase
    {
        public DataAccessDeniedException(string functionality, string accessKey, string additionalInformation)
            : base(functionality, accessKey, additionalInformation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Access to {0} Information for {1} is denied. {2}";
        }
        public override bool IsSecurityException()
        {
            return true;
        }
        public override int ExceptionNumber()
        {
            return -367;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a uniqueness constraint violation
    /// </summary>

    public class NotUniqueException : ApplicationExceptionBase
    {
        public NotUniqueException(string pCriterium, string value)
            : base(pCriterium, value)
        {
        }
        public override String ExceptionPatternString()
        {
            return "The criterium field {0} with value {1} did not respect the uniqueness constraint";
        }
        public override int ExceptionNumber()
        {
            return -212;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Maximum count is reached exception
    /// </summary>

    public class SearchCountLimitExceededException : ApplicationExceptionBase
    {
        public SearchCountLimitExceededException(int limit, string entity)
            : base(limit, entity, "Please refine your search criteria")
        {
        }
        public SearchCountLimitExceededException(int limit, string entity, string comment)
            : base(limit, entity, comment)
        {
        }
        public override String ExceptionPatternString()
        {
            return "The maximum count of {0} for search on {1} is reached. {2}";
        }
        public override int ExceptionNumber()
        {
            return -433;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }


    /// <summary>
    /// Maximum count is reached exception
    /// </summary>

    public class CountLimitExceededException : ApplicationExceptionBase
    {
        public CountLimitExceededException(int limit, string action)
            : base(limit, action, string.Empty)
        {
        }
        public CountLimitExceededException(int limit, string action, string comment)
            : base(limit, action, comment)
        {
        }
        public override String ExceptionPatternString()
        {
            return "The maximum count of {0} for action {1} is reached. {2}";
        }
        public override int ExceptionNumber()
        {
            return -423;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Result list is too long
    /// </summary>

    public class ResultListTooLongException : ApplicationExceptionBase
    {
        public ResultListTooLongException()
            : base()
        {
        }
        public override String ExceptionPatternString()
        {
            return "The result list is too long. Please refine your search criteria.";
        }
        public override int ExceptionNumber()
        {
            return -425;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a parameter is not specified as expected
    /// </summary>

    public class MandatoryParameterNotSpecifiedException : ApplicationExceptionBase
    {
        public MandatoryParameterNotSpecifiedException(string parameterName)
            : base(parameterName, string.Empty)
        {
        }
        public MandatoryParameterNotSpecifiedException(string parameterName, string context)
            : base(parameterName, context)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Parameter {0} is not specified. {1}";
        }
        public override int ExceptionNumber()
        {
            return -211;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }


    /// <summary>
    /// Throw this exception to indicate a service is busy doing all other things, except what you ask from it
    /// </summary>

    public class ServiceBusyException : ApplicationExceptionBase
    {
        public ServiceBusyException(string serviceType, string serviceName)
            : base(serviceType, serviceName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Service {1} of type {0} is busy";
        }
        public override int ExceptionNumber()
        {
            return -608;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }

    /// <summary>
    /// Catch unexcptected exceptions at high levels and wrap them in this exception to get these exceptions published
    /// </summary>

    public class UnexpectedException : ApplicationExceptionBase
    {
        public UnexpectedException(Exception exception)
            : base(exception, exception.Message, "")
        {
        }
        public UnexpectedException(Exception exception, string message)
            : base(exception, message, exception.Message, "")
        {
        }

        public UnexpectedException(string message, string pDetailMessage)
            : base(message, pDetailMessage)
        {
        }
        public UnexpectedException(string message)
            : base(message, "")
        {
        }
        public override String ExceptionPatternString()
        {
            return "Unexpected Exception: {0}  {1}";
        }
        public override int ExceptionNumber()
        {
            return -911;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a SQL exception occurred
    /// </summary>

    public class DataException : ApplicationExceptionBase
    {
        public DataException(SqlException exception, string message, string commandText)
            : base(exception, message, commandText)
        {
        }

        public DataException(string message, string commandText)
            : base(null, message, commandText)
        {
        }

        public override String ExceptionPatternString()
        {
            return "{0}  Executing Command: {1}";
        }
        public override int ExceptionNumber()
        {
            return -909;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Warning;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a SQL exception occurred
    /// </summary>

    public class DataInconsistencyException : ApplicationExceptionBase
    {
        public DataInconsistencyException(string contextMessage, string contextTable, string contextKey)
            : base(contextMessage, contextTable, contextKey)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Data inconsistency detected : {0} table={1},key={2}";
        }
        public override int ExceptionNumber()
        {
            return -910;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ValidationException : ApplicationExceptionBase
    {
        private static System.Collections.Specialized.StringDictionary sValidationMessageTemplateDictionary = null;

        public static System.Collections.Specialized.StringDictionary ValidationMessageTemplateDictionary
        {
            get { return sValidationMessageTemplateDictionary; }
            set { sValidationMessageTemplateDictionary = value; }
        }

        public ValidationException(string validationCode, string validationMessageTemplate, string validationMessageArguments, int validationMessageSeverity)
            : base(validationCode, validationMessageTemplate, validationMessageArguments, validationMessageSeverity)
        {
        }
        public override String ExceptionPatternString()
        {
            if ((sValidationMessageTemplateDictionary == null) || (!sValidationMessageTemplateDictionary.ContainsKey(ValidationCode)))
                return "{0}-{1} ({3}): {2} ";
            else
                return sValidationMessageTemplateDictionary[ValidationCode];
        }


        public override string ExceptionArgumentsAsCSV
        {
            get
            {
                if ((sValidationMessageTemplateDictionary == null) || (!sValidationMessageTemplateDictionary.ContainsKey(ValidationCode)))
                    return DXC.Technology.Utilities.StringArrayHelper.ToCsvString(ExceptionArguments.ToArray());
                else
                    return DXC.Technology.Utilities.StringArrayHelper.ToCsvString(ExceptionArguments.Skip(2).ToArray());
            }
        }

        public override int ExceptionNumber()
        {
            return -910;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            int lSeverity = this.ValidationMessageSeverity;
            switch (lSeverity)
            {
                case 1: return SeverityLevelEnum.Critical;
                case 2: return SeverityLevelEnum.High;
                case 3: return SeverityLevelEnum.Medium;
                case 4: return SeverityLevelEnum.Low;
                case 5: return SeverityLevelEnum.Warning;
                default: return SeverityLevelEnum.Information;
            }
        }

        public override string Message
        {
            get
            {
                if ((sValidationMessageTemplateDictionary == null) || (!sValidationMessageTemplateDictionary.ContainsKey(ValidationCode)))
                    return base.Message;
                else
                    return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, ExceptionPatternString(), ValidationMessageArguments.Split(';', ','));
            }
        }
        public string ValidationCode
        {
            get
            {
                return ExceptionArguments[0];
            }
        }

        public string ValidationMessageTemplate
        {
            get
            {
                return ExceptionArguments[1];
            }
        }

        public string ValidationMessageArguments
        {
            get
            {
                return ExceptionArguments[2];
            }
        }

        public int ValidationMessageSeverity
        {
            get
            {
                return Convert.ToInt32(ExceptionArguments[3], DXC.Technology.Utilities.CultureInfoProvider.Default);
            }
        }
    }

    public interface IAnomalyExceptionSubtypeProvider
    {
        string GetSubtypeFormatStringForAnomalySubtypeCode(string pAnomalySubtypeCode);
    }

    public class DefaultAnomalyExceptionSubtypeProvider : IAnomalyExceptionSubtypeProvider
    {
        public string GetSubtypeFormatStringForAnomalySubtypeCode(string pAnomalySubtypeCode)
        {
            return "{0} {1} {2} {3} {4} {5}";
        }
    }

    /// <summary>
    /// 
    /// </summary>

    public class AnomalyException : ApplicationExceptionBase
    {
        private static IAnomalyExceptionSubtypeProvider sAnomalyExceptionSubtypeProvider = new DefaultAnomalyExceptionSubtypeProvider();
        public static IAnomalyExceptionSubtypeProvider AnomalyExceptionSubtypeProvider
        {
            get
            {
                return sAnomalyExceptionSubtypeProvider;
            }
            set
            {
                sAnomalyExceptionSubtypeProvider = value;
            }
        }

        public AnomalyException(string pAnomalySubtypeCode, int pSeverity, string pField1, string pField2, string pField3, string pField4, string identifier)
            : base(pAnomalySubtypeCode, pSeverity, pField1, pField2, pField3, pField4, identifier)
        {
        }

        /// <summary>
        /// Binds the exception message pattern with the exception arguments to have a nice explenation of the exception message)
        /// </summary>
        public override string FullMessage
        {
            get
            {
                try
                {
                    string[] lExceptionArguments;
                    string lPatternString = this.ExceptionPatternString();
                    int lParameterCount;
                    for (lParameterCount = 6; lParameterCount >= 0; lParameterCount--)
                    {
                        if (lPatternString.Contains("{" + (lParameterCount - 1).ToString(DXC.Technology.Utilities.CultureInfoProvider.Default) + "}")) break;
                    }

                    if (lParameterCount == -1)
                    {

                        if (ExceptionHelper.TranslationManager != null)
                            return ExceptionHelper.TranslationManager.Localize(this.ExceptionPatternString());
                        else
                            return this.ExceptionPatternString();
                    }
                    else
                    {
                        lExceptionArguments = new string[lParameterCount];
                        for (int i = 0; i < lParameterCount; i++)
                        {
                            if (i <= 3)
                            {
                                if (ExceptionHelper.TranslationManager != null)
                                    lExceptionArguments[i] = ExceptionHelper.TranslationManager.Localize(this.ExceptionArguments[i + 2]);
                                else
                                    lExceptionArguments[i] = this.ExceptionArguments[i + 2];
                            }
                            else
                            {
                                if (ExceptionHelper.TranslationManager != null)
                                    lExceptionArguments[i] = ExceptionHelper.TranslationManager.Localize(this.ExceptionArguments[i - 4]);
                                else
                                    lExceptionArguments[i] = this.ExceptionArguments[i - 4];
                            }
                        }
                        if (ExceptionHelper.TranslationManager != null)
                        {
                            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, ExceptionHelper.TranslationManager.Localize(this.ExceptionPatternString()), lExceptionArguments);
                        }
                        else
                        {
                            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, this.ExceptionPatternString(), lExceptionArguments);
                        }
                    }

                }
                catch (FormatException)
                {
                    System.Diagnostics.Debug.WriteLine("Debug me - this is wrong! Somebody has succeeded in creating a corrupt exception! ");
                    //JB: Join ExceptionArguments into string. ExceptionPatternString is of no use...
                    if (this.ExceptionArguments != null)
                    {
                        System.Text.StringBuilder lMessage = new System.Text.StringBuilder();
                        lMessage.AppendLine("(ApplicationExceptionBase - Error getting FullMessage of Exception: Pattern '" + this.ExceptionPatternString() + "' not compatible with number of ExceptionArguments (= " + this.ExceptionArguments.Count.ToString(DXC.Technology.Utilities.CultureInfoProvider.Default) + "))");
                        lMessage.AppendLine("=> Your Exception Message:");
                        foreach (string lArg in this.ExceptionArguments)
                        {
                            lMessage.AppendLine(lArg);
                        }
                        return lMessage.ToString();
                    }
                    return this.ExceptionPatternString();
                }
            }
        }

        public override String ExceptionPatternString()
        {
            //return "{0} {1} {2} {3} {4} {5}";
            return sAnomalyExceptionSubtypeProvider.GetSubtypeFormatStringForAnomalySubtypeCode(this.AnomalySubtypeCode);
        }
        public override int ExceptionNumber()
        {
            return -911;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            int lSeverity = this.Severity;
            switch (lSeverity)
            {
                case 1: return SeverityLevelEnum.Critical;
                case 2: return SeverityLevelEnum.High;
                case 3: return SeverityLevelEnum.Medium;
                case 4: return SeverityLevelEnum.Low;
                case 5: return SeverityLevelEnum.Warning;
                default: return SeverityLevelEnum.Information;
            }
        }

        public string AnomalySubtypeCode
        {
            get
            {
                return ExceptionArguments[0];
            }
        }

        public int Severity
        {
            get
            {
                int lSeverity = 1;
                bool lSuccesfullParse = Int32.TryParse(ExceptionArguments[1], out lSeverity);
                if (lSuccesfullParse) return lSeverity;
                else return 1;
            }
        }

        public string Field1
        {
            get
            {
                return ExceptionArguments[2];
            }
        }

        public string Field2
        {
            get
            {
                return ExceptionArguments[3];
            }
        }

        public string Field3
        {
            get
            {
                return ExceptionArguments[4];
            }
        }

        public string Field4
        {
            get
            {
                return ExceptionArguments[5];
            }
        }

        public string Identifier
        {
            get
            {
                return ExceptionArguments[6];
            }
        }
        public bool IsBlockingException
        {
            get
            {
                switch (SeverityLevel())
                {
                    case SeverityLevelEnum.Critical:
                    case SeverityLevelEnum.High:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsNonBlockingException
        {
            get
            {
                switch (SeverityLevel())
                {
                    case SeverityLevelEnum.Medium:
                    case SeverityLevelEnum.Low:
                    case SeverityLevelEnum.Warning:
                        return true;
                    case SeverityLevelEnum.Critical:
                    case SeverityLevelEnum.High:
                    case SeverityLevelEnum.Information:
                    case SeverityLevelEnum.None:
                        return false;
                    default:
                        return false;
                }
            }
        }
    }

    /// <summary>
    /// Throw this exception to indicate a SQL exception occurred
    /// </summary>

    public class SQLCommandExecutionException : ApplicationExceptionBase
    {
        public SQLCommandExecutionException(Exception exception, string message, string commandText)
            : base(exception, message, commandText)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}  Executing Command: {1}";
        }
        public override int ExceptionNumber()
        {
            return -907;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a Configuration item is missing
    /// </summary>

    public class MissingConfigurationException : ApplicationExceptionBase
    {
        public MissingConfigurationException(string parameterName)
            : base(parameterName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Missing configuration item: {0}";
        }
        public override int ExceptionNumber()
        {
            return -923;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Warning;
        }
    }

    /// <summary>
    /// Throw this generic exception to indicate a item was expected but not found. 
    /// </summary>

    public class ItemNotFoundException : ApplicationExceptionBase
    {
        public ItemNotFoundException(string pItemName)
            : base(pItemName)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Item {0} not found";
        }
        public override int ExceptionNumber()
        {
            return -227;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this generic exception to indicate a item was expected but not found. 
    /// </summary>

    public class InvalidStateException : ApplicationExceptionBase
    {
        public InvalidStateException(string pCurrentState, string pExpectedState)
            : base(pCurrentState, pExpectedState)
        {
        }
        public override String ExceptionPatternString()
        {
            return "State Invalid:  Expected = {1}  Current = {0}";
        }
        public override int ExceptionNumber()
        {
            return -286;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to pass a simple exception to the end
    /// </summary>

    public class SimpleException : ApplicationExceptionBase
    {
        public SimpleException(string message)
            : base(message)
        {
        }

        public SimpleException(Exception innerException, string message)
            : base(innerException, message, innerException.Message)
        {
        }

        public override String ExceptionPatternString()
        {
            if (this.ExceptionArguments.Count == 1)
            {
                return "{0}";
            }
            else
            {
                return "{0}: {1}";
            }
        }
        public override int ExceptionNumber()
        {
            return -225;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Allows you to additional information to an Exception
    /// </summary>
    public class WrapperException : ApplicationExceptionBase
    {
        private ApplicationExceptionBase iAppExpBaseInnerException;

        public WrapperException(string[] pParameters)
            : base(pParameters)
        {
            this.ExceptionArguments.AddRange(pParameters);
            if (pParameters[0] == "OTHER")
            {
                // Other type of Exception
            }
            else if (pParameters[0] == "APPEXPBASE")
            {
                // ApplicationExceptionBase => Reconstruct Original Exception 
                System.Collections.Generic.List<string> lBaseParameters = new System.Collections.Generic.List<string>();
                lBaseParameters.AddRange(this.ExceptionArguments);
                lBaseParameters.RemoveRange(0, 3);

                DXC.Technology.Exceptions.ExceptionHelper.ExceptionDecoder = new DXC.Technology.Exceptions.ExceptionDecoder();
                iAppExpBaseInnerException = DXC.Technology.Exceptions.ExceptionHelper.ExceptionDecoder.Decode(pParameters[2], lBaseParameters.ToArray());
            }
        }

        public WrapperException(Exception innerException, string message)
            : base(innerException)
        {
            ApplicationExceptionBase lAppBaseException = innerException as ApplicationExceptionBase;
            if (lAppBaseException == null)
            {
                // Other type of Exception
                this.ExceptionArguments = new string[] { "OTHER", message, innerException.Message }.ToList();
            }
            else
            {
                this.iAppExpBaseInnerException = lAppBaseException;
                // ApplicationExceptionBase
                System.Collections.Generic.List<string> lArguments = new System.Collections.Generic.List<string>();
                // Add custom type to differentiate
                lArguments.Add("APPEXPBASE");
                // Additional Message
                lArguments.Add(message);
                // Exception Type
                lArguments.Add(lAppBaseException.GetType().Name);
                // ApplicatinBase arguments
                string[] lArgumentsAsStringArray = new string[lAppBaseException.ExceptionArguments.Count];
                lAppBaseException.ExceptionArguments.CopyTo(lArgumentsAsStringArray, 0);
                lArguments.AddRange(lArgumentsAsStringArray);
                this.ExceptionArguments = lArguments;
            }
        }

        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -225;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }

        public override string FullMessage
        {
            get
            {
                return FullMessageGetter();
            }
        }

        //required by CA1065
        public string FullMessageGetter()
        {
            if (this.ExceptionArguments[0] == "OTHER")
            {
                // Other type of Exception
                return string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "{0}: {1}", this.ExceptionArguments[1], this.ExceptionArguments[2]);
            }
            else if (this.ExceptionArguments[0] == "APPEXPBASE")
            {
                string lHeader = this.ExceptionArguments[1];
                if (ExceptionHelper.TranslationManager != null)
                {
                    lHeader = ExceptionHelper.TranslationManager.Localize(lHeader);
                }

                if (this.iAppExpBaseInnerException == null)
                {
                    return lHeader;
                }
                else
                {
                    return string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, "{0}: {1}",
                        lHeader,
                        this.iAppExpBaseInnerException.FullMessage);
                }
            }
            else
            {
                throw new NotImplementedException("Invalid use of WrapperException");
            }
        }
    }

    /// <summary>
    /// Throw this exception if the business logic/functional logic is broken
    /// </summary>

    public class BusinessLogicException : ApplicationExceptionBase
    {
        public BusinessLogicException(string message)
            : base(message)
        {
        }

        public BusinessLogicException(string message, string param1)
            : base(message, param1)
        {
        }

        public BusinessLogicException(string message, string param1, string param2)
            : base(message, param1, param2)
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -545;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }

        public override string FullMessage
        {
            get
            {
                // FullMessage returns the first parameter using the ExceptionPatternString
                // if there are extra parameter, fill them in in this string
                // else return the base message
                string lBaseMessage = base.FullMessage;
                if (ExceptionArguments.Count == 1)
                {
                    return lBaseMessage;
                }
                else
                {
                    return string.Format(DXC.Technology.Utilities.CultureInfoProvider.Default, lBaseMessage, ExceptionArguments.ToArray());
                }
            }
        }
    }



    /// <summary>
    /// Throw this exception if the business logic/functional logic is broken
    /// </summary>

    public sealed class ExternalPatternBusinessLogicException : ApplicationExceptionBase
    {
        public ExternalPatternBusinessLogicException(ExceptionPatternEnum externalPatternEnum, params object[] externalPatternArgs)
            : base(EnumerationHelper.EnumToCode(externalPatternEnum), externalPatternArgs)
        {
        }

        private ExternalPatternException iExternalPatternException;
        private ExternalPatternException ExternalPatternException
        {
            get
            {
                if (iExternalPatternException == null)
                {
                    if (ExceptionArguments.Count == 1)
                    {
                        ExceptionArguments = ExceptionArguments.First().Split(',').ToList();
                    }
                    iExternalPatternException = new ExternalPatternException(EnumerationHelper.CodeToEnum<ExceptionPatternEnum>(ExceptionArguments[0]), ExceptionArguments.Skip(1).ToArray());
                }
                return iExternalPatternException;
            }
        }

        public string ExternalPatternCode
        {
            get
            {
                return ExternalPatternException.ExternalPatternCode;
            }
        }

        public override String ExceptionPatternString()
        {
            return ExternalPatternException.ExceptionPatternString();
        }
        public override bool IsSecurityException()
        {
            return true;
        }
        public override int ExceptionNumber()
        {
            return -555;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Information;
        }
        public override string FullMessage
        {
            get
            {
                return ExternalPatternException.FullMessage;
            }
        }

        public override string ToString()
        {
            return FullMessage;
        }
    }


    /// <summary>
    /// Throw this exception if the business logic/functional logic is broken
    /// </summary>

    public class CustomBusinessLogicException : ApplicationExceptionBase
    {
        string lExceptionPatternString = "{0}";

        public CustomBusinessLogicException(string pExceptionPatternString, params object[] pExceptionArguments)
            : base(pExceptionArguments)
        {
            lExceptionPatternString = pExceptionPatternString;
        }

        public override String ExceptionPatternString()
        {
            return lExceptionPatternString;
        }
        public override int ExceptionNumber()
        {
            return -555;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception if the Technical logic is broken
    /// </summary>

    public class TechnicalException : ApplicationExceptionBase
    {
        public TechnicalException(string message)
            : base(message)
        {
        }

        public override String ExceptionPatternString()
        {
            return "{0}";
        }
        public override int ExceptionNumber()
        {
            return -454;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    /// <summary>
    /// Throw this exception if the business logic/functional logic is broken
    /// </summary>

    public class TimespanLimitExceededException : ApplicationExceptionBase
    {
        public TimespanLimitExceededException(DateTime pDateToTest, DateTime pStartDate, DateTime pEndDate)
            : base(pDateToTest, pStartDate, pEndDate)
        {
        }

        public override String ExceptionPatternString()
        {
            return "The given date {0} doesn't fit in the given timespan starting {1} and ending {2}.";
        }
        public override int ExceptionNumber()
        {
            return -545;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }


    public class ConditionNotSatisfiedException : ApplicationExceptionBase
    {
        public ConditionNotSatisfiedException(string pConditionCode, string pConditionDescription)
            : base(pConditionCode, pConditionDescription)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Condition not satisfied {0}: {1} ";
        }
        public override int ExceptionNumber()
        {
            return -362;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    /// <summary>
    /// Throw this exception to pass multiple logical exceptions in one business exception. This helps to eliminate 
    /// lots of round trips e.g. when validating a lot of input...
    /// </summary>

    public class BrokenRulesException : ApplicationExceptionBase
    {
        private List<Exception> iBrokenRulesExceptions = new List<Exception>();
        private bool iRenderMessageAsHtml;

        public BrokenRulesException()
            : base(false)
        {
        }

        //If a broken rules exception contains but one exception it is thrown as a single exception (default behavior)
        //However sometimes you want the exception to always be thrown as a broken rules exception e.g. when you catch that in a rule engine context.
        //This is why you can create a broken rules exception with parm pNeverThrowAsSimpleException
        public BrokenRulesException(bool pNeverThrowAsSimpleException)
            : base(pNeverThrowAsSimpleException)
        {
        }
        public string MessageAsHtml
        {
            get
            {
                string lMessageAsHtml;
                this.iRenderMessageAsHtml = true;
                lMessageAsHtml = this.Message;
                this.iRenderMessageAsHtml = false;
                return lMessageAsHtml;
            }
        }

        public bool NeverThrowAsSimpleException
        {
            get
            {
                return Convert.ToBoolean(this.ExceptionArguments[0], DXC.Technology.Utilities.CultureInfoProvider.Default);
            }
            set
            {
                this.ExceptionArguments[0] = value.ToString(DXC.Technology.Utilities.CultureInfoProvider.Default);
            }
        }

        public override String ExceptionPatternString()
        {
            using (System.IO.StringWriter lswExceptionPattern = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default))
            {
                //lswExceptionPattern.WriteLine("Broken Rules:");
                lswExceptionPattern.WriteLine("");

                if (iBrokenRulesExceptions != null)
                {
                    for (int i = 0; i < iBrokenRulesExceptions.Count; i++)
                    {
                        if (this.iRenderMessageAsHtml)
                        {
                            lswExceptionPattern.Write("- {" + i.ToString(DXC.Technology.Utilities.CultureInfoProvider.Default) + "}<br>");
                        }
                        else
                        {
                            lswExceptionPattern.Write("{" + i.ToString(DXC.Technology.Utilities.CultureInfoProvider.Default) + "}");
                        }
                    }
                }
                return lswExceptionPattern.ToString();
            }
        }

        /// <summary>
        /// Binds the exception message pattern with the exception arguments to have a nice explenation of the exception message)
        /// </summary>
        public override string FullMessage
        {
            get
            {
                try
                {
                    using (System.IO.StringWriter lswExceptionPattern = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default))
                    {
                        bool lFirst = true;
                        //lswExceptionPattern.WriteLine("Broken Rules:");
                        lswExceptionPattern.WriteLine("");

                        if (iBrokenRulesExceptions != null)
                        {
                            for (int i = 0; i < iBrokenRulesExceptions.Count; i++)
                            {
                                if (this.iRenderMessageAsHtml)
                                {
                                    lswExceptionPattern.Write("- {0} <br>", iBrokenRulesExceptions[i].Message);
                                }
                                else
                                {
                                    if (lFirst) lFirst = false;
                                    else lswExceptionPattern.Write(", ");
                                    lswExceptionPattern.Write(iBrokenRulesExceptions[i].Message);
                                }
                            }
                        }
                        return lswExceptionPattern.ToString();
                    }
                }
                catch
                {
                    return base.FullMessage;
                }
            }
        }

        public void AddException(Exception exception)
        {
            if (exception == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Exception");

            BrokenRulesException lBrokenRulesException = exception as BrokenRulesException;

            if (lBrokenRulesException != null)
            {
                //The exception is a broken rule exception itself - add all the individual exceptions
                foreach (Exception lex in lBrokenRulesException.BrokenRulesExceptions)
                {
                    this.AddException(lex);
                }
            }
            else
            {
                iBrokenRulesExceptions.Add(exception);
                ExceptionArguments.Add(exception.Message);
            }
        }

        public void ThrowIfSingleException()
        {
            if (NeverThrowAsSimpleException) return;

            if (iBrokenRulesExceptions.Count == 1)
            {
                throw iBrokenRulesExceptions[0];
            }
        }

        public void AddException(ApplicationExceptionBase pApplicationExceptionBase)
        {
            if (pApplicationExceptionBase == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Exception");
            iBrokenRulesExceptions.Add(pApplicationExceptionBase);
            this.ExceptionArguments.Add(pApplicationExceptionBase.Message);
        }

        public System.Collections.Generic.List<Exception> BrokenRulesExceptions
        {
            get
            {
                System.Collections.Generic.List<Exception> lsc = new System.Collections.Generic.List<Exception>();
                if (iBrokenRulesExceptions != null) lsc.AddRange(iBrokenRulesExceptions);
                return lsc;
            }
        }

        public override int ExceptionNumber()
        {
            return -222;
        }

        public bool ContainsAnomalyExceptionsOnly()
        {
            foreach (Exception lex in iBrokenRulesExceptions)
            {
                if (lex is AnomalyException)
                {
                    //continue
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public override SeverityLevelEnum SeverityLevel()
        {
            SeverityLevelEnum lSeverityLevelSoFar = SeverityLevelEnum.None;
            //find the highest secerity level
            foreach (Exception lex in iBrokenRulesExceptions)
            {
                SeverityLevelEnum lSeverityLevel = SeverityLevelEnum.Medium;

                ApplicationExceptionBase laex = lex as ApplicationExceptionBase;
                if (laex != null)
                {
                    lSeverityLevel = laex.SeverityLevel();
                }

                if (lSeverityLevel.GetHashCode() > lSeverityLevelSoFar.GetHashCode())
                    lSeverityLevelSoFar = lSeverityLevel;
            }
            return lSeverityLevelSoFar;
        }

        public bool IsBlockingException
        {
            get
            {
                switch (SeverityLevel())
                {
                    case SeverityLevelEnum.Critical:
                    case SeverityLevelEnum.High:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsNonBlockingException
        {
            get
            {
                switch (SeverityLevel())
                {
                    case SeverityLevelEnum.Medium:
                    case SeverityLevelEnum.Low:
                    case SeverityLevelEnum.Warning:
                        return true;
                    case SeverityLevelEnum.Critical:
                    case SeverityLevelEnum.High:
                    case SeverityLevelEnum.Information:
                    case SeverityLevelEnum.None:
                        return false;
                    default:
                        return false;
                }
            }
        }


        public override IEnumerable<SerializableException> AsListOfSerializableExceptions()
        {
            List<SerializableException> result = new List<SerializableException>();
            foreach (Exception ex in this.BrokenRulesExceptions)
            {
                result.Add(SerializableException.CreateSerializableException(ex, VerboseModeEnum.Simple));
            }
            return result;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a user setting could not be saved
    /// </summary>

    public class MementoSaveException : ApplicationExceptionBase
    {
        public MementoSaveException(Exception innerException, string pStorageType)
            : base(innerException, pStorageType)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Memento could not be written to {0} medium";
        }
        public override int ExceptionNumber()
        {
            return -768;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }


    /// <summary>
    /// Throw this exception to indicate to the caller that the previous transaction has been aborted due to 'DeveloperSimulationMode'
    /// </summary>

    public class FunctionalityNotSupportedException : ApplicationExceptionBase
    {
        public FunctionalityNotSupportedException(string notSupportedFunctionality)
            : base(notSupportedFunctionality)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Functionality {0} not supported";
        }
        public override int ExceptionNumber()
        {
            return -324;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }


    /// <summary>
    /// Throw this exception to indicate to the caller that the previous transaction has been aborted due to 'DeveloperSimulationMode'
    /// </summary>

    public class FunctionalityDiscontinuedException : ApplicationExceptionBase
    {
        public FunctionalityDiscontinuedException(string pDiscontinuedFunctionality)
            : base(pDiscontinuedFunctionality)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Functionality '{0}'has been discontinued";
        }
        public override int ExceptionNumber()
        {
            return -324;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }



    public class FlexibleReportException : ApplicationExceptionBase
    {
        public override String ExceptionPatternString()
        {
            return "The flexible report query failed to execute. Please change the criteria";
        }
        public override int ExceptionNumber()
        {
            return -687;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }

    [Serializable]
    public class InvalidDataException : ApplicationExceptionBase
    {
        public InvalidDataException()
            : base()
        {
        }
        public InvalidDataException(string data)
            : base(data)
        {
        }
        public InvalidDataException(string data, Exception ex)
            : base(data, ex)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Invalid Data: {0}";
        }
        public override int ExceptionNumber()
        {
            return -689;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }
    /// <summary>
    /// Throw this exception to indicate a Memento could not be restored (typically deserialized)
    /// </summary>

    public class MementoRestoreException : ApplicationExceptionBase
    {
        public MementoRestoreException(Exception innerException, string pStorageType)
            : base(innerException, pStorageType)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Memento could not be restored from {0} medium";
        }
        public override int ExceptionNumber()
        {
            return -769;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception wherever you implement a retry mechanism
    /// </summary>

    public class RetryException : ApplicationExceptionBase
    {
        public RetryException(int numberOfRetriesSoFar, Exception innerException)
            : base(innerException, numberOfRetriesSoFar, innerException.Message)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Retrying {0}th time: {1}";
        }
        public override int ExceptionNumber()
        {
            return -149;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Warning;
        }
    }


    /// <summary>
    /// Throw this exception wherever you implement a retry mechanism
    /// </summary>

    public class MaliciousInputException : ApplicationExceptionBase
    {
        public MaliciousInputException(string field)
            : base(field)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Inputstring {0} contains potentially malicious characters";
        }

        public override bool IsSecurityException()
        {
            return true;
        }

        public override int ExceptionNumber()
        {
            return -354;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Warning;
        }
    }
    [Serializable]
    public class NonConvergenceException : ApplicationExceptionBase
    {
        public NonConvergenceException()
            : base()
        {
        }
        public NonConvergenceException(string message)
            : base(message)
        {
        }
        public NonConvergenceException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Non Convergence: {0}";
        }
        public override int ExceptionNumber()
        {
            return -659;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }


    [Serializable]
    public class ConfirmationCodeRequiredException : ApplicationExceptionBase
    {
        public ConfirmationCodeRequiredException()
            : base()
        {
        }
        public ConfirmationCodeRequiredException(string message)
            : base(message)
        {
        }
        public ConfirmationCodeRequiredException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Confirmation Code not specified";
        }
        public override int ExceptionNumber()
        {
            return -657;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
    }


    [Serializable]
    public class ConfirmationCodeInvalidException : ApplicationExceptionBase
    {
        public ConfirmationCodeInvalidException()
            : base()
        {
        }
        public ConfirmationCodeInvalidException(string message)
            : base(message)
        {
        }
        public ConfirmationCodeInvalidException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Confirmation Code Invalid";
        }
        public override int ExceptionNumber()
        {
            return -658;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }

    [Serializable]
    public class ConfirmationCodeNotGeneratedException : ApplicationExceptionBase
    {
        public ConfirmationCodeNotGeneratedException()
            : base()
        {
        }
        public ConfirmationCodeNotGeneratedException(string message)
            : base(message)
        {
        }
        public ConfirmationCodeNotGeneratedException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public override String ExceptionPatternString()
        {
            return "Confirmation Code Not Generated Exception";
        }
        public override int ExceptionNumber()
        {
            return -659;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }



    /// <summary>
    /// Throw this exception to indicate a WebService invocation failed
    /// </summary>
    public class WebServiceInvocationException : ApplicationExceptionBase
    {
        public WebServiceInvocationException(string webServiceLogicalName, Exception innerException)
            : base(webServiceLogicalName, innerException)
        {
        }

        public override String ExceptionPatternString()
        {
            return "WebService {0} invocation Failed";
        }
        public override int ExceptionNumber()
        {
            return -143;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }


    /// <summary>
    /// Throw this exception to indicate a WebService invocation failed
    /// </summary>

    public class COMApplicationHangsException : ApplicationExceptionBase
    {
        string iCOMApplicationName;
        public COMApplicationHangsException(string pCOMApplicationName)
            : base(pCOMApplicationName)
        {
            iCOMApplicationName = pCOMApplicationName;
        }

        public override String ExceptionPatternString()
        {
            return "COM Application {0} invocation failed. Application probably hangs";
        }

        public override int ExceptionNumber()
        {
            return -989;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
        public string COMApplicationName
        {
            get
            {
                return this.iCOMApplicationName;
            }
        }
    }

    /// <summary>
    /// Throw this exception to indicate a certain parameter is not one of the oredetermined values
    /// </summary>
    public class NoResultFoundException : ApplicationExceptionBase
    {
        public NoResultFoundException(string searchParameterName, object searchValue, string comment)
            : base(searchParameterName, searchValue, comment)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Search on parameter {0} with value {1} failed. No results found.";
        }
        public override int ExceptionNumber()
        {
            return -178;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a 'low-level' problem within a given protocol
    /// </summary>
    public class ProtocolErrorException : ApplicationExceptionBase
    {
        public ProtocolErrorException(string pProtocolName, string pProtocolVersion, string pErrorCode, string pExplanation)
            : base(pProtocolName, pProtocolVersion, pErrorCode, pExplanation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Usage of protocol {0}/{1} is erroneous. Status code : {2} Description : {3}.";
        }
        public override int ExceptionNumber()
        {
            return -139;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }

    /// <summary>
    /// Throw this exception to indicate a problem during network proxy authentication action
    /// </summary>
    public class NetworkProxyAuthenticationException : ApplicationExceptionBase
    {
        public NetworkProxyAuthenticationException(System.Uri uri, string errorDetails)
            : base(uri, errorDetails)
        {
        }
        public override String ExceptionPatternString()
        {
            return "Authentication on network proxy {0} failed. Expected authentication details : {1}.";
        }
        public override int ExceptionNumber()
        {
            return -136;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.High;
        }
    }
    /// <summary>
    /// Throw this exception to indicate to the caller that the previous transaction has been aborted due to 'DeveloperSimulationMode'
    /// </summary>
    public class UseCaseSimulationException : ApplicationExceptionBase
    {
        public UseCaseSimulationException()
            : base()
        {
        }
        public override String ExceptionPatternString()
        {
            return "UseCase transaction is being aborted due to 'DeveloperSimulation' mode";
        }
        public override int ExceptionNumber()
        {
            return -999;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }
    /// <summary>
    /// Data set merge error. Using this exception neatly prints out the errors in the exception log
    /// </summary>

    public class DataSetMergeException : ApplicationExceptionBase
    {
        public DataSetMergeException(System.Data.DataSet erroroneousDataSet)
            : base(DXC.Technology.Exceptions.ExceptionHelper.GetDataSetErrors(erroroneousDataSet))
        {
        }
        public override String ExceptionPatternString()
        {
            return "DataSet in Error: \r\n{0}";
        }
        public override int ExceptionNumber()
        {
            return -123;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }
    /// <summary>
    /// Data set constraints error. Using this exception neatly prints out the errors in the exception log
    /// </summary>	

    public class DataSetConstraintException : ApplicationExceptionBase
    {
        public DataSetConstraintException(System.Data.DataSet erroroneousDataSet)
            : base(DXC.Technology.Exceptions.ExceptionHelper.GetDataSetErrors(erroroneousDataSet))
        {
        }
        public override String ExceptionPatternString()
        {
            return "DataSet in Error: \r\n{0}";
        }
        public override int ExceptionNumber()
        {
            return -154;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }
    /// <summary>
    /// Data set fill error. Using this exception neatly prints out the errors in the exception log
    /// </summary>	
    public class DataSetFillException : ApplicationExceptionBase
    {
        public DataSetFillException(System.Exception exception, string message, string commandText, System.Data.DataSet erroroneousDataSet)
            : base(exception, message, commandText, DXC.Technology.Exceptions.ExceptionHelper.GetDataSetErrors(erroroneousDataSet))
        {
        }
        public override String ExceptionPatternString()
        {
            return "{0}  Executing Command: {1} \r\n Erronous Dataset: \r\n{2}";
        }
        public override int ExceptionNumber()
        {
            return -346;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }


    /// <summary>
    /// Print Exception
    /// </summary>
    public class PrintException : ApplicationExceptionBase
    {
        public PrintException(System.Exception exception)
            : base(exception)
        {
        }
        public PrintException()
            : base()
        {
        }
        public override String ExceptionPatternString()
        {
            return "Print Error  No Default Printer Specified Or Printer Unavailable";
        }
        public override int ExceptionNumber()
        {
            return -331;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Medium;
        }
    }


    /// <summary>
    /// Throw this exception to indicate databinding from a certain field of a certain table to a control is invalid
    /// </summary>
    public class DataBindingToControlException : ApplicationExceptionBase
    {
        public DataBindingToControlException(string tableName, string fieldName, string controlName, string additionalInformation)
            : base(tableName, fieldName, controlName, additionalInformation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "DataBinding failed updating control {2} from table {0} field {1}. {3}";
        }
        public override int ExceptionNumber()
        {
            return -321;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    /// <summary>
    /// Throw this exception to indicate databinding from a control to a certain field in a certain table is invalid
    /// </summary>
    public class DataBindingFromControlException : ApplicationExceptionBase
    {
        public DataBindingFromControlException(string tableName, string fieldName, string controlName, string additionalInformation)
            : base(tableName, fieldName, controlName, additionalInformation)
        {
        }
        public override String ExceptionPatternString()
        {
            return "DataBinding failed updating DataSet table {0} field {1} from control {2}. {3}";
        }
        public override int ExceptionNumber()
        {
            return -322;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }

    public class CannotLogException : ApplicationExceptionBase
    {
        public CannotLogException()
        { }
        public override int ExceptionNumber()
        {
            return -572;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Critical;
        }
        public override string ExceptionPatternString()
        {
            return "Cannot Log Exception";
        }
    }

    /// <summary>
    /// Throw this exception to indicate that adding an Entity is not allowed
    /// </summary>
    public class AddEntityNotAllowedException : ApplicationExceptionBase
    {
        public AddEntityNotAllowedException(string entity, ExceptionPatternEnum externalPatternEnum, params object[] externalPatternArgs)
            : base(entity, EnumerationHelper.EnumToCode(externalPatternEnum), externalPatternArgs)
        {
        }

        private ExternalPatternException iExternalPatternException;
        private ExternalPatternException ExternalPatternException
        {
            get
            {
                if (iExternalPatternException == null)
                {
                    if (ExceptionArguments.Count == 1)
                    {
                        ExceptionArguments = ExceptionArguments.First().Split(',').ToList();
                    }
                    iExternalPatternException = new ExternalPatternException(EnumerationHelper.CodeToEnum<ExceptionPatternEnum>(ExceptionArguments[1]), ExceptionArguments.Skip(2).ToArray());
                }
                return iExternalPatternException;
            }
        }

        public override String ExceptionPatternString()
        {
            return "Add {0} is not allowed";
        }
        public override int ExceptionNumber()
        {
            return -323;
        }
        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }

        public override string FullMessage
        {
            get
            {
                string lExceptionPattern = this.ExceptionPatternString();
                string lEntity = this.ExceptionArguments[0];
                if (ExceptionHelper.TranslationManager != null)
                {
                    lExceptionPattern = ExceptionHelper.TranslationManager.Localize(lExceptionPattern);
                    lEntity = ExceptionHelper.TranslationManager.Localize(lEntity);
                }
                string lBaseMessage = string.Format(
                    DXC.Technology.Utilities.StringFormatProvider.Default,
                    lExceptionPattern,
                    lEntity);
                return lBaseMessage + ": " + ExternalPatternException.FullMessage;
            }
        }
    }

    public class ExternalPatternException : ApplicationExceptionBase
    {
        private ExceptionPatternEnum iExternalPatternEnum;

        public ExternalPatternException(ExceptionPatternEnum externalPatternEnum, params Object[] pExceptionArguments)
            : base(pExceptionArguments)
        {
            if (pExceptionArguments == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Exception Arguments");
            iExternalPatternEnum = externalPatternEnum;
        }

        public override int ExceptionNumber()
        {
            return -324;
        }

        public string ExternalPatternCode
        {
            get
            {
                return EnumerationHelper.EnumToCode(iExternalPatternEnum);
            }
        }

        public override string ExceptionPatternString()
        {
            switch (iExternalPatternEnum)
            {
                case ExceptionPatternEnum.P0001_TextContent_1BasedOnStandardText_2CanNotBeAddedToTextContent_3WithContentBasedOnStandardText_4:
                    return "TextContent {0} based on StandardText {1} can not be added to TextContent {2} with content based on StandardText {3}";
                case ExceptionPatternEnum.P0002_User_1HasNoPermissionForConfidentialDocument_2:
                    return "User {0} has no permission for Confidential document {1}";
                case ExceptionPatternEnum.P0003_UserHasNo_1PermissionForTextContent_2:
                    return "User has no {0} permission for TextContent {1}";
                case ExceptionPatternEnum.P0004_UserHasNoPermissionForConfidentialDocument_1:
                    return "User has no permission for Confidential document {0}";
                case ExceptionPatternEnum.P0005_UserHasNo_1PermissionForDocumentsOfType_2:
                    return "User has no {0} permission for Documents of Type {1}";
                case ExceptionPatternEnum.P0006_UserHasNo_1PermissionForNonPublishedDocumentsOfType_2ForLegalEntity_3:
                    return "User has no {0} permission for non-Published Documents of Type {1} for Legal Entity {2}";
                case ExceptionPatternEnum.P0007_UserOfLegalEntity_1HasNo_2PermissionForDocumentsOfLegalEntity_3:
                    return "User of Legal Entity {0} has no {1} permission for Documents of Legal Entity {2}";
                case ExceptionPatternEnum.P0008_AddingA_1TextcontentToA_2IsNotAllowed:
                    return "Adding a {0} TextContent to a {1} is not allowed";
                case ExceptionPatternEnum.P0009_UserHasNo_1PermissionForNonApprovedCataloguesForLegalEntity_2:
                    return "User has no {0} permission for non-approved catalogues for legal Entity {1}";
                case ExceptionPatternEnum.P0010_UserHasNo_1PermissionForCatalogues:
                    return "User has no {0} permission for catalogues";
                case ExceptionPatternEnum.P0011_UserOfLegalEntity_1HasNo_2PermissionForCataloguesOfLegalEntity_3:
                    return "User of Legal Entity {0} has no {1} permission for Catalogues of Legal Entity {2}";
                case ExceptionPatternEnum.P0012_DesignNumber_1AlreadyExistsChangeNumberAndSaveAgain:
                    return "DesignNumber {0} already exists: change DesignNumber and save again";
                case ExceptionPatternEnum.P0013_DesignNumbers_1AlreadyExistChangeNumbersAndSaveAgain:
                    return "DesignNumbers {0} already exist: change DesignNumbers and save again";
                case ExceptionPatternEnum.P0014_DesignNumber_1MayNotBeChangedInto_2BecauseItIsBasedOnAStandardDocument:
                    return "DesignNumber {0}  may not be chanted into {1} because it is based on a Standard Document";
                case ExceptionPatternEnum.P0015_DesignNumber_1MayNotBeChangedInto_2Because_3AlreadyExistsInBaseDocument_4:
                    return "DesignNumber {0} may not be changed into {1} because {2} already exists in Base Document {3}";
                case ExceptionPatternEnum.P0016_TextContentMayNotBeDeletedBecauseItIsStillReferencedInTextCont_1:
                    return "TextContent may not be deleted because it is still referenced in TextContent {0}";
                case ExceptionPatternEnum.P0017_TextContentMayNotBeDeletedBecauseItIsStillReferencedInTextContents_1:
                    return "TextContent may not be deleted because it is still referenced in TextContents {0}";
                case ExceptionPatternEnum.P0018_TheStandardDocumentWasNotApprovedYet:
                    return "The Standard Document was not approved yet";
                case ExceptionPatternEnum.P0019_TextContent_1WasAlreadyChangedByUser_2:
                    return "TextContent {0} was already changed by user {1}";
                case ExceptionPatternEnum.P0020_TextContents_1WereAlreadyChangedByUser_2:
                    return "TextContents {0} were already changed by user {1}";
                case ExceptionPatternEnum.P0021_TextContents_1WereAlreadyChangedByUsers_2:
                    return "TextContents {0} were already changed by users {1}";
                case ExceptionPatternEnum.P0022_TextContentDocument_1WasAlreadyChangedByUser_2:
                    return "TextContentDocument {0} was already changed by User {1}";
                case ExceptionPatternEnum.P0023_Catalogue_1WasAlreadyChangedByUser_2:
                    return "Catalogue {0} was already changed by User {1}";
                case ExceptionPatternEnum.P0024_DesignNumber_1AlreadyExistsInDocumentAndMayNotBeCopiedTwice:
                    return "DesignNumber {0} already exists in Document and may not be copied twice.";
                case ExceptionPatternEnum.P0025_StatusChangedToPublishedConfirmWithTheSaveButtonOrHitCancel:
                    return "Status changed to Published - Confirm with the save button or hit cancel";
                case ExceptionPatternEnum.P0026_UserHasNoPermissionForDocument_1:
                    return "User has no permission for Confidential document {0}";
                case ExceptionPatternEnum.P0027_Field_1IsMandatory:
                    return "Field {0} is mandatory";
                case ExceptionPatternEnum.P0028_TheCurrentStatusOfTheCatalogueDoesNotAllowToDelete:
                    return "The current status of the catalogue does not allow to delete";
                case ExceptionPatternEnum.P0029_CatalogueCanNotBeDeletedItIsStillLinkedToDocuments:
                    return "Catalogue can not be deleted.  It is still linked to Documents";
                case ExceptionPatternEnum.P0030_TheCurrentStatusOfTheDocumentDoesNotAllowToDelete:
                    return "The current status of the document does not allow to delete";
                case ExceptionPatternEnum.P0031_DocumentCanNotBeDeletedItHasLinkedDocuments:
                    return "Document can not be deleted. Is has linked documents.";
                case ExceptionPatternEnum.P0032_InvalidExcelFormatForCatalogue:
                    return "Invalid Excel format for Catalogue";
                case ExceptionPatternEnum.P0033_PleaseSelectAFileFirst:
                    return "Please select a File first";
                case ExceptionPatternEnum.P0034_Version_1AlreadyExistsForDocument:
                    return "Version {0} already exists for document";
                case ExceptionPatternEnum.P0035_TextContent_1CanNotBeAddedTo_2InDocument_3:
                    return "TextContent {0} can not be added to {1} in document {2}";
                case ExceptionPatternEnum.P0036_TextContents_1CanNotBeAddedTo_2InDocument_3:
                    return "TextContents {0} can not be added to {1} in document {2}";
                case ExceptionPatternEnum.P0037_PleaseSelectAtLeastOneTextContentToAddTo_1InDocument_2:
                    return "Please select at least one TextContent to add to {0} in Document {1}";
                case ExceptionPatternEnum.P0038_PleaseReduceTheSelectionOf_1TextContentsToAMaximumOf_2TextContents:
                    return "Please reduce the selection of {0} TextContents to a maximum of {1}";
                case ExceptionPatternEnum.P0039_Template_1WasAlreadyChangedByUser_2:
                    return "Template {0} was already changed by user {1}";
                case ExceptionPatternEnum.P0040_OnlyFilesWithExtension_1CanBeUploaded:
                    return "Only files with extension {0} can be uploaded";
                case ExceptionPatternEnum.P0041_DownloadTemplateForDocumentIsNotDefinedYet:
                    return "Download Template for Document is not defined yet";
                case ExceptionPatternEnum.P0042_User_1HasNo_2PermissionForTask_3_4:
                    return "User '{0}' has no '{1}' permission for task '{2}' ({3}).";
                case ExceptionPatternEnum.P0043_Field_1IsTooLongLengthOfValue_2Is_3MaximumLengthIs_4:
                    return "Field {0} is too long. The Length of value '{1}' is {2}. The Maximum Length is {3}.";
                case ExceptionPatternEnum.P0044_TextContent_1MayNotBeMovedBecauseItIsBasedOnStandardDocument_2:
                    return "TextContent {0} may not be moved because it is based on Standard Document {1}";
                default:
                    return "{0}";
            }
        }

        public override SeverityLevelEnum SeverityLevel()
        {
            return SeverityLevelEnum.Low;
        }
    }


}
