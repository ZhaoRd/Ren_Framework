// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpgradeableReadLockDisposable.cs" company="Skymate">
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
    /// The upgradeable read lock disposable.
    /// </summary>
    public sealed class UpgradeableReadLockDisposable : IDisposable
    {
        /// <summary>
        /// The _rw lock.
        /// </summary>
        private readonly ReaderWriterLockSlim readerWriterLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeableReadLockDisposable"/> class.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw lock.
        /// </param>
        public UpgradeableReadLockDisposable(ReaderWriterLockSlim readerWriterLock)
        {
            this.readerWriterLock = readerWriterLock;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.readerWriterLock.ExitUpgradeableReadLock();
        }
    }
}
