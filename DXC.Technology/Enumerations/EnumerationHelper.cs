using System;
using System.Collections;
using System.Collections.Generic;

namespace DXC.Technology.Enumerations
{
	/// <summary>
	/// This type implements useful logic regarding 'applicational enumerations'
	/// </summary>
	/// <remarks>
	/// Simple Utility Class to help managing enumerations (e.g. finding an enumeration based 
	/// on a XXX_ prefix,  finding an enumeration based on the full string, finding based on the 
	/// name part, finding the code of an enumeration (all before "XXX_")  etc...
	/// </remarks>
	public sealed class EnumerationHelper
	{
        // "Blank" user code. This is the value returned by Comboboxes when nothing is selected. Cannot be used for updating in the database.
        public const string BlankCode = "_BLNK";
        // "All" user code. This is the value returned by Comboboxes when the user does not want to ignore any filtering implied by Blank - code.
        public const string AllCode = "_ALL";
		
        public const string AllValidCode = "_AVAL";
		// "Unknown" user code that can be used for updating in database with a valid foreign key. This code is not shown in Comboboxes in edit mode.
		public const string UnknownCode = "-----"; 

		/// <summary>
		/// Constructor disallowing instantiation
		/// </summary>
		private EnumerationHelper()
		{
		}

		/// <summary>
		/// Return the prefix within the supplied enumeration value
		/// </summary>
		/// <remarks>
		/// Given an enumeration returns the prefix (i.e. the part before the first underscore
		/// Use this code e.g. to specify an Enumeration type in the Database
		/// </remarks>
		/// <example>
		/// EnumerationHelper.EnumToCode(S_SomeEnum) -> "S"
		/// </example>
		/// <param name="anyEnum">The enumeration element</param>
		public static string EnumToCode(System.Enum anyEnum)
		{
            if (anyEnum == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Enum");

			string lFullCode = anyEnum.ToString();
            int lPositionOfUnderscore = lFullCode.IndexOf("_", Utilities.StringComparisonProvider.Default);

			if (lPositionOfUnderscore == -1)
			{
				return lFullCode;
			}
			else
			{
				if (lPositionOfUnderscore == 0)
				{
					return "";
				}
				else
				{
					return lFullCode.Substring(0, lPositionOfUnderscore);
				}
			}
		}

		/// <summary>
		/// Return the corresponding enumeration value for the given input string
		/// </summary>
		/// <remarks>
		/// Given an enumeration code and type returns the enumeration corresponding with 
		/// the code prefix (i.e. the part before the first underscore)
		/// </remarks>
		/// <example>
		/// EnumerationHelper.CodeToEnum("S", [System.Enum-type]) -> S_SomeEnum
		/// </example>
		/// <param name="code">The code of the enumeration element to find</param>
		/// <param name="enumType">The enumeration type in which to lookup the code</param>
		public static Object CodeToEnum(string code, Type enumType)
		{
            if (enumType == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("EnumType");
            
			if (string.IsNullOrEmpty(code))
			{ 
				code = "_";		 //Initialisatie is altijd _null
			}

			code = code.Trim();

			IEnumerator lEnumerator = System.Enum.GetNames(enumType).GetEnumerator();

            //Do a first pass with _ attached
			while (lEnumerator.MoveNext())
			{
                string lCurrentEnumName = Convert.ToString(lEnumerator.Current, DXC.Technology.Utilities.StringFormatProvider.Default);
                string lCodeWithUnderscore = code.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default) + "_";
                if (lCurrentEnumName.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default).StartsWith(lCodeWithUnderscore, Utilities.StringComparisonProvider.Default))
				{
					return System.Enum.Parse(enumType, lCurrentEnumName, true);
				}
			}
            
            //Do a second pass - just as before...

            lEnumerator.Reset();
            while (lEnumerator.MoveNext())
            {
                string lCurrentEnumName = Convert.ToString(lEnumerator.Current, DXC.Technology.Utilities.StringFormatProvider.Default);

                if (lCurrentEnumName.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default).StartsWith(code.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default), Utilities.StringComparisonProvider.Default))
                {
                    return System.Enum.Parse(enumType, lCurrentEnumName, true);
                }
            }

			if (code == BlankCode) //Blank is default - if not specified it must be that it is a mandatory parameter
				throw new Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException(enumType.FullName);
			else // it is an invalid parameter
				throw new Exceptions.NamedExceptions.ParameterInvalidException(enumType.FullName, code, "");	    
		}

        public static TEnum CodeToEnum<TEnum>(string code)
            where TEnum : struct
        {
            Object lEnumObject = CodeToEnum(code, typeof(TEnum));
            return (TEnum)lEnumObject;            
        }
            

		/// <summary>
		/// Return the 'stringified' version of the supplied enumeration value
		/// </summary>
		/// <remarks>
		/// Given an enumeration returns the full name (this is actually a simple "ToString()"
		/// e.g.  EnumerationHelper.EnumToFullName(S_SomeEnum) -> "S_SomeEnum" 
		/// </remarks>
		/// <param name="anyEnum">The enumeration element</param>
		public static string EnumToFullName(System.Enum anyEnum)
		{
            if (anyEnum == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Enum");

			return anyEnum.ToString();
		}

		/// <summary>
		/// Return the name part of the supplied enumeration value
		/// </summary>
		/// <remarks>
		/// Given an enumeration returns the name part
		/// e.g.  EnumerationHelper.EnumToName(S_SomeEnum) -> "SomeEnum" 
		/// </remarks>
		/// <param name="anyEnum">The enumeration element</param>
		public static string EnumToName(System.Enum anyEnum)
		{
            if (anyEnum == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Enum");
            string lFullCode = anyEnum.ToString();
            int lPositionOfUnderscore = lFullCode.IndexOf("_", Utilities.StringComparisonProvider.Default);

			if (lPositionOfUnderscore == -1)
			{
				return lFullCode;
			}
			else
			{
				if (lPositionOfUnderscore == 0)
				{
					return lFullCode.Substring(1);
				}
				else
				{
					return lFullCode.Substring(lPositionOfUnderscore + 1);
				}
			}
		}

		/// <summary>
		/// Return the full enumeration value for supplied input string value
		/// </summary>
		/// <remarks>
		/// Given an enumeration full name and type returns the enumeration corresponding with the name
		/// e.g.  EnumerationHelper.NameToEnum("S_SomeEnum", [System.Enum-type]) -> S_SomeEnum 
		/// </remarks>
		/// <param name="pFullName">Full name of the enumeration </param>
		/// <param name="enumType">The enumeration type in which to lookup the code</param>
		public static Object FullNameToEnum(string pFullName, Type enumType)
		{
            if (enumType == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("EnumType");
            if (string.IsNullOrEmpty(pFullName)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Full Name");

            IEnumerator lEnumerator = System.Enum.GetNames(enumType).GetEnumerator();

			while (lEnumerator.MoveNext())
			{
                string lCurrentEnumName = Convert.ToString(lEnumerator.Current, DXC.Technology.Utilities.StringFormatProvider.Default);

                if (lCurrentEnumName.Equals(pFullName, StringComparison.CurrentCulture))
				{
					return System.Enum.Parse(enumType, lCurrentEnumName, true );
				}
			}
			throw new Exceptions.NamedExceptions.ParameterInvalidException(enumType.ToString(), pFullName,"Enumeration full name invalid for enumeration type");	    
		}

        public static TEnum FullNameToEnum<TEnum>(string pFullName)
            where TEnum : struct
        {
            Object lEnumObject = FullNameToEnum(pFullName, typeof(TEnum));
            return (TEnum)lEnumObject;
        }
		/// <summary>
		/// Return the enumeration value corresponding to the supplied name part within the supplied enumeration type
		/// </summary>
		/// <remarks>
		/// Given an enumeration name and type returns the enumeration corresponding with the name
		/// e.g.  EnumerationHelper.NameToEnum("SomeEnum", [System.Enum-type]) -> S_SomeEnum 
		/// </remarks>
		/// <param name="name">The name of the enumeration element to find</param>
		/// <param name="enumType">The enumeration type in which to lookup the code</param>
		public static Object NameToEnum(string name, Type enumType)
		{
            if (enumType == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("EnumType");
            if (string.IsNullOrEmpty(name)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Enum");

            IEnumerator lEnumerator = System.Enum.GetNames(enumType).GetEnumerator();

			while (lEnumerator.MoveNext())
			{
                string lCurrentEnumName = Convert.ToString(lEnumerator.Current, DXC.Technology.Utilities.StringFormatProvider.Default);

                if (lCurrentEnumName.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default).EndsWith(name.ToUpper(DXC.Technology.Utilities.CultureInfoProvider.Default), Utilities.StringComparisonProvider.Default))
				{
					return System.Enum.Parse(enumType, lCurrentEnumName, true );
				}
			}

			//We did not find it. See if it was positional e.g. 0-n
			int lIndex = 0;
			try
			{
                lIndex = Convert.ToInt16(name, DXC.Technology.Utilities.IntFormatProvider.Default);
				return System.Enum.Parse(enumType, System.Enum.GetNames(enumType)[lIndex], true);
			}
            //CA1031 fix - REFINED GENERAL EXCEPTION HANDLING - might throw exceptions unexpectedly now                
            catch (System.InvalidCastException)
            {
                DXC.Technology.Exceptions.ExceptionHelper.Publish(new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("Enum", name, enumType.ToString()));
            }
            catch (FormatException aex)
            {
                DXC.Technology.Exceptions.ExceptionHelper.Publish(aex);
            }

			throw new Exceptions.NamedExceptions.ParameterInvalidException(enumType.ToString(), name, "Enumeration name invalid for enumeration type");	    
		}

        public static TEnum NameToEnum<TEnum>(string name)
            where TEnum : struct
        {
            Object lEnumObject = NameToEnum(name, typeof(TEnum));
            return (TEnum)lEnumObject;
        }
		/// <summary>
		/// Return the hashcode of the supplied enumeration value
		/// </summary>
		/// <remarks>
		/// Given an enumeration returns the internal value of the enumeration (This is 
		/// a simple "GetHashCode" call) e.g.  EnumerationHelper.EnumToInteger(S_SomeEnum) -> 16
		/// </remarks>
		/// <param name="anyEnum">The enumeration element</param>
		public static int EnumToHash(System.Enum anyEnum)
        {
            if (anyEnum == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Enum");

			return anyEnum.GetHashCode();
		}

		/// <summary>
		/// Return the enumeration value with supplied hashcode within the supplied enumeration type
		/// </summary>
		/// <remarks>
		/// Given an enumeration hashcode and type returns the enumeration corresponding with the hash
		/// e.g.  EnumerationHelper.CodeToEnum(16, [System.Enum-type]) -> S_SomeEnum 
		/// </remarks>
		/// <param name="pHash">The code of the enumeration element to find</param>
		/// <param name="enumType">The enumeration type in which to lookup the code</param>
		public static Object HashToEnum(int pHash, Type enumType)
		{
            if (enumType == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("EnumType");
            if (System.Enum.IsDefined(enumType, pHash))
			{
				return System.Enum.ToObject(enumType, pHash);
			}
			else
			{
				throw new DXC.Technology.Exceptions.NamedExceptions.BusinessLogicException(string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "Unknown Hash: {0} for Enumeration Type: {1}", pHash.ToString(DXC.Technology.Utilities.StringFormatProvider.Default), enumType.FullName ));
			}
		}

        public static TEnum HashToEnum<TEnum>(int pHash)
            where TEnum : struct
        {
            Object lEnumObject = HashToEnum(pHash, typeof(TEnum));
            return (TEnum)lEnumObject;
        }

		/// <summary>
		/// Return the supplied enumeration type as an EnumerationDataSet
		/// </summary>
		/// <remarks>
		/// Given an enumeration type returns a datatable of name-code-hash pairs
		/// This is useful to bind comboboxes with. Bind DisplayMember on "Name" and 
		/// ValueMember on either "Code" or "Hash" in order to have the selectedValue return 
		/// the code / hash rather than the name
		/// <seealso cref="DXC.Technology.Enumerations.EnumerationDataSet"/>
		/// </remarks>
		/// <param name="enumType">The enumeration type</param>
        public static List<Enumeration> GetEnumerationTable(Type enumType)
        {
            List<Enumeration> et = new List<Enumeration>();

            int NumberOfItems;
            NumberOfItems = System.Enum.GetNames(enumType).GetLength(0);
            Translations.ITranslationProvider lTranslationManager = DXC.Technology.Translations.TranslationManager.Current;
            for (int i = 0; i < NumberOfItems; i++)
            {
                et.Add(new Enumeration(System.Enum.GetNames(enumType)[i],
                                      StringToName(System.Enum.GetNames(enumType)[i]),
                                      lTranslationManager.Localize(StringToCode(System.Enum.GetNames(enumType)[i]) ),
                                      Convert.ToInt16(System.Enum.GetValues(enumType).GetValue(i), DXC.Technology.Utilities.IntFormatProvider.Default)));
            }
            return et;
        }

        /// <summary>
        /// Return the supplied enumeration type as an EnumerationDataSet
        /// </summary>
        /// <remarks>
        /// Given an enumeration type returns a datatable of name-code-hash pairs
        /// This is useful to bind comboboxes with. Bind DisplayMember on "Name" and 
        /// ValueMember on either "Code" or "Hash" in order to have the selectedValue return 
        /// the code / hash rather than the name
        /// <seealso cref="DXC.Technology.Enumerations.EnumerationDataSet"/>
        /// </remarks>
        /// <param name="enumType">The enumeration type</param>
        public static List<EnumerationCodeDescription> GetEnumerationAsList(Type enumType)
        {
            List<EnumerationCodeDescription> lEnumerationCodeDescriptions = new List<EnumerationCodeDescription>();

            int NumberOfItems;
            NumberOfItems = System.Enum.GetNames(enumType).GetLength(0);
			Translations.ITranslationProvider lTranslationManager = DXC.Technology.Translations.TranslationManager.Current;
			for (int i = 0; i < NumberOfItems; i++)
            {
                lEnumerationCodeDescriptions.Add(
                    new EnumerationCodeDescription(
                        lTranslationManager.Localize(System.Enum.GetNames(enumType)[i]),
                        StringToCode(System.Enum.GetNames(enumType)[i]),
                        lTranslationManager.Localize(StringToName(System.Enum.GetNames(enumType)[i])),
                        Convert.ToInt16(System.Enum.GetValues(enumType).GetValue(i), DXC.Technology.Utilities.IntFormatProvider.Default)));
            }
            return lEnumerationCodeDescriptions;
        }


        /// <summary>
        /// Return the supplied enumeration type as an EnumerationDataSet
        /// </summary>
        /// <remarks>
        /// Given an enumeration type returns a datatable of name-code-hash pairs
        /// This is useful to bind comboboxes with. Bind DisplayMember on "Name" and 
        /// ValueMember on either "Code" or "Hash" in order to have the selectedValue return 
        /// the code / hash rather than the name
        /// <seealso cref="DXC.Technology.Enumerations.EnumerationDataSet"/>
        /// </remarks>
        /// <param name="enumType">The enumeration type</param>
        public static List<EnumerationCodeDescription> GetEnumerationAsList(params Enum[] pEnums)
        {
            List<EnumerationCodeDescription> lEnumerationCodeDescriptions = new List<EnumerationCodeDescription>();

            int NumberOfItems;
            NumberOfItems = pEnums.GetLength(0);
			Translations.ITranslationProvider lTranslationManager = DXC.Technology.Translations.TranslationManager.Current;
			for (int i = 0; i < NumberOfItems; i++)
            {
                string lEnumDesc = Convert.ToString(pEnums[i], DXC.Technology.Utilities.StringFormatProvider.Default);
                lEnumerationCodeDescriptions.Add(
                   new EnumerationCodeDescription(
                       lTranslationManager.Localize(lEnumDesc),
                       lTranslationManager.Localize(StringToName(lEnumDesc)),
                       StringToCode(lEnumDesc),
                       Convert.ToInt16(pEnums[i], DXC.Technology.Utilities.IntFormatProvider.Default)));

            }
            return lEnumerationCodeDescriptions;
        }


        /// <summary>
        /// Return the supplied enumeration type as an EnumerationDataSet
        /// </summary>
        /// <remarks>
        /// Given an enumeration type returns a Comma Separated List
        /// This is useful to use in communicating the list of possible values of an enumeration to the user (e.g. in exceptions)
        /// <seealso cref="DXC.Technology.Enumerations.EnumerationDataSet"/>
        /// </remarks>
        /// <param name="enumType">The enumeration type</param>
        public static string GetAsCsvString(Type enumType)
        {
            System.IO.StringWriter lsw = new System.IO.StringWriter(DXC.Technology.Utilities.StringFormatProvider.Default);
            int NumberOfItems;
            NumberOfItems = System.Enum.GetNames(enumType).GetLength(0);
            Translations.ITranslationProvider lTranslationManager = DXC.Technology.Translations.TranslationManager.Current;
            for (int i = 0; i < NumberOfItems; i++)
            {
                lsw.Write(lTranslationManager.Localize(System.Enum.GetNames(enumType)[i].ToString()));
                if (i < NumberOfItems - 1)
                    lsw.Write(",");
            }
            return lsw.ToString();
        }
		/// <summary>
		/// Given an enumeration type and a datatable, fills the table with the enumeration's name information
		/// </summary>
		/// <param name="tableToFill">The enumeration type</param>
		/// <param name="enumType">The enumeration type</param>
		/// <param name="nameFieldName">The fieldName of the column in which to put the Name-information. If left blank or if the name is not found does not fill in any field</param>
		public static void FillEnumerationTable(System.Data.DataTable tableToFill, Type enumType, String nameFieldName)
		{
			FillEnumerationTable(tableToFill, enumType,nameFieldName,"", "");
		}

		/// <summary>
		/// Given an enumeration type and a datatable, fills the table with the enumeration's name-code-hash pairs
		/// </summary>
		/// <param name="tableToFill">The enumeration type</param>
		/// <param name="enumType">The enumeration type</param>
		/// <param name="nameFieldName">The fieldName of the column in which to put the Name-information. If left blank or if the name is not found does not fill in any field</param>
		/// <param name="pHashFieldName">The fieldName of the column in which to put the Hash-information. If left blank or if the name is not found does not fill in any field</param>
		/// <param name="pCodeFieldName">The fieldName of the column in which to put the Code-information. If left blank or if the name is not found does not fill in any field</param>
		public static void FillEnumerationTable(System.Data.DataTable tableToFill, Type enumType, String nameFieldName,  String pHashFieldName, String pCodeFieldName)
		{
            if (tableToFill == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Table To Fill");

			int NumberOfItems;
			NumberOfItems = System.Enum.GetNames(enumType).GetLength(0);

			for (int i = 0; i < NumberOfItems; i++)
			{
				System.Data.DataRow dr;
				dr = tableToFill.NewRow(); 
				if (!string.IsNullOrEmpty(nameFieldName) & (dr.Table.Columns[nameFieldName] != null )) 
				{ 
					dr[nameFieldName] = System.Enum.GetNames(enumType)[i]; 
				}
				if (!string.IsNullOrEmpty(pHashFieldName) & (dr.Table.Columns[pHashFieldName] != null )) 
				{
                    dr[pHashFieldName] = Convert.ToInt16(System.Enum.GetValues(enumType).GetValue(i), DXC.Technology.Utilities.IntFormatProvider.Default); 
				}
				if (!string.IsNullOrEmpty(pCodeFieldName) & (dr.Table.Columns[pCodeFieldName] != null )) 
				{ 
					dr[pCodeFieldName] = StringToCode(System.Enum.GetNames(enumType)[i]) ; 
				}
				tableToFill.Rows.Add(dr);
			}
			return; 
		}

		/// <summary>
		/// Test if the supplied enumeration value is included within the supplied (domain) enumeration type
		/// </summary>
		/// <remarks>
		/// Example implementation on how to test whether an enumeration includes one of its enumeration flags
		/// Example: 
		/// public enum SomeEnum {
		///    CanEdit = 4,
		///    CanInsert = 8,
		///    CanDelete = 16
		///  }
		/// SomeEnum SomeEnumCollection = CanEdit | CanInsert;
		/// IncludesFlag( SomeEnumCollection, CanEdit) -> true
		/// IncludesFlag( SomeEnumCollection, CanDelete) -> false
		/// </remarks>
		/// <param name="enumCollection"></param>
		/// <param name="anyEnum"></param>
		/// <returns>True : Supplied enumeration value is part of domain enumeration</returns>
        public static bool IncludesFlag(System.Enum enumCollection, System.Enum anyEnum)
		{
            if (anyEnum == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Enum");
            if (enumCollection == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Enum Collection");
            return ((enumCollection.GetHashCode() & anyEnum.GetHashCode()) == anyEnum.GetHashCode());
		}

		/// <summary>
		/// Return the code part of the supplied input string
		/// </summary>
		/// <remarks>
		/// Given an enumeration returns the prefix (i.e. the part before the first underscore
		/// Use this code e.g. to specify an Enumeration type in the Database
		/// e.g.  EnumerationHelper.StringToCode("S_SomeEnum") -> "S" 
		/// </remarks>
		/// <param name="pFullCode">The input string</param>
		private static string StringToCode(String pFullCode)
		{
            int lPositionOfUnderscore = pFullCode.IndexOf("_", Utilities.StringComparisonProvider.Default);

			if (lPositionOfUnderscore == -1)
			{
				return pFullCode;
			}
			else
			{
				if (lPositionOfUnderscore == 0)
				{
					return "";
				}
				else
				{
					return pFullCode.Substring(0, lPositionOfUnderscore);
				}
			}
		}

		/// <summary>
		/// Return the name part of the supplied input string
		/// </summary>
		/// <remarks>
		/// Given an enumeration returns the prefix (i.e. the part before the first underscore
		/// Use this code e.g. to specify an Enumeration type in the Database
		/// e.g.  EnumerationHelper.EnumToCode("S_SomeEnum") -> "SomeEnum" 
		/// </remarks>
		/// <param name="pFullCode">The enumeration element</param>
		private static string StringToName(String pFullCode)
		{
            int lPositionOfUnderscore = pFullCode.IndexOf("_", Utilities.StringComparisonProvider.Default);

			if (lPositionOfUnderscore == -1)
			{
				return pFullCode;
			}
			else
			{
				if (lPositionOfUnderscore == 0)
				{
					return pFullCode.Substring(1);
				}
				else
				{
					return pFullCode.Substring(lPositionOfUnderscore + 1);
				}
			}
		}
	}
}
