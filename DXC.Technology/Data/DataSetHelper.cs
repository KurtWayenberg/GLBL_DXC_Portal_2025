using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Data
{
    /// <summary>
    /// Provides helper methods for working with DataSet objects.
    /// </summary>
    public static class DataSetHelper
    {
        #region Public Static Methods

        /// <summary>
        /// Gets the primary key fields in a displayable format.
        /// </summary>
        /// <param name="dataRow">The DataRow to retrieve keys from.</param>
        /// <returns>A string representation of the primary key fields.</returns>
        public static string GetPrimaryKeyFields(DataRow dataRow)
        {
            if (dataRow == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Data Row");

            using StringWriter stringWriter = new StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default);
            stringWriter.Write("|");
            foreach (DataColumn dataColumn in dataRow.Table.PrimaryKey)
            {
                stringWriter.Write(dataColumn.ColumnName);
                stringWriter.Write(":");
                stringWriter.Write(dataRow[dataColumn]);
                stringWriter.Write("|");
            }
            return stringWriter.ToString();
        }

        #endregion
    }
}