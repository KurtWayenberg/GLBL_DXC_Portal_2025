using System;
using System.Security.Cryptography;
using System.Text;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Provides utility methods for generating security tokens.
    /// </summary>
    public static class SecurityToken
    {
        #region Public Static Methods

        /// <summary>
        /// Generates a security token based on the provided salt and dateTime.
        /// </summary>
        /// <param name="salt">The salt value used for token generation.</param>
        /// <param name="dateTime">The dateTime used for token generation.</param>
        /// <returns>A security token string.</returns>
        public static string GetToken(string salt, DateTime dateTime)
        {
            DateTime universalDateTime = dateTime.ToUniversalTime();
            DateTime tokenDateTime = new DateTime(universalDateTime.Year, universalDateTime.Month, universalDateTime.Day, universalDateTime.Hour, universalDateTime.Minute, ((universalDateTime.Second / 10) * 10), 0);
            string fullToken = string.Format("{1} {0}", salt, tokenDateTime.ToString("HH:mm:ss yyyy-MM-dd"));
            string hashedToken;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] dataInBytes = Encoding.ASCII.GetBytes(fullToken);
                var hashInBytes = sha256.ComputeHash(dataInBytes);
                hashedToken = Convert.ToBase64String(hashInBytes);
            }

            byte[] hashedTokenAsBytes = Encoding.ASCII.GetBytes(hashedToken);
            using System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            for (int i = 1; i <= 6; i++)
            {
                stringWriter.Write(hashedTokenAsBytes[i * 3] % 10);
            }
            return stringWriter.ToString();
        }

        #endregion
    }
}