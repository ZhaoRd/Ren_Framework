// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LockExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   错误类
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities.Threading
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// The lock extensions.
    /// Lock扩展
    /// </summary>
    public static class LockExtensions
    {
        /// <summary>
        /// Acquires a disposable reader lock that can be used with a using statement.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw Lock.
        /// </param>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IDisposable GetReadLock(this ReaderWriterLockSlim readerWriterLock)
        {
            return readerWriterLock.GetReadLock(-1);
        }

        /// <summary>
        /// Acquires a disposable reader lock that can be used with a using statement.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw Lock.
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, or -1 to wait indefinitely.
        /// </param>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IDisposable GetReadLock(this ReaderWriterLockSlim readerWriterLock, int millisecondsTimeout)
        {
            bool acquire = readerWriterLock.IsReadLockHeld == false ||
                           readerWriterLock.RecursionPolicy == LockRecursionPolicy.SupportsRecursion;

            if (acquire)
            {
                if (readerWriterLock.TryEnterReadLock(millisecondsTimeout))
                {
                    return new ReadLockDisposable(readerWriterLock);
                }
            }

            return ActionDisposable.Empty;
        }

        /// <summary>
        /// Acquires a disposable and upgradeable reader lock that can be used with a using statement.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw Lock.
        /// </param>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IDisposable GetUpgradeableReadLock(this ReaderWriterLockSlim readerWriterLock)
        {
            return readerWriterLock.GetUpgradeableReadLock(-1);
        }

        /// <summary>
        /// Acquires a disposable and upgradeable reader lock that can be used with a using statement.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw Lock.
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, or -1 to wait indefinitely.
        /// </param>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IDisposable GetUpgradeableReadLock(this ReaderWriterLockSlim readerWriterLock, int millisecondsTimeout)
        {
            bool acquire = readerWriterLock.IsUpgradeableReadLockHeld == false ||
                           readerWriterLock.RecursionPolicy == LockRecursionPolicy.SupportsRecursion;

            if (acquire)
            {
                if (readerWriterLock.TryEnterUpgradeableReadLock(millisecondsTimeout))
                {
                    return new UpgradeableReadLockDisposable(readerWriterLock);
                }
            }

            return ActionDisposable.Empty;
        }

        /// <summary>
        /// Acquires a disposable writer lock that can be used with a using statement.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw Lock.
        /// </param>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IDisposable GetWriteLock(this ReaderWriterLockSlim readerWriterLock)
        {
            return readerWriterLock.GetWriteLock(-1);
        }

        /// <summary>
        /// Tries to enter a disposable write lock that can be used with a using statement.
        /// </summary>
        /// <param name="readerWriterLock">
        /// The rw Lock.
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, or -1 to wait indefinitely.
        /// </param>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IDisposable GetWriteLock(this ReaderWriterLockSlim readerWriterLock, int millisecondsTimeout)
        {
            bool acquire = readerWriterLock.IsWriteLockHeld == false ||
                           readerWriterLock.RecursionPolicy == LockRecursionPolicy.SupportsRecursion;

            if (acquire)
            {
                if (readerWriterLock.TryEnterWriteLock(millisecondsTimeout))
                {
                    return new WriteLockDisposable(readerWriterLock);
                }
            }

            return ActionDisposable.Empty;
        }
    }
}
