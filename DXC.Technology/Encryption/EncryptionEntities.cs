using System;
using System.Runtime.Serialization;

namespace DXC.Technology.Encryption
{
    /// <summary>
    /// Represents encryption-related information for a specific table and field.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = false)]
    public class Encryption
    {
        #region Public Properties

        /// <summary>
        /// The name of the table.
        /// </summary>
        [DataMember]
        public string TableName { get; set; }

        /// <summary>
        /// The name of the field.
        /// </summary>
        [DataMember]
        public string FieldName { get; set; }

        /// <summary>
        /// The encryptor used for encryption.
        /// </summary>
        [DataMember]
        public string Encryptor { get; set; }

        /// <summary>
        /// The signer used for signing.
        /// </summary>
        [DataMember]
        public string Signer { get; set; }

        #endregion
    }
}