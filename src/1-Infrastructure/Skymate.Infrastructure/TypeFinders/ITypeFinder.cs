// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITypeFinder.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.TypeFinders
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Classes implementing this interface provide information about types 
    /// to various services in the SmartStore engine.
    /// </summary>
    public interface ITypeFinder
    {
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
        IList<Assembly> GetAssemblies(bool ignoreInactivePlugins = false);

        /// <summary>
        /// The find classes of type.
        /// </summary>
        /// <param name="assignTypeFrom">
        /// The assign type from.
        /// </param>
        /// <param name="assemblies">
        /// The assemblies.
        /// </param>
        /// <param name="onlyConcreteClasses">
        /// The only concrete classes.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
    }
}
