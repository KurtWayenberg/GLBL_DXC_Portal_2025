using System;
using System.Text;
using DXC.Technology.Encryption;

namespace DXC.Technology.Security
{
    /// <summary>
    /// Provides helper methods for handling sensitive information securely.
    /// </summary>
    public static class SensitiveInformationHelper
    {
        #region Static Fields

        /// <summary>
        /// A constant salt value used for hashing sensitive data.
        /// </summary>
        private const string salt = "VGh5U2hhbGxOb3RQYXNzISE=";

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Obfuscates sensitive data by hashing it with a salt.
        /// </summary>
        /// <param name="sensitiveData">The sensitive data to obfuscate.</param>
        /// <returns>The obfuscated hash.</returns>
        public static string Obfuscate(string sensitiveData)
        {
            string hashBase = salt + sensitiveData;
            var hashEncryptionHelper = new HashEncryptionHelper(HashAlgorithmEnum.MD5, Encoding.UTF8);
            string hash = hashEncryptionHelper.CreateHashAsHex(hashBase);
            return hash;
        }

        /// <summary>
        /// Verifies if the obfuscated hash matches the sensitive data.
        /// </summary>
        /// <param name="sensitiveData">The sensitive data to verify.</param>
        /// <param name="obfuscatedHash">The obfuscated hash to compare.</param>
        /// <returns>True if the hash matches, otherwise false.</returns>
        public static bool VerifyObfuscated(string sensitiveData, string obfuscatedHash)
        {
            string hashBase = salt + sensitiveData;
            var hashEncryptionHelper = new HashEncryptionHelper(HashAlgorithmEnum.MD5, Encoding.UTF8);
            string hash = hashEncryptionHelper.CreateHashAsHex(hashBase);
            return obfuscatedHash == hash;
        }

        #endregion
    }
}