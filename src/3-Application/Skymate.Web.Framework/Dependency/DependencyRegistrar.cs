// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyRegistrar.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   The dependency registrar.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.Dependency
{
    using System.Linq;
    using System.Web;

    using Autofac;
    using Autofac.Integration.Mvc;

    using Skymate.DependencyManagement;
    using Skymate.TypeFinders;
   

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
            get { return 0; }
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
             // HTTP context and other related stuff
           builder.Register(
               c => // register FakeHttpContext when HttpContext is not available
               (new HttpContextWrapper(HttpContext.Current) as HttpContextBase))
               .As<HttpContextBase>()
               .InstancePerLifetimeScope();
           builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
           builder.Register(c => c.Resolve<HttpContextBase>().Response)
               .As<HttpResponseBase>()
               .InstancePerLifetimeScope();
           builder.Register(c => c.Resolve<HttpContextBase>().Server)
               .As<HttpServerUtilityBase>()
               .InstancePerLifetimeScope();
           builder.Register(c => c.Resolve<HttpContextBase>().Session)
               .As<HttpSessionStateBase>()
               .InstancePerLifetimeScope();

           // controllers
           var controllers = typeFinder.GetAssemblies().ToArray();
           builder.RegisterControllers(controllers);
        }
    }
}
