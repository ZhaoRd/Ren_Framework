// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullLocalizer.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Localization
{
    /// <summary>
    /// The null localizer.
    /// </summary>
    public static class NullLocalizer
    {
        /// <summary>
        /// The s_instance.
        /// </summary>
        private static readonly Localizer SInstance;

        /// <summary>
        /// Initializes static members of the <see cref="NullLocalizer"/> class.
        /// </summary>
        static NullLocalizer()
        {
            SInstance = (format, args) => new LocalizedString((args == null || args.Length == 0) ? format : string.Format(format, args));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Localizer Instance
        {
            get { return SInstance; }
        }
    }
}