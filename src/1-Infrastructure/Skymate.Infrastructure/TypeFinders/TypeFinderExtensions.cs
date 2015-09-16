// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeFinderExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.TypeFinders
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The i type finder extensions.
    /// </summary>
    public static class TypeFinderExtensions
    {
        /// <summary>
        /// The find classes of type.
        /// </summary>
        /// <param name="finder">
        /// The finder.
        /// </param>
        /// <param name="onlyConcreteClasses">
        /// The only concrete classes.
        /// </param>
        /// <param name="ignoreInactivePlugins">
        /// The ignore inactive plugins.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<Type> FindClassesOfType<T>(this ITypeFinder finder, bool onlyConcreteClasses = true, bool ignoreInactivePlugins = false)
        {
            return finder.FindClassesOfType(typeof(T), finder.GetAssemblies(ignoreInactivePlugins), onlyConcreteClasses);
        }

        /// <summary>
        /// The find classes of type.
        /// </summary>
        /// <param name="finder">
        /// The finder.
        /// </param>
        /// <param name="assignTypeFrom">
        /// The assign type from.
        /// </param>
        /// <param name="onlyConcreteClasses">
        /// The only concrete classes.
        /// </param>
        /// <param name="ignoreInactivePlugins">
        /// The ignore inactive plugins.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<Type> FindClassesOfType(this ITypeFinder finder, Type assignTypeFrom, bool onlyConcreteClasses = true, bool ignoreInactivePlugins = false)
        {
            return finder.FindClassesOfType(assignTypeFrom, finder.GetAssemblies(ignoreInactivePlugins), onlyConcreteClasses);
        }

        /// <summary>
        /// The find classes of type.
        /// </summary>
        /// <param name="finder">
        /// The finder.
        /// </param>
        /// <param name="assemblies">
        /// The assemblies.
        /// </param>
        /// <param name="onlyConcreteClasses">
        /// The only concrete classes.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<Type> FindClassesOfType<T>(this ITypeFinder finder, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return finder.FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        }
    }
}