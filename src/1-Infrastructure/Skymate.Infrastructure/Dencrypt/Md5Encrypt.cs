// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Md5Encrypt.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Dencrypt
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// MD5加密.
    /// </summary>
    public static class Md5Encrypt
    {
        /// <summary>
        /// 加密.
        /// </summary>
        /// <param name="source">
        /// 明文.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Encrypt(string source)
        {
            string result;

            using (var md5 = new MD5CryptoServiceProvider())
            {
                var bytes = Encoding.UTF8.GetBytes(source);
                var resultEncrypt = md5.ComputeHash(bytes);
                md5.Clear();

                result = BitConverter.ToString(resultEncrypt);
                result = result.Replace("-", string.Empty);
            }

            return result;
        }

        /// <summary>
        /// 验证md5字符串
        /// </summary>
        /// <param name="source">
        /// 明文
        /// </param>
        /// <param name="md5String">
        /// 密文
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool VerifyMd5Hash(string source, string md5String)
        {
            var hasinput = Encrypt(source);

            // 不区分大小写
            var compaper = StringComparer.OrdinalIgnoreCase;
            return 0 == compaper.Compare(hasinput, md5String);
        }
    }
}
