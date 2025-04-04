using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXC.Technology.Enumerations
{
    /// <summary>
    /// Represents a Boolean enumeration with True and False values.
    /// </summary>
    public enum BooleanEnum // codes of the BooleanTrueFalse code group
    {
        /// <summary>
        /// Represents the False value.
        /// </summary>
        False = 0,

        /// <summary>
        /// Represents the True value.
        /// </summary>
        True = 1,
    }

    [Serializable]
    public class EnumerationCodeDescription
    {
        #region Public Properties

        /// <summary>
        /// Full name of the enumeration.
        /// </summary>
        public string EnumerationFullName { get; set; }

        /// <summary>
        /// Code of the enumeration.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Description of the enumeration.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Hash of the enumeration.
        /// </summary>
        public int EnumerationHash { get; set; }

        /// <summary>
        /// Indicates whether the enumeration is selected.
        /// </summary>
        public bool IsSelected { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationCodeDescription"/> class using an enumeration.
        /// </summary>
        /// <param name="anyEnum">The enumeration to initialize the instance with.</param>
        public EnumerationCodeDescription(Enum anyEnum)
        {
            EnumerationFullName = EnumerationHelper.EnumToFullName(anyEnum);
            Code = EnumerationHelper.EnumToCode(anyEnum);
            Description = EnumerationHelper.EnumToName(anyEnum);
            EnumerationHash = EnumerationHelper.EnumToHash(anyEnum);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationCodeDescription"/> class using specific values.
        /// </summary>
        /// <param name="fullName">The full name of the enumeration.</param>
        /// <param name="code">The code of the enumeration.</param>
        /// <param name="description">The description of the enumeration.</param>
        /// <param name="hash">The hash of the enumeration.</param>
        public EnumerationCodeDescription(string fullName, string code, string description, int hash)
        {
            EnumerationFullName = fullName;
            Code = code;
            Description = description;
            EnumerationHash = hash;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationCodeDescription"/> class.
        /// </summary>
        public EnumerationCodeDescription()
        {
        }

        #endregion
    }
}