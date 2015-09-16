// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Skymate;

    /// <summary>
    /// The list extensions.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// The to separated string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToSeparatedString<T>(this IList<T> value)
        {
            return ToSeparatedString(value, ",");
        }

        /// <summary>
        /// The to separated string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToSeparatedString<T>(this IList<T> value, string separator)
        {
            if (value.Count == 0)
            {
                return string.Empty;
            }

            if (value.Count == 1)
            {
                return value[0] != null ? value[0].ToString() : string.Empty;
            }

            var builder = new StringBuilder();
            var flag = true;
            var flag2 = false;
            foreach (object obj2 in value)
            {
                if (!flag)
                {
                    builder.Append(separator);
                }

                if (obj2 != null)
                {
                    builder.Append(obj2.ToString().TrimEnd(new char[0]));
                    flag2 = true;
                }

                flag = false;
            }

            if (!flag2)
            {
                return string.Empty;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Makes a slice of the specified list in between the start and end indexes.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="start">
        /// The start index.
        /// </param>
        /// <param name="end">
        /// The end index.
        /// </param>
        /// <returns>
        /// A slice of the list.
        /// </returns>
        public static IList<T> Slice<T>(this IList<T> list, int? start, int? end)
        {
            return list.Slice(start, end, null);
        }

        /// <summary>
        /// Makes a slice of the specified list in between the start and end indexes,
        /// getting every so many items based upon the step.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="start">
        /// The start index.
        /// </param>
        /// <param name="end">
        /// The end index.
        /// </param>
        /// <param name="step">
        /// The step.
        /// </param>
        /// <returns>
        /// A slice of the list.
        /// </returns>
        public static IList<T> Slice<T>(this IList<T> list, int? start, int? end, int? step)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (step == 0)
            {
                throw Error.Argument("step", "Step cannot be zero.");
            }

            var slicedList = new List<T>();

            if (list.Count == 0)
            {
                return slicedList;
            }

            var s = step ?? 1;
            var startIndex = start ?? 0;
            var endIndex = end ?? list.Count;

            startIndex = (startIndex < 0) ? list.Count + startIndex : startIndex;

            endIndex = (endIndex < 0) ? list.Count + endIndex : endIndex;

            startIndex = Math.Max(startIndex, 0);
            endIndex = Math.Min(endIndex, list.Count - 1);

            for (var i = startIndex; i < endIndex; i += s)
            {
                slicedList.Add(list[i]);
            }

            return slicedList;
        }
    }
}
