using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DXC.Technology.Encryption;
using DXC.Technology.Utilities;

namespace DXC.Technology.BlockChains
{
    public static class BlockChainHashingHelper
    {
    #region Static Fields

    /// <summary>
    /// Identity of the blockchain owner.
    /// </summary>
    public const string IdentityBlockChainOwner = "SYS";

    /// <summary>
    /// Identity code for the blockchain.
    /// </summary>
    public const string IdentityBlockChainCode = "IDS";

    /// <summary>
    /// Identity currency for the blockchain.
    /// </summary>
    public const string IdentityBlockChainCurrency = "IDSN";

    /// <summary>
    /// Hashing algorithm used.
    /// </summary>
    public static readonly HashAlgorithmEnum hashingAlgorithm = HashAlgorithmEnum.SHA256;

    /// <summary>
    /// Encoding used for blockchain.
    /// </summary>
    public static readonly Encoding blockChainEncoding = Encoding.ASCII;

    #endregion

    #region Public Static Properties

    /// <summary>
    /// Technology public key.
    /// </summary>
    public static string technologyPublicKey { get; set; }

    /// <summary>
    /// Technology private key.
    /// </summary>
    public static string technologyPrivateKey { get; set; }

    /// <summary>
    /// Hash for an empty string.
    /// </summary>
    public static string EmptyStringHash => "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855";

    /// <summary>
    /// Example symmetric key.
    /// </summary>
    public static string ExampleSymmetricKey => EncryptionHelper.Current.GetAESEncryptionHelper("SXMPL").KeyAsXMLString();

    /// <summary>
    /// Example public key pair.
    /// </summary>
    public static string ExamplePublicKeyPair => EncryptionHelper.Current.GetRSAEncryptionHelper("AXMPL").PublicKeyAsXMLString();

    /// <summary>
    /// Example private key pair.
    /// </summary>
    public static string ExamplePrivateKeyPair => EncryptionHelper.Current.GetRSAEncryptionHelper("AXMPL").PrivateKeyAsXMLString();

    #endregion

    #region Public Static Methods

    /// <summary>
    /// Creates a hexadecimal hash for a given string.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <returns>A hexadecimal hash of the input string.</returns>
    public static string CreateHexHashForString(string input)
    {
        return new HashEncryptionHelper(hashingAlgorithm, Encoding.ASCII).CreateHashAsHex(input);
    }

    /// <summary>
    /// Creates a Base64 hash for a given string.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <returns>A Base64 hash of the input string.</returns>
    public static string CreateBase64HashForString(string input)
    {
        return new HashEncryptionHelper(hashingAlgorithm, Encoding.ASCII).CreateHashAsBase64(input);
    }

    /// <summary>
    /// Reverses a string and appends its length.
    /// </summary>
    /// <param name="input">The input string to reverse.</param>
    /// <returns>The reversed string with its length appended.</returns>
    public static string ReversedStringWithAddedLength(string input)
    {
        if (string.IsNullOrEmpty(input)) return "0";
        return new string(input.Reverse().ToArray()) + input.Length;
    }

    /// <summary>
    /// Creates a Base64 hash for a reversed string with added length.
    /// </summary>
    /// <param name="input">The input string to process.</param>
    /// <returns>A Base64 hash of the processed string.</returns>
    public static string CreateBase64HashForReversedStringWithAddedLength(string input)
    {
        return CreateBase64HashForString(ReversedStringWithAddedLength(input));
    }

    /// <summary>
    /// Encrypts input using a symmetric key pair.
    /// </summary>
    /// <param name="input">The input string to encrypt.</param>
    /// <param name="symmetricKeyPair">The symmetric key pair used for encryption.</param>
    /// <returns>The encrypted input as a Base64 string.</returns>
    public static string EncryptInputFromSymmetricKeyPair(string input, string symmetricKeyPair)
    {
        var aes = new AESEncryptionHelper(symmetricKeyPair);
        return aes.EncryptAsBase64(input);
    }

    /// <summary>
    /// Decrypts input using a symmetric key pair.
    /// </summary>
    /// <param name="base64EncryptedInput">The encrypted input as a Base64 string.</param>
    /// <param name="symmetricKeyPair">The symmetric key pair used for decryption.</param>
    /// <returns>The decrypted input string.</returns>
    public static string DecryptInputFromSymmetricKeyPair(string base64EncryptedInput, string symmetricKeyPair)
    {
        var aes = new AESEncryptionHelper(symmetricKeyPair);
        return aes.DecryptFromBase64(base64EncryptedInput);
    }

    /// <summary>
    /// Decrypts input using a private key pair.
    /// </summary>
    /// <param name="base64EncryptedInput">The encrypted input as a Base64 string.</param>
    /// <param name="privateKeyPair">The private key pair used for decryption.</param>
    /// <returns>The decrypted input string.</returns>
    public static string DecryptInputFromPrivateKeyPair(string base64EncryptedInput, string privateKeyPair)
    {
        var rsa = new RSAEncryptionHelper(privateKeyPair);
        return rsa.DecryptFromBase64(base64EncryptedInput);
    }

    /// <summary>
    /// Decrypts input using the technology private key pair.
    /// </summary>
    /// <param name="base64EncryptedInput">The encrypted input as a Base64 string.</param>
    /// <returns>The decrypted input string.</returns>
    public static string DecryptInputFromTechnologyPrivateKeyPair(string base64EncryptedInput)
    {
        var rsa = new RSAEncryptionHelper(technologyPrivateKey);
        return rsa.DecryptFromBase64(base64EncryptedInput);
    }

    /// <summary>
    /// Checks if a public key is valid.
    /// </summary>
    /// <param name="publicKeyPair">The public key pair to validate.</param>
    /// <returns>True if the public key is valid; otherwise, false.</returns>
    public static bool IsValidPublicKey(string publicKeyPair)
    {
        try
        {
            var rsa = new RSAEncryptionHelper(publicKeyPair);
            return rsa.IsPublicKeyOnly();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Encrypts input using a public key pair.
    /// </summary>
    /// <param name="input">The input string to encrypt.</param>
    /// <param name="publicKeyPair">The public key pair used for encryption.</param>
    /// <returns>The encrypted input as a Base64 string.</returns>
    public static string EncryptInputFromPublicKeyPair(string input, string publicKeyPair)
    {
        var rsa = new RSAEncryptionHelper(publicKeyPair);
        return rsa.EncryptAsBase64(input);
    }

    /// <summary>
    /// Signs input using a private key pair.
    /// </summary>
    /// <param name="input">The input string to sign.</param>
    /// <param name="privateKeyPair">The private key pair used for signing.</param>
    /// <returns>The signed hash as a Base64 string.</returns>
    public static string SignInputFromPrivateKeyPair(string input, string privateKeyPair)
    {
        var rsa = new RSAEncryptionHelper(privateKeyPair);
        return rsa.HashAndSign(input, hashingAlgorithm);
    }

    /// <summary>
    /// Signs input using the technology private key pair.
    /// </summary>
    /// <param name="input">The input string to sign.</param>
    /// <returns>The signed hash as a Base64 string.</returns>
    public static string SignInputFromTechnologyPrivateKeyPair(string input)
    {
        var rsa = new RSAEncryptionHelper(technologyPrivateKey);
        return rsa.HashAndSign(input, hashingAlgorithm);
    }

    /// <summary>
    /// Verifies input using a public key pair.
    /// </summary>
    /// <param name="base64EncodedSignedHash">The signed hash as a Base64 string.</param>
    /// <param name="originalString">The original string to verify against.</param>
    /// <param name="publicKeyPair">The public key pair used for verification.</param>
    /// <returns>True if the verification is successful; otherwise, false.</returns>
    public static bool VerifyInputFromPublicKeyPair(string base64EncodedSignedHash, string originalString, string publicKeyPair)
    {
        var rsa = new RSAEncryptionHelper(publicKeyPair);
        return rsa.IsSignedHashValid(originalString, base64EncodedSignedHash, hashingAlgorithm);
    }

    /// <summary>
    /// Verifies input using the technology public key pair.
    /// </summary>
    /// <param name="base64EncodedSignedHash">The signed hash as a Base64 string.</param>
    /// <param name="originalString">The original string to verify against.</param>
    /// <returns>True if the verification is successful; otherwise, false.</returns>
    public static bool VerifyInputFromPublicKeyPair(string base64EncodedSignedHash, string originalString)
    {
        var rsa = new RSAEncryptionHelper(technologyPublicKey);
        return rsa.IsSignedHashValid(originalString, base64EncodedSignedHash, hashingAlgorithm);
    }

    /// <summary>
    /// Converts an amount to a string for hashing.
    /// </summary>
    /// <param name="amount">The amount to convert.</param>
    /// <returns>The amount as a formatted string.</returns>
    public static string AmountAsStringForHash(decimal amount)
    {
        if (amount.Equals(0)) return "0";
        return amount.ToString("N9", DecimalFormatProvider.Default);
    }

    /// <summary>
    /// Creates a random RSA crypto service provider.
    /// </summary>
    /// <returns>A new instance of RSACryptoServiceProvider.</returns>
    public static RSACryptoServiceProvider CreateRandomRsaCryptoService()
    {
        return new RSACryptoServiceProvider(2048);
    }

    /// <summary>
    /// Generates a random blockchain party address.
    /// </summary>
    /// <returns>A random blockchain party address.</returns>
    public static string GetRandomBlockChainPartyAddress()
    {
        return Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Generates a random blockchain wallet address.
    /// </summary>
    /// <returns>A random blockchain wallet address.</returns>
    public static string GetRandomBlockChainWalletAddress()
    {
        return Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Generates a symmetric key.
    /// </summary>
    /// <returns>A symmetric key as an XML string.</returns>
    public static string GenerateSymmetricKey()
    {
        return new AESEncryptionHelper(SystemGenerateEnum.RandomKey).KeyAsXMLString();
    }

    #endregion
}

    public enum BlockChainTransactionType
    {
        NONE_Unspecified,
        PC_PartyCreation,
        WC_WalletCreation,
        PWC_PartyWalletCreation,
        BCS_StandardBlockChain,
        BCA_BlockChainForAssetsOnly,
        BCC_BlockChainForCurrencyOnly,
        BCI_BlockChainForInformationRegistrationOnly,
        WIB_WalletInitialBalance,
        WIA_WalletInitialAsset,
        PUTX_RegisterPublicText,
        PRTX_RegisterPrivateText,
        RTR_RegisterTableRecord,
        TRC_TransferCurrency,
        TRA_TransferAsset
    }
    public enum BlockChainType
    {
        NONE_Unspecidied,
        SYS_InitialBlockChainForPayments,
        STD_StandardBlockChain,
        ASSET_BlockChainForAssetsOnly,
        CUR_BlockChainForCurrencyTransfersOnly,
        INFO_BlockChainForInformationRegistrationOnly
    }
}