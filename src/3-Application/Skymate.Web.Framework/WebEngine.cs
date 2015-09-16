// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebEngine.cs" company="zhaord">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   web项目IOC引擎.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Skymate.DependencyManagement;
    using Skymate.Engines;
    using Skymate.TypeFinders;
    using Skymate.Web.DependencyManagement;

    /// <summary>
    /// web项目IOC引擎.
    /// </summary>
    public class WebEngine : Engine
    {
        #region Utilities

        /// <summary>
        /// The create dependency resolver.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        protected override object CreateDependencyResolver(IContainer container)
        {
            var scopeProvider = new AutofacLifetimeScopeProvider(container);
            var dependencyResolver = new AutofacDependencyResolver(container, scopeProvider);
            return dependencyResolver;
        }

        /// <summary>
        /// 注册依赖项.
        /// </summary>
        /// <returns>
        /// The <see cref="ContainerManager"/>.
        /// </returns>
        protected override ContainerManager RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            var typeFinder = this.CreateTypeFinder();

            builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            builder = new ContainerBuilder();
            var registrarTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var registrarInstances = registrarTypes.Select(type => (IDependencyRegistrar)Activator.CreateInstance(type)).ToList();

            // sort
            registrarInstances = registrarInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var registrar in registrarInstances)
            {
                registrar.Register(builder, typeFinder, false);
                /*registrar.Register(
                    builder,
                    typeFinder,
                    PluginManager.IsActivePluginAssembly(registrar.GetType().Assembly));*/
            }

            builder.Update(container);

            // AutofacDependencyResolver
            var dependencyResolver = this.CreateDependencyResolver(container);
            var locator = dependencyResolver as IDependencyResolver;
            if (locator != null)
            {
                DependencyResolver.SetResolver(locator);
            }
            else if (dependencyResolver != null)
            {
                DependencyResolver.SetResolver(dependencyResolver);
            }

            return new WebContainerManager(container);
        }

        #endregion Utilities
    }
}
