using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Excel
{
    public static class ExcelExportHelper
    {
        /// <summary>
        /// Exports a report to a CSV file.
        /// </summary>
        /// <param name="exportItems">The collection of export items to be written to the CSV file.</param>
        /// <param name="filename">The name of the file to which the report will be exported.</param>
        public static void ExportReport(ICollection<ExcelExportLine> exportItems, string filename)
        {
            var csvFile = new System.IO.FileInfo(filename);
            using (System.IO.FileStream csvStream = csvFile.OpenWrite())
            {
                WriteCsvFull(csvStream, exportItems);
            }
        }

        /// <summary>
        /// Reads a file and returns its content as a byte array.
        /// </summary>
        /// <param name="filename">The name of the file to be read.</param>
        /// <returns>A byte array containing the file's content.</returns>
        public static byte[] GetFile(string filename)
        {
            using (System.IO.FileStream fileStream = System.IO.File.OpenRead(filename))
            {
                byte[] data = new byte[fileStream.Length];
                int bytesRead = fileStream.Read(data, 0, data.Length);
                if (bytesRead != fileStream.Length)
                    throw new System.IO.IOException(filename);
                return data;
            }
        }

        /// <summary>
        /// Writes the full CSV content to the provided file stream.
        /// </summary>
        /// <param name="fileStream">The file stream to which the CSV content will be written.</param>
        /// <param name="exportItems">The collection of export items to be written to the CSV file.</param>
        internal static void WriteCsvFull(System.IO.FileStream fileStream, ICollection<ExcelExportLine> exportItems)
        {
            using var streamWriter = new System.IO.StreamWriter(fileStream, System.Text.Encoding.UTF8);
            var builder = new CsvStringBuilder("\t");

            foreach (var item in exportItems)
            {
                builder.Clear();
                builder.Append(item.Column01);
                builder.Append(item.Column02);
                builder.Append(item.Column03);
                builder.Append(item.Column04);
                builder.Append(item.Column05);
                builder.Append(item.Column06);
                builder.Append(item.Column07);
                builder.Append(item.Column08);
                builder.Append(item.Column09);
                builder.Append(item.Column10);
                builder.Append(item.Column11);
                builder.Append(item.Column12);
                builder.Append(item.Column13);
                builder.Append(item.Column14);
                builder.Append(item.Column15);
                builder.Append(item.Column16);
                builder.Append(item.Column17);
                builder.Append(item.Column18);
                builder.Append(item.Column19);
                builder.Append(item.Column20);
                builder.Append(item.Column21);
                builder.Append(item.Column22);
                builder.Append(item.Column23);
                builder.Append(item.Column24);
                builder.Append(item.Column25);
                builder.Append(item.Column26);
                builder.Append(item.Column27);
                builder.Append(item.Column28);
                builder.Append(item.Column29);
                builder.Append(item.Column30);
                builder.Append(item.Column31);
                builder.Append(item.Column32);
                builder.Append(item.Column33);
                builder.Append(item.Column34);
                builder.Append(item.Column35);

                streamWriter.WriteLine(builder.ToString());
            }
        }

        /// <summary>
        /// Converts a nullable boolean value to an Excel-compatible match value.
        /// </summary>
        /// <param name="value">The nullable boolean value to be converted.</param>
        /// <returns>A string representing the Excel-compatible match value.</returns>
        private static string ToExcelMatchValue(bool? value)
        {
            return value.HasValue && value.Value ? "x" : "";
        }

        /// <summary>
        /// Converts a string value to an Excel-compatible value.
        /// </summary>
        /// <param name="value">The string value to be converted.</param>
        /// <returns>A string representing the Excel-compatible value.</returns>
        private static string ToExcelValue(string value)
        {
            return value;
        }

        /// <summary>
        /// Converts a decimal value to an Excel-compatible value.
        /// </summary>
        /// <param name="value">The decimal value to be converted.</param>
        /// <returns>A string representing the Excel-compatible value.</returns>
        private static string ToExcelValue(decimal value)
        {
            return value.ToString(DXC.Technology.Utilities.DecimalFormatProvider.Default).Replace(".", ",");
        }

        /// <summary>
        /// Converts a nullable decimal value to an Excel-compatible value.
        /// </summary>
        /// <param name="value">The nullable decimal value to be converted.</param>
        /// <returns>A string representing the Excel-compatible value.</returns>
        private static string ToExcelValue(decimal? value)
        {
            return value.HasValue ? ToExcelValue(value.Value) : "";
        }
    }

    public class CsvStringBuilder
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();
        private readonly string separator = ";";

        /// <summary>
        /// Initializes a new instance of the CsvStringBuilder class with the default separator.
        /// </summary>
        public CsvStringBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvStringBuilder class with a specified separator.
        /// </summary>
        /// <param name="separator">The separator to be used in the CSV string.</param>
        public CsvStringBuilder(string separator)
        {
            this.separator = separator;
        }

        /// <summary>
        /// Appends a cell content to the CSV string.
        /// </summary>
        /// <param name="cellContent">The content of the cell to be appended.</param>
        public void Append(string cellContent)
        {
            var content = string.IsNullOrEmpty(cellContent) ? "" : cellContent.Replace(';', ';');
            stringBuilder.Append("\"" + content + "\"" + separator);
        }

        /// <summary>
        /// Appends a line to the CSV string.
        /// </summary>
        /// <param name="cellContent">The content of the line to be appended.</param>
        public void AppendLine(string cellContent)
        {
            stringBuilder.AppendLine(cellContent);
        }

        /// <summary>
        /// Clears the CSV string builder.
        /// </summary>
        public void Clear()
        {
            stringBuilder.Clear();
        }

        /// <summary>
        /// Returns the CSV string.
        /// </summary>
        /// <returns>The CSV string.</returns>
        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }

    [DataContract(IsReference = true)]
    public class ExcelExportLine
    {
        /// <summary>
        /// Gets or sets the type of the line in the Excel export.
        /// </summary>
        [DataMember]
        public ExcelLineType LineType { get; set; }

        /// <summary>
        /// Gets or sets the value of column 01.
        /// </summary>
        [DataMember]
        public string Column01 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 02.
        /// </summary>
        [DataMember]
        public string Column02 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 03.
        /// </summary>
        [DataMember]
        public string Column03 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 04.
        /// </summary>
        [DataMember]
        public string Column04 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 05.
        /// </summary>
        [DataMember]
        public string Column05 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 06.
        /// </summary>
        [DataMember]
        public string Column06 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 07.
        /// </summary>
        [DataMember]
        public string Column07 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 08.
        /// </summary>
        [DataMember]
        public string Column08 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 09.
        /// </summary>
        [DataMember]
        public string Column09 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 10.
        /// </summary>
        [DataMember]
        public string Column10 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 11.
        /// </summary>
        [DataMember]
        public string Column11 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 12.
        /// </summary>
        [DataMember]
        public string Column12 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 13.
        /// </summary>
        [DataMember]
        public string Column13 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 14.
        /// </summary>
        [DataMember]
        public string Column14 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 15.
        /// </summary>
        [DataMember]
        public string Column15 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 16.
        /// </summary>
        [DataMember]
        public string Column16 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 17.
        /// </summary>
        [DataMember]
        public string Column17 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 18.
        /// </summary>
        [DataMember]
        public string Column18 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 19.
        /// </summary>
        [DataMember]
        public string Column19 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 20.
        /// </summary>
        [DataMember]
        public string Column20 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 21.
        /// </summary>
        [DataMember]
        public string Column21 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 22.
        /// </summary>
        [DataMember]
        public string Column22 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 23.
        /// </summary>
        [DataMember]
        public string Column23 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 24.
        /// </summary>
        [DataMember]
        public string Column24 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 25.
        /// </summary>
        [DataMember]
        public string Column25 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 26.
        /// </summary>
        [DataMember]
        public string Column26 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 27.
        /// </summary>
        [DataMember]
        public string Column27 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 28.
        /// </summary>
        [DataMember]
        public string Column28 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 29.
        /// </summary>
        [DataMember]
        public string Column29 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 30.
        /// </summary>
        [DataMember]
        public string Column30 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 31.
        /// </summary>
        [DataMember]
        public string Column31 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 32.
        /// </summary>
        [DataMember]
        public string Column32 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 33.
        /// </summary>
        [DataMember]
        public string Column33 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 34.
        /// </summary>
        [DataMember]
        public string Column34 { get; set; }

        /// <summary>
        /// Gets or sets the value of column 35.
        /// </summary>
        [DataMember]
        public string Column35 { get; set; }

        /// <summary>
        /// Sets the value of a column by its index.
        /// </summary>
        /// <param name="index">The index of the column to be set.</param>
        /// <param name="value">The value to be assigned to the column.</param>
        public void SetColumn(int index, string value)
        {
            switch (index)
            {
                case 1:
                    Column01 = value;
                    break;
                case 2:
                    Column02 = value;
                    break;
                case 3:
                    Column03 = value;
                    break;
                case 4:
                    Column04 = value;
                    break;
                case 5:
                    Column05 = value;
                    break;
                case 6:
                    Column06 = value;
                    break;
                case 7:
                    Column07 = value;
                    break;
                case 8:
                    Column08 = value;
                    break;
                case 9:
                    Column09 = value;
                    break;
                case 10:
                    Column10 = value;
                    break;
                case 11:
                    Column11 = value;
                    break;
                case 12:
                    Column12 = value;
                    break;
                case 13:
                    Column13 = value;
                    break;
                case 14:
                    Column14 = value;
                    break;
                case 15:
                    Column15 = value;
                    break;
                case 16:
                    Column16 = value;
                    break;
                case 17:
                    Column17 = value;
                    break;
                case 18:
                    Column18 = value;
                    break;
                case 19:
                    Column19 = value;
                    break;
                case 20:
                    Column20 = value;
                    break;
                case 21:
                    Column21 = value;
                    break;
                case 22:
                    Column22 = value;
                    break;
                case 23:
                    Column23 = value;
                    break;
                case 24:
                    Column24 = value;
                    break;
                case 25:
                    Column25 = value;
                    break;
                case 26:
                    Column26 = value;
                    break;
                case 27:
                    Column27 = value;
                    break;
                case 28:
                    Column28 = value;
                    break;
                case 29:
                    Column29 = value;
                    break;
                case 30:
                    Column30 = value;
                    break;
                case 31:
                    Column31 = value;
                    break;
                case 32:
                    Column32 = value;
                    break;
                case 33:
                    Column33 = value;
                    break;
                case 34:
                    Column34 = value;
                    break;
                case 35:
                    Column35 = value;
                    break;
            }
        }
    }

    public enum ExcelLineType
    {
        /// <summary>
        /// Represents a content line in the Excel export.
        /// </summary>
        ContentLine = 0,

        /// <summary>
        /// Represents a title line in the Excel export.
        /// </summary>
        TitleLine = 1,

        /// <summary>
        /// Represents a blank line in the Excel export.
        /// </summary>
        BlankLine = 2
    }
}