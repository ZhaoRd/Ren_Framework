// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebContainerManager.cs" company="zhaord">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   IOC容器管理器.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.DependencyManagement
{
    using Autofac;
    using Autofac.Integration.Mvc;

    using Skymate.DependencyManagement;

    /// <summary>
    /// IOC容器管理器.
    /// </summary>
    public class WebContainerManager : ContainerManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebContainerManager"/> class.
        /// 初始化IOC管理器实例
        /// </summary>
        /// <param name="container">
        /// The container.
        /// 容器
        /// </param>
        public WebContainerManager(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// The scope.
        /// 生命周期
        /// </summary>
        /// <returns>
        /// The <see cref="ILifetimeScope"/>.
        /// </returns>
        public override ILifetimeScope Scope()
        {
            var scope = AutofacDependencyResolver.Current.RequestLifetimeScope
                                   ?? this.Container.BeginLifetimeScope("AutofacWebRequest");

            return scope ?? this.Container;
        }
    }
}