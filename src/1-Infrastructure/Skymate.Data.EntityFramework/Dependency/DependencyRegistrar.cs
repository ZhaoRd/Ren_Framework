// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyRegistrar.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   The dependency registrar.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.EntityFramework.Dependency
{
    using System.Data.Entity;

    using Apworks.Repositories;

    using Autofac;

    using Data.EntityFramework;
    using DependencyManagement;
    using Entities;
    using TypeFinders;

    /// <summary>
    /// The dependency registrar.
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Gets the order.
        /// </summary>
        public int Order
        {
            get { return 1; }
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <param name="typeFinder">
        /// The type finder.
        /// </param>
        /// <param name="isActiveModule">
        /// The is active module.
        /// </param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, bool isActiveModule)
        {
            builder.Register(x => new SkymateDataRepositoryContext(x.Resolve<DbContext>()))
                .As<IRepositoryContext>().InstancePerLifetimeScope();

            builder.Register(x => new UnitOfWork(x.Resolve<IRepositoryContext>()))
                .As<ISkymateUnitOfWork>()
                .InstancePerLifetimeScope();
        }
    }
}
