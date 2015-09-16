// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyEntry.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Data.Notifies
{
    using System;

    using Skymate.Attributes;
    using Skymate.Localization;

    /// <summary>
    /// The notify entry.
    /// </summary>
    [Serializable]
    public class NotifyEntry : ComparableObject<NotifyEntry>
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [ObjectSignature]
        public NotifyType Type { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [ObjectSignature]
        public LocalizedString Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether durable.
        /// </summary>
        public bool Durable { get; set; }
    }
}