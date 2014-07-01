using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HotelSupervisorClientUpdater.Managers
{
    /// <summary>
    /// 提供加密与解密服务。
    /// </summary>
    internal class EncryptionManager
    {
        public static readonly string IV = global::HotelSupervisorClientUpdater.Properties.Resources.EncryptionIV;
        public static readonly string Key = global::HotelSupervisorClientUpdater.Properties.Resources.EncryptionKey;

        /// <summary>
        /// 获取加密服务类。
        /// </summary>
        /// <returns>加密服务类。</returns>
        public static TripleDESCryptoServiceProvider GetCryptoProvider()
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.IV = Convert.FromBase64String(IV);
            provider.Key = Convert.FromBase64String(Key);
            return provider;
        }

        /// <summary>
        /// 对字符串加密。
        /// </summary>
        /// <param name="text">需要加密的字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string Encrypt(string text)
        {
            TripleDESCryptoServiceProvider provider = GetCryptoProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] toEncrypt = new UTF8Encoding().GetBytes(text);
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();
            byte[] ret = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return Convert.ToBase64String(ret);
        }

        /// <summary>
        /// 对字符串解密。
        /// </summary>
        /// <param name="text">经过加密字符串。</param>
        /// <returns>解密后的字符串。</returns>
        public static string Decrypt(string text)
        {
            TripleDESCryptoServiceProvider provider = GetCryptoProvider();
            byte[] inputEquivalent = Convert.FromBase64String(text);
            MemoryStream msDecrypt = new MemoryStream();
            CryptoStream csDecrypt = new CryptoStream(msDecrypt,
            provider.CreateDecryptor(),
            CryptoStreamMode.Write);
            csDecrypt.Write(inputEquivalent, 0, inputEquivalent.Length);
            csDecrypt.FlushFinalBlock();
            csDecrypt.Close();
            return new UTF8Encoding().GetString(msDecrypt.ToArray());
        }

        public static string NormalEncryptOne(string text)
        {
            string str = text;
            str = str.Replace(@"\", @"<<?><?>>");
            str = str.Replace(@":", @"{{*}{*}}");
            str = str.Replace(@"_", @"{{?*}{?*}}");
            return str;
        }

        public static string NormalDecryptOne(string text)
        {
            string str = text;
            str = str.Replace(@"<<?><?>>", @"\");
            str = str.Replace(@"{{*}{*}}", @":");
            str = str.Replace(@"{{?*}{?*}}", @"_");
            return str;
        }

        public static string NormalEncryptTwo(string text)
        {
            string str = text;
            str = str.Replace(@"0", @"A>*>");
            str = str.Replace(@"1", @"c>*>");
            str = str.Replace(@"2", @"E>*>");
            str = str.Replace(@"3", @"g>*>");
            str = str.Replace(@"4", @"I>*>");
            str = str.Replace(@"5", @"k>*>");
            str = str.Replace(@"6", @"M>*>");
            str = str.Replace(@"7", @"o>*>");
            str = str.Replace(@"8", @"Q>*>");
            str = str.Replace(@"9", @"s>*>");
            return str;
        }

        public static string NormalDecryptTwo(string text)
        {
            string str = text;
            str = str.Replace(@"A>*>", @"0");
            str = str.Replace(@"c>*>", @"1");
            str = str.Replace(@"E>*>", @"2");
            str = str.Replace(@"g>*>", @"3");
            str = str.Replace(@"I>*>", @"4");
            str = str.Replace(@"k>*>", @"5");
            str = str.Replace(@"M>*>", @"6");
            str = str.Replace(@"o>*>", @"7");
            str = str.Replace(@"Q>*>", @"8");
            str = str.Replace(@"s>*>", @"9");
            return str;
        }

        /// <summary>
        /// 以单字节形式对字符串进行倒序排列。
        /// </summary>
        /// <param name="text">需要排列的字符串。</param>
        /// <returns>排列后的字符串。</returns>
        public static string ReverseString(string text)
        {
            byte[] bytesOld = Encoding.Default.GetBytes(text);
            byte[] bytesNew = new byte[bytesOld.Length];
            int i = 0;
            for (int j = bytesOld.Length - 1; j >= 0; j--)
            {
                bytesNew[j] = bytesOld[i];
                i++;
            }
            return Encoding.Default.GetString(bytesNew);
        }
    }
}
