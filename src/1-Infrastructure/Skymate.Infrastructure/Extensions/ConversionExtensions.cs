// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConversionExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   转换扩展
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text;

    using Skymate.Utilities.Converts;

    /// <summary>
    /// The conversion extensions.
    /// 转换扩展
    /// </summary>
    public static class ConversionExtensions
    {
        /// <summary>
        /// The s_custom type converters.
        /// </summary>
        private readonly static IDictionary<Type, TypeConverter> CustomTypeConverters;

        /// <summary>
        /// Initializes static members of the <see cref="ConversionExtensions"/> class.
        /// </summary>
        static ConversionExtensions()
        {
            /*var intConverter = new GenericListTypeConverter<int>();
            var decConverter = new GenericListTypeConverter<decimal>();
            var stringConverter = new GenericListTypeConverter<string>();
            var soListConverter = new ShippingOptionListTypeConverter();
            var bundleDataListConverter = new ProductBundleDataListTypeConverter();

            CustomTypeConverters = new Dictionary<Type, TypeConverter>();
            CustomTypeConverters.Add(typeof(List<int>), intConverter);
            CustomTypeConverters.Add(typeof(IList<int>), intConverter);
            CustomTypeConverters.Add(typeof(List<decimal>), decConverter);
            CustomTypeConverters.Add(typeof(IList<decimal>), decConverter);
            CustomTypeConverters.Add(typeof(List<string>), stringConverter);
            CustomTypeConverters.Add(typeof(IList<string>), stringConverter);
            */
        }

        #region Object

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Convert<T>(this object value)
        {
            return (T)Convert(value, typeof(T));
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Convert<T>(this object value, CultureInfo culture)
        {
            return (T)Convert(value, typeof(T), culture);
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object Convert(this object value, Type to)
        {
            return value.Convert(to, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="???">
        /// </exception>
        public static object Convert(this object value, Type to, CultureInfo culture)
        {
            Guard.ArgumentNotNull(to, "to");

            if (value == null || to.IsInstanceOfType(value))
            {
                return value;
            }

            // array conversion results in four cases, as below
            Array valueAsArray = value as Array;
            if (to.IsArray)
            {
                Type destinationElementType = to.GetElementType();
                if (valueAsArray != null)
                {
                    // case 1: both destination + source type are arrays, so convert each element
                    IList valueAsList = (IList)valueAsArray;
                    IList converted = Array.CreateInstance(destinationElementType, valueAsList.Count);
                    for (int i = 0; i < valueAsList.Count; i++)
                    {
                        converted[i] = valueAsList[i].Convert(destinationElementType, culture);
                    }

                    return converted;
                }
                else
                {
                    // case 2: destination type is array but source is single element, so wrap element in array + convert
                    object element = value.Convert(destinationElementType, culture);
                    IList converted = Array.CreateInstance(destinationElementType, 1);
                    converted[0] = element;
                    return converted;
                }
            }
            else if (valueAsArray != null)
            {
                // case 3: destination type is single element but source is array, so extract first element + convert
                IList valueAsList = (IList)valueAsArray;
                if (valueAsList.Count > 0)
                {
                    value = valueAsList[0];
                }

                // .. fallthrough to case 4
            }

            // case 4: both destination + source type are single elements, so convert
            Type fromType = value.GetType();

            // if (to.IsInterface || to.IsGenericTypeDefinition || to.IsAbstract)
            // 	throw Error.Argument("to", "Target type '{0}' is not a value type or a non-abstract class.", to.FullName);

            // use Convert.ChangeType if both types are IConvertible
            if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(to))
            {
                if (to.IsEnum)
                {
                    if (value is string)
                    {
                        return Enum.Parse(to, value.ToString(), true);
                    }
                    else if (fromType.IsInteger())
                    {
                        return Enum.ToObject(to, value);
                    }
                }

                return System.Convert.ChangeType(value, to, culture);
            }

            if (value is DateTime && to == typeof(DateTimeOffset))
            {
                return new DateTimeOffset((DateTime)value);
            }

            if (value is string && to == typeof(Guid))
            {
                return new Guid((string)value);
            }

            // see if source or target types have a TypeConverter that converts between the two
            TypeConverter toConverter = GetTypeConverter(fromType);

            Type nonNullableTo = to.GetNonNullableType();
            bool isNullableTo = to != nonNullableTo;

            if (toConverter != null && toConverter.CanConvertTo(nonNullableTo))
            {
                object result = toConverter.ConvertTo(null, culture, value, nonNullableTo);
                return isNullableTo
                           ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(nonNullableTo), result)
                           : result;
            }

            TypeConverter fromConverter = GetTypeConverter(nonNullableTo);

            if (fromConverter != null && fromConverter.CanConvertFrom(fromType))
            {
                object result = fromConverter.ConvertFrom(null, culture, value);
                return isNullableTo
                           ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(nonNullableTo), result)
                           : result;
            }

            // TypeConverter doesn't like Double to Decimal
            if (fromType == typeof(double) && nonNullableTo == typeof(decimal))
            {
                decimal result = new Decimal((double)value);
                return isNullableTo
                           ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(nonNullableTo), result)
                           : result;
            }

            throw Error.InvalidCast(fromType, to);

            // TypeConverter converter = TypeDescriptor.GetConverter(to);
            // bool canConvertFrom = converter.CanConvertFrom(value.GetType());
            // if (!canConvertFrom)
            // {
            // converter = TypeDescriptor.GetConverter(value.GetType());
            // }
            // if (!(canConvertFrom || converter.CanConvertTo(to)))
            // {
            // throw Error.InvalidOperation(@"The parameter conversion from type '{0}' to type '{1}' failed
            // because no TypeConverter can convert between these types.",
            // value.GetType().FullName,
            // to.FullName);
            // }

            // try
            // {
            // CultureInfo cultureToUse = culture ?? CultureInfo.CurrentCulture;
            // object convertedValue = (canConvertFrom) ?
            // converter.ConvertFrom(null /* context */, cultureToUse, value) :
            // converter.ConvertTo(null /* context */, cultureToUse, value, to);
            // return convertedValue;
            // }
            // catch (Exception ex)
            // {
            // throw Error.InvalidOperation(@"The parameter conversion from type '{0}' to type '{1}' failed.
            // See the inner exception for more information.", ex,
            // value.GetType().FullName,
            // to.FullName);
            // }
        }

        /// <summary>
        /// The get type converter.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="TypeConverter"/>.
        /// </returns>
        internal static TypeConverter GetTypeConverter(Type type)
        {
            TypeConverter converter;
            if (CustomTypeConverters.TryGetValue(type, out converter))
            {
                return converter;
            }

            return TypeDescriptor.GetConverter(type);
        }

        #endregion Object

        #region int

        /// <summary>
        /// The to hex.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="char"/>.
        /// </returns>
        public static char ToHex(this int value)
        {
            if (value <= 9)
            {
                return (char)(value + 48);
            }

            return (char)((value - 10) + 97);
        }

        /// <summary>
        /// Returns kilobytes
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ToKb(this int value)
        {
            return value * 1024;
        }

        /// <summary>
        /// Returns megabytes
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ToMb(this int value)
        {
            return value * 1024 * 1024;
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of minutes.
        /// </summary>
        /// <param name="minutes">
        /// number of minutes
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        /// <example>
        /// 3.Minutes()
        /// </example>
        public static TimeSpan ToMinutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of seconds.
        /// </summary>
        /// <param name="seconds">
        /// number of seconds
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        /// <example>
        /// 2.Seconds()
        /// </example>
        public static TimeSpan ToSeconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">
        /// milliseconds for this timespan
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        public static TimeSpan ToMilliseconds(this int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of days.
        /// </summary>
        /// <param name="days">
        /// Number of days.
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        public static TimeSpan ToDays(this int days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of hours.
        /// </summary>
        /// <param name="hours">
        /// Number of hours.
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        public static TimeSpan ToHours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        #endregion int

        #region double

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of minutes.
        /// </summary>
        /// <param name="minutes">
        /// number of minutes
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        /// <example>
        /// 3D.Minutes()
        /// </example>
        public static TimeSpan ToMinutes(this double minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of hours.
        /// </summary>
        /// <param name="hours">
        /// number of hours
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        /// <example>
        /// 3D.Hours()
        /// </example>
        public static TimeSpan ToHours(this double hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of seconds.
        /// </summary>
        /// <param name="seconds">
        /// number of seconds
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        /// <example>
        /// 2D.Seconds()
        /// </example>
        public static TimeSpan ToSeconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">
        /// milliseconds for this timespan
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        public static TimeSpan ToMilliseconds(this double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of days.
        /// </summary>
        /// <param name="days">
        /// Number of days, accurate to the milliseconds.
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that represents a value.
        /// </returns>
        public static TimeSpan ToDays(this double days)
        {
            return TimeSpan.FromDays(days);
        }

        #endregion double

        #region String

        /// <summary>
        /// The to enum.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T ToEnum<T>(this string value, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), value.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        /// <summary>
        /// The to array.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string[] ToArray(this string value)
        {
            return value.ToArray(new[] { ',' });
        }

        /// <summary>
        /// The to array.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string[] ToArray(this string value, params char[] separator)
        {
            return value.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// The to int.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ToInt(this string value, int defaultValue = 0)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// The to float.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public static float ToFloat(this string value, float defaultValue = 0)
        {
            float result;
            if (float.TryParse(value, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// The to bool.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ToBool(this string value, bool defaultValue = false)
        {
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// The to date time.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultValue)
        {
            return value.ToDateTime(null, defaultValue);
        }

        /// <summary>
        /// The to date time.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="formats">
        /// The formats.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime?"/>.
        /// </returns>
        public static DateTime? ToDateTime(this string value, string[] formats, DateTime? defaultValue)
        {
            return value.ToDateTime(formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces, defaultValue);
        }

        /// <summary>
        /// The to date time.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="formats">
        /// The formats.
        /// </param>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="styles">
        /// The styles.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime?"/>.
        /// </returns>
        public static DateTime? ToDateTime(this string value, string[] formats, IFormatProvider provider, DateTimeStyles styles, DateTime? defaultValue)
        {
            DateTime result;

            if (formats.IsNullOrEmpty())
            {
                if (DateTime.TryParse(value, provider, styles, out result))
                {
                    return result;
                }
            }

            if (DateTime.TryParseExact(value, formats, provider, styles, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// The to guid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid ToGuid(this string value)
        {
            if ((!string.IsNullOrEmpty(value)) && (value.Trim().Length == 22))
            {
                string encoded = string.Concat(value.Trim().Replace("-", "+").Replace("_", "/"), "==");

                byte[] base64 = System.Convert.FromBase64String(encoded);

                return new Guid(base64);
            }

            return Guid.Empty;
        }

        /// <summary>
        /// The to byte array.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] ToByteArray(this string value)
        {
            return Encoding.Default.GetBytes(value);
        }

        /// <summary>
        /// The to version.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultVersion">
        /// The default version.
        /// </param>
        /// <returns>
        /// The <see cref="Version"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static Version ToVersion(this string value, Version defaultVersion = null)
        {
            try
            {
                return new Version(value);
            }
            catch
            {
                return defaultVersion ?? new Version("1.0");
            }
        }

        #endregion String

        #region DateTime

        // [...]

        #endregion DateTime

        #region Stream

        /// <summary>
        /// The to byte array.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            Guard.ArgumentNotNull(stream, "stream");

            byte[] buffer;

            if (stream is MemoryStream && stream.CanRead && stream.CanSeek)
            {
                int len = System.Convert.ToInt32(stream.Length);
                buffer = new byte[len];
                stream.Read(buffer, 0, len);
                return buffer;
            }

            MemoryStream memStream = null;
            try
            {
                buffer = new byte[1024];
                memStream = new MemoryStream();
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                if (memStream != null)
                    memStream.Close();
            }

            if (memStream != null)
            {
                return memStream.ToArray();
            }

            return null;
        }

        /// <summary>
        /// The as string.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AsString(this Stream stream)
        {
            // convert memory stream to string
            string result;
            stream.Position = 0;

            using (StreamReader sr = new StreamReader(stream))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        #endregion Stream

        #region ByteArray

        /// <summary>
        /// Converts a byte array into an object.
        /// </summary>
        /// <param name="bytes">
        /// Object to deserialize. May be null.
        /// </param>
        /// <returns>
        /// Deserialized object, or null if input was null.
        /// </returns>
        public static object ToObject(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        /// <summary>
        /// The to stream.
        /// </summary>
        /// <param name="bytes">
        /// The bytes.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public static Stream ToStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// The as string.
        /// </summary>
        /// <param name="bytes">
        /// The bytes.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AsString(this byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// Computes the MD5 hash of a byte array
        /// </summary>
        /// <param name="value">
        /// The byte array to compute the hash for
        /// </param>
        /// <param name="toBase64">
        /// The to Base 64.
        /// </param>
        /// <returns>
        /// The hash value
        /// </returns>
        public static string Hash(this byte[] value, bool toBase64 = false)
        {
            Guard.ArgumentNotNull(value, "value");

            using (MD5 md5 = MD5.Create())
            {
                if (toBase64)
                {
                    byte[] hash = md5.ComputeHash(value);
                    return System.Convert.ToBase64String(hash);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();

                    byte[] hashBytes = md5.ComputeHash(value);
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2").ToLower());
                    }

                    return sb.ToString();
                }
            }
        }

        #endregion ByteArray

        #region Enumerable: Collections/List/Dictionary...

        /// <summary>
        /// The to object.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T ToObject<T>(this IDictionary<string, object> values) where T : class
        {
            return (T)values.ToObject(typeof(T));
        }

        /// <summary>
        /// The to object.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="objectType">
        /// The object type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="???">
        /// </exception>
        public static object ToObject(this IDictionary<string, object> values, Type objectType)
        {
            Guard.ArgumentNotEmpty(values, "values");
            Guard.ArgumentNotNull(objectType, "objectType");

            if (!DictionaryConverter.CanCreateType(objectType))
            {
                throw Error.Argument(
                    "objectType",
                    "The type '{0}' must be a class and have a parameterless default constructor in order to deserialize properly.",
                    objectType.FullName);
            }

            return DictionaryConverter.SafeCreateAndPopulate(objectType, values);
        }

        #endregion
    }
}