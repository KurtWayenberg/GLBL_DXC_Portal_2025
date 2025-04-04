using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Encryption
{
    public class PemAsn1Reader
    {
        #region Public Static Methods

        /// <summary>
        /// Retrieves the private key from the provided PEM-encoded private key lines.
        /// </summary>
        /// <param name="privateKeyLines">An array of strings representing the PEM-encoded private key lines.</param>
        public static RSACryptoServiceProvider GetPrivateKey(string[] privateKeyLines)
        {
            var keyString = string.Join("", privateKeyLines.Skip(1).Take(privateKeyLines.Length - 2));
            return Asn1Reader.GetPrivateKey(keyString);
        }

        /// <summary>
        /// Retrieves the public key from the provided PEM-encoded public key lines.
        /// </summary>
        /// <param name="publicKeyLines">An array of strings representing the PEM-encoded public key lines.</param>
        public static RSACryptoServiceProvider GetPublicKey(string[] publicKeyLines)
        {
            var keyString = string.Join("", publicKeyLines.Skip(1).Take(publicKeyLines.Length - 2));
            return Asn1Reader.GetPublicKey(keyString);
        }

        #endregion
    }

    public class Asn1Reader
    {
        #region Private Static Methods

        /// <summary>
        /// Displays the bytes of the provided data with the given information.
        /// </summary>
        /// <param name="info">A string containing information about the data.</param>
        /// <param name="data">A byte array containing the data to display.</param>
        private static void ShowBytes(string info, byte[] data)
        {
            Console.WriteLine("{0}  [{1} bytes]", info, data.Length);
            for (int i = 1; i <= data.Length; i++)
            {
                Console.Write("{0:X2}  ", data[i - 1]);
                if (i % 16 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// Retrieves the size of the integer from the binary reader.
        /// </summary>
        /// <param name="binaryReader">A BinaryReader instance to read the integer size from.</param>
        private static int GetIntegerSize(BinaryReader binaryReader)
        {
            byte bt = 0;
            byte lowByte = 0x00;
            byte highByte = 0x00;
            int count = 0;
            bt = binaryReader.ReadByte();
            if (bt != 0x02)     //expect integer
                return 0;
            bt = binaryReader.ReadByte();

            if (bt == 0x81)
                count = binaryReader.ReadByte();    // data size in next byte
            else if (bt == 0x82)
            {
                highByte = binaryReader.ReadByte(); // data size in next 2 bytes
                lowByte = binaryReader.ReadByte();
                byte[] modInt = { lowByte, highByte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modInt, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binaryReader.ReadByte() == 0x00)
            {   //remove high order zeros in data
                count -= 1;
            }
            binaryReader.BaseStream.Seek(-1, SeekOrigin.Current);     //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Retrieves the private key from the provided Base64-encoded private key string.
        /// </summary>
        /// <param name="privateKey">A Base64-encoded string representing the private key.</param>
        public static RSACryptoServiceProvider GetPrivateKey(string privateKey)
        {
            byte[] privateKeyBytes = Convert.FromBase64String(privateKey);
            byte[] modulus, exponent, d, p, q, dp, dq, iq;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            using MemoryStream memoryStream = new(privateKeyBytes);
            using BinaryReader binaryReader = new(memoryStream);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twoBytes = 0;
            int elements = 0;
            try
            {
                twoBytes = binaryReader.ReadUInt16();
                if (twoBytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binaryReader.ReadByte();    //advance 1 byte
                else if (twoBytes == 0x8230)
                    binaryReader.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twoBytes = binaryReader.ReadUInt16();
                if (twoBytes != 0x0102) //version number
                    return null;
                bt = binaryReader.ReadByte();
                if (bt != 0x00)
                    return null;

                //------ all private key components are Integer sequences ----
                elements = GetIntegerSize(binaryReader);
                modulus = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                exponent = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                d = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                p = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                q = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                dp = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                dq = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                iq = binaryReader.ReadBytes(elements);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters cspParameters = new() { Flags = CspProviderFlags.UseMachineKeyStore };
                RSACryptoServiceProvider rsa = new(1024, cspParameters);

                RSAParameters rsaParams = new()
                {
                    Modulus = modulus,
                    Exponent = exponent,
                    D = d,
                    P = p,
                    Q = q,
                    DP = dp,
                    DQ = dq,
                    InverseQ = iq
                };
                rsa.ImportParameters(rsaParams);
                return rsa;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the public key from the provided Base64-encoded public key string.
        /// </summary>
        /// <param name="publicKeyString">A Base64-encoded string representing the public key.</param>
        public static RSACryptoServiceProvider GetPublicKey(string publicKeyString)
        {
            // encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] x509Key;
            byte[] seq = new byte[15];
            int x509Size;

            x509Key = Convert.FromBase64String(publicKeyString);
            x509Size = x509Key.Length;

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using MemoryStream memoryStream = new(x509Key);
            using BinaryReader binaryReader = new(memoryStream);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twoBytes = 0;

            try
            {
                twoBytes = binaryReader.ReadUInt16();
                if (twoBytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binaryReader.ReadByte();    //advance 1 byte
                else if (twoBytes == 0x8230)
                    binaryReader.ReadInt16();   //advance 2 bytes
                else
                    return null;

                seq = binaryReader.ReadBytes(15);       //read the Sequence OID

                twoBytes = binaryReader.ReadUInt16();
                if (twoBytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                    binaryReader.ReadByte();    //advance 1 byte
                else if (twoBytes == 0x8203)
                    binaryReader.ReadInt16();   //advance 2 bytes
                else
                    return null;

                bt = binaryReader.ReadByte();
                if (bt != 0x00)     //expect null byte next
                    return null;

                twoBytes = binaryReader.ReadUInt16();
                if (twoBytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binaryReader.ReadByte();    //advance 1 byte
                else if (twoBytes == 0x8230)
                    binaryReader.ReadInt16();   //advance 2 bytes
                else
                    return null;

                twoBytes = binaryReader.ReadUInt16();
                byte lowByte = 0x00;
                byte highByte = 0x00;

                if (twoBytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                    lowByte = binaryReader.ReadByte();  // read next bytes which is bytes in modulus
                else if (twoBytes == 0x8202)
                {
                    highByte = binaryReader.ReadByte(); //advance 2 bytes
                    lowByte = binaryReader.ReadByte();
                }
                else
                    return null;
                byte[] modInt = { lowByte, highByte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modSize = BitConverter.ToInt32(modInt, 0);

                int firstByte = binaryReader.PeekChar();
                if (firstByte == 0x00)
                {   //if first byte (highest order) of modulus is zero, don't include it
                    binaryReader.ReadByte();    //skip this null byte
                    modSize -= 1;   //reduce modulus buffer size by 1
                }

                byte[] modulus = binaryReader.ReadBytes(modSize);   //read the modulus bytes

                if (binaryReader.ReadByte() != 0x02)            //expect an Integer for the exponent data
                    return null;
                int expBytes = binaryReader.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                byte[] exponent = binaryReader.ReadBytes(expBytes);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider rsa = new();
                RSAParameters rsaKeyInfo = new()
                {
                    Modulus = modulus,
                    Exponent = exponent
                };
                rsa.ImportParameters(rsaKeyInfo);

                return rsa;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}