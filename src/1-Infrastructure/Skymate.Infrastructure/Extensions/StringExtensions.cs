// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    using Skymate.Expressions;

    using Skymate;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The carriage return line feed.
        /// </summary>
        public const string CarriageReturnLineFeed = "\r\n";

        /// <summary>
        /// The empty.
        /// </summary>
        public const string Empty = "";

        /// <summary>
        /// The carriage return.
        /// </summary>
        public const char CarriageReturn = '\r';

        /// <summary>
        /// The line feed.
        /// </summary>
        public const char LineFeed = '\n';

        /// <summary>
        /// The tab.
        /// </summary>
        public const char Tab = '\t';

        /// <summary>
        /// The action line.
        /// </summary>
        /// <param name="textWriter">
        /// The text writer.
        /// </param>
        /// <param name="line">
        /// The line.
        /// </param>
        private delegate void ActionLine(TextWriter textWriter, string line);

        #region Char extensions

        /// <summary>
        /// The to int.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static int ToInt(this char value)
        {
            if ((value >= '0') && (value <= '9'))
            {
                return value - '0';
            }

            if ((value >= 'a') && (value <= 'f'))
            {
                return (value - 'a') + 10;
            }

            if ((value >= 'A') && (value <= 'F'))
            {
                return (value - 'A') + 10;
            }

            return -1;
        }

        /// <summary>
        /// The to unicode.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string ToUnicode(this char c)
        {
            using (StringWriter w = new StringWriter(CultureInfo.InvariantCulture))
            {
                WriteCharAsUnicode(c, w);
                return w.ToString();
            }
        }

        /// <summary>
        /// The write char as unicode.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        internal static void WriteCharAsUnicode(char c, TextWriter writer)
        {
            Guard.ArgumentNotNull(writer, "writer");

            char h1 = ((c >> 12) & '\x000f').ToHex();
            char h2 = ((c >> 8) & '\x000f').ToHex();
            char h3 = ((c >> 4) & '\x000f').ToHex();
            char h4 = (c & '\x000f').ToHex();

            writer.Write('\\');
            writer.Write('u');
            writer.Write(h1);
            writer.Write(h2);
            writer.Write(h3);
            writer.Write(h4);
        }

        #endregion

        #region String extensions

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
        [DebuggerStepThrough]
        public static T ToEnum<T>(this string value, T defaultValue)
        {
            if (!value.HasValue())
            {
                return defaultValue;
            }

            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// The to safe.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string ToSafe(this string value, string defaultValue = null)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            return defaultValue ?? string.Empty;
        }

        /// <summary>
        /// The empty null.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string EmptyNull(this string value)
        {
            return (value ?? string.Empty).Trim();
        }

        /// <summary>
        /// The null empty.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string NullEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        /// <summary>
        /// Formats a string to an invariant culture
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="objects">
        /// The objects.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string FormatInvariant(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.InvariantCulture, format, objects);
        }

        /// <summary>
        /// Formats a string to the current culture.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="objects">
        /// The objects.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string FormatCurrent(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentCulture, format, objects);
        }

        /// <summary>
        /// Formats a string to the current UI culture.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="objects">
        /// The objects.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string FormatCurrentUI(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentUICulture, format, objects);
        }

        /// <summary>
        /// The format with.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string FormatWith(this string format, params object[] args)
        {
            return FormatWith(format, CultureInfo.CurrentCulture, args);
        }

        /// <summary>
        /// The format with.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }

        /// <summary>
        /// Determines whether this instance and another specified System.String object have the same value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="comparing">
        /// The comparing with string.
        /// </param>
        /// <returns>
        /// <c>true</c> if the value of the comparing parameter is the same as this string; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsCaseSensitiveEqual(this string value, string comparing)
        {
            return string.CompareOrdinal(value, comparing) == 0;
        }

        /// <summary>
        /// Determines whether this instance and another specified System.String object have the same value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="comparing">
        /// The comparing with string.
        /// </param>
        /// <returns>
        /// <c>true</c> if the value of the comparing parameter is the same as this string; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsCaseInsensitiveEqual(this string value, string comparing)
        {
            return string.Compare(value, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Determines whether the string is null, empty or all whitespace.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsEmpty(this string value)
        {
			return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Determines whether the string is all white space. Empty string will return false.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// <c>true</c> if the string is all white space; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsWhiteSpace(this string value)
        {
            Guard.ArgumentNotNull(value, "value");

            if (value.Length == 0)
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// The has value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

		/// <summary>
		/// The hash.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="encoding">
		/// The encoding.
		/// </param>
		/// <param name="toBase64">
		/// The to Base 64.
		/// </param>
		/// <remarks>
		/// to get equivalent result to PHPs md5 function call Hash("my value", Encoding.ASCII, false).
		/// </remarks>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
        [DebuggerStepThrough]
		public static string Hash(this string value, Encoding encoding, bool toBase64 = false)
        {
			if (value.IsEmpty())
				return value;

            using (var md5 = MD5.Create())
            {
				byte[] data = encoding.GetBytes(value);

				if (toBase64) 
                {
					byte[] hash = md5.ComputeHash(data);
					return Convert.ToBase64String(hash);
				}
				else 
                {
					return md5.ComputeHash(data).ToHexString().ToLower();
				}
            }
        }

		/// <summary>
		/// Mask by replacing characters with asterisks.
		/// </summary>
		/// <param name="value">
		/// The string
		/// </param>
		/// <param name="length">
		/// Number of characters to leave untouched.
		/// </param>
		/// <returns>
		/// The mask string
		/// </returns>
		[DebuggerStepThrough]
		public static string Mask(this string value, int length)
		{
			if (value.HasValue())
				return value.Substring(0, length) + new String('*', value.Length - length);
			return value;
		}

        /// <summary>
        /// The is web url.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsWebUrl(this string value)
        {
            return !string.IsNullOrEmpty(value) && RegularExpressions.IsWebUrl.IsMatch(value.Trim());
        }

        /// <summary>
        /// The is email.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsEmail(this string value)
        {
            return !string.IsNullOrEmpty(value) && RegularExpressions.IsEmail.IsMatch(value.Trim());
        }

        /// <summary>
        /// The is numeric.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNumeric(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return !RegularExpressions.IsNotNumber.IsMatch(value) &&
                   !RegularExpressions.HasTwoDot.IsMatch(value) &&
                   !RegularExpressions.HasTwoMinus.IsMatch(value) &&
                   RegularExpressions.IsNumeric.IsMatch(value);
        }

		/// <summary>
		/// Ensures that a string only contains numeric values
		/// </summary>
		/// <param name="str">
		/// Input string
		/// </param>
		/// <returns>
		/// Input string with only numeric values, empty string if input is null or empty
		/// </returns>
		[DebuggerStepThrough]
		public static string EnsureNumericOnly(this string str)
		{
			if (string.IsNullOrEmpty(str))
				return string.Empty;

			return new String(str.Where(c => char.IsDigit(c)).ToArray());
		}

        /// <summary>
        /// The is alpha.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsAlpha(this string value)
        {
            return RegularExpressions.IsAlpha.IsMatch(value);
        }

        /// <summary>
        /// The is alpha numeric.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsAlphaNumeric(this string value)
        {
            return RegularExpressions.IsAlphaNumeric.IsMatch(value);
        }

        /// <summary>
        /// The truncate.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="maxLength">
        /// The max length.
        /// </param>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        [DebuggerStepThrough]
        public static string Truncate(this string value, int maxLength, string suffix = "")
        {
            Guard.ArgumentNotNull(suffix, "suffix");
            Guard.ArgumentIsPositive(maxLength, "maxLength");

            int subStringLength = maxLength - suffix.Length;

            if (subStringLength <= 0)
                throw Error.Argument("maxLength", "Length of suffix string is greater or equal to maximumLength");

            if (value != null && value.Length > maxLength)
            {
                string truncatedString = value.Substring(0, subStringLength);

                // in case the last character is a space
                truncatedString = truncatedString.Trim();
                truncatedString += suffix;

                return truncatedString;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Determines whether the string contains white space.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// <c>true</c> if the string contains white space; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool ContainsWhiteSpace(this string value)
        {
            Guard.ArgumentNotNull(value, "value");

            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsWhiteSpace(value[i]))
                    return true;
            }

            return false;
        }

		/// <summary>
		/// Ensure that a string starts with a string.
		/// </summary>
		/// <param name="value">
		/// The target string
		/// </param>
		/// <param name="startsWith">
		/// The string the target string should start with
		/// </param>
		/// <returns>
		/// The resulting string
		/// </returns>
		[DebuggerStepThrough]
		public static string EnsureStartsWith(this string value, string startsWith)
		{
			Guard.ArgumentNotNull(value, "value");
			Guard.ArgumentNotNull(startsWith, "startsWith");

			return value.StartsWith(startsWith) ? value : (startsWith + value);
		}

        /// <summary>
        /// Ensures the target string ends with the specified string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="endWith">
        /// The end With.
        /// </param>
        /// <returns>
        /// The target string with the value string at the end.
        /// </returns>
        [DebuggerStepThrough]
        public static string EnsureEndsWith(this string value, string endWith)
        {
            Guard.ArgumentNotNull(value, "value");
            Guard.ArgumentNotNull(endWith, "endWith");

            if (value.Length >= endWith.Length)
            {
                if (string.Compare(value, value.Length - endWith.Length, endWith, 0, endWith.Length, StringComparison.OrdinalIgnoreCase) == 0)
                    return value;

                string trimmedString = value.TrimEnd(null);

                if (string.Compare(trimmedString, trimmedString.Length - endWith.Length, endWith, 0, endWith.Length, StringComparison.OrdinalIgnoreCase) == 0)
                    return value;
            }

            return value + endWith;
        }

        /// <summary>
        /// The get length.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static int? GetLength(this string value)
        {
            if (value == null)
                return null;
            else
                return value.Length;
        }

        /// <summary>
        /// The url encode.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        /// <summary>
        /// The url decode.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string UrlDecode(this string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        /// <summary>
        /// The attribute encode.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string AttributeEncode(this string value)
        {
            return HttpUtility.HtmlAttributeEncode(value);
        }

        /// <summary>
        /// The html encode.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string HtmlEncode(this string value)
        {
            return HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// The html decode.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string HtmlDecode(this string value)
        {
            return HttpUtility.HtmlDecode(value);
        }

        /// <summary>
        /// The remove html.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string RemoveHtml(this string value)
        {
            return RemoveHtmlInternal(value, null);
        }

        /// <summary>
        /// The remove html.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="removeTags">
        /// The remove tags.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveHtml(this string value, ICollection<string> removeTags)
        {
            return RemoveHtmlInternal(value, removeTags);
        }

        /// <summary>
        /// The remove html internal.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="removeTags">
        /// The remove tags.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string RemoveHtmlInternal(string s, ICollection<string> removeTags)
        {
            List<string> removeTagsUpper = null;
            if (removeTags != null)
            {
                removeTagsUpper = new List<string>(removeTags.Count);

                foreach (string tag in removeTags)
                {
                    removeTagsUpper.Add(tag.ToUpperInvariant());
                }
            }

            return RegularExpressions.RemoveHtml.Replace(s, delegate(Match match)
            {
                string tag = match.Groups["tag"].Value.ToUpperInvariant();

                if (removeTagsUpper == null)
                    return string.Empty;
                else if (removeTagsUpper.Contains(tag))
                    return string.Empty;
                else
                    return match.Value;
            });
        }

        /// <summary>
        /// Replaces pascal casing with spaces. For example "CustomerId" would become "Customer Id".
        /// Strings that already contain spaces are ignored.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The string after being split
        /// </returns>
        [DebuggerStepThrough]
        public static string SplitPascalCase(this string value)
        {
            // return Regex.Replace(input, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
            StringBuilder sb = new StringBuilder();
            char[] ca = value.ToCharArray();
            sb.Append(ca[0]);
            for (int i = 1; i < ca.Length - 1; i++)
            {
                char c = ca[i];
                if (char.IsUpper(c) && (char.IsLower(ca[i + 1]) || char.IsLower(ca[i - 1])))
                {
                    sb.Append(" ");
                }

                sb.Append(c);
            }

            if (ca.Length > 1)
            {
                sb.Append(ca[ca.Length - 1]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// The split safe.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        [DebuggerStepThrough]
		public static string[] SplitSafe(this string value, string separator) 
        {
			if (string.IsNullOrEmpty(value))
				return new string[0];
			return value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// Splits a string into two strings
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="strLeft">
		/// The str Left.
		/// </param>
		/// <param name="strRight">
		/// The str Right.
		/// </param>
		/// <param name="delimiter">
		/// The delimiter.
		/// </param>
		/// <returns>
		/// true: success, false: failure
		/// </returns>
        [DebuggerStepThrough]
		public static bool SplitToPair(this string value, out string strLeft, out string strRight, string delimiter) {
			int idx = -1;
			if (value.IsEmpty() || delimiter.IsEmpty() || (idx = value.IndexOf(delimiter)) == -1)
			{
				strLeft = value;
				strRight = string.Empty;
				return false;
			}

			strLeft = value.Substring(0, idx);
			strRight = value.Substring(idx + delimiter.Length);
			return true;
		}

        /// <summary>
        /// The to camel case.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string ToCamelCase(this string instance)
        {
            char ch = instance[0];
            return ch.ToString().ToLowerInvariant() + instance.Substring(1);
        }

        /// <summary>
        /// The replace new lines.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="replacement">
        /// The replacement.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string ReplaceNewLines(this string value, string replacement)
        {
            StringReader sr = new StringReader(value);
            StringBuilder sb = new StringBuilder();

            bool first = true;

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (first)
                    first = false;
                else
                    sb.Append(replacement);

                sb.Append(line);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Indents the specified string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="indentation">
        /// The number of characters to indent by.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string Indent(this string value, int indentation)
        {
            return Indent(value, indentation, ' ');
        }

        /// <summary>
        /// Indents the specified string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="indentation">
        /// The number of characters to indent by.
        /// </param>
        /// <param name="indentChar">
        /// The indent character.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string Indent(this string value, int indentation, char indentChar)
        {
            Guard.ArgumentNotNull(value, "value");
            Guard.ArgumentIsPositive(indentation, "indentation");

            StringReader sr = new StringReader(value);
            StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);

            ActionTextReaderLine(sr, sw, delegate(TextWriter tw, string line)
            {
                tw.Write(new string(indentChar, indentation));
                tw.Write(line);
            });

            return sw.ToString();
        }

        /// <summary>
        /// Numbers the lines.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string NumberLines(this string value)
        {
            Guard.ArgumentNotNull(value, "value");

            StringReader sr = new StringReader(value);
            StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);

            int lineNumber = 1;

            ActionTextReaderLine(sr, sw, delegate(TextWriter tw, string line)
            {
                tw.Write(lineNumber.ToString(CultureInfo.InvariantCulture).PadLeft(4));
                tw.Write(". ");
                tw.Write(line);

                lineNumber++;
            });

            return sw.ToString();
        }

        /// <summary>
        /// The encode js string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string EncodeJsString(this string value)
        {
            return EncodeJsString(value, '"', true);
        }

        /// <summary>
        /// The encode js string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter.
        /// </param>
        /// <param name="appendDelimiters">
        /// The append delimiters.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string EncodeJsString(this string value, char delimiter, bool appendDelimiters)
        {
            StringBuilder sb = new StringBuilder(value.GetLength() ?? 16);
            using (StringWriter w = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                EncodeJsString(w, value, delimiter, appendDelimiters);
                return w.ToString();
            }
        }

        /// <summary>
        /// The is enclosed in.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="enclosedIn">
        /// The enclosed in.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string enclosedIn)
        {
            return value.IsEnclosedIn(enclosedIn, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// The is enclosed in.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="enclosedIn">
        /// The enclosed in.
        /// </param>
        /// <param name="comparisonType">
        /// The comparison type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string enclosedIn, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(enclosedIn))
                return false;

            if (enclosedIn.Length == 1)
                return value.IsEnclosedIn(enclosedIn, enclosedIn, comparisonType);

            if (enclosedIn.Length % 2 == 0)
            {
                int len = enclosedIn.Length / 2;
                return value.IsEnclosedIn(
                    enclosedIn.Substring(0, len), 
                    enclosedIn.Substring(len, len), 
                    comparisonType);

            }

            return false;
        }

        /// <summary>
        /// The is enclosed in.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="end">
        /// The end.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string start, string end)
        {
            return value.IsEnclosedIn(start, end, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// The is enclosed in.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="end">
        /// The end.
        /// </param>
        /// <param name="comparisonType">
        /// The comparison type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string start, string end, StringComparison comparisonType)
        {
            return value.StartsWith(start, comparisonType) && value.EndsWith(end, comparisonType);
        }

        /// <summary>
        /// The remove encloser.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="encloser">
        /// The encloser.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveEncloser(this string value, string encloser)
        {
            return value.RemoveEncloser(encloser, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// The remove encloser.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="encloser">
        /// The encloser.
        /// </param>
        /// <param name="comparisonType">
        /// The comparison type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveEncloser(this string value, string encloser, StringComparison comparisonType)
        {
            if (value.IsEnclosedIn(encloser, comparisonType))
            {
                int len = encloser.Length / 2;
                return value.Substring(
                    len, 
                    value.Length - (len * 2));
            }

            return value;
        }

        /// <summary>
        /// The remove encloser.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="end">
        /// The end.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveEncloser(this string value, string start, string end)
        {
            return value.RemoveEncloser(start, end, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// The remove encloser.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="end">
        /// The end.
        /// </param>
        /// <param name="comparisonType">
        /// The comparison type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveEncloser(this string value, string start, string end, StringComparison comparisonType)
        {
            if (value.IsEnclosedIn(start, end, comparisonType))
                return value.Substring(
                    start.Length, 
                    value.Length - (start.Length + end.Length));

            return value;
        }

		/// <summary>
		/// Debug.WriteLine
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="appendMarks">
		/// The append Marks.
		/// </param>
        [DebuggerStepThrough]
		public static void Dump(this string value, bool appendMarks = false) 
        {
			Debug.WriteLine(value);
			Debug.WriteLineIf(appendMarks, "------------------------------------------------");
		}
		

		/// <summary>
		/// Smart way to create a HTML attribute with a leading space.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="name">
		/// Name of the attribute.
		/// </param>
		/// <param name="htmlEncode">
		/// The html Encode.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		public static string ToAttribute(this string value, string name, bool htmlEncode = true) 
        {
			if (name.IsEmpty())
				return string.Empty;

			if (value == string.Empty && name != "value" && !name.StartsWith("data"))
				return string.Empty;

			if (name == "maxlength" && (value == string.Empty || value == "0"))
				return string.Empty;

			if (name == "checked" || name == "disabled" || name == "multiple") 
            {
				if (value == string.Empty || string.Compare(value, "false", true) == 0)
					return string.Empty;
				value = string.Compare(value, "true", true) == 0 ? name : value;
			}

			if (name.StartsWith("data"))
				name = name.Insert(4, "-");

			return string.Format(" {0}=\"{1}\"", name, htmlEncode ? HttpUtility.HtmlEncode(value) : value);
		}
		

		/// <summary>
		/// Appends grow and uses delimiter if the string is not empty.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="grow">
		/// The grow.
		/// </param>
		/// <param name="delimiter">
		/// The delimiter.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
        [DebuggerStepThrough]
		public static string Grow(this string value, string grow, string delimiter) 
        {
			if (string.IsNullOrEmpty(value))
				return string.IsNullOrEmpty(grow) ? string.Empty : grow;

			if (string.IsNullOrEmpty(grow))
				return string.IsNullOrEmpty(value) ? string.Empty : value;

			return string.Format("{0}{1}{2}", value, delimiter, grow);
		}
		

		/// <summary>
		/// Returns n/a if string is empty else self.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
        [DebuggerStepThrough]
		public static string NaIfEmpty(this string value) 
        {
			return value.HasValue() ? value : "n/a";
		}

		/// <summary>
		/// Replaces substring with position x1 to x2 by replaceBy.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="x1">
		/// The x 1.
		/// </param>
		/// <param name="x2">
		/// The x 2.
		/// </param>
		/// <param name="replaceBy">
		/// The replace By.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
        [DebuggerStepThrough]
		public static string Replace(this string value, int x1, int x2, string replaceBy = null) 
        {
			if (value.HasValue() && x1 > 0 && x2 > x1 && x2 < value.Length) 
            {
				return value.Substring(0, x1) + (replaceBy == null ? string.Empty : replaceBy) + value.Substring(x2 + 1);
			}

			return value;
		}

        /// <summary>
        /// The trim safe.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
		public static string TrimSafe(this string value) 
        {
			return value.HasValue() ? value.Trim() : value;
		}

        /// <summary>
        /// The prettify.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="allowSpace">
        /// The allow space.
        /// </param>
        /// <param name="allowChars">
        /// The allow chars.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
		public static string Prettify(this string value, bool allowSpace = false, char[] allowChars = null) 
        {
			string res = string.Empty;
			try 
            {
				if (value.HasValue()) 
                {
					StringBuilder sb = new StringBuilder();
					bool space = false;
					char ch;

					for (int i = 0; i < value.Length; ++i) 
                    {
						ch = value[i];

						if (ch == ' ' || ch == '-') 
                        {
							if (allowSpace && ch == ' ')
								sb.Append(' ');
							else if (!space)
								sb.Append('-');
							space = true;
							continue;
						}

						space = false;

						if ((ch >= 48 && ch <= 57) || (ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122)) {
							sb.Append(ch);
							continue;
						}

						if (allowChars != null && allowChars.Contains(ch)) 
                        {
							sb.Append(ch);
							continue;
						}

						switch (ch) {
							case '_': sb.Append(ch); break;

							case 'ä': sb.Append("ae"); break;
							case 'ö': sb.Append("oe"); break;
							case 'ü': sb.Append("ue"); break;
							case 'ß': sb.Append("ss"); break;
							case 'Ä': sb.Append("AE"); break;
							case 'Ö': sb.Append("OE"); break;
							case 'Ü': sb.Append("UE"); break;

							case 'é':
							case 'è':
							case 'ê': sb.Append('e'); break;
							case 'á':
							case 'à':
							case 'â': sb.Append('a'); break;
							case 'ú':
							case 'ù':
							case 'û': sb.Append('u'); break;
							case 'ó':
							case 'ò':
							case 'ô': sb.Append('o'); break;
						}
	// switch
					}
	// for

					if (sb.Length > 0) 
                    {
						res = sb.ToString().Trim(new[] { ' ', '-' });

						Regex pat = new Regex(@"(-{2,})");		// remove double SpaceChar
						res = pat.Replace(res, "-");
						res = res.Replace("__", "_");
					}
				}
			}
			catch (Exception exp) 
            {
				exp.Dump();
			}

			return res.Length > 0 ? res : "null";
		}

        /// <summary>
        /// The sanitize html id.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string SanitizeHtmlId(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            StringBuilder builder = new StringBuilder(value.Length);
            int index = value.IndexOf("#");
            int num2 = value.LastIndexOf("#");
            if (num2 > index)
            {
                ReplaceInvalidHtmlIdCharacters(value.Substring(0, index), builder);
                builder.Append(value.Substring(index, (num2 - index) + 1));
                ReplaceInvalidHtmlIdCharacters(value.Substring(num2 + 1), builder);
            }
            else
            {
                ReplaceInvalidHtmlIdCharacters(value, builder);
            }

            return builder.ToString();
        }

        /// <summary>
        /// The is valid html id character.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidHtmlIdCharacter(char c)
        {
            bool invalid = c == '?' || c == '!' || c == '#' || c == '.' || c == ' ' || c == ';' || c == ':';
            return !invalid;
        }

        /// <summary>
        /// The replace invalid html id characters.
        /// </summary>
        /// <param name="part">
        /// The part.
        /// </param>
        /// <param name="builder">
        /// The builder.
        /// </param>
        private static void ReplaceInvalidHtmlIdCharacters(string part, StringBuilder builder)
        {
            for (int i = 0; i < part.Length; i++)
            {
                char c = part[i];
                if (IsValidHtmlIdCharacter(c))
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append('_');
                }
            }
        }

        /// <summary>
        /// The sha.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="encoding">
        /// The encoding.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Sha(this string value, Encoding encoding) 
        {
			if (value.HasValue())
            {
				using (var sha1 = new SHA1CryptoServiceProvider())
				{
				    byte[] data = encoding.GetBytes(value);

				    return sha1.ComputeHash(data).ToHexString();

				    // return BitConverter.ToString(sha1.ComputeHash(data)).Replace("-", "");
				}
            }

			return string.Empty;
		}

        /// <summary>
        /// The is match.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsMatch(this string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// The is match.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="match">
        /// The match.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsMatch(this string input, string pattern, out Match match, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            match = Regex.Match(input, pattern, options);
            return match.Success;
        }

        /// <summary>
        /// The regex remove.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RegexRemove(this string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.Replace(input, pattern, string.Empty, options);
        }

        /// <summary>
        /// The regex replace.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="replacement">
        /// The replacement.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RegexReplace(this string input, string pattern, string replacement, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        /// <summary>
        /// The to valid file name.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="replacement">
        /// The replacement.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string ToValidFileName(this string input, string replacement = "-")
        {
            return input.ToValidPathInternal(false, replacement);
        }

        /// <summary>
        /// The to valid path.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="replacement">
        /// The replacement.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string ToValidPath(this string input, string replacement = "-")
        {
            return input.ToValidPathInternal(true, replacement);
        }

        /// <summary>
        /// The to valid path internal.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="isPath">
        /// The is path.
        /// </param>
        /// <param name="replacement">
        /// The replacement.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ToValidPathInternal(this string input, bool isPath, string replacement)
        {
            var result = input.ToSafe();

            char[] invalidChars = isPath ? Path.GetInvalidPathChars() : Path.GetInvalidFileNameChars();

            foreach (var c in invalidChars)
            {
                result = result.Replace(c.ToString(), replacement ?? "-");
            }

            return result;
        }

        /// <summary>
        /// The to int array.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The <see cref="int[]"/>.
        /// </returns>
        [DebuggerStepThrough]
		public static int[] ToIntArray(this string s)
		{
			return Array.ConvertAll(s.SplitSafe(","), v => int.Parse(v.Trim()));
		}

        /// <summary>
        /// The to int array contains.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
		public static bool ToIntArrayContains(this string s, int value, bool defaultValue)
		{
			if (s == null)
				return defaultValue;
			var arr = s.ToIntArray();
			if (arr == null || arr.Count() <= 0)
				return defaultValue;
			return arr.Contains(value);
		}

        /// <summary>
        /// The remove invalid xml chars.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
		public static string RemoveInvalidXmlChars(this string s)
		{
			if (s.IsEmpty())
				return s;

			return Regex.Replace(s, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", string.Empty, RegexOptions.Compiled);
		}

        /// <summary>
        /// The replace csv chars.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
		public static string ReplaceCsvChars(this string s)
		{
			if (s.HasValue())
			{
				s = s.Replace(';', ',');
				s = s.Replace('\r', ' ');
				s = s.Replace('\n', ' ');
				return s.Replace("'", string.Empty);
			}

			return string.Empty;
		}

		#endregion

        #region Helper

        /// <summary>
        /// The encode js char.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter.
        /// </param>
        private static void EncodeJsChar(TextWriter writer, char c, char delimiter)
        {
            switch (c)
            {
                case '\t':
                    writer.Write(@"\t");
                    break;
                case '\n':
                    writer.Write(@"\n");
                    break;
                case '\r':
                    writer.Write(@"\r");
                    break;
                case '\f':
                    writer.Write(@"\f");
                    break;
                case '\b':
                    writer.Write(@"\b");
                    break;
                case '\\':
                    writer.Write(@"\\");
                    break;

                    // case '<':
                    // case '>':
                    // case '\'':
                    // StringUtils.WriteCharAsUnicode(writer, c);
                    // break;
                case '\'':

                    // only escape if this charater is being used as the delimiter
                    writer.Write((delimiter == '\'') ? @"\'" : @"'");
                    break;
                case '"':

                    // only escape if this charater is being used as the delimiter
                    writer.Write((delimiter == '"') ? "\\\"" : @"""");
                    break;
                default:
                    if (c > '\u001f')
                    {
                        writer.Write(c);
                    }
                    else
                    {
                        WriteCharAsUnicode(c, writer);
                    }

                    break;
            }
        }

        /// <summary>
        /// The encode js string.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter.
        /// </param>
        /// <param name="appendDelimiters">
        /// The append delimiters.
        /// </param>
        private static void EncodeJsString(TextWriter writer, string value, char delimiter, bool appendDelimiters)
        {
            // leading delimiter
            if (appendDelimiters)
                writer.Write(delimiter);

            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    EncodeJsChar(writer, value[i], delimiter);
                }
            }

            // trailing delimiter
            if (appendDelimiters)
                writer.Write(delimiter);
        }

        /// <summary>
        /// The action text reader line.
        /// </summary>
        /// <param name="textReader">
        /// The text reader.
        /// </param>
        /// <param name="textWriter">
        /// The text writer.
        /// </param>
        /// <param name="lineAction">
        /// The line action.
        /// </param>
        private static void ActionTextReaderLine(TextReader textReader, TextWriter textWriter, ActionLine lineAction)
        {
            string line;
            bool firstLine = true;
            while ((line = textReader.ReadLine()) != null)
            {
                if (!firstLine)
                    textWriter.WriteLine();
                else
                    firstLine = false;

                lineAction(textWriter, line);
            }
        }

        #endregion
    }

}
