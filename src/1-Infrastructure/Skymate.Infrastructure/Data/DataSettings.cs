// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSettings.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Skymate;
    using Skymate.Extensions;
    using Skymate.Utilities;
    using Skymate.Utilities.Threading;

    /// <summary>
    /// The data settings.
    /// </summary>
    public class DataSettings
    {
        /// <summary>
        /// The filename.
        /// </summary>
        protected const string FileName = "Settings.txt";

        /// <summary>
        /// The separator.
        /// </summary>
        protected const char Separator = ':';

        /// <summary>
        /// The s_rw lock.
        /// </summary>
        private static readonly ReaderWriterLockSlim ReaderWriterLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The current.
        /// </summary>
        private static DataSettings current;

        /// <summary>
        /// The s_settings factory.
        /// </summary>
        private static Func<DataSettings> settingsFactory = () => new DataSettings();

        /// <summary>
        /// The installed.
        /// </summary>
        private static bool? installed;

        /// <summary>
        /// The s_ test mode.
        /// </summary>
        private static bool testMode;

        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="DataSettings"/> class from being created.
        /// </summary>
        private DataSettings()
        {
            this.RawDataSettings = new Dictionary<string, string>();
        }

        #endregion Ctor

        /// <summary>
        /// Gets the current.
        /// </summary>
        public static DataSettings Current
        {
            get
            {
                using (ReaderWriterLock.GetUpgradeableReadLock())
                {
                    if (current != null)
                    {
                        return current;
                    }

                    using (ReaderWriterLock.GetWriteLock())
                    {
                        if (current != null)
                        {
                            return current;
                        }

                        current = settingsFactory();
                        current.Load();
                    }
                }

                return current;
            }
        }

        /// <summary>
        /// Gets or sets the app version.
        /// </summary>
        public Version AppVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data provider.
        /// </summary>
        public string DataProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the provider invariant name.
        /// </summary>
        public string ProviderInvariantName
        {
            get
            {
                if (this.DataProvider.HasValue() && this.DataProvider.IsCaseInsensitiveEqual("sqlserver"))
                {
                    return "System.Data.SqlClient";
                }

                // SqlCe should always be the default provider
                return "System.Data.SqlServerCe.4.0";
            }
        }

        /// <summary>
        /// Gets the provider friendly name.
        /// </summary>
        public string ProviderFriendlyName
        {
            get
            {
                if (this.DataProvider.HasValue() && this.DataProvider.IsCaseInsensitiveEqual("sqlserver"))
                {
                    return "SQL Server";
                }

                // SqlCe should always be the default provider
                return "SQL Server Compact (SQL CE)";
            }
        }

        /// <summary>
        /// Gets a value indicating whether is sql server.
        /// </summary>
        public bool IsSqlServer
        {
            get
            {
                return this.DataProvider.HasValue() && this.DataProvider.IsCaseInsensitiveEqual("sqlserver");
            }
        }

        /// <summary>
        /// Gets or sets the data connection string.
        /// </summary>
        public string DataConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the raw data settings.
        /// </summary>
        public IDictionary<string, string> RawDataSettings
        {
            get;
            private set;
        }

        /// <summary>
        /// The set default factory.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        public static void SetDefaultFactory(Func<DataSettings> factory)
        {
            Guard.ArgumentNotNull(() => factory);

            lock (ReaderWriterLock.GetWriteLock())
            {
                settingsFactory = factory;
            }
        }

        /// <summary>
        /// The database is installed.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool DatabaseIsInstalled()
        {
            if (testMode)
            {
                return false;
            }

            if (!installed.HasValue)
            {
                installed = Current.IsValid();
            }

            return installed.Value;
        }

        /// <summary>
        /// The reload.
        /// </summary>
        public static void Reload()
        {
            using (ReaderWriterLock.GetWriteLock())
            {
                current = null;
                installed = null;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        public static void Delete()
        {
            using (ReaderWriterLock.GetWriteLock())
            {
                string filePath = Path.Combine(CommonHelper.MapPath("~/App_Data/"), FileName);
                File.Delete(filePath);
                current = null;
                installed = null;
            }
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsValid()
        {
            return this.DataProvider.HasValue() && this.DataConnectionString.HasValue();
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Load()
        {
            using (ReaderWriterLock.GetWriteLock())
            {
                string filePath = Path.Combine(CommonHelper.MapPath("~/App_Data/"), FileName);

                this.Reset();

                if (File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath);
                    var settings = this.ParseSettings(text);
                    if (settings.Any())
                    {
                        this.RawDataSettings.AddRange(settings);
                        if (settings.ContainsKey("AppVersion"))
                        {
                            this.AppVersion = new Version(settings["AppVersion"]);
                        }

                        if (settings.ContainsKey("DataProvider"))
                        {
                            this.DataProvider = settings["DataProvider"];
                        }

                        if (settings.ContainsKey("DataConnectionString"))
                        {
                            this.DataConnectionString = settings["DataConnectionString"];
                        }

                        return this.IsValid();
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// The reset.
        /// </summary>
        public void Reset()
        {
            using (ReaderWriterLock.GetWriteLock())
            {
                this.RawDataSettings.Clear();
                this.AppVersion = null;
                this.DataProvider = null;
                this.DataConnectionString = null;
                installed = null;
            }
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Save()
        {
            if (!this.IsValid())
            {
                return false;
            }

            using (ReaderWriterLock.GetWriteLock())
            {
                var filePath = Path.Combine(CommonHelper.MapPath("~/App_Data/"), FileName);
                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath))
                    {
                        // we use 'using' to close the file after it's created
                    }
                }

                var text = this.SerializeSettings();
                File.WriteAllText(filePath, text);

                return true;
            }
        }

        /// <summary>
        /// The set test mode.
        /// </summary>
        /// <param name="isTestMode">
        /// The is test mode.
        /// </param>
        internal static void SetTestMode(bool isTestMode)
        {
            testMode = isTestMode;
        }

        /// <summary>
        /// The parse settings.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IDictionary</cref>
        ///     </see>
        ///     .
        /// </returns>
        protected virtual IDictionary<string, string> ParseSettings(string text)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (text.IsEmpty())
            {
                return result;
            }

            var settings = new List<string>();
            using (var reader = new StringReader(text))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    settings.Add(str);
                }
            }

            foreach (var setting in settings)
            {
                var separatorIndex = setting.IndexOf(Separator);
                if (separatorIndex == -1)
                {
                    continue;
                }

                var key = setting.Substring(0, separatorIndex).Trim();
                var value = setting.Substring(separatorIndex + 1).Trim();

                if (key.HasValue() && value.HasValue())
                {
                    result.Add(key, value);
                }
            }

            return result;
        }

        /// <summary>
        /// The serialize settings.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected virtual string SerializeSettings()
        {
            return string.Format(
                "AppVersion: {0}{3}DataProvider: {1}{3}DataConnectionString: {2}{3}",
                this.AppVersion,
                this.DataProvider,
                this.DataConnectionString,
                Environment.NewLine);
        }
    }
}