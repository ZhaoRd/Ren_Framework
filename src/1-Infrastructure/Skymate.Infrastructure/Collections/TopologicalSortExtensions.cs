// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopologicalSortExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   TopologicalSort类型扩展.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Collections
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TopologicalSort类型扩展.
    /// </summary>
    public static class TopologicalSortExtensions
    {
        /// <summary>
        /// 拓扑.
        /// </summary>
        /// <param name="items">
        /// 项.
        /// </param>
        /// <typeparam name="T">
        /// 类型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>ITopologicSortable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static ITopologicSortable<T>[] SortTopological<T>(this ITopologicSortable<T>[] items)
        {
            return SortTopological(items, null);
        }

        /// <summary>
        /// 拓扑.
        /// </summary>
        /// <param name="items">
        /// 项.
        /// </param>
        /// <param name="comparer">
        /// 比较.
        /// </param>
        /// <typeparam name="T">
        /// 类型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>ITopologicSortable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static ITopologicSortable<T>[] SortTopological<T>(this ITopologicSortable<T>[] items, IEqualityComparer<T> comparer)
        {
            Guard.ArgumentNotNull(() => items);

            var sortedIndexes = SortIndexesTopological(items, comparer);
            var sortedList = new List<ITopologicSortable<T>>(sortedIndexes.Length);
            sortedList.AddRange(sortedIndexes.Select(t => items[t]));

            return sortedList.ToArray();
        }

        /// <summary>
        /// The sort indexes topological.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="int[]"/>.
        /// </returns>
        public static int[] SortIndexesTopological<T>(this ITopologicSortable<T>[] items)
        {
            return SortIndexesTopological(items, null);
        }

        /// <summary>
        /// The sort indexes topological.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="int[]"/>.
        /// </returns>
        public static int[] SortIndexesTopological<T>(this ITopologicSortable<T>[] items, IEqualityComparer<T> comparer)
        {
            Guard.ArgumentNotNull(() => items);

            if (items.Length == 0)
            {
                return new int[] { };
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            var sorter = new TopologicalSorter(items.Length);
            var indexes = new Dictionary<T, int>(comparer);

            // add vertices
            for (int i = 0; i < items.Length; i++)
            {
                indexes[items[i].Key] = sorter.AddVertex(i);
            }

            // add edges
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].DependsOn != null)
                {
                    for (int j = 0; j < items[i].DependsOn.Length; j++)
                    {
                        if (indexes.ContainsKey(items[i].DependsOn[j]))
                        {
                            sorter.AddEdge(i, indexes[items[i].DependsOn[j]]);
                        }
                    }
                }
            }

            int[] result = sorter.Sort().Reverse().ToArray();
            return result;
        }
    }
}