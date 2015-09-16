// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizedString.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Localization
{
    using System;
    using System.Web;

    /// <summary>
    /// The localized string.
    /// </summary>
    [Serializable]
    public class LocalizedString : IHtmlString
    {
        /// <summary>
        /// The localized.
        /// </summary>
        private readonly string localized;

        /// <summary>
        /// The _text hint.
        /// </summary>
        private readonly string textHint;

        /// <summary>
        /// The _args.
        /// </summary>
        private readonly object[] _args;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedString"/> class.
        /// </summary>
        /// <param name="localized">
        /// The localized.
        /// </param>
        public LocalizedString(string localized)
        {
            this.localized = localized;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedString"/> class.
        /// </summary>
        /// <param name="localized">
        /// The localized.
        /// </param>
        /// <param name="textHint">
        /// The text hint.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public LocalizedString(string localized, string textHint, object[] args)
        {
            this.localized = localized;
            this.textHint = textHint;
            this._args = args;
        }

        /// <summary>
        /// The text or default.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="LocalizedString"/>.
        /// </returns>
        public static LocalizedString TextOrDefault(string text, LocalizedString defaultValue)
        {
            if (string.IsNullOrEmpty(text))
                return defaultValue;
            return new LocalizedString(text);
        }

        /// <summary>
        /// Gets the text hint.
        /// </summary>
        public string TextHint
        {
            get { return this.textHint; }
        }

        /// <summary>
        /// Gets the args.
        /// </summary>
        public object[] Args
        {
            get { return this._args; }
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text
        {
            get { return this.localized; }
        }

        /// <summary>
        /// The op_ implicit.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator string(LocalizedString obj)
        {
            return obj.Text;
        }

        /// <summary>
        /// The op_ implicit.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator LocalizedString(string obj)
        {
            return new LocalizedString(obj);
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return this.localized;
        }

        /// <summary>
        /// The to html string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string ToHtmlString()
        {
            return this.localized;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 0;
            if (this.localized != null)
                hashCode ^= this.localized.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;

            var that = (LocalizedString)obj;
            return string.Equals(this.localized, that.localized);
        }
    }
}