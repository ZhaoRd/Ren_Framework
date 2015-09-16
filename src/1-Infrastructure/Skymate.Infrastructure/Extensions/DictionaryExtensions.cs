// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//    字典扩展
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Linq;

    using Skymate;

    /// <summary>
    /// The dictionary extensions.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <typeparam name="TU">
        /// 泛型
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// 参数异常
        /// </exception>
        public static void AddRange<T, TU>(this IDictionary<T, TU> values, IEnumerable<KeyValuePair<T, TU>> other)
        {
            foreach (var kvp in other)
            {
                if (values.ContainsKey(kvp.Key))
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }

                values.Add(kvp);
            }
        }

        /// <summary>
        /// The merge.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="replaceExisting">
        /// The replace existing.
        /// </param>
        public static void Merge(
            this IDictionary<string, object> instance, 
            string key, 
            object value, 
            bool replaceExisting = true)
        {
            if (replaceExisting || !instance.ContainsKey(key))
            {
                instance[key] = value;
            }
        }

        /// <summary>
        /// The merge.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="replaceExisting">
        /// The replace existing.
        /// </param>
        /// <typeparam name="T">
        /// 泛型T
        /// </typeparam>
        /// <typeparam name="TU">
        /// 泛型U
        /// </typeparam>
        public static void Merge<T, TU>(
            this IDictionary<T, TU> instance, 
            IDictionary<T, TU> from, 
            bool replaceExisting = true)
        {
            foreach (var keyValuePair in @from.Where(keyValuePair => replaceExisting || !instance.ContainsKey(keyValuePair.Key)))
            {
                instance[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        /// <summary>
        /// The append in value.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void AppendInValue(
            this IDictionary<string, object> instance, 
            string key, 
            string separator, 
            object value)
        {
            instance[key] = !instance.ContainsKey(key) ? value.ToString() : (instance[key] + separator + value);
        }

        /// <summary>
        /// The prepend in value.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void PrependInValue(
            this IDictionary<string, object> instance, 
            string key, 
            string separator, 
            object value)
        {
            instance[key] = !instance.ContainsKey(key) ? value.ToString() : (value + separator + instance[key]);
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="TK">
        /// 泛型K
        /// </typeparam>
        /// <typeparam name="T">
        /// 泛型T
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T GetValue<TK, T>(this IDictionary<TK, object> instance, TK key)
        {
            try
            {
                object val;
                if (instance != null && instance.TryGetValue(key, out val) && val != null)
                {
                    return (T)Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
                }
            }
            catch (Exception exc)
            {
                exc.Dump();
            }

            return default(T);
        }

        /// <summary>
        /// The to expando object.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="castIfPossible">
        /// The cast if possible.
        /// </param>
        /// <returns>
        /// The <see cref="ExpandoObject"/>.
        /// </returns>
        public static ExpandoObject ToExpandoObject(
            this IDictionary<string, object> source, 
            bool castIfPossible = false)
        {
            Guard.ArgumentNotNull(source, "source");

            if (castIfPossible && source is ExpandoObject)
            {
                return source as ExpandoObject;
            }

            var result = new ExpandoObject();
            result.AddRange(source);

            return result;
        }
    }
}