// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Engine.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Engines
{
    using Autofac;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Skymate.Data;
    using Skymate.DependencyManagement;
    using Skymate.TypeFinders;

    using Skymate.Extensions;

    /// <summary>
    /// The engine.
    /// </summary>
    public abstract class Engine : IEngine
    {
        /// <summary>
        /// The container manager.
        /// </summary>
        private ContainerManager containerManager;

        #region Utilities

        /// <summary>
        /// The run startup tasks.
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            var typeFinder = this.containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>(ignoreInactivePlugins: true);
            var startUpTasks = new List<IStartupTask>();

            foreach (var startUpTaskType in startUpTaskTypes)
            {
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            }

            // execute tasks async grouped by order
            var groupedTasks = startUpTasks.OrderBy(st => st.Order).ToLookup(x => x.Order);
            foreach (var tasks in groupedTasks)
            {
                Parallel.ForEach(tasks, task => { task.Execute(); });
            }
        }

        /// <summary>
        /// The create type finder.
        /// </summary>
        /// <returns>
        /// The <see cref="ITypeFinder"/>.
        /// </returns>
        protected virtual ITypeFinder CreateTypeFinder()
        {
            return new WebAppTypeFinder();
        }

        /// <summary>
        /// The create dependency resolver.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        protected abstract object CreateDependencyResolver(IContainer container);/*
        {

            /*var scopeProvider = new AutofacLifetimeScopeProvider(container);
            var dependencyResolver = new AutofacDependencyResolver(container, scopeProvider);
            return dependencyResolver;#1#
        }*/

        /// <summary>
        /// The register dependencies.
        /// </summary>
        /// <returns>
        /// The <see cref="ContainerManager"/>.
        /// </returns>
        protected abstract ContainerManager RegisterDependencies();
        /*{
            var builder = new ContainerBuilder();
            var container = builder.Build();
            var typeFinder = this.CreateTypeFinder();

            // core dependencies
            builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            // register dependencies provided by other assemblies
            builder = new ContainerBuilder();
            var registrarTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var registrarInstances = new List<IDependencyRegistrar>();
            foreach (var type in registrarTypes)
            {
                registrarInstances.Add((IDependencyRegistrar)Activator.CreateInstance(type));
            }

            // sort
            registrarInstances = registrarInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var registrar in registrarInstances)
            {
                /*registrar.Register(
                    builder,
                    typeFinder,
                    PluginManager.IsActivePluginAssembly(registrar.GetType().Assembly));#1#
            }

            builder.Update(container);

            // AutofacDependencyResolver
            var dependencyResolver = this.CreateDependencyResolver(container);
            if (dependencyResolver is IDependencyResolver)
            {
                DependencyResolver.SetResolver((IDependencyResolver)dependencyResolver);
            }
            else if (dependencyResolver != null)
            {
                DependencyResolver.SetResolver(dependencyResolver);
            }

            return new ContainerManager(container);
        }*/

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Initialize components and plugins in the sm environment.
        /// </summary>
        public void Initialize()
        {
            this.containerManager = this.RegisterDependencies();
            if (DataSettings.DatabaseIsInstalled())
            {
                this.RunStartupTasks();
            }
        }

        /// <summary>
        /// The resolve.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Resolve<T>(string name = null) where T : class
        {
            if (name.HasValue())
            {
                return this.ContainerManager.ResolveNamed<T>(name);
            }

            return this.ContainerManager.Resolve<T>();
        }

        /// <summary>
        /// The resolve.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Resolve(Type type, string name = null)
        {
            if (name.HasValue())
            {
                return this.ContainerManager.ResolveNamed(name, type);
            }

            return this.ContainerManager.Resolve(type);
        }

        /// <summary>
        /// The resolve all.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T[]"/>.
        /// </returns>
        public T[] ResolveAll<T>()
        {
            return this.ContainerManager.ResolveAll<T>();
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets the container.
        /// </summary>
        public IContainer Container
        {
            get { return this.containerManager.Container; }
        }

        /// <summary>
        /// Gets the container manager.
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return this.containerManager; }
        }

        #endregion Properties
    }
}