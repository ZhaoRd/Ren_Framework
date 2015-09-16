// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparableObject<T>.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System;
    using System.Linq.Expressions;

    using Skymate.Extensions;

    /// <summary>
    /// The comparable object.
    /// </summary>
    /// <typeparam name="T">
    /// 泛型
    /// </typeparam>
    [Serializable]
    public abstract class ComparableObject<T> : ComparableObject, IEquatable<T>
    {
        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Equals(T other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other);
        }

        /// <summary>
        /// Adds an extra property to the type specific signature properties list.
        /// </summary>
        /// <param name="expression">
        /// The lambda expression for the property to add.
        /// </param>
        /// <remarks>
        /// Both lists are <c>unioned</c>, so
        /// that no duplicates can occur within the global descriptor collection.
        /// </remarks>
        protected void RegisterSignatureProperty(Expression<Func<T, object>> expression)
        {
            Guard.ArgumentNotNull(() => expression);

            this.RegisterSignatureProperty(expression.ExtractPropertyInfo());
        }
    }
}
