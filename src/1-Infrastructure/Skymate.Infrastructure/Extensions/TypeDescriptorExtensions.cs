// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeDescriptorExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// The type descriptor extensions.
    /// </summary>
    public static class TypeDescriptorExtensions
    {
        /// <summary>
        /// The get attributes.
        /// </summary>
        /// <param name="td">
        /// The td.
        /// </param>
        /// <typeparam name="TAttribute">
        /// TAttribute
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ICustomTypeDescriptor td) where TAttribute : Attribute
        {
            var attributes = td.GetAttributes().OfType<TAttribute>();
            return TypeExtensions.SortAttributesIfPossible(attributes);
        }

        /// <summary>
        /// The get attributes.
        /// </summary>
        /// <param name="pd">
        /// The pd.
        /// </param>
        /// <typeparam name="TAttribute">
        /// TAttribute
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this PropertyDescriptor pd) where TAttribute : Attribute
        {
            var attributes = pd.Attributes.OfType<TAttribute>();
            return TypeExtensions.SortAttributesIfPossible(attributes);
        }

        /// <summary>
        /// The get attributes.
        /// </summary>
        /// <param name="pd">
        /// The pd.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <typeparam name="TAttribute">
        /// TAttribute
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(
            this PropertyDescriptor pd,
            Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            Guard.ArgumentNotNull(predicate, "predicate");

            var attributes = pd.Attributes.OfType<TAttribute>().Where(predicate);
            return TypeExtensions.SortAttributesIfPossible(attributes);
        }

        /// <summary>
        /// The get property.
        /// </summary>
        /// <param name="td">
        /// The td.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyDescriptor"/>.
        /// </returns>
        public static PropertyDescriptor GetProperty(this ICustomTypeDescriptor td, string name)
        {
            Guard.ArgumentNotEmpty(name, "name");
            return td.GetProperties().Find(name, true);
        }

        /// <summary>
        /// The get properties with.
        /// </summary>
        /// <param name="td">
        /// The td.
        /// </param>
        /// <typeparam name="TAttribute">
        /// TAttribute
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<PropertyDescriptor> GetPropertiesWith<TAttribute>(this ICustomTypeDescriptor td)
            where TAttribute : Attribute
        {
            return td.GetPropertiesWith<TAttribute>(x => true);
        }

        /// <summary>
        /// The get properties with.
        /// </summary>
        /// <param name="td">
        /// The td.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <typeparam name="TAttribute">
        /// TAttribute
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<PropertyDescriptor> GetPropertiesWith<TAttribute>(
            this ICustomTypeDescriptor td, 
            Func<TAttribute, bool> predicate)
            where TAttribute : Attribute
        {
            Guard.ArgumentNotNull(predicate, "predicate");

            return td.GetProperties()
                    .Cast<PropertyDescriptor>()
                    .Where(p => p.GetAttributes<TAttribute>().Any(predicate));
        }
    }
}
