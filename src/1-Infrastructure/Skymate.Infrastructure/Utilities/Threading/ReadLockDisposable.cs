// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadLockDisposable.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   ReadLockDisposable
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities.Threading
{
    using System;
    using System.Threading;

    /// <summary>
    /// The read lock disposable.
    /// </summary>
    public sealed class ReadLockDisposable : IDisposable
    {
        /// <summary>
        /// The _rw lock.
        /// </summary>
        private readonly ReaderWriterLockSlim readerWriterLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadLockDisposable"/> class.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw lock.
        /// </param>
        public ReadLockDisposable(ReaderWriterLockSlim readerWriterLock)
        {
            this.readerWriterLock = readerWriterLock;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.readerWriterLock.ExitReadLock();
        }
    }
}
