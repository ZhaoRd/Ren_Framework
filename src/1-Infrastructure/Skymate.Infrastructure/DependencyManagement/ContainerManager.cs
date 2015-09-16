// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Skymate" file="ContainerManager.cs">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
//
// --------------------------------------------------------------------------------------------------------------------
namespace Skymate.DependencyManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;
    
    using Skymate;

    /// <summary>
    /// The container manager.
    /// </summary>
    public abstract class ContainerManager
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerManager"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public ContainerManager(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        public IContainer Container
        {
            get { return this.container; }
        }

        /// <summary>
        /// The resolve.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            return string.IsNullOrEmpty(key) ? (scope ?? this.Scope()).Resolve<T>() : (scope ?? this.Scope()).ResolveKeyed<T>(key);
        }

        /// <summary>
        /// The resolve named.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T ResolveNamed<T>(string name, ILifetimeScope scope = null) where T : class
        {
            return (scope ?? this.Scope()).ResolveNamed<T>(name);
        }

        /// <summary>
        /// The resolve.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Resolve(Type type, ILifetimeScope scope = null)
        {
            return (scope ?? this.Scope()).Resolve(type);
        }

        /// <summary>
        /// The resolve named.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object ResolveNamed(string name, Type type, ILifetimeScope scope = null)
        {
            return (scope ?? this.Scope()).ResolveNamed(name, type);
        }

        /// <summary>
        /// The resolve all.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>T[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        public T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return (scope ?? this.Scope()).Resolve<IEnumerable<T>>().ToArray();
            }

            return (scope ?? this.Scope()).ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// The resolve unregistered.
        /// </summary>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return this.ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// The resolve unregistered.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="SkymateException">
        /// 异常
        /// </exception>
        public object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = this.Resolve(parameter.ParameterType, scope);
                        if (service == null)
                        {
                            throw new SkymateException("Unkown dependency");
                        }

                        parameterInstances.Add(service);
                    }

                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (SkymateException)
                {
                }
            }

            throw new SkymateException("No contructor was found that had all the dependencies satisfied.");
        }

        /// <summary>
        /// The try resolve.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            return (scope ?? this.Scope()).TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// The try resolve.
        /// </summary>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool TryResolve<T>(ILifetimeScope scope, out T instance)
        {
            return (scope ?? this.Scope()).TryResolve(out instance);
        }

        /// <summary>
        /// The is registered.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            return (scope ?? this.Scope()).IsRegistered(serviceType);
        }

        /// <summary>
        /// The resolve optional.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            return (scope ?? this.Scope()).ResolveOptional(serviceType);
        }

        /// <summary>
        /// The inject properties.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T InjectProperties<T>(T instance, ILifetimeScope scope = null)
        {
            return (scope ?? this.Scope()).InjectProperties(instance);
        }

        /// <summary>
        /// The inject unset properties.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T InjectUnsetProperties<T>(T instance, ILifetimeScope scope = null)
        {
            return (scope ?? this.Scope()).InjectUnsetProperties(instance);
        }

        /// <summary>
        /// The scope.
        /// </summary>
        /// <returns>
        /// The <see cref="ILifetimeScope"/>.
        /// </returns>
        public abstract ILifetimeScope Scope();
/*
    {
        ILifetimeScope scope = null;
        try
        {
            scope = AutofacDependencyResolver.Current.RequestLifetimeScope;
        }
        catch
        {

        }

        if (scope == null)
        {
            scope = this.container.BeginLifetimeScope("AutofacWebRequest");
        }

        return scope ?? this.container;
    }*/
    }
}