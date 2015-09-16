// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//    Enumerable扩展
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    using Skymate.Collections;

    /// <summary>
    /// The enumerable extensions.
    /// <p>
    /// Enumerable扩展
    /// </p>
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Nested classes

        /// <summary>
        /// The default read only collection.
        /// </summary>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        private static class DefaultReadOnlyCollection<T>
        {
            /// <summary>
            /// The default collection.
            /// </summary>
            private static ReadOnlyCollection<T> defaultCollection;

            /// <summary>
            /// Gets the empty.
            /// </summary>
            internal static ReadOnlyCollection<T> Empty
            {
                get
                {
                    return EnumerableExtensions.DefaultReadOnlyCollection<T>.defaultCollection
                           ?? (EnumerableExtensions.DefaultReadOnlyCollection<T>.defaultCollection =
                               new ReadOnlyCollection<T>(new T[0]));
                }
            }
        }

        #endregion Nested classes

        #region IEnumerable

        /// <summary>
        /// The status.
        /// </summary>
        private class Status
        {
            /// <summary>
            /// The end of sequence.
            /// </summary>
            public bool EndOfSequence;
        }

        /// <summary>
        /// The take on enumerator.
        /// </summary>
        /// <param name="enumerator">
        /// The enumerator.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private static IEnumerable<T> TakeOnEnumerator<T>(IEnumerator<T> enumerator, int count, Status status)
        {
            while (--count > 0 && (enumerator.MoveNext() || !(status.EndOfSequence = true)))
            {
                yield return enumerator.Current;
            }
        }

        /// <summary>
        /// Slices the iteration over an enumerable by the given chunk size.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="items">
        /// </param>
        /// <param name="chunkSize">
        /// SIze of chunk
        /// </param>
        /// <returns>
        /// The sliced enumerable
        /// </returns>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> items, int chunkSize = 100)
        {
            if (chunkSize < 1)
            {
                throw new ArgumentException("Chunks should not be smaller than 1 element");
            }

            var status = new Status { EndOfSequence = false };
            using (var enumerator = items.GetEnumerator())
            {
                while (!status.EndOfSequence)
                {
                    yield return TakeOnEnumerator(enumerator, chunkSize, status);
                }
            }
        }

        /// <summary>
        /// Performs an action on each item while iterating through a list.
        /// This is a handy shortcut for <c>foreach(item in list) { ... }</c>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the items.
        /// </typeparam>
        /// <param name="source">
        /// The list, which holds the objects.
        /// </param>
        /// <param name="action">
        /// The action delegate which is called on each item while iterating.
        /// </param>
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T t in source)
            {
                action(t);
            }
        }

        /// <summary>
        /// Performs an action on each item while iterating through a list.
        /// This is a handy shortcut for <c>foreach(item in list) { ... }</c>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the items.
        /// </typeparam>
        /// <param name="source">
        /// The list, which holds the objects.
        /// </param>
        /// <param name="action">
        /// The action delegate which is called on each item while iterating.
        /// </param>
        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (T t in source)
            {
                action(t, i++);
            }
        }

        /// <summary>
        /// The cast valid.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<T> CastValid<T>(this IEnumerable source)
        {
            return source.Cast<object>().Where(o => o is T).Cast<T>();
        }

        /// <summary>
        /// The as read only.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="ReadOnlyCollection{T}"/>.
        /// </returns>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
                return DefaultReadOnlyCollection<T>.Empty;

            var readOnly = source as ReadOnlyCollection<T>;
            if (readOnly != null)
            {
                return readOnly;
            }

            var list = source as List<T>;
            if (list != null)
            {
                return list.AsReadOnly();
            }

            return new ReadOnlyCollection<T>(source.ToArray());
        }

        /// <summary>
        /// The order by ordinal.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<T> OrderByOrdinal<T>(this IEnumerable<T> source)
            where T : IOrdered
        {
            return source.OrderByOrdinal(false);
        }

        /// <summary>
        /// The order by ordinal.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="descending">
        /// The descending.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<T> OrderByOrdinal<T>(this IEnumerable<T> source, bool descending)
            where T : IOrdered
        {
            if (!descending)
                return source.OrderBy(x => x.Ordinal);
            else
                return source.OrderByDescending(x => x.Ordinal);
        }

        #endregion IEnumerable

        #region Multimap

        /// <summary>
        /// The to multimap.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="keySelector">
        /// The key selector.
        /// </param>
        /// <param name="valueSelector">
        /// The value selector.
        /// </param>
        /// <typeparam name="TSource">
        /// </typeparam>
        /// <typeparam name="TKey">
        /// </typeparam>
        /// <typeparam name="TValue">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Multimap{TKey,TValue}"/>.
        /// </returns>
        public static Multimap<TKey, TValue> ToMultimap<TSource, TKey, TValue>(
                                                this IEnumerable<TSource> source,
                                                Func<TSource, TKey> keySelector,
                                                Func<TSource, TValue> valueSelector)
        {
            Guard.ArgumentNotNull(() => source);
            Guard.ArgumentNotNull(() => keySelector);
            Guard.ArgumentNotNull(() => valueSelector);

            var map = new Multimap<TKey, TValue>();

            foreach (var item in source)
            {
                map.Add(keySelector(item), valueSelector(item));
            }

            return map;
        }

        #endregion Multimap

        #region NameValueCollection

        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="initial">
        /// The initial.
        /// </param>
        /// <param name="other">
        /// The other.
        /// </param>
        public static void AddRange(this NameValueCollection initial, NameValueCollection other)
        {
            Guard.ArgumentNotNull(initial, "initial");
            if (other == null)
                return;

            foreach (var item in other.AllKeys)
            {
                initial.Add(item, other[item]);
            }
        }

        #endregion NameValueCollection

        #region AsSerializable

        /// <summary>
        /// Convenience API to allow an IEnumerable[T] (such as returned by Linq2Sql, NHibernate, EF etc.)
        /// to be serialized by DataContractSerializer.
        /// </summary>
        /// <typeparam name="T">
        /// The type of item.
        /// </typeparam>
        /// <param name="source">
        /// The original collection.
        /// </param>
        /// <returns>
        /// A serializable enumerable wrapper.
        /// </returns>
        public static IEnumerable<T> AsSerializable<T>(this IEnumerable<T> source) where T : class
        {
            return new IEnumerableWrapper<T>(source);
        }

        /// <summary>
        /// The i enumerable wrapper.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        private class IEnumerableWrapper<T> : IEnumerable<T>
            where T : class
        {
            /// <summary>
            /// The _collection.
            /// </summary>
            private IEnumerable<T> _collection;

            // The DataContractSerilizer needs a default constructor to ensure the object can be
            // deserialized. We have a dummy one since we don't actually need deserialization.
            /// <summary>
            /// Initializes a new instance of the <see cref="IEnumerableWrapper{T}"/> class.
            /// </summary>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public IEnumerableWrapper()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="IEnumerableWrapper{T}"/> class.
            /// </summary>
            /// <param name="collection">
            /// The collection.
            /// </param>
            internal IEnumerableWrapper(IEnumerable<T> collection)
            {
                this._collection = collection;
            }

            // The DataContractSerilizer needs an Add method to ensure the object can be
            // deserialized. We have a dummy one since we don't actually need deserialization.
            /// <summary>
            /// The add.
            /// </summary>
            /// <param name="item">
            /// The item.
            /// </param>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public void Add(T item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// The get enumerator.
            /// </summary>
            /// <returns>
            /// The <see cref="IEnumerator"/>.
            /// </returns>
            public IEnumerator<T> GetEnumerator()
            {
                return this._collection.GetEnumerator();
            }

            /// <summary>
            /// The get enumerator.
            /// </summary>
            /// <returns>
            /// The <see cref="IEnumerator"/>.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this._collection).GetEnumerator();
            }
        }

        #endregion AsSerializable
    }
}