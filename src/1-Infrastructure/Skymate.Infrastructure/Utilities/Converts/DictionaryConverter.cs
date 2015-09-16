// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryConverter.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   字典转换
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities.Converts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Fasterflect;

    using Skymate.Extensions;

    /// <summary>
    /// The dictionary converter.
    /// 字典转换
    /// </summary>
    public static class DictionaryConverter
    {
        /// <summary>
        /// The can create type.
        /// 判断是否可创建类型
        /// </summary>
        /// <param name="itemType">
        /// The item type.
        /// 项目类型
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>bool</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static bool CanCreateType(Type itemType)
        {
            return itemType.IsClass && itemType.GetConstructor(Type.EmptyTypes) != null;
        }

        /// <summary>
        /// The create and populate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// 源
        /// </param>
        /// <param name="problems">
        /// The problems.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T CreateAndPopulate<T>(IDictionary<string, object> source, out ICollection<ConvertProblem> problems)
            where T : class, new()
        {
            return (T)CreateAndPopulate(typeof(T), source, out problems);
        }

        /// <summary>
        /// The create and populate.
        /// </summary>
        /// <param name="targetType">
        /// The target type.
        /// 目标类型
        /// </param>
        /// <param name="source">
        /// The source.
        /// 来源类型
        /// </param>
        /// <param name="problems">
        /// The problems.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object CreateAndPopulate(Type targetType, IDictionary<string, object> source, out ICollection<ConvertProblem> problems)
        {
            Guard.ArgumentNotNull(() => targetType);

            var target = targetType.CreateInstance();

            Populate(source, target, out problems);

            return target;
        }

        /// <summary>
        /// The safe create and populate.
        /// </summary>
        /// <param name="targetType">
        /// The target type.
        /// 目标类型
        /// </param>
        /// <param name="source">
        /// The source.
        /// 源类型
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="DictionaryConvertException">
        /// 字典异常
        /// </exception>
        public static object SafeCreateAndPopulate(Type targetType, IDictionary<string, object> source)
        {
            ICollection<ConvertProblem> problems;
            var item = CreateAndPopulate(targetType, source, out problems);

            if (problems.Count > 0)
            {
                throw new DictionaryConvertException(problems);
            }

            return item;
        }

        /// <summary>
        /// The safe create and populate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// 源类型
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T SafeCreateAndPopulate<T>(IDictionary<string, object> source)
            where T : class, new()
        {
            return (T)SafeCreateAndPopulate(typeof(T), source);
        }

        /// <summary>
        /// The populate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// 源类型
        /// </param>
        /// <param name="target">
        /// The target.
        /// 目标类型
        /// </param>
        /// <param name="populated">
        /// The populated.
        /// </param>
        /// <exception cref="DictionaryConvertException">
        /// 字典异常
        /// </exception>
        public static void Populate(IDictionary<string, object> source, object target, params object[] populated)
        {
            ICollection<ConvertProblem> problems;

            Populate(source, target, out problems, populated);

            if (problems.Count > 0)
            {
                throw new DictionaryConvertException(problems);
            }
        }

        /// <summary>
        /// The populate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// 源类型
        /// </param>
        /// <param name="target">
        /// The target.
        /// 目标类型
        /// </param>
        /// <param name="problems">
        /// The problems.
        /// </param>
        /// <param name="populated">
        /// The populated.
        /// </param>
        public static void Populate(IDictionary<string, object> source, object target, out ICollection<ConvertProblem> problems, params object[] populated)
        {
            Guard.ArgumentNotNull(() => source);
            Guard.ArgumentNotNull(() => target);

            problems = new List<ConvertProblem>();

            if (populated.Any(x => x == target))
            {
                return;
            }

            var t = target.GetType();

            if (source == null)
            {
                return;
            }

            foreach (var pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                object value;

                if (!pi.PropertyType.IsPredefinedSimpleType() && source.TryGetValue(pi.Name, out value) && value is IDictionary<string, object>)
                {
                    var nestedValue = target.GetPropertyValue(pi.Name);
                    ICollection<ConvertProblem> nestedProblems;

                    populated = populated.Concat(new[] { target }).ToArray();
                    Populate((IDictionary<string, object>)value, nestedValue, out nestedProblems, populated);

                    if (nestedProblems != null && nestedProblems.Any())
                    {
                        problems.AddRange(nestedProblems);
                    }

                    WriteToProperty(target, pi, nestedValue, problems);
                }
                else if (pi.PropertyType.IsArray && !source.ContainsKey(pi.Name))
                {
                    WriteToProperty(target, pi, RetrieveArrayValues(pi, source, problems), problems);
                }
                else
                {
                    if (source.TryGetValue(pi.Name, out value))
                    {
                        WriteToProperty(target, pi, value, problems);
                    }
                }
            }
        }

        /// <summary>
        /// The retrieve array values.
        /// </summary>
        /// <param name="arrayProp">
        /// The array prop.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="problems">
        /// The problems.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        private static object RetrieveArrayValues(PropertyInfo arrayProp, IDictionary<string, object> source, ICollection<ConvertProblem> problems)
        {
            var elemType = arrayProp.PropertyType.GetElementType();
            var anyValuesFound = true;
            var idx = 0;

            var elements = (IList)typeof(List<>).MakeGenericType(elemType).CreateInstance();

            var properties = elemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (anyValuesFound)
            {
                object curElement = null;
                anyValuesFound = false; // false until proven otherwise

                foreach (var pi in properties)
                {
                    var key = string.Format("{0}[{1}].{2}", arrayProp.Name, idx, pi.Name);
                    object value;

                    if (source.TryGetValue(key, out value))
                    {
                        anyValuesFound = true;

                        if (curElement == null)
                        {
                            curElement = elemType.CreateInstance();
                            elements.Add(curElement);
                        }

                        SetPropFromValue(value, curElement, pi, problems);
                    }
                }

                idx++;
            }

            var elementArray = Array.CreateInstance(elemType, elements.Count);
            elements.CopyTo(elementArray, 0);

            return elementArray;
        }

        /// <summary>
        /// The set prop from value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="pi">
        /// The pi.
        /// </param>
        /// <param name="problems">
        /// The problems.
        /// </param>
        private static void SetPropFromValue(object value, object item, PropertyInfo pi, ICollection<ConvertProblem> problems)
        {
            WriteToProperty(item, pi, value, problems);
        }

        /// <summary>
        /// The write to property.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="pi">
        /// The pi.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="problems">
        /// The problems.
        /// </param>
        private static void WriteToProperty(object item, PropertyInfo pi, object value, ICollection<ConvertProblem> problems)
        {
            if (!pi.CanWrite)
            {
                return;
            }

            try
            {
                if (value == null || Equals(value, string.Empty))
                {
                    return;
                }

                var destType = pi.PropertyType;

                if (destType == typeof(bool) && Equals(value, pi.Name))
                {
                    value = true;
                }

                if (pi.PropertyType.IsInstanceOfType(value))
                {
                     item.SetPropertyValue(pi.Name, value);
                    return;
                }

                if (pi.PropertyType.IsNullable())
                {
                    destType = pi.PropertyType.GetGenericArguments()[0];
                }

                item.SetPropertyValue(pi.Name, value.Convert(destType));
            }
            catch (Exception ex)
            {
                problems.Add(new ConvertProblem { Item = item, Property = pi, AttemptedValue = value, Exception = ex });
            }
        }
    }
}
