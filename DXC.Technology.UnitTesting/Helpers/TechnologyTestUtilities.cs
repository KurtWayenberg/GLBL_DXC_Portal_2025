using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DXC.Technology.Enumerations;

namespace DXC.Technology.UnitTesting.Helpers
{
    public class TechnologyTestUtilities
    {
        #region Public Static Methods

        /// <summary>
        /// Creates a valid register number based on the input string.
        /// Example: 78.07.29 299 -> 78.07.29 299 33
        /// Example: 75.09.23 063 -> 75.09.23 063 12
        /// Example: 62.0310267' -> 62.0310267 77
        /// </summary>
        /// <param name="numberFrom1To9">Input string containing numbers from 1 to 9.</param>
        /// <returns>A valid register number.</returns>
        /// <exception cref="DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException">
        /// Thrown when the input string does not contain exactly 9 digits.
        /// </exception>
        public static string CreateValidRegisterNumber(string numberFrom1To9)
        {
            string registerNumber = string.Empty;
            const string NUMBERS = "0123456789";

            foreach (char character in numberFrom1To9.ToCharArray())
            {
                if (NUMBERS.IndexOf(character) > -1) registerNumber += character;
            }

            if (registerNumber.Length != 9)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException(
                    "Register Number", numberFrom1To9, "There should be 9 digits");
            }

            long value = Convert.ToInt64(registerNumber.Substring(0, 9));
            long remainder;
            Math.DivRem(value, 97, out remainder);
            long check = 97 - remainder;
            return string.Concat(registerNumber, check.ToString("00"));
        }

        /// <summary>
        /// Compares two DataSets for equality.
        /// </summary>
        /// <param name="dataSet1">First DataSet.</param>
        /// <param name="dataSet2">Second DataSet.</param>
        /// <returns>True if the DataSets are equal, otherwise false.</returns>
        public static bool AreDataSetsEqual(DataSet dataSet1, DataSet dataSet2)
        {
            return AreDataSetsEqual(dataSet1, dataSet2, false);
        }

        /// <summary>
        /// Compares two DataSets for equality with an option to check technical information.
        /// </summary>
        /// <param name="dataSet1">First DataSet.</param>
        /// <param name="dataSet2">Second DataSet.</param>
        /// <param name="checkTechnicalInfoToo">Whether to check technical information.</param>
        /// <returns>True if the DataSets are equal, otherwise false.</returns>
        public static bool AreDataSetsEqual(DataSet dataSet1, DataSet dataSet2, bool checkTechnicalInfoToo)
        {
            foreach (DataTable table1 in dataSet1.Tables)
            {
                if ((!table1.TableName.StartsWith("Workflow")) || checkTechnicalInfoToo)
                {
                    DataTable table2 = null;

                    if (dataSet2.Tables.Contains(table1.TableName))
                        table2 = dataSet2.Tables[table1.TableName];

                    if (table2 == null)
                        return false;

                    if (table1.Rows.Count != table2.Rows.Count)
                        return false;

                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        if (!AreDataRowsEqual(table1.Rows[i], table2.Rows[i], checkTechnicalInfoToo))
                            return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Compares two DataRows for equality.
        /// </summary>
        /// <param name="dataRow1">First DataRow.</param>
        /// <param name="dataRow2">Second DataRow.</param>
        /// <returns>True if the DataRows are equal, otherwise false.</returns>
        public static bool AreDataRowsEqual(DataRow dataRow1, DataRow dataRow2)
        {
            return AreDataRowsEqual(dataRow1, dataRow2, false);
        }

        /// <summary>
        /// Compares two DataRows for equality with an option to check technical information.
        /// </summary>
        /// <param name="dataRow1">First DataRow.</param>
        /// <param name="dataRow2">Second DataRow.</param>
        /// <param name="checkTechnicalInfoToo">Whether to check technical information.</param>
        /// <returns>True if the DataRows are equal, otherwise false.</returns>
        public static bool AreDataRowsEqual(DataRow dataRow1, DataRow dataRow2, bool checkTechnicalInfoToo)
        {
            if (dataRow1.Table.Columns.Count != dataRow2.Table.Columns.Count)
                return false;

            foreach (DataColumn column1 in dataRow1.Table.Columns)
            {
                DataColumn column2 = null;

                if (dataRow2.Table.Columns.Contains(column1.ColumnName))
                    column2 = dataRow2.Table.Columns[column1.ColumnName];

                if (((column1.ColumnName.IndexOf("_WF_") <= 0)  // Workflow Field
                    && (!DXC.Technology.Data.DataSetHelper.IsTrackingField(column1.ColumnName))
                    && (!DXC.Technology.Data.DataSetHelper.IsVersionNumberField(column1.ColumnName))
                    ) || checkTechnicalInfoToo)
                {
                    if (column2 == null)
                        return false;

                    if ((dataRow1[column1] == DBNull.Value) || (Convert.ToString(dataRow1[column1]).Equals(EnumerationHelper.BlankCode)))
                    {
                        if ((dataRow2[column2] != DBNull.Value) && (!Convert.ToString(dataRow2[column2]).Equals(EnumerationHelper.BlankCode)))
                            return false;
                    }
                    else
                    {
                        if ((dataRow2[column2] == DBNull.Value) || (Convert.ToString(dataRow2[column2]).Equals(EnumerationHelper.BlankCode)))
                            return false;
                        else if (!dataRow1[column1].Equals(dataRow2[column2]))
                        {
                            // A difference exists - the only acceptable difference is in auto increment field 
                            if ((column1.AutoIncrement && Convert.ToInt64(dataRow1[column1]) == 0) ||
                                (column2.AutoIncrement && Convert.ToInt64(dataRow2[column2]) == 0))
                            {
                                // OK no problem
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        #endregion
    }
}