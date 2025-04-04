using System;
using System.Runtime.Serialization;

namespace DXC.Technology.Enumerations
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class Enumeration
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the full name of the enumeration.
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the name of the enumeration.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code of the enumeration.
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the hash value of the enumeration.
        /// </summary>
        [DataMember]
        public int Hash { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        /// <param name="fullName">The full name of the enumeration.</param>
        /// <param name="name">The name of the enumeration.</param>
        /// <param name="code">The code of the enumeration.</param>
        /// <param name="hash">The hash value of the enumeration.</param>
        public Enumeration(string fullName, string name, string code, int hash)
        {
            FullName = fullName;
            Name = name;
            Code = code;
            Hash = hash;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of the <see cref="Enumeration"/> class that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="Enumeration"/> object that is a copy of this instance.</returns>
        public Enumeration Clone()
        {
            return new Enumeration(FullName, Name, Code, Hash);
        }

        #endregion
    }
}