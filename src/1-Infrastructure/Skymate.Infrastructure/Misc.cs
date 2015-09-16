// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Misc.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System;

    /// <summary>
    /// The misc.
    /// </summary>
    internal static class Misc
    {
        /// <summary>
        /// The try action.
        /// </summary>
        /// <param name="func">
        /// The func.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryAction<T>(Func<T> func, out T output)
        {
            Guard.ArgumentNotNull(() => func);

            try
            {
                output = func();
                return true;
            }
            catch
            {
                output = default(T);
                return false;
            }
        }

        /// <summary>
        /// Perform an action if the string is not null or empty.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="action">
        /// The action to perform.
        /// </param>
        public static void IfNotNullOrEmpty(string value, Action<string> action)
        {
            IfNotNullOrEmpty(value, action, null);
        }

        /// <summary>
        /// The if not null or empty.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="trueAction">
        /// The true action.
        /// </param>
        /// <param name="falseAction">
        /// The false action.
        /// </param>
        private static void IfNotNullOrEmpty(string value, Action<string> trueAction, Action<string> falseAction)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (trueAction != null)
                {
                    trueAction(value);
                }
            }
            else
            {
                if (falseAction != null)
                {
                    falseAction(value);
                }
            }
        }
    }
}
