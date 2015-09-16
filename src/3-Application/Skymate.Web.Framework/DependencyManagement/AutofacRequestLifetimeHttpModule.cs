// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutofacRequestLifetimeHttpModule.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   An <see cref="IHttpModule" /> and <see cref="ILifetimeScopeProvider" /> implementation
//   that creates a nested lifetime scope for each HTTP request.
//   创建每个Http请求的生命周期
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.DependencyManagement
{
    using System;
    using System.Web;

    using Autofac.Integration.Mvc;

    using Skymate;

    /// <summary>
    /// An <see cref="IHttpModule"/> and <see cref="ILifetimeScopeProvider"/> implementation
    /// that creates a nested lifetime scope for each HTTP request.
    /// 创建每个Http请求的生命周期
    /// </summary>
    public class AutofacRequestLifetimeHttpModule : IHttpModule
    {
        /// <summary>
        /// Gets or sets the lifetime scope provider.
        /// 获取或设置生命周期驱动器
        /// </summary>
        internal static ILifetimeScopeProvider LifetimeScopeProvider
        {
            get;
            set;
        }

        #region New

        /// <summary>
        /// The on end request.
        /// 请求结束
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        public static void OnEndRequest(object sender, EventArgs e)
        {
            if (LifetimeScopeProvider != null)
            {
                LifetimeScopeProvider.EndLifetimeScope();
            }
        }

        /// <summary>
        /// The set lifetime scope provider.
        /// 生命周期驱动器
        /// </summary>
        /// <param name="lifetimeScopeProvider">
        /// The lifetime scope provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 参数为空异常
        /// </exception>
        public static void SetLifetimeScopeProvider(ILifetimeScopeProvider lifetimeScopeProvider)
        {
            if (lifetimeScopeProvider == null)
            {
                throw new ArgumentNullException("lifetimeScopeProvider");
            }

            LifetimeScopeProvider = lifetimeScopeProvider;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// The init.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public void Init(HttpApplication context)
        {
            Guard.ArgumentNotNull(() => context);

            context.EndRequest += OnEndRequest;
        }

        #endregion 
    }
}