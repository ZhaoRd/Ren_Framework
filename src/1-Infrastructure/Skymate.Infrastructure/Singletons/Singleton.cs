// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Singleton.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Singletons
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides access to all "singletons" stored by <see cref="Singleton{T}"/>.
    /// </summary>
    public class Singleton
    {
        /// <summary>
        /// The all singletons.
        /// </summary>
        static readonly IDictionary<Type, object> allSingletons;

        /// <summary>
        /// Initializes static members of the <see cref="Singleton"/> class.
        /// </summary>
        static Singleton()
        {
            allSingletons = new Dictionary<Type, object>();
        }

        /// <summary>Dictionary of type to singleton instances.</summary>
        public static IDictionary<Type, object> AllSingletons
        {
            get { return allSingletons; }
        }
    }
}
