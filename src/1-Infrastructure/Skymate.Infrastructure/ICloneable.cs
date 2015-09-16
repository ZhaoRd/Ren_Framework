// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICloneable.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System;

    /// <summary>
    /// Generic variant of <see cref="ICloneable"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object that is cloned
    /// </typeparam>
    public interface ICloneable<out T> : ICloneable
    {
        /// <summary>
        /// Clones the object.
        /// </summary>
        /// <returns>The cloned instance</returns>
        new T Clone();
    }
}
