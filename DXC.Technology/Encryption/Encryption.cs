using System;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Security.Cryptography;
using DXC.Technology.Configuration;
using System.Collections;
using System.Linq;

namespace DXC.Technology.Encryption
{
    /// <summary>
    /// Interface to abstrahate the behavior of encryption into an encryption part and a decryption part
    /// </summary>
    public interface IEncryptionHelper
    {
        /// <summary>
        /// Encrypt a string. 
        /// </summary>
        /// <param name="stringToEncrypt">Ascii string to encrypt</param>
        /// <returns>Encrypted bytes</returns>
        byte[] Encrypt(string stringToEncrypt);

        /// <summary>
        /// Encrypt a string. 
        /// </summary>
        /// <param name="stringToEncrypt">Ascii string to encrypt</param>
        /// <returns>Encrypted string in Base64 format</returns>
        string EncryptAsBase64(string stringToEncrypt);

        /// <summary>
        /// Decrypt a (Base64 formatted) string
        /// </summary>
        /// <param name="encryptedBytes">Encrypted set of bytes</param>
        /// <returns>Decrypted string</returns>
        string Decrypt(byte[] encryptedBytes);

        /// <summary>
        /// Decrypt a (Base64 formatted) string
        /// </summary>
        /// <param name="encryptedBase64Text">Encrypted string in Base64 format</param>
        /// <returns>Decrypted string in Ascii</returns>
        string DecryptFromBase64(string encryptedBase64Text);

        /// <summary>
        /// Take a string, hash it and sign it with your private key
        /// </summary>
        /// <param name="stringToHashAndSign">Ascii string to hash and sign</param>
        /// <param name="hashAlgorithm">Hash Algorithm used in </param>
        /// <returns>Base 64 encoded signed hash</returns>
        string HashAndSign(string stringToHashAndSign, HashAlgorithmEnum hashAlgorithm);

        /// <summary>
        /// Take a string that was hashed and signed and verify it with the public key
        /// </summary>
        /// <param name="stringToHashAndSign">Ascii string to hash and sign</param>
        /// <param name="hashAlgorithm">Hash Algorithm used in </param>
        /// <returns>Base 64 Signature</returns>
        bool IsSignedHashValid(string originalString, string base64EncodedSignedHash, HashAlgorithmEnum hashAlgorithm);
    }

    /// <summary>
    /// Specifies which hashing algorithm to use
    /// </summary>
    public enum HashAlgorithmEnum
    {
        SHA1 = 0,
        SHA256 = 1,
        SHA384 = 2,
        SHA512 = 3,
        MD5 = 4
    }

    public enum SecurityTokenLifetime
    {
        Volatile = 0,
        ValidForCurrentDay = 1,
        VaidForCurrentMonth = 2,
        ValidForCurrentYear = 3,
        Permanent = 4
    }

    /// <summary>
    /// KeySizeEnum is used for the AES (=Rijndael) and determines the key size to be used. Larger keys are harder to crack
    /// </summary>
    public enum KeySizeEnum
    {
        NotSpecified = 0,
        Size128 = 128,
        Size192 = 192,
        Size256 = 256
    }

    /// <summary>
    /// Indicates how a key is / should be specified.
    /// Either by passing a number of parms (Specified)
    /// Or "Random", just for this occasion (RandomKey)
    /// Or use of the "Hardcoded" keys (AssmeblyFixedKey)
    /// </summary>
    public enum SystemGenerateEnum
    {
        Specified = 0,
        RandomKey = 1,
        AssemblyFixedKey = 2,
    }

    /// <summary>
    /// This class uses RSA Encryption as the default implementer for any form of public / private keys encryption need
    /// </summary>
    public class RSAEncryptionHelper : IEncryptionHelper, IDisposable
    {
        #region Constructors 
        RSACryptoServiceProvider iRSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
        System.Text.Encoding iEncoding = System.Text.Encoding.ASCII;

        /// <summary>
        /// Create a simple RSA Encryption Helper either "Random", just for this occasion (RandomKey)
        /// Or based on the "Hardcoded" keys (AssmeblyFixedKey)
        /// </summary>
        /// <param name="pSystemGenerate">"Random", just for this occasion (RandomKey)or based on the "Hardcoded" keys (AssmeblyFixedKey)</param>
        public RSAEncryptionHelper(SystemGenerateEnum pSystemGenerate)
            : base()
        {
            switch (pSystemGenerate)
            {
                case SystemGenerateEnum.RandomKey:
                    RSA lRSA = RSACryptoServiceProvider.Create();
                    iRSACryptoServiceProvider.FromXmlString(lRSA.ToXmlString(true));
                    break;
                case SystemGenerateEnum.AssemblyFixedKey:
                    //iRSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>sr/Ahxodb98I9ddzqGonWZ02lp/64lrmHChh/XrgtW9kTrfebyqG6/DXv87/0WwOIM16HXSCLDWl6uROHVqUmhanZuUSbn/eqMMh1BNyZOlJ3s81c21u0gIg3Yt3BL1pjwa4PYZxHWunLTcMt8SMHoMKxljD8zSaSbNP27mMrkc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
                    iRSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>r9OfIFeYo2s1F/WcH/oBYsT2DKZMbyZynfFx3Vr1wo5kyZE0J0cc0i1uHqUzgJoI9Hmj50cpE0KbfOO/0BRQ0zBNz1qk4lKu5dNYSjJMue8joIUySkzmEP+FzaCqHFsND9GJ+nKPTFg2m5Jj3tdL7qIwWOBz2lgRIHWfUJZRfvU=</Modulus><Exponent>AQAB</Exponent><P>8irWj2XitD/UeWMxOB5Lu0gaJgJr3UoHmZdlq+LUabSCt8tWLnStLbKdTFh7M1fZ/JHMAL9aurXMSKAAPCC/MQ==</P><Q>ud6ykswFi6ykLBQPyoxE0cburqgloOKg82bXUGn7b79aZgP6n9/xTVnj5gIskWfWzHF8Pk17yYpk8NxYbUczBQ==</Q><DP>2u2/a4RAn5PVEqiKJqG89WMJwLMPsM7sb/5e/DXksmtugZpiHz18EoQXkJ2tQ414lM16EHLuIz2XoVCE5db1AQ==</DP><DQ>c4GQaad+HsGnuHizI4Ut00uT0lhOIgAUU45qad+i0FwS+mionCfX1eDxvmNFfQsRsjUoJ7ORJLrrZb8Y+LgRXQ==</DQ><InverseQ>ZOlZ8D31bVrRti1k8GueWYcg0+7sDtxErQHTW/0TpaMdjAWqDFyUYk1f8e3+dwx1MT8vWiyFIOuZFZRQSWv4Ag==</InverseQ><D>JDzGSAEluVCIIccCBWkeTf/wAg1oJWRULQ8s+4Uar51vUke16v5yBzNLxc8gfTnB7LkBOdJluc15hFTgHH+DZJsRAzPxJm8lWSEihyUlE8heHCFK56Dbf5ZTMFFf1TvEmjmGmB375gd1/PecDpipBLo02n+ZXqWSIyxOvLA8NkE=</D></RSAKeyValue>");
                    break;
                default:
                    throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("SystemGenerate", pSystemGenerate.ToString(), " Choose RandomKey or AssemblyFixedKey");
            }
            iRSACryptoServiceProvider.PersistKeyInCsp = false;
        }

        /// <summary>
        /// Create a simple RSA Encryption Helper based on the parameters as specified in a standard XMLString 
        /// For a full RSA encryptor/decryptor the XML-string should be of the form "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>" 
        /// where 0-7 are the corresponding parameters
        /// For a simple RSA encryptor the XML-stirng only needs to be of the form "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>"
        /// Besides various ways to create an RSAEncryptionHelper, there are methods to encrypt and decrypt as well as to sign data and verify signatures
        /// </summary>
        /// <param name="pFromXML">XML Parameters</param>
        public RSAEncryptionHelper(string pFromXML)
            : base()
        {
            iRSACryptoServiceProvider.FromXmlString(pFromXML);
            iRSACryptoServiceProvider.PersistKeyInCsp = false;
        }

        /// <summary>
        /// Create a simple RSA encryptor based on the specified parameters
        /// </summary>
        /// <param name="pModulus">RSA parm: Modulus</param>
        /// <param name="pExponent">RSA parm: Exponent</param>
        public RSAEncryptionHelper(string pModulus, string pExponent)
            : base()
        {
            string lParmsXML;
            lParmsXML = string.Format(DXC.Technology.Utilities.StringFormatProvider.Default,
                "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                pModulus, pExponent);
            iRSACryptoServiceProvider.FromXmlString(lParmsXML);
            iRSACryptoServiceProvider.PersistKeyInCsp = false;
        }

        /// <summary>
        /// Create a full RSA encryptor/decryptor based on the parms specified
        /// </summary>
        /// <param name="pP">RSA Parm: P</param>
        /// <param name="pQ">RSA Parm: Q</param>
        /// <param name="pModulus">RSA Parm: Modulus</param>
        /// <param name="pExponent">RSA Parm: Exponent</param>
        /// <param name="pDP">RSA Parm: DP</param>
        /// <param name="pDQ">RSA Parm: DQ</param>
        /// <param name="pInverseQ">RSA Parm: InverseQ</param>
        /// <param name="pD">RSA Parm: D</param>
        public RSAEncryptionHelper(string pP, string pQ, string pModulus, string pExponent, string pDP, string pDQ, string pInverseQ, string pD)
            : base()
        {
            string lParmsXML;
            lParmsXML = string.Format(DXC.Technology.Utilities.StringFormatProvider.Default,
                "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                pModulus, pExponent, pP, pQ, pDP, pDQ, pInverseQ, pD);
            iRSACryptoServiceProvider.FromXmlString(lParmsXML);
            iRSACryptoServiceProvider.PersistKeyInCsp = false;
        }

        /// <summary>
        /// Create an RSA-encryptor based on XML-info strored in the file specified.
        /// For a full RSA encryptor/decryptor the XML-string in the file should be of the form "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>" 
        /// where 0-7 are the corresponding parameters
        /// For a simple RSA encryptor the XML-stirng in the file only needs to be of the form "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>"
        /// When an extra encryptionHelper is passed (e.g. the standard encryptor), it is assumed that the keyfile itself has been encrypted by this encryption helper and
        /// as such this encryptionhelper will now decrypt this information. This is useful to protect your key-files
        /// </summary>
        /// <param name="fileName">File where the key-info is stored</param>
        /// <param name="encryptionHelper">if specified, use this one to first decrypt the info in the file</param>
        public RSAEncryptionHelper(string fileName, IEncryptionHelper encryptionHelper)
            : base()
        {
            System.IO.FileStream fs = null;
            try
            {
                fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open);

                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                {
                    //nested using is not allowed. The inner "using" cleas up the resource so try finally is needed as well as putting the resource to null 
                    fs = null;

                    if (encryptionHelper == null)
                    {
                        iRSACryptoServiceProvider.FromXmlString(sr.ReadToEnd());
                    }
                    else
                    {
                        iRSACryptoServiceProvider.FromXmlString(encryptionHelper.DecryptFromBase64(Convert.ToBase64String(iEncoding.GetBytes(sr.ReadToEnd()))));
                    }
                }

                iRSACryptoServiceProvider.PersistKeyInCsp = false;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        /// <summary>
        /// Initialize the RSACrypto object
        /// For a full RSA encryptor/decryptor the XML-string should be of the form "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>" 
        /// where 0-7 are the corresponding parameters
        /// For a simple RSA encryptor the XML-stirng only needs to be of the form "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>"
        /// </summary>
        /// <param name="pXML">RSA Parms</param>
        private void FromXMLString(string pXML)
        {
            iRSACryptoServiceProvider.FromXmlString(pXML);
        }

        #endregion

        #region Keys
        /// <summary>
        /// Gets the public key as an XMLString.
        /// This string is of the form <RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>
        /// and can be used by anyone who wants to encrypt something only the owner of this key can decrypt
        /// </summary>
        /// <returns></returns>
        public string PublicKeyAsXMLString()
        {
            return iRSACryptoServiceProvider.ToXmlString(false);
        }
        public bool IsPublicKeyOnly()
        {
            return iRSACryptoServiceProvider.PublicOnly;
        }
        public string PrivateKeyAsXMLString()
        {
            return iRSACryptoServiceProvider.ToXmlString(true);
        }
        #endregion

        /// <summary>
        /// Encrypt the passed set of bytes
        /// </summary>
        /// <param name="pRgb">Bytes to encrypt</param>
        /// <returns>Encrypted bytes</returns>
        /// 



        ///// <summary>
        ///// Decrypt the passed Base 64 - encrypted string 
        ///// </summary>
        ///// <param name="pEncryptedText">String to decrypt</param>
        ///// <returns>Original string</returns>
        //public string Decrypt(string pEncryptedText)
        //{
        //    return iEncoding.GetString(this.iRSACryptoServiceProvider.Decrypt(iEncoding.GetBytes(pEncryptedText), false));
        //}

        ///// <summary>
        ///// Hashes a set of original data and signs the hash. Uses the SHA1 - algrithm 
        ///// </summary>
        ///// <param name="data">Original Data to sign</param>
        ///// <returns>Signature of original data</returns>
        //public byte[] SignData(byte[] data)
        //{
        //    return SignData(data, "SHA1");
        //}

        ///// <summary>
        ///// Hashes a set of original data and signs the hash. Uses the SHA1 - algrithm  
        ///// </summary>
        ///// <param name="data">Original Data to sign</param>
        ///// <returns>Signature of original data</returns>
        //public string SignData(string data)
        //{
        //    return SignData(data, "SHA1");
        //}

        ///// <summary>
        ///// Hashes a set of original data and signs the hash. Uses the passed hashing algrithm  
        ///// </summary>
        ///// <param name="data">Original Data to sign</param>
        ///// <param name="hashAlgorithm">HashAlgorithm to do the hashing</param>
        ///// <returns>Signature of original data</returns>
        //public byte[] SignData(byte[] data, HashAlgorithmEnum hashAlgorithm)
        //{
        //    return this.SignData(data, hashAlgorithm.ToString());
        //}

        ///// <summary>
        ///// Hashes a set of original data and signs the hash. Uses the passed hashing algrithm  
        ///// </summary>
        ///// <param name="data">Original Data to sign</param>
        ///// <param name="hashAlgorithm">HashAlgorithm to do the hashing</param>
        ///// <returns>Signature of original data</returns>
        //public string SignData(string data, HashAlgorithmEnum hashAlgorithm)
        //{
        //    return this.SignData(data, hashAlgorithm.ToString());
        //}

        ///// <summary>
        ///// Hashes a set of original data and signs the hash. Uses the passed hashing algrithm  
        ///// </summary>
        ///// <param name="data">Original Data to sign</param>
        ///// <param name="hashAlgorithm">HashAlgorithm to do the hashing</param>
        ///// <returns>Signature of original data</returns>
        //public byte[] SignData(byte[] data, string hashAlgorithm)
        //{
        //    return this.iRSACryptoServiceProvider.SignData(data, hashAlgorithm);
        //}

        ///// <summary>
        ///// Hashes a set of original data and signs the hash. Uses the passed hashing algrithm  
        ///// </summary>
        ///// <param name="data">Original Data to sign</param>
        ///// <param name="hashAlgorithm">HashAlgorithm to do the hashing</param>
        ///// <returns>Signature of original data</returns>
        //public string SignData(string data, string hashAlgorithm)
        //{
        //    return iEncoding.GetString( this.iRSACryptoServiceProvider.SignData(iEncoding.GetBytes(data), hashAlgorithm));
        //}

        ///// <summary>
        ///// Verifies whether the signature  a set of original data and signs the hash. Uses the passed hashing algrithm  
        ///// </summary>
        ///// <param name="data">Original Data to sign</param>
        ///// <param name="signedData">Data to verify</param>
        ///// <returns>Signature of original data</returns>
        //public bool VerifyData(byte[] data, byte[] signedData)
        //{
        //    return VerifyData(data, "SHA1", signedData);
        //}

        //public bool VerifyData(string data, string base64EncodedSignedData)
        //{
        //    return VerifyData(data, "SHA1", base64EncodedSignedData);
        //}

        //public bool VerifyData(byte[] data, HashAlgorithmEnum hashAlgorithm, byte[] signedData)
        //{
        //    return this.VerifyData(data, hashAlgorithm.ToString(), signedData);
        //}

        //public bool VerifyData(string data, HashAlgorithmEnum hashAlgorithm, string base64EncodedSignedData)
        //{
        //    return this.VerifyData(data, hashAlgorithm.ToString(), base64EncodedSignedData);
        //}

        //public bool VerifyData(string data, string hashAlgorithm, string base64EncodedSignedData)
        //{
        //    return VerifyData(iEncoding.GetBytes(data), hashAlgorithm, Convert.FromBase64String(base64EncodedSignedData));
        //}

        //public bool VerifyData(byte[] data, string hashAlgorithm, byte[] signedData)
        //{
        //    return this.iRSACryptoServiceProvider.VerifyData(data, hashAlgorithm, signedData);
        //}

        /// <summary>
        /// Encrypt a string. 
        /// </summary>
        /// <param name="stringToEncrypt">Ascii string to encrypt</param>
        /// <returns>Encrypted bytes</returns>
        public byte[] Encrypt(string stringToEncrypt)
        {
            if (string.IsNullOrEmpty(stringToEncrypt)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("String to Encrypt");
            return this.iRSACryptoServiceProvider.Encrypt(iEncoding.GetBytes(stringToEncrypt), false);
        }

        /// <summary>
        /// Encrypt a string. 
        /// </summary>
        /// <param name="stringToEncrypt">Ascii string to encrypt</param>
        /// <returns>Encrypted string in Base64 format</returns>
        public string EncryptAsBase64(string stringToEncrypt)
        {
            if (string.IsNullOrEmpty(stringToEncrypt)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("String to Encrypt");
            return Convert.ToBase64String(this.iRSACryptoServiceProvider.Encrypt(iEncoding.GetBytes(stringToEncrypt), false));
        }

        /// <summary>
        /// Decrypt a (Base64 formatted) string
        /// </summary>
        /// <param name="encryptedBytes">Encrypted set of bytes</param>
        /// <returns>Decrypted string</returns>
        public string Decrypt(byte[] encryptedBytes)
        {
            return iEncoding.GetString(this.iRSACryptoServiceProvider.Decrypt(encryptedBytes, false));
        }

        /// <summary>
        /// Decrypt a (Base64 formatted) string
        /// </summary>
        /// <param name="encryptedBase64Text">Encrypted string in Base64 format</param>
        /// <returns>Decrypted string in Ascii</returns>
        public string DecryptFromBase64(string encryptedBase64Text)
        {
            if (string.IsNullOrEmpty(encryptedBase64Text)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("String to Encrypt");
            return iEncoding.GetString(this.iRSACryptoServiceProvider.Decrypt(Convert.FromBase64String(encryptedBase64Text), false));
        }

        /// <summary>
        /// Take a string, hash it and sign it with your private key
        /// </summary>
        /// <param name="stringToHashAndSign">Ascii string to hash and sign</param>
        /// <param name="hashAlgorithm">Hash Algorithm used in </param>
        /// <returns>Base 64 encoded signed hash</returns>

        public string HashAndSign(string stringToHashAndSign, HashAlgorithmEnum hashAlgorithm)
        {
            if (string.IsNullOrEmpty(stringToHashAndSign)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("String To hash and sign");

            DXC.Technology.Encryption.HashEncryptionHelper heh = new HashEncryptionHelper(hashAlgorithm, iEncoding);
            var hashed = heh.CreateHash(stringToHashAndSign);

            return Convert.ToBase64String(iRSACryptoServiceProvider.SignHash(hashed, CryptoConfig.MapNameToOID(hashAlgorithm.ToString())));
        }
        /// <summary>
        /// Take a string that was hashed and signed and verify it with the public key
        /// </summary>
        /// <param name="stringToHashAndSign">Ascii string to hash and sign</param>
        /// <param name="hashAlgorithm">Hash Algorithm used in </param>
        /// <returns>Base 64 Signature</returns>

        public bool IsSignedHashValid(string originalString, string base64EncodedSignedHash, HashAlgorithmEnum hashAlgorithm)
        {
            if (iRSACryptoServiceProvider.VerifyData(
                iEncoding.GetBytes(originalString),
                hashAlgorithm.ToString(),
                 Convert.FromBase64String(base64EncodedSignedHash)))
            {
                DXC.Technology.Encryption.HashEncryptionHelper heh = new HashEncryptionHelper(hashAlgorithm, iEncoding);
                var hashed = heh.CreateHash(originalString);

                return iRSACryptoServiceProvider.VerifyHash(
                    hashed,
                    CryptoConfig.MapNameToOID(hashAlgorithm.ToString()),
                    Convert.FromBase64String(base64EncodedSignedHash));

            }
            return false;
        }

        #region IDisposable Members

        // Track whether Dispose has been called.
        private bool disposed;

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    iRSACryptoServiceProvider.Clear();
                    iRSACryptoServiceProvider.Dispose();
                }
            }
            disposed = true;
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~RSAEncryptionHelper()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        // Allow your Dispose method to be called multiple times,
        // but throw an exception if the object has been disposed.
        // Whenever you do something with this class, 
        // check to see if it has been disposed.
        public void ValidateNonDisposedness()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }


        #endregion
    }


    /// <summary>
    /// This class uses AES Encryption as the default implementer for any form symmetric keys encryption need.
    /// AES is also known under the name Rijndael
    /// </summary>
    public class AESEncryptionHelper : IEncryptionHelper
    {
        private System.Text.Encoding iEncoding = System.Text.Encoding.UTF8;
        private ICryptoTransform iEncryptor;
        private ICryptoTransform iDecryptor;

        private const string cXMLTemplate = "<AES><Key>{0}</Key><IV>{1}</IV></AES>";

        public AESEncryptionHelper(SystemGenerateEnum pSystemGenerate)
            : base()
        {
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                switch (pSystemGenerate)
                {
                    case SystemGenerateEnum.RandomKey:
                        rijndaelManaged.GenerateKey();
                        rijndaelManaged.GenerateIV();
                        rijndaelManaged.Mode = CipherMode.CBC;
                        iEncryptor = rijndaelManaged.CreateEncryptor();
                        iDecryptor = rijndaelManaged.CreateDecryptor();
                        break;
                    case SystemGenerateEnum.AssemblyFixedKey:
                        this.FromXMLString("<AESKeyValue><Key>CgmtyOP7RDH1eANhPeFyw7QazA6J5vUP6Tf8AE2BmQ8=</Key><IV>gucpLaGRq4hU75Yp+1zmnA==</IV></AESKeyValue>");
                        break;
                    default:
                        throw new DXC.Technology.Exceptions.NamedExceptions.ParameterInvalidException("SystemGenerate", pSystemGenerate.ToString(), " Choose RandomKey or AssemblyFixedKey");
                }
            }
        }

        public AESEncryptionHelper(byte[] key, byte[] pIV)
            : base()
        {
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Padding = PaddingMode.PKCS7;

                rijndaelManaged.Key = key;
                rijndaelManaged.IV = pIV;
                rijndaelManaged.Mode = CipherMode.CBC;
                iEncryptor = rijndaelManaged.CreateEncryptor();
                iDecryptor = rijndaelManaged.CreateDecryptor();
            }
        }

        public AESEncryptionHelper(string pPassPhrase, string pSaltValue, HashAlgorithmEnum hashAlgorithm, string pInitVector, KeySizeEnum pKeySize)
            : base()
        {
            //Rijndael rijndael = Rijndael.Create();
            //rijndael.Padding = PaddingMode.PKCS7;
            //Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pPassPhrase, iEncoding.GetBytes(pSaltValue));
            //rijndael.Key = pdb.GetBytes(32);
            //rijndael.IV = pdb.GetBytes(16);
            //rijndael.Mode = CipherMode.CBC;

            //iEncryptor = rijndael.CreateEncryptor();
            //iDecryptor = rijndael.CreateDecryptor();


            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.Padding = PaddingMode.PKCS7;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pPassPhrase, iEncoding.GetBytes(pSaltValue));
                rijndael.Key = pdb.GetBytes(32);
                rijndael.IV = pdb.GetBytes(16);
                rijndael.Mode = CipherMode.CBC;

                iEncryptor = rijndael.CreateEncryptor();
                iDecryptor = rijndael.CreateDecryptor();
            }
        }

        public AESEncryptionHelper(string pXML)
            : base()
        {
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Padding = PaddingMode.PKCS7;

                this.FromXMLString(pXML);
            }
        }

        public AESEncryptionHelper(string fileName, IEncryptionHelper encryptionHelper)
            : base()
        {
            System.IO.FileStream fs = null;
            try
            {
                fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                {
                    fs = null;
                    if (encryptionHelper == null)
                    {
                        this.FromXMLString(sr.ReadToEnd());
                    }
                    else
                    {
                        this.FromXMLString(encryptionHelper.DecryptFromBase64(sr.ReadToEnd()));
                    }
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        private void FromBase64KeyAndIv(string base64Key, string base64IV)
        {

            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Key = Convert.FromBase64String(base64Key);
                rijndaelManaged.IV = Convert.FromBase64String(base64IV);
                rijndaelManaged.Mode = CipherMode.CBC;
                iEncryptor = rijndaelManaged.CreateEncryptor();
                iDecryptor = rijndaelManaged.CreateDecryptor();
            }
        }

        private void FromXMLString(string pXML)
        {

            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                if (string.IsNullOrEmpty(pXML)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("XML String");
                int iKeyStart = pXML.IndexOf("<Key>", Utilities.StringComparisonProvider.Default);
                int iKeyEnd = pXML.IndexOf("</Key>", Utilities.StringComparisonProvider.Default);
                string lKey = pXML.Substring(iKeyStart + 5, iKeyEnd - iKeyStart - 5);
                rijndaelManaged.Key = Convert.FromBase64String(lKey);
                int iIVStart = pXML.IndexOf("<IV>", Utilities.StringComparisonProvider.Default);
                int iIVEnd = pXML.IndexOf("</IV>", Utilities.StringComparisonProvider.Default);
                string lIV = pXML.Substring(iIVStart + 4, iIVEnd - iIVStart - 4);
                rijndaelManaged.IV = Convert.FromBase64String(lIV);
                rijndaelManaged.Mode = CipherMode.CBC;
                iEncryptor = rijndaelManaged.CreateEncryptor();
                iDecryptor = rijndaelManaged.CreateDecryptor();
            }
        }

        public string KeyAsXMLString()
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            return string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, cXMLTemplate, Convert.ToBase64String(rijndaelManaged.Key), Convert.ToBase64String(rijndaelManaged.IV));
        }


        /// <summary>
        /// Encrypt a string. 
        /// </summary>
        /// <param name="stringToEncrypt">Ascii string to encrypt</param>
        /// <returns>Encrypted bytes</returns>
        public byte[] Encrypt(string stringToEncrypt)
        {
            return EncryptFromByteArray(iEncoding.GetBytes(stringToEncrypt));
        }

        public byte[] EncryptFromByteArray(byte[] data)
        {
            if (data == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Binary Data");

            // Define memory stream which will be used to hold encrypted data.
            System.IO.MemoryStream lMemoryStream = null;
            try
            {
                lMemoryStream = new System.IO.MemoryStream();
                // Define cryptographic stream (always use Write mode for encryption).
                using (CryptoStream lCryptoStream = new CryptoStream(lMemoryStream, iEncryptor, CryptoStreamMode.Write))
                {
                    // Start encrypting
                    lCryptoStream.Write(data, 0, data.GetLength(0));

                    // Finish encrypting.
                    lCryptoStream.FlushFinalBlock();

                    // Convert encrypted data into a base64-encoded string.
                    byte[] lResult = lMemoryStream.ToArray();

                    // Return encrypted string.
                    return lResult;
                }
            }
            finally
            {
                if (lMemoryStream != null)
                    lMemoryStream.Dispose();
            }
        }
        /// <summary>
        /// Encrypt a string. 
        /// </summary>
        /// <param name="stringToEncrypt">Ascii string to encrypt</param>
        /// <returns>Encrypted string in Base64 format</returns>

        public string EncryptAsBase64(string stringToEncrypt)
        {
            // Define memory stream which will be used to hold encrypted data.
            System.IO.MemoryStream lMemoryStream = null;
            try
            {
                lMemoryStream = new System.IO.MemoryStream();
                // Define cryptographic stream (always use Write mode for encryption).
                using (CryptoStream lCryptoStream = new CryptoStream(lMemoryStream, iEncryptor, CryptoStreamMode.Write))
                {
                    // Start encrypting
                    byte[] lData;
                    lData = iEncoding.GetBytes(stringToEncrypt);
                    lCryptoStream.Write(lData, 0, lData.GetLength(0));

                    // Finish encrypting.
                    lCryptoStream.FlushFinalBlock();

                    // Convert encrypted data into a base64-encoded string.
                    string lCipherText = Convert.ToBase64String(lMemoryStream.ToArray());

                    // Return encrypted string.
                    return lCipherText;
                }
            }
            finally
            {
                //nested using is not allowed. The inner "using" cleans up the resource so try finally is needed as well as putting the resource to null 
                lMemoryStream = null;
            }
        }



        /// <summary>
        /// Decrypt a (Base64 formatted) string
        /// </summary>
        /// <param name="encryptedBytes">Encrypted set of bytes</param>
        /// <returns>Decrypted string</returns>
        public string Decrypt(byte[] encryptedBytes)
        {
            if (encryptedBytes == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Binary Data");
            // Define memory stream which will be used to hold encrypted data.
            System.IO.MemoryStream lMemoryStream = null;
            try
            {
                lMemoryStream = new System.IO.MemoryStream(encryptedBytes);
                // Define cryptographic stream (always use Read mode for encryption).
                using (CryptoStream lCryptoStream = new CryptoStream(lMemoryStream, iDecryptor, CryptoStreamMode.Read))
                {
                    // Start decrypting.
                    byte[] lDecryptedByteArray = new byte[encryptedBytes.GetLength(0)];
                    int lDecryptedArrayNrOfBytes = lCryptoStream.Read(lDecryptedByteArray, 0, encryptedBytes.GetLength(0));

                    byte[] lResult = new byte[lDecryptedArrayNrOfBytes];
                    Array.Copy(lDecryptedByteArray, lResult, lDecryptedArrayNrOfBytes);

                    // Return decrypted string.   
                    return iEncoding.GetString(lResult);
                }
            }
            finally
            {
                //nested using is not allowed. The inner "using" cleas up the resource so try false is needed as well as putting the resource to null 
                lMemoryStream = null;
            }
        }

        /// <summary>
        /// Decrypt a (Base64 formatted) string
        /// </summary>
        /// <param name="encryptedBase64Text">Encrypted string in Base64 format</param>
        /// <returns>Decrypted string in Ascii</returns>
        public string DecryptFromBase64ThreadUnsafe(string encryptedBase64Text)
        {
            if (encryptedBase64Text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Binary Data");


            byte[] lData = Convert.FromBase64String(encryptedBase64Text);

            System.IO.MemoryStream lMemoryStream = null;
            try
            {
                lMemoryStream = new System.IO.MemoryStream(lData);
                // Define cryptographic stream (always use Read mode for encryption).
                using (CryptoStream lCryptoStream = new CryptoStream(lMemoryStream, iDecryptor, CryptoStreamMode.Read))
                {
                    // Since at this point we don't know what the size of decrypted data
                    // will be, allocate the buffer long enough to hold ciphertext;
                    // plaintext is never longer than ciphertext.
                    byte[] lDecryptedByteArray = new byte[encryptedBase64Text.Length];
                    int lDecryptedArrayNrOfBytes = lCryptoStream.Read(lDecryptedByteArray, 0, encryptedBase64Text.Length);

                    byte[] lResult = new byte[lDecryptedArrayNrOfBytes];
                    Array.Copy(lDecryptedByteArray, lResult, lDecryptedArrayNrOfBytes);

                    // Convert decrypted data into a string. 
                    // Let us assume that the original plaintext string was UTF8-encoded.
                    return iEncoding.GetString(lResult);
                }
            }
            finally
            {
                //nested using is not allowed. The inner "using" cleas up the resource so try finally is needed as well as putting the resource to null 
                lMemoryStream = null;

            }
        }



        public string DecryptFromBase64(string encryptedBase64Text)
        {
            if (encryptedBase64Text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Binary Data");

            return ThreadSafeDecryptFromBase64(encryptedBase64Text, iDecryptor, iEncoding);
        }

        public static string ThreadSafeDecryptFromBase64(string encryptedBase64Text, ICryptoTransform decryptor, System.Text.Encoding encoding)
        {
            if (encryptedBase64Text == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Binary Data");


            byte[] lData = Convert.FromBase64String(encryptedBase64Text);

            System.IO.MemoryStream lMemoryStream = null;
            try
            {
                lMemoryStream = new System.IO.MemoryStream(lData);
                // Define cryptographic stream (always use Read mode for encryption).
                using (CryptoStream lCryptoStream = new CryptoStream(lMemoryStream, decryptor, CryptoStreamMode.Read))
                {
                    // Since at this point we don't know what the size of decrypted data
                    // will be, allocate the buffer long enough to hold ciphertext;
                    // plaintext is never longer than ciphertext.
                    byte[] lDecryptedByteArray = new byte[encryptedBase64Text.Length];
                    int lDecryptedArrayNrOfBytes = lCryptoStream.Read(lDecryptedByteArray, 0, encryptedBase64Text.Length);

                    byte[] lResult = new byte[lDecryptedArrayNrOfBytes];
                    Array.Copy(lDecryptedByteArray, lResult, lDecryptedArrayNrOfBytes);

                    // Convert decrypted data into a string. 
                    // Let us assume that the original plaintext string was UTF8-encoded.
                    return encoding.GetString(lResult);
                }
            }
            finally
            {
                //nested using is not allowed. The inner "using" cleas up the resource so try finally is needed as well as putting the resource to null 
                lMemoryStream = null;

            }
        }


        /// <summary>
        /// Take a string, hash it and sign it with your key
        /// </summary>
        /// <param name="stringToHashAndSign">Ascii string to hash and sign</param>
        /// <param name="hashAlgorithm">Hash Algorithm used in </param>
        /// <returns>Base 64 encoded signed hash</returns>
        public string HashAndSign(string stringToHashAndSign, HashAlgorithmEnum hashAlgorithm)
        {
            if (string.IsNullOrEmpty(stringToHashAndSign)) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("String To hash and sign");

            DXC.Technology.Encryption.HashEncryptionHelper heh = new HashEncryptionHelper(hashAlgorithm, iEncoding);
            var hashed = heh.CreateHashAsBase64(stringToHashAndSign);
            return EncryptAsBase64(hashed);
        }

        /// <summary>
        /// Take a string that was hashed and signed and verify it with the public key
        /// </summary>
        /// <param name="stringToHashAndSign">Ascii string to hash and sign</param>
        /// <param name="hashAlgorithm">Hash Algorithm used in </param>
        /// <returns>Base 64 Signature</returns>
        public bool IsSignedHashValid(string originalString, string base64EncodedSignedHash, HashAlgorithmEnum hashAlgorithm)
        {
            DXC.Technology.Encryption.HashEncryptionHelper heh = new HashEncryptionHelper(hashAlgorithm, iEncoding);
            var hashed = heh.CreateHashAsBase64(originalString);
            return EncryptAsBase64(hashed).Equals(base64EncodedSignedHash);
        }

    }



    /// <remarks>
    /// In the DXC.Technology, encryption has received a good basic support, especially regarding caching and configuring.
    /// The reason is that encryption is powerful, yet the management of keys cumbersome (hence -> configuration) and the creation
    /// of encryptors is very time-consuming (hence -> caching). The three basic types of encryption each have 1
    /// major implementation in this DXC.Technology. 
    /// The encryption Helper is implemented as a singleton
    /// The encryption helper thypically is loaded as encryptions are required, or e.g. on the config-info
    /// Three encryptor types are supported:
    /// - Symmetric Key encryption -> AES Algoritm (previously known as Rheindael)
    /// - Asymmetric Key Encryption -> RSA algorithm 
    /// - Hashing -> SHA1
    /// When reading the specific encryption configuration settings out of the XML in the encryptionHandling section
    /// a number of Valid Encryptor definitions are possible:
    /// - RSA / AES: Random keys
    /// - RSA / AES: Technology hardcoded keys
    /// - RSA / AES: KeyFile
    /// - RSA / AES: KeyFile + Encrypted with one of the Technology hardcoded keys
    /// - AES: Passphrase - SaltValue - InitializationVector - KeySize
    /// - RSA: Modulus - Exponent - P - Q - DP - DQ - Inverse Q - D
    /// </remarks>
    public class EncryptionHelper
    {
        private static EncryptionHelper sCurrent;

        private Hashtable iRSAEncryptionHelpers = new Hashtable();
        private Hashtable iAESEncryptionHelpers = new Hashtable();

        private static DXC.Technology.Objects.Lock sLock = new DXC.Technology.Objects.Lock();

        /// <summary>
        /// Singletong Encryption Helper
        /// </summary>
        public static EncryptionHelper Current
        {
            get
            {
                lock (sLock)
                {
                    if (sCurrent == null)
                    {
                        sCurrent = new EncryptionHelper();
                        foreach (var lEncryptor in DXC.Technology.Configuration.EncryptionOptionsManager.Current.GetEncryptionOptions().Encryptors)
                        {
                            sCurrent.AddEncryptionHelper(lEncryptor.LogicalName);
                        }
                    }
                }
                return sCurrent;
            }
        }

        /// <summary>
        /// Gets the specified RSA Encryption Helper.
        /// </summary>
        /// <param name="logicalName">Logical Name of the RSA Encryption Helper</param>
        /// <returns></returns>
        public RSAEncryptionHelper GetRSAEncryptionHelper(string logicalName)
        {
            lock (sLock)
            {
                if (!iRSAEncryptionHelpers.Contains(logicalName))
                {
                    this.AddEncryptionHelper(logicalName);
                }
                return (RSAEncryptionHelper)iRSAEncryptionHelpers[logicalName];
            }
        }

        /// <summary>
        /// Gets the specified AES Encryption Helper.
        /// </summary>
        /// <param name="logicalName">Logical Name of the AES Encryption Helper</param>
        /// <returns></returns>
        public AESEncryptionHelper GetAESEncryptionHelper(string logicalName)
        {
            lock (sLock)
            {
                if (!iAESEncryptionHelpers.Contains(logicalName))
                {
                    this.AddEncryptionHelper(logicalName);
                }
            }
            return (AESEncryptionHelper)iAESEncryptionHelpers[logicalName];
        }

        /// <summary>
        /// Searches the RSA/AES/Hash cashes of encryption helpers to find a helper matching the logical name
        /// </summary>
        /// <param name="logicalName">Logical Name of the Encryption Helper</param>
        /// <returns></returns>
        public IEncryptionHelper GetEncryptionHelper(string logicalName)
        {
            if (this.iRSAEncryptionHelpers.ContainsKey(logicalName))
                return (IEncryptionHelper)GetRSAEncryptionHelper(logicalName);
            if (this.iAESEncryptionHelpers.ContainsKey(logicalName))
                return (IEncryptionHelper)GetAESEncryptionHelper(logicalName);
            return null;
        }

        public static string CreateSecurityToken(string encryptorLogicalName, string userName)
        {
            DXC.Technology.Encryption.AESEncryptionHelper lEncryptor = DXC.Technology.Encryption.EncryptionHelper.Current.GetAESEncryptionHelper(encryptorLogicalName);
            if (lEncryptor == null)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ItemNotFoundException("Encryptor" + encryptorLogicalName);
            }
            else
            {
                //DateTime lServerDateTime = DateTime.Now.Add(DXC.Technology.Utilities.Date.TimeDifferenceWithServer);
                //lServerDateTime = DateTime.Now.Date.AddHours(10);
                DateTime lServerDateTime = DateTime.Now;
                string lSecurityToken = string.Format(DXC.Technology.Utilities.StringFormatProvider.Default, "{0}|{1}", userName, DXC.Technology.Utilities.Date.ToYYYYMMDDHHMMSSFFFString(lServerDateTime));
                string lEncryptedSecurityToken = lEncryptor.EncryptAsBase64(lSecurityToken);

                string lFixedEncodedEncryptedSecurityToken = lEncryptedSecurityToken.Replace("+", "_PLUS_").Replace("/", "_SLASH_");
                return lFixedEncodedEncryptedSecurityToken;
                //string lEncodedEncryptedSecurityToken = System.Net.WebUtility.UrlEncode(lEncryptedSecurityToken);
                //string lFixedEncodedEncryptedSecurityToken = lEncodedEncryptedSecurityToken;
                ////lFixedEncodedEncryptedSecurityToken = lEncodedEncryptedSecurityToken.Replace("+", "_PLUS_");

                //string linv1 = lFixedEncodedEncryptedSecurityToken;
                ////linv1 = lFixedEncodedEncryptedSecurityToken.Replace("_PLUS_", "+").Trim('\0');
                //string linv2 = System.Net.WebUtility.UrlDecode(linv1);
                //string linv3 = lEncryptor.Decrypt(linv2);
                //if (!lSecurityToken.Equals(linv3))
                //    throw new Exception("strnage");
                //return lEncodedEncryptedSecurityToken;
            }
        }

        public static string CreateWebRequestSecurityToken(string userName)
        {
            string lEncryptorLogicalName = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptor;
            if (string.IsNullOrEmpty(lEncryptorLogicalName))
            {
                if (DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptionMandatory)
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Web Request Encryptor");
                else
                    return userName;
            }
            else
            {
                return CreateSecurityToken(lEncryptorLogicalName, userName);
            }
        }

        public static string CreateAutoLogOnSecurityToken(string userName)
        {
            string lEncryptorLogicalName = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().AutoLogOnEncryptor;
            if (string.IsNullOrEmpty(lEncryptorLogicalName))
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("AutoLogOn Encryptor");
            }
            else
            {
                return CreateSecurityToken(lEncryptorLogicalName, userName);
            }
        }

        public static string CreateActivationToken(string userName)
        {
            string lEncryptorLogicalName = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptor;
            if (string.IsNullOrEmpty(lEncryptorLogicalName))
            {
                if (DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptionMandatory)
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Web Request Encryptor");
                else
                    return userName;
            }
            else
            {
                string token = CreateSecurityToken(lEncryptorLogicalName, userName);
                token = token.Replace("+", "_PLUS_").Replace("/", "_SLASH_");
                return token;
            }
        }

        public static void ValidateSecurityTokenExpiration(string encryptedSecurityTokenUrlEncoded, SecurityTokenLifetime securityTokenLifetime)
        {
            string encryptedSecurityToken = encryptedSecurityTokenUrlEncoded.Replace("_PLUS_", "+").Replace("_SLASH_", "/");
            string lEncryptorLogicalName = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptor;

            if (lEncryptorLogicalName == null)
            {
                return;
            }
            else
            {
                string lDecryptedSecurityToken = string.Empty;
                DXC.Technology.Encryption.AESEncryptionHelper lEncryptor = DXC.Technology.Encryption.EncryptionHelper.Current.GetAESEncryptionHelper(lEncryptorLogicalName);
                lDecryptedSecurityToken = lEncryptor.DecryptFromBase64(encryptedSecurityToken);
                string[] lSecurityTokens = lDecryptedSecurityToken.Split('|');
                if ((lSecurityTokens == null) || (lSecurityTokens.Length == 0) || (lSecurityTokens.Length > 2)) // Missing / Empty or corrupt security token
                    throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Invalid Security Token");
                DateTime tokencreationdatetime = DXC.Technology.Utilities.Date.FromYYYYMMDDHHMMSSFFFString(lSecurityTokens[1]);
                //Case lSecurityTokens.Length == 2 - = typical case = user name + date time , both encrpyted

                DateTime serverDateTime = DateTime.Now;

                switch (securityTokenLifetime)
                {
                    case SecurityTokenLifetime.Volatile:
                        {
                            TimeSpan diff;
                            if (tokencreationdatetime < serverDateTime)
                                diff = serverDateTime.Subtract(tokencreationdatetime);
                            else
                                diff = tokencreationdatetime.Subtract(serverDateTime);
                            if (diff.TotalSeconds > 500)
                                throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Expired Security Token");
                            break;
                        }
                    case SecurityTokenLifetime.ValidForCurrentDay:
                        if (!serverDateTime.Date.Equals(tokencreationdatetime.Date))
                            throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Expired Security Token");
                        break;
                    case SecurityTokenLifetime.VaidForCurrentMonth:
                        if (!serverDateTime.Date.Year.Equals(tokencreationdatetime.Date.Year) || !serverDateTime.Date.Month.Equals(tokencreationdatetime.Date.Month))
                            throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Expired Security Token");
                        break;
                    case SecurityTokenLifetime.ValidForCurrentYear:
                        if (!serverDateTime.Date.Year.Equals(tokencreationdatetime.Date.Year))
                            throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Expired Security Token");
                        break;
                    case SecurityTokenLifetime.Permanent:
                        break;
                }
            }
        }

        public static string ExtractUserName(string encryptedSecurityToken)
        {
            string lEncryptorLogicalName = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptor;

            if (lEncryptorLogicalName == null)
            {
                return encryptedSecurityToken;
            }
            else
            {
                string lDecryptedSecurityToken = string.Empty;
                DXC.Technology.Encryption.AESEncryptionHelper lEncryptor = DXC.Technology.Encryption.EncryptionHelper.Current.GetAESEncryptionHelper(lEncryptorLogicalName);

                lDecryptedSecurityToken = lEncryptor.DecryptFromBase64(encryptedSecurityToken.Replace("_PLUS_", "+").Replace("_SLASH_", "/"));
                string[] lSecurityTokens = lDecryptedSecurityToken.Split('|');
                if ((lSecurityTokens == null) || (lSecurityTokens.Length == 0) || (lSecurityTokens.Length > 2)) // Missing / Empty or corrupt security token
                    throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Invalid Security Token");

                //Case lSecurityTokens.Length == 2 - = typical case = user name + date time , both encrpyted
                string lEncryptedUserName = lSecurityTokens[0];
                return lEncryptedUserName;
            }
        }
        public static void ValidateWebRequestSecurityToken(string expectedUserName, string encryptedSecurityToken)
        {
            if (expectedUserName == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("User Name");

            string lEncryptorLogicalName = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptor;
            if (string.IsNullOrEmpty(lEncryptorLogicalName))
            {
                if (DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptionMandatory)
                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Web Request Encryptor");
                else
                {
                    if ((encryptedSecurityToken != null) && (!string.IsNullOrEmpty(encryptedSecurityToken)) && (!expectedUserName.Equals(expectedUserName)))
                        throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Invalid Security Token");
                    else
                        return; //do nothing -- no decryptor present and names match
                }
            }
            else
            {
                if ((encryptedSecurityToken == null) || (string.IsNullOrEmpty(encryptedSecurityToken)) || (encryptedSecurityToken.Equals(expectedUserName))) // The security token was not encrypted and matches
                {
                    if (DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().WebRequestEncryptionMandatory)
                    {
                        throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Client Web Request Not Encrypted");
                    }
                    else
                    {
                        return; //do nothing -- security token not needed and names match					}
                    }
                }
                else
                {
                    ValidateSecurityToken(lEncryptorLogicalName, expectedUserName, encryptedSecurityToken);
                }
            }
        }

        public static void ValidateAutoLogOnSecurityToken(string expectedUserName, string encryptedSecurityToken)
        {
            if (expectedUserName == null) throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("User Name");

            string lEncryptorLogicalName = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().AutoLogOnEncryptor;
            if (string.IsNullOrEmpty(lEncryptorLogicalName))
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("AutoLogOnEncryptor");
            }
            else
            {
                if ((encryptedSecurityToken == null) || (string.IsNullOrEmpty(encryptedSecurityToken)) || (encryptedSecurityToken.Equals(expectedUserName))) // The security token was not encrypted and matches
                {
                    throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Client Web Request Not Encrypted");
                }
                else
                {
                    ValidateSecurityToken(lEncryptorLogicalName, expectedUserName, encryptedSecurityToken);
                }
            }
        }

        public static void ValidateActivationToken(string encryptorLogicalName, string expectedUserName, string encryptedSecurityToken)
        {
            ValidateActivationToken(encryptorLogicalName, expectedUserName, encryptedSecurityToken, 3);
        }
        public static void ValidateActivationToken(string encryptorLogicalName, string expectedUserName, string encryptedSecurityToken, double daysBeforExpiration)
        {
            string lDecryptedSecurityToken = string.Empty;
            DXC.Technology.Encryption.AESEncryptionHelper lEncryptor = DXC.Technology.Encryption.EncryptionHelper.Current.GetAESEncryptionHelper(encryptorLogicalName);
            if (lEncryptor == null)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ItemNotFoundException("Encryptor" + encryptorLogicalName);
            }
            else
            {
                lDecryptedSecurityToken = lEncryptor.DecryptFromBase64(encryptedSecurityToken.Replace("_PLUS_", "+").Replace("_SLASH_", "/"));
                string[] lSecurityTokens = lDecryptedSecurityToken.Split('|');
                if ((lSecurityTokens == null) || (lSecurityTokens.Length == 0) || (lSecurityTokens.Length > 2)) // Missing / Empty or corrupt security token
                    throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Invalid Security Token");

                //Case lSecurityTokens.Length == 2 - = typical case = user name + date time , both encrpyted
                string lEncryptedUserName = lSecurityTokens[0];
                DateTime lDateTime = DXC.Technology.Utilities.Date.FromYYYYMMDDHHMMSSFFFString(lSecurityTokens[1]);

                TimeSpan lDiff;
                DateTime lServerDateTime = DateTime.Now;
                if (lDateTime < lServerDateTime)
                {
                    lDiff = lServerDateTime.Subtract(lDateTime);
                }
                else
                {
                    lDiff = lDateTime.Subtract(lServerDateTime);
                }

                if ((lEncryptedUserName.EndsWith(expectedUserName)) && (lDiff.TotalDays < daysBeforExpiration))
                    return; //Encrypted token has correct name and time in range!
                else
                    throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Your activation token is expired");

            }
        }

        public static string EncryptSecret(string unencrypted)
        {
            return DXC.Technology.Encryption.EncryptionHelper.Current.GetAESEncryptionHelper("WSCM").EncryptAsBase64(unencrypted);
        }
        public static string UnencryptSecret(string encrypted)
        {
            return DXC.Technology.Encryption.EncryptionHelper.Current.GetAESEncryptionHelper("WSCM").DecryptFromBase64(encrypted);
        }
        public static void ValidateSecurityToken(string encryptorLogicalName, string expectedUserName, string encryptedSecurityToken)
        {
            ValidateSecurityToken(encryptorLogicalName, expectedUserName, encryptedSecurityToken, 1000);
        }
        public static void ValidateSecurityToken(string encryptorLogicalName, string expectedUserName, string encryptedSecurityToken, double pSecondsBeforeExpiration)
        {
            string lDecryptedSecurityToken = string.Empty;
            DXC.Technology.Encryption.AESEncryptionHelper lEncryptor = DXC.Technology.Encryption.EncryptionHelper.Current.GetAESEncryptionHelper(encryptorLogicalName);
            if (lEncryptor == null)
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.ItemNotFoundException("Encryptor" + encryptorLogicalName);
            }
            else
            {
                lDecryptedSecurityToken = lEncryptor.DecryptFromBase64(encryptedSecurityToken.Replace("_PLUS_", "+").Replace("_SLASH_", "/"));
                string[] lSecurityTokens = lDecryptedSecurityToken.Split('|');
                if ((lSecurityTokens == null) || (lSecurityTokens.Length == 0) || (lSecurityTokens.Length > 2)) // Missing / Empty or corrupt security token
                    throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Invalid Security Token");

                //Case lSecurityTokens.Length == 2 - = typical case = user name + date time , both encrpyted
                string lEncryptedUserName = lSecurityTokens[0];
                DateTime lDateTime = DXC.Technology.Utilities.Date.FromYYYYMMDDHHMMSSFFFString(lSecurityTokens[1]);

                TimeSpan lDiff;
                DateTime lServerDateTime = DateTime.Now;
                if (lDateTime < lServerDateTime)
                {
                    lDiff = lServerDateTime.Subtract(lDateTime);
                }
                else
                {
                    lDiff = lDateTime.Subtract(lServerDateTime);
                }

                if (!lEncryptedUserName.Equals(expectedUserName))
                    throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Invalid UserName in Security Token");
                else if (lDiff.TotalSeconds > pSecondsBeforeExpiration)
                    throw new DXC.Technology.Exceptions.NamedExceptions.IllegalApplicationAccessException("Expired Security Token");
                else
                    return; //Encrypted token has correct name and time in range!					
            }
        }

        public static string GetRandomBase64Salt()
        {
            var saltBytes = new byte[32];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Add an encryption helper to the appropriate cache. Throws an exception if the name is already "taken" 
        /// </summary>
        /// <param name="logicalName">Logical Name under which to add the encryption helper</param>
        /// <param name="encryptionHelper">Encryption Helper To Add</param>
        public void AddEncryptionHelper(string logicalName, IEncryptionHelper encryptionHelper)
        {
            if (encryptionHelper == null) throw new Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Encryption Helper");
            if (encryptionHelper.GetType() == typeof(RSAEncryptionHelper))
            {
                if (!this.iRSAEncryptionHelpers.ContainsKey(logicalName))
                {
                    this.iRSAEncryptionHelpers.Add(logicalName, encryptionHelper);
                }
                else
                {
                    throw new Exceptions.NamedExceptions.NotUniqueException("Encryptor Name", logicalName);
                }
            }
            else
            {
                if (encryptionHelper.GetType() == typeof(AESEncryptionHelper))
                {
                    if (!this.iAESEncryptionHelpers.ContainsKey(logicalName))
                    {
                        this.iAESEncryptionHelpers.Add(logicalName, encryptionHelper);
                    }
                    else
                    {
                        throw new Exceptions.NamedExceptions.NotUniqueException("Encryptor Name", logicalName);
                    }
                }
                else
                {
                    throw new Exceptions.NamedExceptions.ParameterTypeInvalidException("Encryptor Tupe", encryptionHelper.GetType().Name, "RSA, AES or Hash");
                }
            }
        }

        /// <summary>
        /// Add an encryption helper to the appropriate cache. Retrieves the encryption configuration information out of the config file.
        /// Throws an exception if the name is already "taken" 
        /// </summary>
        /// <param name="logicalName">Logical Name under which to add the encryption helper</param>
        public void AddEncryptionHelper(string logicalName)
        {
            EncryptorOptions lEncryptorOptions;
            lEncryptorOptions = DXC.Technology.Configuration.EncryptionOptionsManager.Current.GetEncryptionOptions().GetEncryptorOptions(logicalName);
            if (lEncryptorOptions != null)
            {
                switch (lEncryptorOptions.EncryptionType)
                {
                    case Configuration.EncryptionTypeEnum.RSA:
                        if (!string.IsNullOrEmpty(lEncryptorOptions.KeyFile))
                        {
                            if (lEncryptorOptions.EncryptorLogicalName == logicalName) throw new Exceptions.NamedExceptions.ParameterInvalidException("EncryptorLogicalName", logicalName, "Cannot Decrypt Itself");
                            iRSAEncryptionHelpers.Add(logicalName, new RSAEncryptionHelper(lEncryptorOptions.KeyFile, GetEncryptionHelper(lEncryptorOptions.EncryptorLogicalName)));
                        }
                        else
                        {
                            if (lEncryptorOptions.SystemGenerate != SystemGenerateEnum.Specified)
                            {
                                iRSAEncryptionHelpers.Add(logicalName, new RSAEncryptionHelper(lEncryptorOptions.SystemGenerate));
                            }
                            else
                            {
                                iRSAEncryptionHelpers.Add(logicalName, new RSAEncryptionHelper(lEncryptorOptions.P, lEncryptorOptions.Q, lEncryptorOptions.Modulus,
                                    lEncryptorOptions.Exponent, lEncryptorOptions.DP, lEncryptorOptions.DQ,
                                    lEncryptorOptions.InverseQ, lEncryptorOptions.D));
                            }
                        }
                        break;
                    case Configuration.EncryptionTypeEnum.AES:
                        if (!string.IsNullOrEmpty(lEncryptorOptions.KeyFile))
                        {
                            if (lEncryptorOptions.EncryptorLogicalName == logicalName) throw new Exceptions.NamedExceptions.ParameterInvalidException("EncryptorLogicalName", logicalName, "Cannot Decrypt Itself");
                            iAESEncryptionHelpers.Add(logicalName, new AESEncryptionHelper(lEncryptorOptions.KeyFile, GetEncryptionHelper(lEncryptorOptions.EncryptorLogicalName)));
                        }
                        else
                        {
                            if (lEncryptorOptions.SystemGenerate != SystemGenerateEnum.Specified)
                            {
                                iAESEncryptionHelpers.Add(logicalName, new AESEncryptionHelper(lEncryptorOptions.SystemGenerate));
                            }
                            else
                            {
                                iAESEncryptionHelpers.Add(logicalName, new AESEncryptionHelper(lEncryptorOptions.PassPhrase, lEncryptorOptions.SaltValue,
                                    lEncryptorOptions.HashAlgorithm, lEncryptorOptions.InitVector, lEncryptorOptions.KeySize));
                            }
                        }
                        break;
                }
            }
        }

    }



    /// <summary>
    /// This class use SHA1 or MD5 Hashing for any form of hashing / signature hashes
    /// </summary>
    public class HashEncryptionHelper
    {
        #region Constructor

        SHA1 iSHA1;
        SHA256 iSHA256;
        SHA384 iSHA384;
        SHA512 iSHA512;
        MD5 iMD5;
        System.Text.Encoding iEncoding = System.Text.Encoding.UTF8;

        /// <summary>
        /// Default constructor -- Uses SHA1
        /// </summary>
        public HashEncryptionHelper()
            : base()
        {
            iSHA1 = SHA1Managed.Create();
        }

        /// <summary>
        /// Hash constructor. Specify the HashAlgorithm to use
        /// </summary>
        /// <param name="hashAlgorithm">HashAlgorithm to use: SHA1 or MD5</param>
        public HashEncryptionHelper(HashAlgorithmEnum hashAlgorithm, System.Text.Encoding encoding)
            : base()
        {
            iEncoding = encoding;

            switch (hashAlgorithm)
            {
                case HashAlgorithmEnum.SHA1:
                    iSHA1 = SHA1Managed.Create();
                    break;
                case HashAlgorithmEnum.SHA256:
                    iSHA256 = SHA256Managed.Create();
                    break;
                case HashAlgorithmEnum.SHA384:
                    iSHA384 = SHA384Managed.Create();
                    break;
                case HashAlgorithmEnum.SHA512:
                    iSHA512 = SHA512Managed.Create();
                    break;
                case HashAlgorithmEnum.MD5:
                    iMD5 = MD5CryptoServiceProvider.Create();
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Hash a string. 
        /// </summary>
        /// <param name="stringToHash">Ascii string to hash</param>
        /// <returns>Encrypted bytes</returns>
        public byte[] CreateHash(string stringToHash)
        {
            var stringToHashWithAsciiNewLines = stringToHash.Replace(Environment.NewLine, "\n");
            if (iSHA1 != null)
            {
                return iSHA1.ComputeHash(iEncoding.GetBytes(stringToHashWithAsciiNewLines));
            }
            if (iSHA256 != null)
            {
                return iSHA256.ComputeHash(iEncoding.GetBytes(stringToHashWithAsciiNewLines));
            }
            if (iSHA384 != null)
            {
                return iSHA384.ComputeHash(iEncoding.GetBytes(stringToHashWithAsciiNewLines));
            }
            if (iSHA512 != null)
            {
                return iSHA512.ComputeHash(iEncoding.GetBytes(stringToHashWithAsciiNewLines));
            }
            else
            {
                return iMD5.ComputeHash(iEncoding.GetBytes(stringToHashWithAsciiNewLines));
            }
        }

        /// <summary>
        /// Hash a string. 
        /// </summary>
        /// <param name="stringToHash">Ascii string to hash</param>
        /// <returns>Encrypted string in Base64 format</returns>
        public string CreateHashAsBase64(string stringToHash)
        {
            return Convert.ToBase64String(CreateHash(stringToHash));
        }
        /// <summary>
        /// Hash a string. 
        /// </summary>
        /// <param name="stringToHash">Ascii string to hash</param>
        /// <returns>Encrypted string in Base64 format</returns>
        public string CreateHashAsHex(string stringToHash)
        {
            return string.Join("", CreateHash(stringToHash).Select(c => ((int)c).ToString("X2")));
        }

        /// <summary>
        /// Hash a string. 
        /// </summary>
        /// <param name="stringToHash">Ascii string to hash</param>
        /// <returns>Encrypted string in Hex format</returns>
        public string CreateDashedHashAsHex(string stringToHash)
        {
            return System.Convert.ToHexString(CreateHash(stringToHash));
        }
        public string CreateHashAsString(string stringToHash)
        {
            return System.Text.ASCIIEncoding.ASCII.GetString(CreateHash(stringToHash));
        }

    }

    public class SaltedHash
    {
        public string Base64Hash { get; set; }
        public string Base64Salt { get; set; }

    }
    public static class SaltedHashHelper
    {
        public static SaltedHash CreateSaltedHash(string password)
        {
            string salt = GetRandomBase64Salt();
            string hash = ComputeHashAsBase64(salt, password);
            var saltedHash = new SaltedHash() { Base64Salt = salt, Base64Hash = hash };
            return saltedHash;
        }
        public static string GetRandomBase64Salt()
        {
            var saltBytes = new byte[32];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        public static string ComputeHashAsBase64(string base64Salt, string password, int iterations = 10)
        {
            var saltBytes = Convert.FromBase64String(base64Salt);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterations))
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
        }

        public static bool VerifyHash(string base64Salt, string base64Hash, string password, int iterations = 10)
        {
            return base64Hash == ComputeHashAsBase64(base64Salt, password, iterations);
        }
    }
}
