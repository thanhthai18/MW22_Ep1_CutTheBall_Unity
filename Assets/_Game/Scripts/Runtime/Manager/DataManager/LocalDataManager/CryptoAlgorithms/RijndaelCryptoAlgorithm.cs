using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Runtime.Manager.Data
{
    public static class RijndaelCryptoAlgorithm
    {
        #region Members

        private static readonly byte[] s_bIV = { 0x20, 0x07, 0xE4, 0xC9, 0xD1, 0x3C, 0xA5, 0x16,0xBB, 0x09, 0x17, 0x3B, 0x55, 0x26, 0x71, 0xCA };
        private static readonly string s_cryptoKey = "Y0urCrypt0KeyH3r3L3tt3rAndNumb3rsBeCr3ative=";

        #endregion Members

        #region Class Methods

        /// <summary>
        /// Encrypt the input string and return the corresponding encrypted string of it.
        /// </summary>
        public static string Encrypt(string text)
        {
            byte[] keyBytes = Convert.FromBase64String(s_cryptoKey);
            byte[] textBytes = new UTF8Encoding().GetBytes(text);
            Rijndael rijndael = new RijndaelManaged();
            rijndael.KeySize = 256;
            MemoryStream mStream = new MemoryStream();
            CryptoStream encryptor = new CryptoStream(mStream, rijndael.CreateEncryptor(keyBytes, s_bIV), CryptoStreamMode.Write);
            encryptor.Write(textBytes, 0, textBytes.Length);
            encryptor.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// Decrypt the input string and return the corresponding decrypted string of it.
        /// </summary>
        public static string Decrypt(string text)
        {
            byte[] keyBytes = Convert.FromBase64String(s_cryptoKey);
            byte[] textBytes = Convert.FromBase64String(text);
            Rijndael rijndael = new RijndaelManaged();
            rijndael.KeySize = 256;
            MemoryStream mStream = new MemoryStream();
            CryptoStream decryptor = new CryptoStream(mStream, rijndael.CreateDecryptor(keyBytes, s_bIV), CryptoStreamMode.Write);
            decryptor.Write(textBytes, 0, textBytes.Length);
            decryptor.FlushFinalBlock();
            UTF8Encoding utf8 = new UTF8Encoding();
            return utf8.GetString(mStream.ToArray());
        }

        #endregion Class Methods
    }
}