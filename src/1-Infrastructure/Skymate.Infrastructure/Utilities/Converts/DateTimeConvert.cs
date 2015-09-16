// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeConvert.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   日期转换
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities.Converts
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Converts between <see cref="DateTime"/>s and <see langword="string"/>s.
    /// 在日期类型和字符串之间转换
    /// </summary>
    /// <remarks>
    /// Accepted formats for parsing are "dd MMM yyyy HH:mm:ss.ff", "yyyy-MM-ddTHH:mm:ss", "dd MMM yyyy hh:mm tt", "dd MMM yyyy hh:mm:ss tt", "dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm" and "dd MMM yyyy".
    /// 接收字符串的类型可以是
    /// "dd MMM yyyy HH:mm:ss.ff", 
    /// "yyyy-MM-ddTHH:mm:ss", 
    /// "dd MMM yyyy hh:mm tt", 
    /// "dd MMM yyyy hh:mm:ss tt", 
    /// "dd MMM yyyy HH:mm:ss", 
    /// "dd MMM yyyy HH:mm" and 
    /// "dd MMM yyyy".
    /// </remarks>
    public static class DateTimeConvert
    {
        #region Properties

        /// <summary>
        /// The default format used by <see cref="ToString(DateTime)"/> and <see cref="ToString(Nullable{DateTime})"/>.
        /// 默认格式化字符串
        /// </summary>
        public static readonly string DateTimeFormat = "dd MMM yyyy HH:mm:ss.ff";

        /// <summary>
        /// The supported formats used to parse strings.
        /// 支持的字符串格式
        /// </summary>
        private static readonly string[] ParseFormats =
            {
                DateTimeFormat, "s", 
                "dd MMM yyyy hh:mm tt", 
                "dd MMM yyyy hh:mm:ss tt", 
                "dd MMM yyyy HH:mm:ss", 
                "dd MMM yyyy HH:mm", 
                "dd MMM yyyy"
            };

        #endregion

        #region Methods

        /// <summary>
        /// Converts the specified string representation of a date and time to its <see cref="DateTime"/> equivalent. 
        /// 转换字符串到日期
        /// </summary>
        /// <remarks>
        /// Accepted formats for parsing are "dd MMM yyyy HH:mm:ss.ff", "yyyy-MM-ddTHH:mm:ss", "dd MMM yyyy hh:mm tt", "dd MMM yyyy hh:mm:ss tt", "dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm" and "dd MMM yyyy". <see cref="DateTime.ParseExact(string,string[],IFormatProvider,DateTimeStyles)"/> is used to attempt to parse <paramref>
        ///         <name>s</name>
        ///     </paramref>
        ///     .
        /// 接受字符串格式：
        /// "dd MMM yyyy HH:mm:ss.ff", 
        /// "yyyy-MM-ddTHH:mm:ss", 
        /// "dd MMM yyyy hh:mm tt", 
        /// "dd MMM yyyy hh:mm:ss tt", 
        /// "dd MMM yyyy HH:mm:ss", 
        /// "dd MMM yyyy HH:mm" and 
        /// "dd MMM yyyy"
        /// </remarks>
        /// <param name="date">
        /// The date.
        /// 日期
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> equivalent to the date and time contained in <paramref>
        ///         <name>s</name>
        ///     </paramref>
        ///     .
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref>
        ///     <name>s</name>
        /// </paramref>
        ///     cannot be parsed.
        /// 转换不支持
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref>
        ///     <name>s</name>
        /// </paramref>
        ///     is a null reference.
        /// 空引用
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 参数异常
        /// <paramref>
        ///     <name>s</name>
        /// </paramref>
        ///     is <see cref="string.Empty"/>.
        /// </exception>
        public static DateTime Parse(string date)
        {
            try
            {
                return DateTime.ParseExact(date, ParseFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
            }
            catch (FormatException formatException)
            {
                throw new FormatException(string.Format("{0}. Value = {1}", formatException.Message, date), formatException);
            }
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its <see cref="Nullable"/> <see cref="DateTime"/> equivalent. 
        /// 转换字符串到日期
        /// </summary>
        /// <param name="s">
        /// A string containing a date and (optionally) time to convert.
        /// </param>
        /// <returns>
        /// <see langword="null"/> 
        /// if <paramref name="s"/> 
        /// is <see cref="string.IsNullOrEmpty"/>; 
        /// otherwise the value returned from <see cref="Parse"/>.
        /// </returns>
        public static DateTime? ParseNullable(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return Parse(s);
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to its equivalent string representation. 
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="DateTime"/> to convert.
        /// </param>
        /// <returns>
        /// A string representation <paramref name="dateTime"/>.
        /// </returns>
        public static string ToString(DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }

        /// <summary>
        /// Converts a <see cref="Nullable"/> <see cref="DateTime"/> to its equivalent string representation. 
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="Nullable"/> <see cref="DateTime"/> to convert.
        /// </param>
        /// <returns>
        /// <see langword="null"/> if A string representation <paramref name="dateTime"/>.
        /// </returns>
        public static string ToString(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return ToString(dateTime.Value);
            }

            return string.Empty;
        }

        #endregion
    }
}