// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionDisposable.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   ActionDisposable
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities
{
    using System;

    /// <summary>
    /// Allows action to be executed when it is disposed
    /// </summary>
    public struct ActionDisposable : IDisposable
    {
        /// <summary>
        /// The empty.
        /// </summary>
        public static readonly ActionDisposable Empty = new ActionDisposable(() => { });

        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDisposable"/> struct.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public ActionDisposable(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.action();
        }
    }
}
