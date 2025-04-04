using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Utilty class implementing some old VB goodies and other string routines
    /// </summary>
    public sealed class String
    {
        private String()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static string[] iPlaceHolders = { "{0}", "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}" };

        /// <summary>
        /// The good old VB-right to get the last X characters or the whole string if shorter
        /// </summary>
        /// <param name="text">String</param>
        /// <param name="pNumberOfCharacters">MAX Number of characters at the end</param>
        /// <returns></returns>
        public static string Right(string text, int pNumberOfCharacters)
        {
            if (text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Text String");
            if (pNumberOfCharacters >= text.Length)
                return text;
            return text.Substring(text.Length - pNumberOfCharacters, pNumberOfCharacters);
        }

        public static string CreateUnambiguouslySearchableCSV(string pDependentCodesCSV)
        {
            if (string.IsNullOrEmpty(pDependentCodesCSV)) return pDependentCodesCSV;
            string lDependentCodesCSV = pDependentCodesCSV.Replace(" ", "");
            if (!lDependentCodesCSV.StartsWith(",", Utilities.StringComparisonProvider.Default))
                lDependentCodesCSV = "," + lDependentCodesCSV;
            if (!lDependentCodesCSV.EndsWith(",", Utilities.StringComparisonProvider.Default))
                lDependentCodesCSV = lDependentCodesCSV + ",";
            return lDependentCodesCSV;
        }

        /// <summary>
        /// The good old VB-left to get the first X characters or the whole string if shorter
        /// </summary>
        /// <param name="text">String</param>
        /// <param name="pNumberOfCharacters">MAX Number of characters at the start</param>
        /// <returns></returns>
        public static string Left(string text, int pNumberOfCharacters)
        {
            if (text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Text String");
            if (pNumberOfCharacters >= text.Length)
                return text;
            return text.Substring(0, pNumberOfCharacters);
        }


        public enum FormatStringAlignmentEnum
        {
            Left,
            Right,
            Center
        }

        public static string FormatString(string text, int pTotalNumbeOfCharacters)
        {
            return FormatString(text, pTotalNumbeOfCharacters, FormatStringAlignmentEnum.Right, " ");
        }

        public static string FormatString(int number, int pTotalNumbeOfCharacters)
        {
            return FormatString(number.ToString(DXC.Technology.Utilities.StringFormatProvider.Default), pTotalNumbeOfCharacters, FormatStringAlignmentEnum.Left, " ");
        }

        public static string FormatString(string text, int pTotalNumbeOfCharacters, FormatStringAlignmentEnum pFormatStringAlignment, string pFormatCharacter)
        {
            string lText = string.Empty;
            if (!string.IsNullOrEmpty(text))
                lText = text;
            System.IO.StringWriter lsw = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default);
            if (lText.Length > pTotalNumbeOfCharacters)
                lText = lText.Substring(0, pTotalNumbeOfCharacters);
            int lCharsToFill = pTotalNumbeOfCharacters - lText.Length;
            int lCharsLeft = 0;
            int lCharsRight = 0;

            if (lCharsToFill > 0)
            {
                switch (pFormatStringAlignment)
                {
                    case FormatStringAlignmentEnum.Left:
                        lCharsLeft = lCharsToFill;
                        break;
                    case FormatStringAlignmentEnum.Right:
                        lCharsRight = lCharsToFill;
                        break;
                    case FormatStringAlignmentEnum.Center:
                        lCharsLeft = ((lCharsToFill - 1) / 2) + 1;
                        lCharsRight = lCharsToFill - lCharsLeft;
                        break;
                }
            }

            while (lCharsLeft > 0)
            {
                lsw.Write(pFormatCharacter);
                lCharsLeft--;
            }
            lsw.Write(lText);
            while (lCharsRight > 0)
            {
                lsw.Write(pFormatCharacter);
                lCharsRight--;
            }
            return lsw.ToString();
        }

        /// <summary>
        /// Returns the vertical representation of a string e.g. Hello returns
        /// H
        /// E
        /// L
        /// L
        /// O
        /// This is needed because Crystal Reporting Web Reports does not support 90% text rotation
        /// Yet some forms do use this. In web context the otherwise rotated text is verticalized...
        /// </summary>
        /// <param name="text">String to veticalize</param>
        /// <returns></returns>
        public static string Vertical(string text)
        {
            if (text == null)
                return string.Empty;
            System.IO.StringWriter lsw = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default);
            foreach (char c in text)
            {
                lsw.WriteLine(c);
            }
            return lsw.ToString();
        }

        /// <summary>
        /// Remove Whitespace
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveWhitespace(string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }

        /// <summary>
        /// The InsertOrConcat-method (which is a never-crash version of the String.Format, never generating errors and always 
        /// appending all specified arguments  
        /// </summary>
        /// <param name="pTemplate"></param>
        /// <param name="pArguments"></param>
        /// <returns></returns>
        public static string InsertOrConcat(string pTemplate, string[] pArguments)
        {
            if (pTemplate == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Template");
            if ((pArguments == null) || (pArguments.Length == 0))
                return pTemplate;

            for (int i = 0; i < pArguments.Length; i++)
            {
                if (pTemplate.IndexOf(iPlaceHolders[i], Utilities.StringComparisonProvider.Default) < 0)
                    pTemplate = string.Concat(pTemplate, " ", pArguments[i]);
                else
                    pTemplate = pTemplate.Replace(iPlaceHolders[i], pArguments[i]);
            }
            return pTemplate;
        }

        /// <summary> 
        /// Trims double spaces to single space 
        /// </summary> 
        /// <param name="text">string to trim </param> 
        /// <returns>trimmed string with no more than single space </returns> 
        public static string TrimDoubleSpaces(string text)
        {
            if (text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Text String");

            string stringSingleSpace = " ";
            string stringDoubleSpace = "  ";
            while (text.IndexOf(stringDoubleSpace, Utilities.StringComparisonProvider.Default) > 0)
                text = text.Replace(stringDoubleSpace, stringSingleSpace);
            return text;
        }

        /// <summary> 
        /// Removes all quotes from the [JSON] string 
        /// </summary> 
        /// <param name="text">[JSON] string to remove quotes from </param> 
        /// <returns>string with no beginning and ending quote </returns> 
        public static string Unquote(string text)
        {
            if (text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Text String");

            if (text.StartsWith(@"""")) text = text.Substring(1);
            if (text.EndsWith(@"""")) text = text.Substring(0, text.Length - 1);
            return text;
        }
        /// <summary> 
        /// Trims all spaces 
        /// </summary> 
        /// <param name="text">string to trim </param> 
        /// <returns>trimmed string with no single spaces </returns> 
        public static string TrimAllSpaces(string text)
        {
            if (text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Text String");

            string stringSingleSpace = " ";

            while (text.IndexOf(stringSingleSpace, Utilities.StringComparisonProvider.Default) >= 0)
                text = text.Replace(stringSingleSpace, "");
            return text;
        }

        public static string ConvertTo_az_AZ_09_Blank_String(string text)
        {
            return ConvertTo_az_AZ_09_Blank_String(text, string.Empty);
        }

        public static string ConvertTo_az_AZ_09_Blank_String(string text, string pAllowedPunctuationCharacters)
        {
            string lResultString = text;
            if (string.IsNullOrEmpty(text)) return string.Empty;
            lResultString = lResultString.Replace("Š", "S");
            lResultString = lResultString.Replace("Œ", "OE");
            lResultString = lResultString.Replace("Ž", "Z");
            lResultString = lResultString.Replace("š", "s");
            lResultString = lResultString.Replace("œ", "oe");
            lResultString = lResultString.Replace("ž", "z");
            lResultString = lResultString.Replace("Ÿ", "Y");
            lResultString = lResultString.Replace("¡", "i");
            lResultString = lResultString.Replace("¢", "c");
            lResultString = lResultString.Replace("ª", "a");
            lResultString = lResultString.Replace("µ", "u");
            lResultString = lResultString.Replace("À", "A");
            lResultString = lResultString.Replace("Á", "A");
            lResultString = lResultString.Replace("Â", "A");
            lResultString = lResultString.Replace("Ã", "A");
            lResultString = lResultString.Replace("Ä", "A");
            lResultString = lResultString.Replace("Å", "A");
            lResultString = lResultString.Replace("Æ", "AE");
            lResultString = lResultString.Replace("Ç", "C");
            lResultString = lResultString.Replace("È", "E");
            lResultString = lResultString.Replace("É", "E");
            lResultString = lResultString.Replace("Ê", "E");
            lResultString = lResultString.Replace("Ë", "E");
            lResultString = lResultString.Replace("Ì", "I");
            lResultString = lResultString.Replace("Í", "I");
            lResultString = lResultString.Replace("Î", "I");
            lResultString = lResultString.Replace("Ï", "I");
            lResultString = lResultString.Replace("Ð", "D");
            lResultString = lResultString.Replace("Ñ", "N");
            lResultString = lResultString.Replace("Ò", "O");
            lResultString = lResultString.Replace("Ó", "O");
            lResultString = lResultString.Replace("Ô", "O");
            lResultString = lResultString.Replace("Õ", "O");
            lResultString = lResultString.Replace("Ö", "O");
            lResultString = lResultString.Replace("×", "X");
            lResultString = lResultString.Replace("Ø", "O");
            lResultString = lResultString.Replace("Ù", "U");
            lResultString = lResultString.Replace("Ú", "U");
            lResultString = lResultString.Replace("Û", "U");
            lResultString = lResultString.Replace("Ü", "U");
            lResultString = lResultString.Replace("Ý", "Y");
            lResultString = lResultString.Replace("Þ", "R");
            lResultString = lResultString.Replace("ß", "B");
            lResultString = lResultString.Replace("à", "a");
            lResultString = lResultString.Replace("á", "a");
            lResultString = lResultString.Replace("â", "a");
            lResultString = lResultString.Replace("ã", "a");
            lResultString = lResultString.Replace("ä", "a");
            lResultString = lResultString.Replace("å", "a");
            lResultString = lResultString.Replace("æ", "ae");
            lResultString = lResultString.Replace("ç", "c");
            lResultString = lResultString.Replace("è", "e");
            lResultString = lResultString.Replace("é", "e");
            lResultString = lResultString.Replace("ê", "e");
            lResultString = lResultString.Replace("ë", "e");
            lResultString = lResultString.Replace("ì", "i");
            lResultString = lResultString.Replace("í", "i");
            lResultString = lResultString.Replace("î", "i");
            lResultString = lResultString.Replace("ï", "i");
            lResultString = lResultString.Replace("ð", "o");
            lResultString = lResultString.Replace("ñ", "n");
            lResultString = lResultString.Replace("ò", "o");
            lResultString = lResultString.Replace("ó", "o");
            lResultString = lResultString.Replace("ô", "o");
            lResultString = lResultString.Replace("õ", "o");
            lResultString = lResultString.Replace("ö", "o");
            lResultString = lResultString.Replace("ø", "oe");
            lResultString = lResultString.Replace("ù", "u");
            lResultString = lResultString.Replace("ú", "u");
            lResultString = lResultString.Replace("û", "u");
            lResultString = lResultString.Replace("ü", "u");
            lResultString = lResultString.Replace("ý", "y");
            lResultString = lResultString.Replace("þ", "r");
            lResultString = lResultString.Replace("ÿ", "y");

            string lAllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 " + pAllowedPunctuationCharacters;
            string lNewResultString = lResultString;
            foreach (char lChar in lResultString)
            {
                if (lAllowedCharacters.IndexOf(lChar) < 0)
                {
                    lNewResultString = lNewResultString.Replace(Convert.ToString(lChar, DXC.Technology.Utilities.StringFormatProvider.Default), "");
                }
            }
            return lNewResultString;
        }

        public static string StripNewLineCharacters(string message)
        {
            if (string.IsNullOrEmpty(message)) return "";

            return message.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", "");
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64Text)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Convert.FromBase64String(base64Text));
            System.IO.StreamReader sr = new System.IO.StreamReader(ms);
            return sr.ReadToEnd();
        }
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string ZipAsBase64(string str)
        {
            return System.Convert.ToBase64String(Zip(str));
        }
        public static byte[] Zip(string str)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new System.IO.Compression.GZipStream(mso, System.IO.Compression.CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }
        public static string UnZipFromBase64(string str)
        {
            return Unzip(System.Convert.FromBase64String(str));
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new System.IO.Compression.GZipStream(msi, System.IO.Compression.CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return System.Text.Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string ConvertTo_AZ_09_String(string text)
        {
            return ConvertTo_AZ_09_Blank_String(text, "_").Replace(' ', '_');
        }
        public static string ConvertTo_AZ_09_Blank_String(string text)
        {
            return ConvertTo_AZ_09_Blank_String(text, "_");
        }

        public static string ConvertTo_AZ_09_Blank_String(string text, string pAllowedPunctuationCharacters)
        {
            string lResultString = text;
            if (string.IsNullOrEmpty(text)) return string.Empty;
            lResultString = lResultString.Replace("Š", "S");
            lResultString = lResultString.Replace("Œ", "OE");
            lResultString = lResultString.Replace("Ž", "Z");
            lResultString = lResultString.Replace("š", "s");
            lResultString = lResultString.Replace("œ", "oe");
            lResultString = lResultString.Replace("ž", "z");
            lResultString = lResultString.Replace("Ÿ", "Y");
            lResultString = lResultString.Replace("¡", "I");
            lResultString = lResultString.Replace("¢", "C");
            lResultString = lResultString.Replace("ª", "A");
            lResultString = lResultString.Replace("µ", "U");
            lResultString = lResultString.Replace("À", "A");
            lResultString = lResultString.Replace("Á", "A");
            lResultString = lResultString.Replace("Â", "A");
            lResultString = lResultString.Replace("Ã", "A");
            lResultString = lResultString.Replace("Ä", "A");
            lResultString = lResultString.Replace("Å", "A");
            lResultString = lResultString.Replace("Æ", "AE");
            lResultString = lResultString.Replace("Ç", "C");
            lResultString = lResultString.Replace("È", "E");
            lResultString = lResultString.Replace("É", "E");
            lResultString = lResultString.Replace("Ê", "E");
            lResultString = lResultString.Replace("Ë", "E");
            lResultString = lResultString.Replace("Ì", "I");
            lResultString = lResultString.Replace("Í", "I");
            lResultString = lResultString.Replace("Î", "I");
            lResultString = lResultString.Replace("Ï", "I");
            lResultString = lResultString.Replace("Ð", "D");
            lResultString = lResultString.Replace("Ñ", "N");
            lResultString = lResultString.Replace("Ò", "O");
            lResultString = lResultString.Replace("Ó", "O");
            lResultString = lResultString.Replace("Ô", "O");
            lResultString = lResultString.Replace("Õ", "O");
            lResultString = lResultString.Replace("Ö", "O");
            lResultString = lResultString.Replace("×", "X");
            lResultString = lResultString.Replace("Ø", "O");
            lResultString = lResultString.Replace("Ù", "U");
            lResultString = lResultString.Replace("Ú", "U");
            lResultString = lResultString.Replace("Û", "U");
            lResultString = lResultString.Replace("Ü", "U");
            lResultString = lResultString.Replace("Ý", "Y");
            lResultString = lResultString.Replace("Þ", "R");
            lResultString = lResultString.Replace("ß", "B");
            lResultString = lResultString.Replace("à", "A");
            lResultString = lResultString.Replace("á", "A");
            lResultString = lResultString.Replace("â", "A");
            lResultString = lResultString.Replace("ã", "A");
            lResultString = lResultString.Replace("ä", "A");
            lResultString = lResultString.Replace("å", "A");
            lResultString = lResultString.Replace("æ", "AE");
            lResultString = lResultString.Replace("ç", "C");
            lResultString = lResultString.Replace("è", "E");
            lResultString = lResultString.Replace("é", "E");
            lResultString = lResultString.Replace("ê", "E");
            lResultString = lResultString.Replace("ë", "E");
            lResultString = lResultString.Replace("ì", "I");
            lResultString = lResultString.Replace("í", "I");
            lResultString = lResultString.Replace("î", "I");
            lResultString = lResultString.Replace("ï", "I");
            lResultString = lResultString.Replace("ð", "O");
            lResultString = lResultString.Replace("ñ", "N");
            lResultString = lResultString.Replace("ò", "O");
            lResultString = lResultString.Replace("ó", "O");
            lResultString = lResultString.Replace("ô", "O");
            lResultString = lResultString.Replace("õ", "O");
            lResultString = lResultString.Replace("ö", "O");
            lResultString = lResultString.Replace("ø", "OE");
            lResultString = lResultString.Replace("ù", "U");
            lResultString = lResultString.Replace("ú", "U");
            lResultString = lResultString.Replace("û", "U");
            lResultString = lResultString.Replace("ü", "U");
            lResultString = lResultString.Replace("ý", "Y");
            lResultString = lResultString.Replace("þ", "R");
            lResultString = lResultString.Replace("ÿ", "Y");

            string lAllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 " + pAllowedPunctuationCharacters;
            string lNewResultString = lResultString;
            foreach (char lChar in lResultString)
            {
                if (lAllowedCharacters.IndexOf(lChar) < 0)
                {
                    lNewResultString = lNewResultString.Replace(Convert.ToString(lChar, DXC.Technology.Utilities.StringFormatProvider.Default), "");
                }
            }
            return lNewResultString;
        }

        public static string ConvertTo_Byte_String(string text)
        {
            string lResultString = text.ToLower();
            StringWriter sw = new StringWriter();
            string lHex = "0123456789abcdef";
            string lHex2 = "ghijklmnopqrstuv";
            foreach (char lChar in lResultString)
            {
                string newchar = "0";
                if (lHex.Contains(lChar))
                    newchar = lChar.ToString();
                else if (lHex2.Contains(lChar))
                {
                    newchar = lHex[lHex2.IndexOf(lChar)].ToString();
                }

                sw.Write(newchar);

            }
            return sw.ToString();
        }


        public static string ConvertTo_Hex_String(string text)
        {
            string lResultString = text;

            string lAllowedCharacters = "0123456789ABCDEF";
            string lNewResultString = lResultString;
            foreach (char lChar in lResultString)
            {
                if (lAllowedCharacters.IndexOf(lChar) < 0)
                {
                    lNewResultString = lNewResultString.Replace(Convert.ToString(lChar, DXC.Technology.Utilities.StringFormatProvider.Default), "");
                }
            }
            return lNewResultString;
        }

        public static string ConvertTo_09_String(string text)
        {
            return ConvertTo_09_String(text, "_");
        }

        public static string ConvertTo_09_String(string text, string pAllowedPunctuationCharacters)
        {
            string lResultString = text;

            string lAllowedCharacters = "0123456789" + pAllowedPunctuationCharacters;
            string lNewResultString = lResultString;
            foreach (char lChar in lResultString)
            {
                if (lAllowedCharacters.IndexOf(lChar) < 0)
                {
                    lNewResultString = lNewResultString.Replace(Convert.ToString(lChar, DXC.Technology.Utilities.StringFormatProvider.Default), "");
                }
            }
            return lNewResultString;
        }

        public static string IncrementStandardText(string text)
        {
            return IncrementCustomText(text.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default), "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }
        public static string IncrementBarcodeText(string text)
        {
            return IncrementCustomText(text.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default), "0123456789ACEHKLNPVXZ");
        }
        public static string IncrementCustomText(string text, string pAllowedCharacters)
        {
            string lText = text;

            if (string.IsNullOrEmpty(lText))
                return Convert.ToString(pAllowedCharacters[0], DXC.Technology.Utilities.CultureInfoProvider.Default);

            //take the last character 
            char lLastChar = lText[lText.Length - 1];
            int lLastCharIndex = pAllowedCharacters.IndexOf(lLastChar);

            if (lLastCharIndex < 0)
                throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidValueException("Character", Convert.ToString(lLastChar, DXC.Technology.Utilities.CultureInfoProvider.Default), pAllowedCharacters);

            if (lLastCharIndex == pAllowedCharacters.Length - 1)
            {
                //We are at the end of a series...

                //Take away the last character
                if (lText.Length <= 1)
                {
                    //This is already the last character
                    //Increase The length !!!
                    return string.Concat(pAllowedCharacters[0], pAllowedCharacters[0]);
                }
                else
                {
                    // Increment the smaller part and start a new series
                    string lTextMinusLast = lText.Substring(0, lText.Length - 1);
                    string lTextMinusLastIntremented = IncrementCustomText(lTextMinusLast, pAllowedCharacters);
                    return string.Concat(lTextMinusLastIntremented, pAllowedCharacters[0]);
                }

            }
            else
            {
                // replace the last character by a next character 
                string lTextMinusLast = lText.Substring(0, lText.Length - 1);
                return string.Concat(lTextMinusLast, pAllowedCharacters[lLastCharIndex + 1]);
            }
        }
        public static string Centralize(string text, int pWidth)
        {
            if (string.IsNullOrEmpty(text)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Text to centralize");

            if (text.Length >= pWidth) return text;
            int lLeftSpaces = Convert.ToInt16((pWidth - text.Length - 1) / 2, DXC.Technology.Utilities.IntFormatProvider.Default) + 1;
            using (System.IO.StringWriter lsw = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default))
            {
                for (int i = 1; i <= lLeftSpaces; i++)
                {
                    lsw.Write(' ');
                }
                lsw.Write(text);
                return lsw.ToString();
            }
        }

        /// <summary>
        /// Creates a Hashtable out of a simple string 
        /// String: "X=A;Y=B;Z=C"	Hashtable:    "X" -> "A" ; "Y" -> "B" ; "Z" -> "C"; 
        /// String: "A;B;C"         Hashtable:    "A" -> "A" ; "B" -> "B" ; "C" -> "C"; 
        /// String: "JustAValue"    Hashtable:    "JustAValue" -> "JustAValue"
        /// String: ""				Hashtable:    Empty
        /// </summary>
        /// <param name="text">String to Convert to Hashtable</param>
        /// <returns></returns>
        public static System.Collections.Hashtable ToHashtable(string text)
        {
            if (text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Text String");

            System.Collections.Hashtable lResult = new System.Collections.Hashtable();
            string[] lElements = text.Split(';');
            foreach (string s in lElements)
            {
                string[] lElementParts = s.Split('=');
                if (lElementParts.Length < 2) //simple CSV
                {
                    lResult.Add(s, s);
                }
                else
                {
                    lResult.Add(lElementParts[0], lElementParts[1]);
                }
            }
            return lResult;
        }

        /// <summary>
        /// Creates a Hashtable out of a simple string 
        /// Hashtable:    "X" -> "A" ; "Y" -> "B" ; "Z" -> "C";		String: "X=A;Y=B;Z=C"	
        /// Hashtable:    "A" -> "A" ; "B" -> "B" ; "C" -> "C";		String: "A;B;C"         
        /// Hashtable:    "A" -> "A" ; "B" -> "B" ; "Z" -> "C";		String: "A=A;B=B;Z=C"	
        /// Hashtable:    "JustAValue" -> "JustAValue"				String: "JustAValue"    
        /// Hashtable:    Empty										String: ""				
        /// </summary>
        /// <param name="pHashtable">Hash Table </param>
        /// <returns></returns>
        public static string FromHashtable(System.Collections.Hashtable pHashtable)
        {
            if (pHashtable == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Hashtable");

            using (System.IO.StringWriter lsw = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default))
            {
                bool lCSV = true;
                foreach (string s in pHashtable.Keys)
                {
                    string v = Convert.ToString(pHashtable[s], DXC.Technology.Utilities.StringFormatProvider.Default);
                    if (!s.Equals(v)) lCSV = false;
                }

                bool lFirst = true;
                foreach (string s in pHashtable.Keys)
                {
                    if (!lFirst)
                    {
                        lsw.Write(";");
                    }
                    if (lCSV)
                        lsw.Write(s);
                    else
                        lsw.Write(string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}={1}", s, Convert.ToString(pHashtable[s], DXC.Technology.Utilities.StringFormatProvider.Default)));
                    lFirst = false;
                }
                return lsw.ToString();
            }
        }

        public static ArrayList AndElementsFromLSVString(string pLSV)
        {
            return ElementsFromLSVString(pLSV, "+");
        }
        public static ArrayList OrElementsFromLSVString(string pLSV)
        {
            return ElementsFromLSVString(pLSV, "-");
        }
        public static ArrayList NotElementsFromLSVString(string pLSV)
        {
            return ElementsFromLSVString(pLSV, @"\");
        }
        private static ArrayList ElementsFromLSVString(string pLSV, string pPrefix)
        {
            string[] lElements = pLSV.Split('+', '-', '\\');
            ArrayList lResult = new ArrayList();
            foreach (string lElement in lElements)
            {
                //Find lElement in original string and look at its prefix
                int lIndex = pLSV.IndexOf(lElement, Utilities.StringComparisonProvider.Default);
                if (pPrefix.Equals(pLSV.Substring(lIndex - 1, 1)))
                    lResult.Add(lElement);
            }
            return lResult;
        }

        /// <summary>
        /// Count Occurences of a particular SubString in BaseString
        /// </summary>
        /// <param name="pBaseString"></param>
        /// <param name="pSubstring"></param>
        /// <returns></returns>
        public static long CountOccurrences(string pBaseString, string pSubstring)
        {
            long StringOccurences = 0;
            if (pBaseString.Length == 0 || pSubstring.Length == 0)
            {
                StringOccurences = -1;
                return StringOccurences;
            }
            else
            {

                while (pBaseString.IndexOf(pSubstring, Utilities.StringComparisonProvider.Default) > -1)
                {
                    StringOccurences++;
                    pBaseString = pBaseString.Substring(pBaseString.IndexOf(pSubstring, Utilities.StringComparisonProvider.Default) + pSubstring.Length);
                }
            }
            return StringOccurences;

        }

        //used for nice aligning out tab titles where sometmies it is split over 2 lines
        public static string CreateChar39DisplayString(string text, int pNumberOfLines, int pNrOfCharactersBeforeSplit, int pNrOfCharsInBalanceBeforeHardSplit, bool pHardSplit)
        {
            //Stupid name it should have been CHR 160 - but on the mainframe it is EBCDIC 39 which is the invisible character
            //I'll change it later ;-)
            char lSeperatorAsChar = Char.ConvertFromUtf32(160)[0];

            string lSeparator = Char.ConvertFromUtf32(160);
            string lText = text.Replace(" ", lSeparator);

            if (lText.Length <= pNrOfCharactersBeforeSplit) return lText.Replace("_", lSeparator);
            ;

            int lNrOfCharsInbalanceBeforeHardSplit = pNrOfCharsInBalanceBeforeHardSplit;
            if (lNrOfCharsInbalanceBeforeHardSplit > (pNrOfCharactersBeforeSplit / 2))
                lNrOfCharsInbalanceBeforeHardSplit = (pNrOfCharactersBeforeSplit / 2);

            if (pNumberOfLines <= 1)
                return lText.Replace("_", lSeparator);
            else
            {
                if (pNumberOfLines == 2)
                {
                    int lMiddle = lText.Length / 2;
                    if (lText[lMiddle] == lSeperatorAsChar)
                        return string.Concat(lText.Substring(0, lMiddle), " ", lText.Substring(lMiddle + 1)).Replace("_", lSeparator);

                    int lOffset = 0;
                    for (lOffset = 1; lOffset < lNrOfCharsInbalanceBeforeHardSplit; lOffset++)
                    {
                        int lIndex;
                        lIndex = lMiddle - lOffset;
                        if (lText[lIndex] == lSeperatorAsChar)
                            return string.Concat(lText.Substring(0, lIndex), " ", lText.Substring(lIndex + 1)).Replace("_", lSeparator);
                        lIndex = lMiddle + lOffset;
                        if (lText[lIndex] == lSeperatorAsChar)
                            return string.Concat(lText.Substring(0, lIndex), " ", lText.Substring(lIndex + 1)).Replace("_", lSeparator);
                    }
                    //hard split;
                    if (pHardSplit)
                        return string.Concat(lText.Substring(0, lMiddle), " ", lText.Substring(lMiddle)).Replace("_", lSeparator);
                    else
                        return lText.Replace("_", lSeparator);
                }
                else
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.ToBeImplementedException("Code 39", "Display for multiple lines");
                }
            }
        }


        private static int SEED = 10000;
        public static string GenerateReadablePassword()
        {
            //No time to test so program on the safe side...
            string lChars = "aabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZZZZZZZZ";
            string lNums = "1123456789999999999";
            Random lRnd = new Random(SEED + DateTime.Now.Millisecond);
            System.Text.StringBuilder lsb = new System.Text.StringBuilder();
            for (int i = 1; i <= 8; i++)
            {
                if ((i == 3) || (i == 7))
                {
                    lsb.Append(lNums[lRnd.Next(9) + 1]);
                }
                else
                {
                    lsb.Append(lChars[lRnd.Next(52) + 1]);
                }
                SEED = SEED + lRnd.Next(1000) - 600;
                if (SEED <= 100) SEED = 10000;
            }
            return lsb.ToString();
        }

        public static string GenerateSalt()
        {
            //No time to test so program on the safe side...
            string lChars = "aabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZZZZZZZZ";
            string lNums = "1123456789999999999";
            System.Text.StringBuilder lsb = new System.Text.StringBuilder();
            for (int j = 1; j <= 5; j++)
            {
                Random lRnd = new Random(SEED + DateTime.Now.Millisecond);
                for (int i = 1; i <= 8 + j; i++)
                {
                    if ((i == 3) || (i == 7))
                    {
                        lsb.Append(lNums[lRnd.Next(9) + 1]);
                    }
                    else
                    {
                        lsb.Append(lChars[lRnd.Next(52) + 1]);
                    }
                    SEED = SEED + lRnd.Next(1000) - 600;
                    if (SEED <= 100) SEED = 10000;
                }
            }
            return lsb.ToString().Substring(1, 40);
        }
        public static string GenerateReadablePassword2()
        {
            var randomNumber = new byte[32];
            string randomstring = "";

            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                randomstring = Convert.ToBase64String(randomNumber);
            }
            var rnd = new Random();
            return randomstring.Substring(4,12) + rnd.Next(13,99).ToString();
        }
        /// <summary>
        /// Returns true if the string only contains number characters
        /// So no '.' or ','!!
        /// If string is empty, returns false
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            foreach (char lChar in value.ToCharArray())
            {
                if (!Char.IsNumber(lChar)) return false;
            }
            return true;
        }


        public static string QueryStringEncode(string base64EncodedString)
        {
            return base64EncodedString.Replace("+", "XPLUSX").Replace("&", "XANDX").Replace("=", "XEQUALX").Replace(" ", "XSPACEX");
        }
        public static string QueryStringDecode(string base64EncodedString)
        {
            return base64EncodedString.Replace("XPLUSX", "+").Replace("XANDX", "&").Replace("XEQUALX", "=").Replace("XSPACEX", " ");
        }

        public static string ExtractNumericPartFromString(string pString)
        {
            System.IO.StringWriter lsw = new System.IO.StringWriter(DXC.Technology.Utilities.CultureInfoProvider.Default);
            foreach (Char lChar in pString)
            {
                switch (lChar)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        lsw.Write(lChar);
                        break;
                }
            }
            string lResult = lsw.ToString();
            return lResult;
        }

        public static string ConvertToReadableText(string text)
        {
            System.IO.StringWriter lsw = new System.IO.StringWriter(DXC.Technology.Utilities.CultureInfoProvider.Default);
            foreach (Char lChar in text)
            {
                switch (lChar)
                {
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                        lsw.Write(' ');
                        lsw.Write(lChar);
                        break;
                    case '_':
                        ;
                        lsw.Write(' ');
                        break;
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        lsw.Write(lChar);
                        break;
                }
            }
            return lsw.ToString();
        }


        /// <summary>
        /// Split string by empty space and trim
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] SplitByEmptySpace(string str)
        {
            return Regex.Split(str, @"[\s\t]+", RegexOptions.IgnoreCase | RegexOptions.Multiline).ToList().Select(p => { return p.Trim(); }).ToArray();
        }

        /// <summary>
        /// Split and trim string extension
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitAndTrim(string str, string separator)
        {
            return str.Split(separator.ToCharArray()).ToList().Select(p => { return p.Trim(); }).ToArray();
        }

        public static string GetCsvFromValuesBetweenSquareBrackets(string text, string csvSeperator, long maxLengthOfValue)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            if (string.IsNullOrEmpty(csvSeperator))
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("csvSeperator");
            }
            string lLeftBracket = "[";
            string lRightBracket = "]";

            return GetCsvFromValuesBetweenBrackets(text, csvSeperator, lLeftBracket, lRightBracket, maxLengthOfValue);
        }

        public static string GetCsvFromValuesBetweenBrackets(string text, string csvSeperator, string leftBracket, string rightBracket, long maxLengthOfValue)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            if (string.IsNullOrEmpty(csvSeperator))
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("csvSeperator");
            }

            return DXC.Technology.Utilities.StringArrayHelper.ToCsvString(
                GetValuesBetweenBrackets(text, leftBracket, rightBracket, maxLengthOfValue)
                .ToArray(), csvSeperator);
        }

        public static List<string> GetValuesBetweenCurleyBrackets(string text, long maxLengthOfValue)
        {
            return GetValuesBetweenBrackets(text, "{", "}", maxLengthOfValue);
        }

        public static List<string> GetValuesBetweenBrackets(string text, string leftBracket, string rightBracket, long maxLengthOfValue)
        {
            List<string> lValuesBetweenBrackets = new List<string>();
            if (string.IsNullOrEmpty(text)) return lValuesBetweenBrackets;

            if (string.IsNullOrEmpty(leftBracket))
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("leftBracket");
            }

            if (string.IsNullOrEmpty(rightBracket))
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("rightBracket");
            }

            string lPositiveLookbehindForLeftBracket = string.Format(@"(?<=\{0})", leftBracket);
            string lNonGreedyMatchForTheContent = @".+?";
            string lPositiveLookaheadForRightBracket = string.Format(@"(?=\{0})", rightBracket);

            string lPattern = lPositiveLookbehindForLeftBracket + lNonGreedyMatchForTheContent + lPositiveLookaheadForRightBracket;

            Match lMatch = Regex.Match(text, lPattern);
            if (lMatch.Success)
            {
                if (lMatch.Value.Length <= maxLengthOfValue)
                {
                    lValuesBetweenBrackets.Add(lMatch.Value);
                }
                do
                {
                    lMatch = lMatch.NextMatch();
                    if (lMatch.Success)
                    {
                        if (lMatch.Value.Length <= maxLengthOfValue)
                        {
                            lValuesBetweenBrackets.Add(lMatch.Value);
                        }
                    }
                } while (lMatch.Success);
            }
            return lValuesBetweenBrackets;
        }

        public static string Ellipsis(string value, int maxChars)
        {
            if (value == null) return null;
            if (value == string.Empty) return "...";
            if (maxChars < 0) return value;
            if (maxChars == 0) return "";
            if (maxChars < 4) return "...".Substring(0, maxChars);
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - 3) + "...";
        }
        public static void ValidateStringToBeSafeHtmlString(string input)
        {

            if (string.IsNullOrEmpty(input)) return;
            string inputLowerCase = input.ToLower();
            if (inputLowerCase.IndexOf("script") < 0) return;
            if (inputLowerCase.IndexOf("javascript") >= 0)
                throw new DXC.Technology.Exceptions.NamedExceptions.InvalidDataException("Html Input with dangorous html");
            if (inputLowerCase.IndexOf("<script") >= 0)
                throw new DXC.Technology.Exceptions.NamedExceptions.InvalidDataException("Html Input with dangorous html");
        }

        public static string FormatAsQuerystring(string format, params string[] arguments)
        {
            string[] encodedArguments = arguments.Select(p => System.Web.HttpUtility.UrlEncode(p)).ToArray();
            return string.Format(format, encodedArguments);
        }
    }

}
