using System;
using System.Collections.Generic;
using System.Text;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Enum representing various boolean formats for conversion.
    /// </summary>
    public enum BooleanFormatEnum
    {
        /// <summary>
        /// Standard boolean format (true/false).
        /// </summary>
        Boolean,

        /// <summary>
        /// Boolean format represented as 1/0.
        /// </summary>
        Value10,

        /// <summary>
        /// Boolean format represented as T/F.
        /// </summary>
        ValueTF,

        /// <summary>
        /// Boolean format represented as True/False.
        /// </summary>
        ValueTrueFalse,

        /// <summary>
        /// Boolean format represented as Y/N.
        /// </summary>
        ValueYN,

        /// <summary>
        /// Boolean format represented as Yes/No.
        /// </summary>
        ValueYesNo
    }

    /// <summary>
    /// Utility class for boolean conversions.
    /// </summary>
    public sealed class Boolean
    {
        #region Constructors

        /// <summary>
        /// Private constructor to prevent instantiation.
        /// </summary>
        private Boolean()
        {
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Converts a boolean value to a string using the default format.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <returns>A string representation of the boolean value.</returns>
        public static string ConvertToString(bool value)
        {
            return ConvertToString(value, BooleanFormatEnum.Boolean);
        }

        /// <summary>
        /// Converts a boolean value to a string using the specified format.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="booleanFormat">The format to use for conversion.</param>
        /// <returns>A string representation of the boolean value.</returns>
        public static string ConvertToString(bool value, BooleanFormatEnum booleanFormat)
        {
            switch (booleanFormat)
            {
                case BooleanFormatEnum.Boolean:
                    return ConvertToString(value, "SYSBool");
                case BooleanFormatEnum.Value10:
                    return ConvertToString(value, "1/0");
                case BooleanFormatEnum.ValueTF:
                    return ConvertToString(value, "T/F");
                case BooleanFormatEnum.ValueTrueFalse:
                    return ConvertToString(value, "True/False");
                case BooleanFormatEnum.ValueYN:
                    return ConvertToString(value, "Y/N");
                case BooleanFormatEnum.ValueYesNo:
                    return ConvertToString(value, "Yes/No");
                default:
                    throw new DXC.Technology.Exceptions.NamedExceptions.ToBeImplementedException("DXC.Technology.Boolean.Utilities.Boolean", "ConvertFromString");
            }
        }

        /// <summary>
        /// Converts a boolean value to a string using a custom format.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="booleanFormat">The custom format to use for conversion.</param>
        /// <returns>A string representation of the boolean value.</returns>
        public static string ConvertToString(bool value, string booleanFormat)
        {
            bool localValue = value;
            switch (booleanFormat)
            {
                case "SYSBool":
                    return localValue.ToString();
                case "1/0":
                case "T/F":
                case "True/False":
                case "Y/N":
                case "YES/NO":
                    string[] trueFalseValues = booleanFormat.Split('/');
                    if (trueFalseValues.Length != 2)
                        throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("BooleanFormat", booleanFormat, "Expected Format: <true value>/<false value>");
                    return localValue ? trueFalseValues[0] : trueFalseValues[1];
                default:
                    throw new DXC.Technology.Exceptions.NamedExceptions.ToBeImplementedException("DXC.Technology.Boolean.Utilities.Boolean", "ConvertFromString");
            }
        }

        /// <summary>
        /// Converts a string to a boolean value using the specified format.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="booleanFormat">The format to use for conversion.</param>
        /// <returns>A boolean representation of the string value.</returns>
        public static bool ConvertFromString(string value, BooleanFormatEnum booleanFormat)
        {
            if (string.IsNullOrEmpty(value)) return false;
            switch (booleanFormat)
            {
                case BooleanFormatEnum.Boolean:
                    return ConvertFromString(value, "SYSBool");
                case BooleanFormatEnum.Value10:
                    return ConvertFromString(value, "1/0");
                case BooleanFormatEnum.ValueTF:
                    return ConvertFromString(value, "T/F");
                case BooleanFormatEnum.ValueTrueFalse:
                    return ConvertFromString(value, "True/False");
                case BooleanFormatEnum.ValueYN:
                    return ConvertFromString(value, "Y/N");
                case BooleanFormatEnum.ValueYesNo:
                    return ConvertFromString(value, "Yes/No");
                default:
                    throw new DXC.Technology.Exceptions.NamedExceptions.ToBeImplementedException("DXC.Technology.Boolean.Utilities.Boolean", "ConvertFromString");
            }
        }

        /// <summary>
        /// Converts a string to a boolean value using a custom format.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="booleanFormat">The custom format to use for conversion.</param>
        /// <returns>A boolean representation of the string value.</returns>
        public static bool ConvertFromString(string value, string booleanFormat)
        {
            switch (booleanFormat)
            {
                case "SYSBool":
                    switch (value)
                    {
                        case "1":
                        case "T":
                        case "True":
                        case "Y":
                        case "Yes":
                        case "t":
                        case "true":
                        case "y":
                        case "yes":
                            return true;
                        case "0":
                        case "F":
                        case "False":
                        case "N":
                        case "No":
                        case "f":
                        case "false":
                        case "n":
                        case "no":
                            return false;
                        default:
                            if (!bool.TryParse(value, out bool result))
                                return false;
                            return result;
                    }
                case "1/0":
                case "T/F":
                case "True/False":
                case "Y/N":
                case "YES/NO":
                    string[] trueFalseValues = booleanFormat.Split('/');
                    if (trueFalseValues.Length != 2)
                        throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("BooleanFormat", booleanFormat, "Expected Format: <true value>/<false value>");
                    if (value.Equals(trueFalseValues[0])) return true;
                    if (value.Equals(trueFalseValues[1])) return false;
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Boolean", value, "Expected Format: " + booleanFormat);
                default:
                    throw new DXC.Technology.Exceptions.NamedExceptions.ToBeImplementedException("DXC.Technology.Boolean.Utilities.Boolean", "ConvertFromString");
            }
        }

        #endregion
    }
}