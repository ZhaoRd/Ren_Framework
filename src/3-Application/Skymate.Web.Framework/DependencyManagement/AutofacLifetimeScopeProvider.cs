// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutofacLifetimeScopeProvider.cs" company="zhaord">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   aotoface生命周期驱动器
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.DependencyManagement
{
    using System;
    using System.Web;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Skymate;

    /// <summary>
    /// autofac生命周期提供者.
    /// </summary>
    public class AutofacLifetimeScopeProvider : ILifetimeScopeProvider
    {
        #region 字段

        /// <summary>
        /// http请求标签
        /// </summary>
        internal static readonly object HttpRequestTag = "AutofacWebRequest";

        /// <summary>
        /// 容器
        /// </summary>
        private readonly ILifetimeScope container;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个 <see cref="AutofacLifetimeScopeProvider"/> 类的实例.
        /// </summary>
        /// <param name="container">
        /// 容器.
        /// </param>
        public AutofacLifetimeScopeProvider(ILifetimeScope container)
        {
            Guard.ArgumentNotNull(() => container);

            this.container = container;
            AutofacRequestLifetimeHttpModule.SetLifetimeScopeProvider(this);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置应用程序容器
        /// </summary>
        public ILifetimeScope ApplicationContainer
        {
            get
            {
                return this.container;
            }
        }

        /// <summary>
        /// 获取或设置生命周期
        /// </summary>
        private static ILifetimeScope LifetimeScope
        {
            get
            {
                return (ILifetimeScope)HttpContext.Current.Items[typeof(ILifetimeScope)];
            }

            set
            {
                HttpContext.Current.Items[typeof(ILifetimeScope)] = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// The end lifetime scope.
        /// </summary>
        public void EndLifetimeScope()
        {
            var lifetimeScope = LifetimeScope;
            if (lifetimeScope != null)
            {
                lifetimeScope.Dispose();
                HttpContext.Current.Items.Remove(typeof(ILifetimeScope));
            }
        }

        /// <summary>
        /// The get lifetime scope.
        /// </summary>
        /// <param name="configurationAction">
        /// The configuration action.
        /// </param>
        /// <returns>
        /// The <see cref="ILifetimeScope"/>.
        /// </returns>
        public ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            // little hack here to get dependencies when HttpContext is not available
            if (HttpContext.Current != null)
            {
                return LifetimeScope ?? (LifetimeScope = this.GetLifetimeScopeCore(configurationAction));
            }

            return this.GetLifetimeScopeCore(configurationAction);
        }

        /// <summary>
        /// The get lifetime scope core.
        /// </summary>
        /// <param name="configurationAction">
        /// The configuration action.
        /// </param>
        /// <returns>
        /// The <see cref="ILifetimeScope"/>.
        /// </returns>
        protected virtual ILifetimeScope GetLifetimeScopeCore(Action<ContainerBuilder> configurationAction)
        {
            return (configurationAction == null)
                       ? this.container.BeginLifetimeScope(HttpRequestTag)
                       : this.container.BeginLifetimeScope(HttpRequestTag, configurationAction);
        }

        #endregion
    }
}