using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Security
{
    public static class JwkHelper
    {
        #region Public Static Methods

        /// <summary>
        /// Reads the public JSON Web Key from a file and deserializes it into a JwkPublicDefinition object.
        /// </summary>
        public static JwkPublicDefinition ReadPublicJsonWebKey()
        {
            string jwsPublicKey = File.ReadAllText(@"C:\Projects\GenerateJsonWebKey\Notifications\Notifications.jwk");
            using var memoryStream = new MemoryStream(Encoding.Default.GetBytes(jwsPublicKey));
            // Deserialization from JSON
            var deserializer = new DataContractJsonSerializer(typeof(JwkPublicDefinition));
            var jwkDefinition = (JwkPublicDefinition)deserializer.ReadObject(memoryStream);

            return jwkDefinition;
        }

        /// <summary>
        /// Reads the private JSON Web Key from a file and deserializes it into a JwkDefinition object.
        /// </summary>
        public static JwkDefinition ReadPrivateJsonWebKey()
        {
            string jwsPrivateKey = File.ReadAllText(@"C:\Projects\GenerateJsonWebKey\Notifications\Notifications.jwk");
            using var memoryStream = new MemoryStream(Encoding.Default.GetBytes(jwsPrivateKey));
            // Deserialization from JSON
            var deserializer = new DataContractJsonSerializer(typeof(JwkDefinition));
            var jwkDefinition = (JwkDefinition)deserializer.ReadObject(memoryStream);

            return jwkDefinition;
        }

        /// <summary>
        /// Converts a JwkDefinition object into an RSAEncryptionHelper object.
        /// </summary>
        /// <param name="jwkDefinition">The JwkDefinition object to convert.</param>
        /// <returns>An RSAEncryptionHelper object initialized with the JwkDefinition data.</returns>
        public static DXC.Technology.Encryption.RSAEncryptionHelper GetRsaEncryptionHelper(JwkDefinition jwkDefinition)
        {
            jwkDefinition.P = ConvertFromUrlFormat(jwkDefinition.P);
            jwkDefinition.Q = ConvertFromUrlFormat(jwkDefinition.Q);
            jwkDefinition.N = ConvertFromUrlFormat(jwkDefinition.N);
            jwkDefinition.Dp = ConvertFromUrlFormat(jwkDefinition.Dp);
            jwkDefinition.Dq = ConvertFromUrlFormat(jwkDefinition.Dq);
            jwkDefinition.Qi = ConvertFromUrlFormat(jwkDefinition.Qi);
            jwkDefinition.D = ConvertFromUrlFormat(jwkDefinition.D);
            return new DXC.Technology.Encryption.RSAEncryptionHelper(jwkDefinition.P, jwkDefinition.Q, jwkDefinition.N, jwkDefinition.E, jwkDefinition.Dp, jwkDefinition.Dq, jwkDefinition.Qi, jwkDefinition.D);
        }

        /// <summary>
        /// Converts a JwkDefinition object into an RSAEncryptionHelper object for public key operations.
        /// </summary>
        /// <param name="jwkDefinition">The JwkDefinition object to convert.</param>
        /// <returns>An RSAEncryptionHelper object initialized with the public key data.</returns>
        public static DXC.Technology.Encryption.RSAEncryptionHelper GetPublicKeyRsaEncryptionHelper(JwkDefinition jwkDefinition)
        {
            jwkDefinition.N = ConvertFromUrlFormat(jwkDefinition.N);
            return new DXC.Technology.Encryption.RSAEncryptionHelper("", "", jwkDefinition.N, jwkDefinition.E, "", "", "", "");
        }

        /// <summary>
        /// Converts a Base64 URL-encoded string into a standard Base64 string.
        /// </summary>
        /// <param name="value">The Base64 URL-encoded string to convert.</param>
        /// <returns>A standard Base64 string.</returns>
        public static string ConvertFromUrlFormat(string value)
        {
            string formattedValue = value.Replace('-', '+').Replace('_', '/');
            formattedValue = (formattedValue.Length % 4 == 2) ? formattedValue + "==" : ((formattedValue.Length % 4 == 3) ? formattedValue + "=" : ((formattedValue.Length % 4 == 1) ? formattedValue + "===" : formattedValue));
            return formattedValue;
        }

        /// <summary>
        /// Converts a standard Base64 string into a Base64 URL-encoded string.
        /// </summary>
        /// <param name="value">The standard Base64 string to convert.</param>
        /// <returns>A Base64 URL-encoded string.</returns>
        public static string ConvertToUrlFormat(string value)
        {
            string formattedValue = value.Replace('+', '-').Replace('/', '_').Replace("=", "");
            return formattedValue;
        }

        #endregion
    }

    public class JwtHeader
    {
        #region Public Properties

        /// <summary>
        /// Algorithm used for signing.
        /// </summary>
        public string Alg { get; set; }

        /// <summary>
        /// Key ID.
        /// </summary>
        public string Kid { get; set; }

        /// <summary>
        /// Type of the token.
        /// </summary>
        public string Typ { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes the header to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the header.</returns>
        public string ToJsonString()
        {
            return DXC.Technology.Objects.SerializationHelper.ObjectSerializeToJson(this);
        }

        /// <summary>
        /// Serializes the header to a Base64 string.
        /// </summary>
        /// <returns>A Base64 string representation of the header.</returns>
        public string ToBase64String()
        {
            var header = DXC.Technology.Objects.SerializationHelper.ObjectSerializeToJson(this);
            var base64Header = Convert.ToBase64String(Encoding.Default.GetBytes(header));
            return base64Header;
        }

        #endregion
    }

    public class JwtPayload
    {
        #region Public Properties

        /// <summary>
        /// Subject of the token.
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// Issuer of the token.
        /// </summary>
        public string Iss { get; set; }

        /// <summary>
        /// Issued at timestamp.
        /// </summary>
        public int Iat { get; set; }

        /// <summary>
        /// Expiration timestamp.
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// Unique identifier for the token.
        /// </summary>
        public string Jti { get; set; }

        /// <summary>
        /// Audience of the token.
        /// </summary>
        public string Aud { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes the payload to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the payload.</returns>
        public string ToJsonString()
        {
            return DXC.Technology.Objects.SerializationHelper.ObjectSerializeToJson(this);
        }

        /// <summary>
        /// Serializes the payload to a Base64 string.
        /// </summary>
        /// <returns>A Base64 string representation of the payload.</returns>
        public string ToBase64String()
        {
            var header = DXC.Technology.Objects.SerializationHelper.ObjectSerializeToJson(this);
            var base64Header = Convert.ToBase64String(Encoding.Default.GetBytes(header));
            return base64Header;
        }

        #endregion
    }

    [DataContract]
    public class JwkDefinition
    {
        #region Public Properties

        /// <summary>
        /// Key ID.
        /// </summary>
        [DataMember] public string Kid { get; set; }

        /// <summary>
        /// Key type.
        /// </summary>
        [DataMember] public string Kty { get; set; }

        /// <summary>
        /// Modulus.
        /// </summary>
        [DataMember] public string N { get; set; }

        /// <summary>
        /// Exponent.
        /// </summary>
        [DataMember] public string E { get; set; }

        /// <summary>
        /// First prime factor.
        /// </summary>
        [DataMember] public string Dp { get; set; }

        /// <summary>
        /// Second prime factor.
        /// </summary>
        [DataMember] public string Dq { get; set; }

        /// <summary>
        /// First CRT coefficient.
        /// </summary>
        [DataMember] public string Qi { get; set; }

        /// <summary>
        /// First prime factor.
        /// </summary>
        [DataMember] public string P { get; set; }

        /// <summary>
        /// Second prime factor.
        /// </summary>
        [DataMember] public string Q { get; set; }

        /// <summary>
        /// Private exponent.
        /// </summary>
        [DataMember] public string D { get; set; }

        /// <summary>
        /// Public key use.
        /// </summary>
        [DataMember] public string Use { get; set; }

        /// <summary>
        /// Algorithm.
        /// </summary>
        [DataMember] public string Alg { get; set; }

        /// <summary>
        /// X.509 certificate SHA-1 thumbprint.
        /// </summary>
        [DataMember] public string X5t { get; set; }

        /// <summary>
        /// X.509 certificate chain.
        /// </summary>
        [DataMember] public string X5c { get; set; }

        #endregion
    }

    [DataContract]
    public class JwkPublicDefinition
    {
        #region Public Properties

        /// <summary>
        /// Key ID.
        /// </summary>
        [DataMember] public string Kid { get; set; }

        /// <summary>
        /// Key type.
        /// </summary>
        [DataMember] public string Kty { get; set; }

        /// <summary>
        /// Modulus.
        /// </summary>
        [DataMember] public string N { get; set; }

        /// <summary>
        /// Exponent.
        /// </summary>
        [DataMember] public string E { get; set; }

        /// <summary>
        /// X.509 certificate SHA-1 thumbprint.
        /// </summary>
        [DataMember] public string X5t { get; set; }

        #endregion
    }
}