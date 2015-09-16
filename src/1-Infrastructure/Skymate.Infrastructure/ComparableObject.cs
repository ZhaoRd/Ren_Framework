// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparableObject.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Fasterflect;

    using Skymate.Attributes;

    /// <summary>
    /// Provides a standard base class for facilitating sophisticated comparison of objects.
    /// </summary>
    [Serializable]
    public abstract class ComparableObject
    {
        /// <summary>
        /// The hash multiplier.
        /// </summary>
        protected const int HashMultiplier = 31;

        /// <summary>
        /// This static member caches the domain signature properties to avoid looking them up for
        /// each instance of the same type.
        /// A description of the ThreadStatic attribute may be found at
        /// http://www.dotnetjunkies.com/WebLog/chris.taylor/archive/2005/08/18/132026.aspx
        /// </summary>
        [ThreadStatic]
        private static IDictionary<Type, IEnumerable<PropertyInfo>> signatureProperties;

        /// <summary>
        /// The _extra signature properties.
        /// </summary>
        private readonly List<PropertyInfo> extraSignatureProperties = new List<PropertyInfo>();

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var compareTo = obj as ComparableObject;

            if (ReferenceEquals(this, compareTo))
            {
                return true;
            }

            return compareTo != null && this.GetType() == compareTo.GetTypeUnproxied() &&
                this.HasSameSignatureAs(compareTo);
        }

        /// <summary>
        /// Used to provide the hashcode identifier of an object using the signature
        /// properties of the object; Since it is recommended that GetHashCode change infrequently,
        /// if at all, in an object's lifetime; it's important that properties are carefully
        /// selected which truly represent the signature of an object.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var properties = this.GetSignatureProperties();
                var t = this.GetType();

                // It's possible for two objects to return the same hash code based on
                // identically valued properties, even if they're of two different types,
                // so we include the object's type in the hash calculation
                var hashCode = t.GetHashCode();

                var propertyInfos = properties as PropertyInfo[] ?? properties.ToArray();
                hashCode = propertyInfos.Select(pi => this.GetPropertyValue(pi.Name)).Where(value => value != null).Aggregate(hashCode, (current, value) => (current * HashMultiplier) ^ value.GetHashCode());

                return propertyInfos.Any() ? hashCode : base.GetHashCode();
            }
        }

        /// <summary>
        /// The get signature properties.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public IEnumerable<PropertyInfo> GetSignatureProperties()
        {
            IEnumerable<PropertyInfo> properties;

            if (signatureProperties == null)
            {
                signatureProperties = new Dictionary<Type, IEnumerable<PropertyInfo>>();
            }

            var t = this.GetType();

            if (signatureProperties.TryGetValue(t, out properties))
            {
                return properties;
            }

            return signatureProperties[t] = this.GetSignaturePropertiesCore();
        }


        /// <summary>
        /// Returns the real underlying type of proxied objects.
        /// </summary>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        protected virtual Type GetTypeUnproxied()
        {
            return this.GetType();
        }

        /// <summary>
        /// You may override this method to provide your own comparison routine.
        /// </summary>
        /// <param name="compareTo">
        /// The compare To.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected virtual bool HasSameSignatureAs(ComparableObject compareTo)
        {
            if (compareTo == null)
            {
                return false;
            }

            var propertyInfos = this.GetSignatureProperties();

            var enumerable = propertyInfos as PropertyInfo[] ?? propertyInfos.ToArray();
            if ((from pi in enumerable
                 let thisValue = this.GetPropertyValue(pi.Name)
                 let thatValue = compareTo.GetPropertyValue(pi.Name)
                 where thisValue != null || thatValue != null
                 where (thisValue == null ^ thatValue == null) || (!thisValue.Equals(thatValue))
                 select thisValue).Any())
            {
                return false;
            }

            return enumerable.Any() || base.Equals(compareTo);
        }

        /// <summary>
        /// Enforces the template method pattern to have child objects determine which specific
        /// properties should and should not be included in the object signature comparison.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        protected virtual IEnumerable<PropertyInfo> GetSignaturePropertiesCore()
        {
            Type t = this.GetType();
            var properties = t.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(ObjectSignatureAttribute), true));

            return properties.Union(this.extraSignatureProperties).ToList();
        }

        /// <summary>
        /// Adds an extra property to the type specific signature properties list.
        /// </summary>
        /// <param name="propertyInfo">
        /// The property to add.
        /// </param>
        /// <remarks>
        /// Both lists are <c>unioned</c>, so
        /// that no duplicates can occur within the global descriptor collection.
        /// </remarks>
        protected void RegisterSignatureProperty(PropertyInfo propertyInfo)
        {
            Guard.ArgumentNotNull(() => propertyInfo);
            this.extraSignatureProperties.Add(propertyInfo);
        }

        /// <summary>
        /// Adds an extra property to the type specific signature properties list.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property to add.
        /// </param>
        /// <remarks>
        /// Both lists are <c>unioned</c>, so
        /// that no duplicates can occur within the global descriptor collection.
        /// </remarks>
        protected void RegisterSignatureProperty(string propertyName)
        {
            Guard.ArgumentNotEmpty(() => propertyName);

            Type t = this.GetType();

            var pi = t.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (pi == null)
            {
                throw Error.Argument("propertyName", "Could not find property '{0}' on type '{1}'.", propertyName, t);
            }

            this.RegisterSignatureProperty(pi);
        }
    }
}