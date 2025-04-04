using System;
using System.Linq;
using DXC.Technology.Configuration;
using DXC.Technology.Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DXC.Technology.Configuration
{
    /// <summary>
    /// Type of encryption: Rijndael (symmetric), RSA (public/private asymmetric) or DLL (the keypair belonging to this assembly
    /// </summary>
    public enum EncryptionTypeEnum
    {
        NotSpecified = 0,
        RSA = 1,
        AES = 2
    }

    public class EncryptionOptions
    {
        #region Public Properties

        /// <summary>
        /// Array of encryptor options.
        /// </summary>
        public EncryptorOptions[] Encryptors { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the encryptor options based on the logical name.
        /// </summary>
        /// <param name="logicalName">The logical name of the encryptor.</param>
        /// <returns>The matching encryptor options or null if not found.</returns>
        public EncryptorOptions GetEncryptorOptions(string logicalName)
        {
            if (Encryptors == null) return null;
            return Encryptors.FirstOrDefault(p => p.LogicalName == logicalName);
        }

        #endregion
    }

    /// <summary>
    /// In the DXC.Technology, encryption has received a good basic support, especially regarding caching and configuring.
    /// The reason is that encryption is powerful, yet the management of keys cumbersome (hence -> configuration) and the creation
    /// of encryptors is very time-consuming (hence -> caching). The three basic types of encryption each have 1
    /// major implementation in this DXC.Technology. 
    /// - Symmetric Key encryption -> AES Algoritm (previously known as Rheindael)
    /// - Asymmetric Key Encryption -> RSA algorithm 
    /// - Hashing -> SHA1
    /// Reads the specific encryption configuration settings out of the XML in the encryptionHandling section.
    /// The Valid Encryptor definitions are:
    /// - RSA / AES: Random keys
    /// - RSA / AES: Technology hardcoded keys
    /// - RSA / AES: KeyFile
    /// - RSA / AES: KeyFile + Encrypted with one of the Technology hardcoded keys
    /// - AES: Passphrase - SaltValue - InitializationVector - KeySize
    /// - RSA: Modulus - Exponent - P - Q - DP - DQ - Inverse Q - D
    /// </summary>
    public class EncryptorOptions
    {
        #region Public Properties

        /// <summary>
        /// Logical name of the encryptor.
        /// </summary>
        public string LogicalName { get; set; }

        /// <summary>
        /// Type of encryption.
        /// </summary>
        public EncryptionTypeEnum EncryptionType { get; set; }

        /// <summary>
        /// Path to the key file.
        /// </summary>
        public string KeyFile { get; set; }

        /// <summary>
        /// Logical name of the encryptor.
        /// </summary>
        public string EncryptorLogicalName { get; set; }

        /// <summary>
        /// Modulus value for RSA encryption.
        /// </summary>
        public string Modulus { get; set; }

        /// <summary>
        /// Exponent value for RSA encryption.
        /// </summary>
        public string Exponent { get; set; }

        /// <summary>
        /// P value for RSA encryption.
        /// </summary>
        public string P { get; set; }

        /// <summary>
        /// Q value for RSA encryption.
        /// </summary>
        public string Q { get; set; }

        /// <summary>
        /// DP value for RSA encryption.
        /// </summary>
        public string DP { get; set; }

        /// <summary>
        /// DQ value for RSA encryption.
        /// </summary>
        public string DQ { get; set; }

        /// <summary>
        /// Inverse Q value for RSA encryption.
        /// </summary>
        public string InverseQ { get; set; }

        /// <summary>
        /// D value for RSA encryption.
        /// </summary>
        public string D { get; set; }

        /// <summary>
        /// Passphrase for AES encryption.
        /// </summary>
        public string PassPhrase { get; set; }

        /// <summary>
        /// Salt value for AES encryption.
        /// </summary>
        public string SaltValue { get; set; }

        /// <summary>
        /// Hash algorithm used.
        /// </summary>
        public HashAlgorithmEnum HashAlgorithm { get; set; }

        /// <summary>
        /// Initialization vector for AES encryption.
        /// </summary>
        public string InitVector { get; set; }

        /// <summary>
        /// Key size for AES encryption.
        /// </summary>
        public KeySizeEnum KeySize { get; set; }

        /// <summary>
        /// System generation type.
        /// </summary>
        public SystemGenerateEnum SystemGenerate { get; set; }

        #endregion
    }

    public class EncryptionOptionsAccessor
    {
        #region Public Properties

        /// <summary>
        /// Encryption options.
        /// </summary>
        public EncryptionOptions Options { get; } // Set only via Secret Manager

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EncryptionOptionsAccessor class.
        /// </summary>
        /// <param name="optionsAccessor">The options accessor for encryption options.</param>
        public EncryptionOptionsAccessor(IOptions<EncryptionOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        #endregion
    }

    public class EncryptionOptionsManager
    {
        #region Static Fields

        private static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        private static EncryptionOptionsManager _current = null;

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the current instance of the EncryptionOptionsManager.
        /// </summary>
        public static EncryptionOptionsManager Current
        {
            get
            {
                if (_current == null)
                    _current = new EncryptionOptionsManager();
                return _current;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the encryption options.
        /// </summary>
        /// <returns>The encryption options.</returns>
        public EncryptionOptions GetEncryptionOptions()
        {
            return HttpContext.RequestServices.GetRequiredService<EncryptionOptionsAccessor>().Options;
        }

        #endregion
    }
}