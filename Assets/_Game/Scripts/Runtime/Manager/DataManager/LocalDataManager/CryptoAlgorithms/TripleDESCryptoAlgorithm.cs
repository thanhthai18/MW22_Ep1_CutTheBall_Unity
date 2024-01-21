using System;
using System.Text;
using System.Security.Cryptography;

namespace Runtime.Manager.Data
{
    public static class TripleDESCryptoAlgorithm
    {
        #region Members

        private static readonly string s_key = "PixelHero";

        #endregion Members

        #region Class Methods

        /// <summary>
        /// Encrypt the input string and return the corresponding encrypted string of it.
        /// </summary>
        public static string Encrypt(string inputString)
        {
            byte[] data = Encoding.UTF8.GetBytes(inputString);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] key = md5.ComputeHash(Encoding.UTF8.GetBytes(s_key));
                using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform tr = trip.CreateEncryptor();
                    byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        /// <summary>
        /// Decrypt the input string and return the corresponding decrypted string of it.
        /// </summary>
        public static string Decrypt(string inputString)
        {
            byte[] data = Convert.FromBase64String(inputString);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] key = md5.ComputeHash(Encoding.UTF8.GetBytes(s_key));
                using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform tr = trip.CreateDecryptor();
                    byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(results);
                }
            }
        }

        #endregion Class Methods
    }
}