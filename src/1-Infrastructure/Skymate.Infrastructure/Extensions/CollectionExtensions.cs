// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   集合扩展
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The collection extensions.
    /// 集合扩展
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// The add range.
        /// 添加范围
        /// </summary>
        /// <param name="initial">
        /// The initial.
        /// 最初
        /// </param>
        /// <param name="other">
        /// The other.
        /// 需要添加的集合
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        public static void AddRange<T>(this ICollection<T> initial, IEnumerable<T> other)
        {
            if (other == null)
            {
                return;
            }

            var list = initial as List<T>;

            if (list != null)
            {
                list.AddRange(other);
                return;
            }

            other.Each(initial.Add);
        }

        /// <summary>
        /// The is null or empty.
        /// 判断是否为非空
        /// </summary>
        /// <param name="source">
        /// The source.
        /// 原
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count == 0;
        }

        /// <summary>
        /// The equals all.
        /// 相等
        /// </summary>
        /// <param name="a">
        /// The a.
        /// 集合a
        /// </param>
        /// <param name="b">
        /// The b.
        /// 集合b
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool EqualsAll<T>(this IList<T> a, IList<T> b)
        {
            if (a == null || b == null)
            {
                return a == null && b == null;
            }

            if (a.Count != b.Count)
            {
                return false;
            }

            var comparer = EqualityComparer<T>.Default;

            return !a.Where((t, i) => !comparer.Equals(t, b[i])).Any();
        }
    }
}