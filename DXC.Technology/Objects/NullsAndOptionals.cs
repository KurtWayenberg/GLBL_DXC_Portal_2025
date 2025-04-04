using System;

namespace DXC.Technology.Objects
{


	/// <summary>
	/// Use this class to obtain a consistent set of default constants
	/// </summary>
	public sealed class DefaultValues : Object
	{
		public static bool IsDefault(int? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(int value)
		{
			return value.Equals(DefaultInteger);
		}
		public static bool IsDefault(long? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(long value)
		{
			return value.Equals(DefaultLong);
		}
		public static bool IsDefault(decimal? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);

		}
		public static bool IsDefault(decimal value)
		{
			return value.Equals(DefaultDecimal);
		}
		public static bool IsDefault(double? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(double value)
		{
			return value.Equals(DefaultDouble);
		}
		public static bool IsDefault(Single? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(Single value)
		{
			return value.Equals(DefaultSingle);
		}
		public static bool IsDefault(short? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(short value)
		{
			return value.Equals(DefaultShort);
		}
		public static bool IsDefault(bool? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(bool value)
		{
			return value.Equals(DefaultBoolean);
		}
		public static bool IsDefault(byte? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(byte value)
		{
			return value.Equals(DefaultByte);
		}
		public static bool IsDefault(DateTime? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(DateTime value)
		{
			return value.Equals(DefaultDateTime);
		}
		public static bool IsDefault(TimeSpan? value)
		{
			return (!value.HasValue) || IsDefault(value.Value);
		}
		public static bool IsDefault(TimeSpan value)
		{
			return value.Equals(DefaultTimespan);
		}
		public static bool IsDefault(string value)
		{
			return string.IsNullOrEmpty(value);
		}
		public static bool IsDefault(byte[] value)
		{
			return value == null;
		}


		public static int? DefaultFor(int? value)
		{
			return new int?();
		}
		public static int DefaultFor(int value)
		{
			return 0;
		}
		public static long? DefaultFor(long? value)
		{
			return new long?();
		}
		public static long DefaultFor(long value)
		{
			return 0;
		}
		public static decimal? DefaultFor(decimal? value)
		{
			return new decimal?();

		}
		public static decimal DefaultFor(decimal value)
		{
			return 0;
		}
		public static double? DefaultFor(double? value)
		{
			return new double?();
		}
		public static double DefaultFor(double value)
		{
			return 0;
		}
		public static Single? DefaultFor(Single? value)
		{
			return new Single?();
		}
		public static Single DefaultFor(Single value)
		{
			return 0;
		}
		public static short? DefaultFor(short? value)
		{
			return new short?();
		}
		public static short DefaultFor(short value)
		{
			return 0;
		}
		public static bool? DefaultFor(bool? value)
		{
			return new bool?();
		}
		public static bool DefaultFor(bool value)
		{
			return false;
		}
		public static byte? DefaultFor(byte? value)
		{
			return new byte?();
		}
		public static byte DefaultFor(byte value)
		{
			return 0;
		}
		public static DateTime? DefaultFor(DateTime? value)
		{
			return new DateTime?();
		}
		public static DateTime DefaultFor(DateTime value)
		{
			return default(DateTime);
		}
		public static TimeSpan? DefaultFor(TimeSpan? value)
		{
			return new TimeSpan?();
		}
		public static TimeSpan DefaultFor(TimeSpan value)
		{
			return default(TimeSpan);
		}
		public static string DefaultFor(string value)
		{
			return null;
		}
		public static byte[] DefaultFor(byte[] value)
		{
			return null;
		}



		/// <summary>
		/// Hidden constructor preventing instantiation
		/// </summary>
		private DefaultValues() : base()
		{
		}

		/// <summary>
		/// Return a null boolean (=false)
		/// </summary>
		public static bool DefaultBoolean
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Return a null integer (=Short.MinValue)
		/// </summary>
		public static int DefaultInteger
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Return a null double (=Short.MinValue)
		/// </summary>
		public static double DefaultDouble
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Return a null short (=Short.MinValue)
		/// </summary>
		public static short DefaultShort
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Return a null long (=Short.MinValue)
		/// </summary>
		public static long DefaultLong
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Return a null single (=Short.MinValue)
		/// </summary>
		public static float DefaultSingle
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Return a null Decimal (=Short.MinValue)
		/// </summary>
		public static decimal DefaultDecimal
		{
			get
			{
				return 0;
			}
		}


		/// <summary>
		/// Return a null DateTime (=DateTime.MinValue)
		/// </summary>
		public static DateTime DefaultDateTime
		{
			get
			{
				return new DateTime();
			}
		}


		/// <summary>
		/// Return a null TimeSpan (=Timespan.MinValue)
		/// </summary>
		public static TimeSpan DefaultTimespan
		{
			get
			{
				return new TimeSpan();
			}
		}

		/// <summary>
		/// Return a null string (=String.Empty = "")
		/// </summary>
		public static string DefaultString
		{
			get
			{
				return string.Empty;
			}
		}

		public static string DefaultPrimaryKeyString
		{
			get
			{
				return "-";
			}
		}

		public static Byte DefaultByte
		{
			get
			{
				return Byte.MinValue;
			}
		}
	}

	/// <summary>
	/// Use this class to obtain a consistent set of default constants
	/// </summary>
	public sealed class NullValues : Object
	{
		public static bool IsNull(int? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(int value)
		{
			return value.Equals(NullInteger);
		}
		public static bool IsNull(long? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(long value)
		{
			return value.Equals(NullLong);
		}
		public static bool IsNull(decimal? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(decimal value)
		{
			return value.Equals(NullDecimal);
		}
		public static bool IsNull(double? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(double value)
		{
			return value.Equals(NullDouble);
		}
		public static bool IsNull(Single? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(Single value)
		{
			return value.Equals(NullSingle);
		}
		public static bool IsNull(short? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(short value)
		{
			return value.Equals(NullShort);
		}
		public static bool IsNull(bool? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(bool value)
		{
			return value.Equals(NullBoolean);
		}
		public static bool IsNull(byte? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(byte value)
		{
			return value.Equals(NullByte);
		}
		public static bool IsNull(DateTime? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(DateTime value)
		{
			return value.Equals(NullDateTime);
		}
		public static bool IsNull(TimeSpan? value)
		{
			return (!value.HasValue) || IsNull(value.Value);
		}
		public static bool IsNull(TimeSpan value)
		{
			return value.Equals(NullTimespan);
		}
		public static bool IsNull(string value)
		{
			return string.IsNullOrEmpty(value);
		}
		public static bool IsNull(byte[] value)
		{
			return value == null;
		}


		/// <summary>
		/// Hidden constructor preventing instantiation
		/// </summary>
		private NullValues() : base()
		{
		}

		/// <summary>
		/// Return a null boolean (=false)
		/// </summary>
		public static bool NullBoolean
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Return a null integer (=Short.MinValue)
		/// </summary>
		public static int NullInteger
		{
			get
			{
				return Convert.ToInt32(short.MinValue, DXC.Technology.Utilities.IntFormatProvider.Default);
			}
		}

		/// <summary>
		/// Return a null double (=Short.MinValue)
		/// </summary>
		public static double NullDouble
		{
			get
			{
				return Convert.ToDouble(short.MinValue, DXC.Technology.Utilities.DoubleFormatProvider.Default);
			}
		}

		/// <summary>
		/// Return a null short (=Short.MinValue)
		/// </summary>
		public static short NullShort
		{
			get
			{
				return short.MinValue;
			}
		}

		/// <summary>
		/// Return a null long (=Short.MinValue)
		/// </summary>
		public static long NullLong
		{
			get
			{
				return Convert.ToInt64(short.MinValue, DXC.Technology.Utilities.IntFormatProvider.Default);
			}
		}

		/// <summary>
		/// Return a null single (=Short.MinValue)
		/// </summary>
		public static float NullSingle
		{
			get
			{
				return Convert.ToSingle(short.MinValue, DXC.Technology.Utilities.SingleFormatProvider.Default);
			}
		}

		/// <summary>
		/// Return a null Decimal (=Short.MinValue)
		/// </summary>
		public static decimal NullDecimal
		{
			get
			{
				return Convert.ToDecimal(short.MinValue, DXC.Technology.Utilities.DecimalFormatProvider.Default);
			}
		}

		
		/// <summary>
		/// Return a null DateTime (=DateTime.MinValue)
		/// </summary>
		public static DateTime NullDateTime
		{
			get
			{
				return DXC.Technology.Objects.NullValues.NullDateTime; 
			}
		}


		/// <summary>
		/// Return a null TimeSpan (=Timespan.MinValue)
		/// </summary>
		public static TimeSpan NullTimespan
		{
			get
			{
				return TimeSpan.MinValue;
			}
		}

		/// <summary>
		/// Return a null string (=String.Empty = "")
		/// </summary>
		public static string NullString
		{
			get
			{
				return string.Empty;
			}
		}

		public static string NullPrimaryKeyString
		{
			get
			{
				return "-";
			}
		}

		public static Byte NullByte
		{
			get
			{
				return Byte.MinValue;
			}
		}
	}


	/// <summary>
	/// Use this class to get NULL-behavior for strings in Objects
	/// </summary>
	public class OptionalString : Object
	{
		private bool iNull;
		private string iValue = string.Empty;

		/// <summary>
		/// Default Constructor 
		/// </summary>
		/// <remarks>
		/// The value will receive the .NET string.Emtpy value. The null indicator
		/// will be set to true
		/// </remarks>
		public OptionalString()
		{
			iNull = true;
		}

		/// <summary>
		/// Cosntructor supplying the string value to be represented
		/// </summary>
		/// <param name="value">string to encapsulate</param>
		public OptionalString(string value)
		{
			iValue = value;
		}

		public static OptionalString Null
		{
			get
			{
				return new OptionalString();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = string.Empty;
				iNull = value;
			}
		}

		public string Value
		{
			get
			{
				return iValue;
			}
			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public virtual bool Equals(OptionalString value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}

	/// <summary>
	/// Use this class to get NULL-behavior for integers in Objects
	/// </summary>
	public class OptionalInteger : Object
	{
		private bool iNull;
		private int iValue;

		public OptionalInteger()
		{
			iNull = true;
		}
		public OptionalInteger(int value)
		{
			iValue = value;
		}

		public static OptionalInteger Null
		{
			get
			{
				return new OptionalInteger();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = 0;
				iNull = value;
			}
		}

		public int Value
		{
			get
			{
				return iValue;
			}

			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public bool Equals(OptionalInteger value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}

	/// <summary>
	/// Use this class to get NULL-behavior for doubles in Objects
	/// </summary>
	public class OptionalDouble : Object
	{
		private bool iNull;
		private double iValue;

		public OptionalDouble()
		{
			iNull = true;
		}
		public OptionalDouble(double value)
		{
			iValue = value;
		}

		public static OptionalDouble Null
		{
			get
			{
				return new OptionalDouble();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = 0;
				iNull = value;
			}
		}

		public double Value
		{
			get
			{
				return iValue;
			}

			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public bool Equals(OptionalDouble value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");

			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}

	/// <summary>
	/// Use this class to get NULL-behavior for DateTimes in Objects
	/// </summary>
	public class OptionalDateTime : Object
	{
		private bool iNull;
		private DateTime iValue = DXC.Technology.Objects.NullValues.NullDateTime;

		public OptionalDateTime()
		{
			iNull = true;
		}
		public OptionalDateTime(DateTime value)
		{
			iValue = value;
		}

		public static OptionalDateTime Null
		{
			get
			{
				return new OptionalDateTime();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = DXC.Technology.Objects.NullValues.NullDateTime;
				iNull = value;
			}
		}

		public DateTime Value
		{
			get
			{
				return iValue;
			}

			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public bool Equals(OptionalDateTime value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}

	/// <summary>
	/// Use this class to get NULL-behavior for bools in Objects
	/// </summary>
	public class OptionalBoolean : Object
	{
		private bool iNull;
		private bool iValue;

		public OptionalBoolean()
		{
			iNull = true;
		}
		public OptionalBoolean(bool value)
		{
			iValue = value;
		}

		public static OptionalBoolean Null
		{
			get
			{
				return new OptionalBoolean();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = false;
				iNull = value;
			}
		}

		public bool Value
		{
			get
			{
				return iValue;
			}

			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public bool Equals(OptionalBoolean value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}

	/// <summary>
	/// Use this class to get NULL-behavior for timespans in Objects
	/// </summary>
	public class OptionalTimespan : Object
	{
		private bool iNull;
		private TimeSpan iValue = TimeSpan.MinValue;

		public OptionalTimespan()
		{
			iNull = true;
		}
		public OptionalTimespan(TimeSpan value)
		{
			iValue = value;
		}

		public static OptionalTimespan Null
		{
			get
			{
				return new OptionalTimespan();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = TimeSpan.MinValue;
				iNull = value;
			}
		}

		public TimeSpan Value
		{
			get
			{
				return iValue;
			}

			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public bool Equals(OptionalTimespan value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}
	/// <summary>
	/// Use this class to get NULL-behavior for shorts in Objects
	/// </summary>
	public class OptionalShort : Object
	{
		private bool iNull;
		private short iValue;

		public OptionalShort()
		{
			iNull = true;
		}
		public OptionalShort(short value)
		{
			iValue = value;
		}

		public static OptionalShort Null
		{
			get
			{
				return new OptionalShort();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = 0;
				iNull = value;
			}
		}

		public short Value
		{
			get
			{
				return iValue;
			}

			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public bool Equals(OptionalShort value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}
	/// <summary>
	/// Use this class to get NULL-behavior for singles in Objects
	/// </summary>
	public class OptionalSingle : Object
	{
		private bool iNull;
		private short iValue;

		public OptionalSingle()
		{
			iNull = true;
		}
		public OptionalSingle(short value)
		{
			iValue = value;
		}

		public static OptionalSingle Null
		{
			get
			{
				return new OptionalSingle();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = 0;
				iNull = value;
			}
		}

		public short Value
		{
			get
			{
				return iValue;
			}

			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public bool Equals(OptionalSingle value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}
	/// <summary>
	/// Use this class to get NULL-behavior for decimals in Objects
	/// </summary>
	public class OptionalDecimal : Object
	{
		private bool iNull;
		private decimal iValue = 0;

		public OptionalDecimal()
		{
			iNull = true;
		}
		public OptionalDecimal(decimal value)
		{
			iValue = value;
		}

		public static OptionalDecimal Null
		{
			get
			{
				return new OptionalDecimal();
			}
		}

		public bool IsNull
		{
			get
			{
				return iNull;
			}
			set
			{
				iValue = 0;
				iNull = value;
			}
		}

		public decimal Value
		{
			get
			{
				return iValue;
			}

			set
			{
				iValue = value;
				iNull = false;
			}
		}

		public bool Equals(OptionalDecimal value)
		{
			if (value == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Value");
			bool lLeft = this.IsNull == true ? value.IsNull == true ? true : false : false;
			bool lRight = this.Value == value.Value ? true : false;
			return (lLeft | lRight);
		}
	}
}
