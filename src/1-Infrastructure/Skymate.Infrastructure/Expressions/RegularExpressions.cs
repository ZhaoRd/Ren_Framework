// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegularExpressions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Expressions
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// The regular expressions.
    /// </summary>
    public static class RegularExpressions
    {
        /// <summary>
        /// The is alpha.
        /// </summary>
        public static readonly Regex IsAlpha = new Regex("[^a-zA-Z]", RegexOptions.Compiled);

        /// <summary>
        /// The is alpha numeric.
        /// </summary>
        public static readonly Regex IsAlphaNumeric = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);

        /// <summary>
        /// The is not number.
        /// </summary>
        public static readonly Regex IsNotNumber = new Regex("[^0-9.-]", RegexOptions.Compiled);

        /// <summary>
        /// The is positive integer.
        /// </summary>
        public static readonly Regex IsPositiveInteger = new Regex(@"\d{1,10}", RegexOptions.Compiled);

        /// <summary>
        /// The is numeric.
        /// </summary>
        public static readonly Regex IsNumeric = new Regex("(" + ValidRealPattern + ")|(" + ValidIntegerPattern + ")", RegexOptions.Compiled);

        /// <summary>
        /// The is web url.
        /// </summary>
        public static readonly Regex IsWebUrl = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// The is email.
        /// </summary>
        public static readonly Regex IsEmail = new Regex("^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The remove html.
        /// </summary>
        public static readonly Regex RemoveHtml = new Regex(@"<[/]{0,1}\s*(?<tag>\w*)\s*(?<attr>.*?=['""].*?[""'])*?\s*[/]{0,1}>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// The is guid.
        /// </summary>
        public static readonly Regex IsGuid = new Regex(@"\{?[a-fA-F0-9]{8}(?:-(?:[a-fA-F0-9]){4}){3}-[a-fA-F0-9]{12}\}?", RegexOptions.Compiled);

        /// <summary>
        /// The is base 64 guid.
        /// </summary>
        public static readonly Regex IsBase64Guid = new Regex(@"[a-zA-Z0-9+/=]{22,24}", RegexOptions.Compiled);

        /// <summary>
        /// The is culture code.
        /// </summary>
        public static readonly Regex IsCultureCode = new Regex(@"^([a-z]{2})|([a-z]{2}-[A-Z]{2})$", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// The is year range.
        /// </summary>
        public static readonly Regex IsYearRange = new Regex(@"^(\d{4})-(\d{4})$", RegexOptions.Compiled);

        /// <summary>
        /// The is iban.
        /// </summary>
        public static readonly Regex IsIban = new Regex(@"[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// The is bic.
        /// </summary>
        public static readonly Regex IsBic = new Regex(@"([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// The valid real pattern.
        /// </summary>
        internal static readonly string ValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";

        /// <summary>
        /// The valid integer pattern.
        /// </summary>
        internal static readonly string ValidIntegerPattern = "^([-]|[0-9])[0-9]*$";

        /// <summary>
        /// The has two dot.
        /// </summary>
        internal static readonly Regex HasTwoDot = new Regex("[0-9]*[.][0-9]*[.][0-9]*", RegexOptions.Compiled);

        /// <summary>
        /// The has two minus.
        /// </summary>
        internal static readonly Regex HasTwoMinus = new Regex("[0-9]*[-][0-9]*[-][0-9]*", RegexOptions.Compiled);
    }
}
