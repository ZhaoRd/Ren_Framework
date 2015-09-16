// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteLockDisposable.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   WriteLockDisposable
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities.Threading
{
    using System;
    using System.Threading;

    /// <summary>
    /// The write lock disposable.
    /// </summary>
    public sealed class WriteLockDisposable : IDisposable
    {
        /// <summary>
        /// The _rw lock.
        /// </summary>
        private readonly ReaderWriterLockSlim readerWriterLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteLockDisposable"/> class.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw lock.
        /// </param>
        public WriteLockDisposable(ReaderWriterLockSlim readerWriterLock)
        {
            this.readerWriterLock = readerWriterLock;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.readerWriterLock.ExitWriteLock();
        }
    }
}
