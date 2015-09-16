// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MiscExtensions.cs" company="Skymate">
//      Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// The misc extensions.
    /// </summary>
    public static class MiscExtensions
    {
        /// <summary>
        /// The dump.
        /// </summary>
        /// <param name="exc">
        /// The exc.
        /// </param>
        public static void Dump(this Exception exc)
        {
            try
            {
                exc.StackTrace.Dump();
                exc.Message.Dump();
            }
            catch
            {
            }
        }

        /// <summary>
        /// The to all messages.
        /// </summary>
        /// <param name="exc">
        /// The exc.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToAllMessages(this Exception exc)
        {
            var sb = new StringBuilder();

            while (exc != null)
            {
                sb.Grow(exc.Message, " ");
                exc = exc.InnerException;
            }

            return sb.ToString();
        }

        /// <summary>
        /// The to elapsed minutes.
        /// </summary>
        /// <param name="watch">
        /// The watch.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToElapsedMinutes(this Stopwatch watch)
        {
            return "{0:0.0}".FormatWith(TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalMinutes);
        }

        /// <summary>
        /// The to elapsed seconds.
        /// </summary>
        /// <param name="watch">
        /// The watch.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToElapsedSeconds(this Stopwatch watch)
        {
            return "{0:0.0}".FormatWith(TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalSeconds);
        }

        /// <summary>
        /// The has column.
        /// </summary>
        /// <param name="dv">
        /// The dv.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool HasColumn(this DataView dv, string columnName)
        {
            dv.RowFilter = "ColumnName='" + columnName + "'";
            return dv.Count > 0;
        }

        /// <summary>
        /// The get data type.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDataType(this DataTable dt, string columnName)
        {
            dt.DefaultView.RowFilter = "ColumnName='" + columnName + "'";
            return dt.Rows[0]["DataType"].ToString();
        }

        /// <summary>
        /// The count execute.
        /// </summary>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sqlCount">
        /// The sql count.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int CountExecute(this OleDbConnection conn, string sqlCount)
        {
            using (OleDbCommand cmd = new OleDbCommand(sqlCount, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// The safe convert.
        /// </summary>
        /// <param name="converter">
        /// The converter.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object SafeConvert(this TypeConverter converter, string value)
        {
            try
            {
                if (converter != null && value.HasValue() && converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFromString(value);
                }
            }
            catch (Exception exc)
            {
                exc.Dump();
            }

            return null;
        }

        /// <summary>
        /// The is equal.
        /// </summary>
        /// <param name="converter">
        /// The converter.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="compareWith">
        /// The compare with.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsEqual(this TypeConverter converter, string value, object compareWith)
        {
            object convertedObject = converter.SafeConvert(value);

            if (convertedObject != null && compareWith != null)
            {
                return convertedObject.Equals(compareWith);
            }

            return false;
        }

        /// <summary>
        /// The is null or default.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        /// <summary>
        /// Converts bytes into a hex string.
        /// </summary>
        /// <param name="bytes">
        /// The bytes.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToHexString(this byte[] bytes, int length = 0)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));

                if (length > 0 && sb.Length >= length)
                {
                    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// The get merged data value.
        /// </summary>
        /// <param name="mergedData">
        /// The merged data.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T GetMergedDataValue<T>(this IMergedData mergedData, string key, T defaultValue)
        {
            try
            {
                if (mergedData.MergedDataValues != null && !mergedData.MergedDataIgnore)
                {
                    object value;

                    if (mergedData.MergedDataValues.TryGetValue(key, out value))
                        return (T)value;
                }
            }
            catch (Exception) { }

            return defaultValue;
        }

        /// <summary>
        /// Append grow if string builder is empty. Append delimiter and grow otherwise.
        /// </summary>
        /// <param name="sb">
        /// Target string builder
        /// </param>
        /// <param name="grow">
        /// Value to append
        /// </param>
        /// <param name="delimiter">
        /// Delimiter to use
        /// </param>
        public static void Grow(this StringBuilder sb, string grow, string delimiter)
        {
            Guard.ArgumentNotNull(delimiter, "delimiter");

            if (!string.IsNullOrWhiteSpace(grow))
            {
                if (sb.Length <= 0)
                    sb.Append(grow);
                else
                    sb.AppendFormat("{0}{1}", delimiter, grow);
            }
        }

        /// <summary>
        /// The safe get.
        /// </summary>
        /// <param name="arr">
        /// The arr.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string SafeGet(this string[] arr, int index)
        {
            return arr != null && index < arr.Length ? arr[index] : string.Empty;
        }
    }
}