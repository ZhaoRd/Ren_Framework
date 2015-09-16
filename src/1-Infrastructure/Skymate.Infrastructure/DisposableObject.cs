// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisposableObject.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// The disposable object.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        #region Fields

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Ctor

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class. 
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether is disposed.
        /// </summary>
        public virtual bool IsDisposed
        {
            [DebuggerStepThrough]
            get
            {
                return this.disposed;
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// The dispose.
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Static Methods

        /// <summary>
        /// The dispose enumerable.
        /// </summary>
        /// <param name="enumerable">
        /// The enumerable.
        /// </param>
        protected static void DisposeEnumerable(IEnumerable enumerable)
        {
            if (enumerable != null)
            {
                foreach (object obj2 in enumerable)
                {
                    DisposeMember(obj2);
                }

                DisposeMember(enumerable);
            }
        }

        /// <summary>
        /// The dispose dictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        /// <typeparam name="TK">
        /// 泛型TK
        /// </typeparam>
        /// <typeparam name="TV">
        /// 泛型TV
        /// </typeparam>
        protected static void DisposeDictionary<TK, TV>(IDictionary<TK, TV> dictionary)
        {
            if (dictionary == null)
            {
                return;
            }

            foreach (KeyValuePair<TK, TV> pair in dictionary)
            {
                DisposeMember(pair.Value);
            }

            DisposeMember(dictionary);
        }

        /// <summary>
        /// The dispose dictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        protected static void DisposeDictionary(IDictionary dictionary)
        {
            if (dictionary != null)
            {
                foreach (IDictionaryEnumerator pair in dictionary)
                {
                    DisposeMember(pair.Value);
                }

                DisposeMember(dictionary);
            }
        }

        /// <summary>
        /// The dispose member.
        /// </summary>
        /// <param name="member">
        /// The member.
        /// </param>
        protected static void DisposeMember(object member)
        {
            var disposable = member as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The on dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected abstract void OnDispose(bool disposing);

        /// <summary>
        /// The check disposed.
        /// </summary>
        /// <exception cref="Exception">
        /// 异常
        /// </exception>
        [DebuggerStepThrough]
        protected void CheckDisposed()
        {
            if (this.IsDisposed)
            {
                throw Error.ObjectDisposed(this.GetType().FullName);
            }
        }

        /// <summary>
        /// The check disposed.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <exception cref="Exception">
        /// 异常
        /// </exception>
        [DebuggerStepThrough]
        protected void CheckDisposed(string errorMessage)
        {
            if (this.IsDisposed)
            {
                throw Error.ObjectDisposed(this.GetType().FullName, errorMessage);
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.OnDispose(disposing);
            }

            this.disposed = true;
        }

        #endregion
    }
}
