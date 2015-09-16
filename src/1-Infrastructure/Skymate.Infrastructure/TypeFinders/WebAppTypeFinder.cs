// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebAppTypeFinder.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   Provides information about types in the current web application.
//   Optionally this class can look at all assemblies in the bin folder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.TypeFinders
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;

    using Skymate.Utilities;

    /// <summary>
    /// Provides information about types in the current web application.
    /// Optionally this class can look at all assemblies in the bin folder.
    /// </summary>
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        #region Fields

        /// <summary>
        /// The ensure bin folder assemblies loaded.
        /// </summary>
        private bool ensureBinFolderAssembliesLoaded = true;

        /// <summary>
        /// The bin folder assemblies loaded.
        /// </summary>
        private bool binFolderAssembliesLoaded;

        #endregion Fields

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="WebAppTypeFinder"/> class.
        /// </summary>
        public WebAppTypeFinder()
        {
            this.ensureBinFolderAssembliesLoaded = CommonHelper.GetAppSetting("skymate:EnableDynamicDiscovery", true);
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether ensure bin folder assemblies loaded.
        /// </summary>
        public bool EnsureBinFolderAssembliesLoaded
        {
            get { return this.ensureBinFolderAssembliesLoaded; }
            set { this.ensureBinFolderAssembliesLoaded = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets a physical disk path of \Bin directory
        /// </summary>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string GetBinDirectory()
        {
            if (HostingEnvironment.IsHosted)
            {
                return HttpRuntime.BinDirectory;
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// The get assemblies.
        /// </summary>
        /// <param name="ignoreInactivePlugins">
        /// The ignore inactive plugins.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IList</cref>
        ///     </see>
        ///     .
        /// </returns>
        public override IList<Assembly> GetAssemblies(bool ignoreInactivePlugins = false)
        {
            if (!this.EnsureBinFolderAssembliesLoaded || this.binFolderAssembliesLoaded)
            {
                return base.GetAssemblies(ignoreInactivePlugins);
            }

            this.binFolderAssembliesLoaded = true;
            string binPath = this.GetBinDirectory();

            this.LoadMatchingAssemblies(binPath);

            return base.GetAssemblies(ignoreInactivePlugins);
        }

        #endregion Methods
    }
}