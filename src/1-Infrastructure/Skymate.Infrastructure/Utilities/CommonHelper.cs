// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonHelper.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Web.Hosting;

    using Skymate.Extensions;

    using Skymate;

    /// <summary>
    /// The common helper.
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// Gets a value indicating whether is dev environment.
        /// </summary>
        public static bool IsDevEnvironment
        {
            get
            {
                if (!HostingEnvironment.IsHosted)
                {
                    return true;
                }

                if (HostingEnvironment.IsDevelopmentEnvironment)
                {
                    return true;
                }

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    return true;
                }

                return FindSolutionRoot(HostingEnvironment.MapPath("~/")) != null;
            }
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">
        /// Length
        /// </param>
        /// <returns>
        /// Result string
        /// </returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (var i = 0; i < length; i++)
            {
                str = string.Concat(str, random.Next(10).ToString(CultureInfo.InvariantCulture));
            }

            return str;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">
        /// Minimum number
        /// </param>
        /// <param name="max">
        /// Maximum number
        /// </param>
        /// <returns>
        /// Result
        /// </returns>
        public static int GenerateRandomInteger(int min = 0, int max = 2147483647)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">
        /// The path to map. E.g. "~/bin"
        /// </param>
        /// <param name="findAppRoot">
        /// Specifies if the app root should be resolved when mapped directory does not exist
        /// </param>
        /// <returns>
        /// The physical path. E.g. "c:\inetpub\wwwroot\bin"
        /// </returns>
        /// <remarks>
        /// This method is able to resolve the web application root
        /// even when it's called during design-time (e.g. from EF design-time tools).
        /// </remarks>
        public static string MapPath(string path, bool findAppRoot = true)
        {
            string path1 = path;
            Guard.ArgumentNotNull(() => path1);

            if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }
            
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", string.Empty).TrimStart('/').Replace('/', '\\');

            var testPath = Path.Combine(baseDirectory, path);

            if (!findAppRoot)
            {
                return testPath;
            }

            var dir = FindSolutionRoot(baseDirectory);

            if (dir == null)
            {
                return testPath;
            }

            baseDirectory = Path.Combine(dir.FullName, "Presentation\\SmartStore.Web");
            testPath = Path.Combine(baseDirectory, path);

            return testPath;
        }

        /// <summary>
        /// The try convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="convertedValue">
        /// The converted value.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryConvert<T>(object value, out T convertedValue)
        {
            return TryConvert(value, CultureInfo.InvariantCulture, out convertedValue);
        }

        /// <summary>
        /// The try convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <param name="convertedValue">
        /// The converted value.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryConvert<T>(object value, CultureInfo culture, out T convertedValue)
        {
            return Misc.TryAction(() => value.Convert<T>(culture), out convertedValue);
        }

        /// <summary>
        /// The try convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="convertedValue">
        /// The converted value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryConvert(object value, Type to, out object convertedValue)
        {
            return TryConvert(value, to, CultureInfo.InvariantCulture, out convertedValue);
        }

        /// <summary>
        /// The try convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <param name="convertedValue">
        /// The converted value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryConvert(object value, Type to, CultureInfo culture, out object convertedValue)
        {
            return Misc.TryAction(() => value.Convert(to, culture), out convertedValue);
        }

        /// <summary>
        /// The get type converter.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="TypeConverter"/>.
        /// </returns>
        public static TypeConverter GetTypeConverter(Type type)
        {
            return ConversionExtensions.GetTypeConverter(type);
        }

        /// <summary>
        /// Gets a setting from the application's <c>web.config</c> <c>appSettings</c> node
        /// </summary>
        /// <typeparam name="T">
        /// The type to convert the setting value to
        /// </typeparam>
        /// <param name="key">
        /// The key of the setting
        /// </param>
        /// <param name="defValue">
        /// The default value to return if the setting does not exist
        /// </param>
        /// <returns>
        /// The casted setting value
        /// </returns>
        public static T GetAppSetting<T>(string key, T defValue = default(T))
        {
            Guard.ArgumentNotEmpty(() => key);

            var setting = ConfigurationManager.AppSettings[key];

            return setting == null ? defValue : setting.Convert<T>();
        }

        public static string GetTimestamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The find solution root.
        /// </summary>
        /// <param name="currentDir">
        /// The current dir.
        /// </param>
        /// <returns>
        /// The <see cref="DirectoryInfo"/>.
        /// </returns>
        private static DirectoryInfo FindSolutionRoot(string currentDir)
        {
            var dir = Directory.GetParent(currentDir);
            while (true)
            {
                if (dir == null || IsSolutionRoot(dir))
                {
                    break;
                }

                dir = dir.Parent;
            }

            return dir;
        }

        /// <summary>
        /// The is solution root.
        /// </summary>
        /// <param name="dir">
        /// The dir.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsSolutionRoot(FileSystemInfo dir)
        {
            return File.Exists(Path.Combine(dir.FullName, "SmartStoreNET.sln"));
        }
    }
}