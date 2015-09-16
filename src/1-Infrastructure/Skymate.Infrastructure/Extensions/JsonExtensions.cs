/*
// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Skymate" file="JsonExtensions.cs">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Zhaord.Extensions
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    /// <summary>
    /// The json extensions.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// The get dynamic json object.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<dynamic> GetDynamicJsonObject(this Uri uri)
        {
            using (var wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E)";
                var response = await wc.DownloadStringTaskAsync(uri);
                return JsonConvert.DeserializeObject(response);
            }
        }
    }
}

*/
