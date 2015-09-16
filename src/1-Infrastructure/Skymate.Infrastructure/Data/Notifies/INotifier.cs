// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INotifier.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Data.Notifies
{
    using System.Collections.Generic;

    using Skymate.Localization;

    /// <summary>
    /// The Notifier interface.
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Gets the entries.
        /// </summary>
        ICollection<NotifyEntry> Entries { get; }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="durable">
        /// The durable.
        /// </param>
        void Add(NotifyType type, LocalizedString message, bool durable = true);
    }
}
