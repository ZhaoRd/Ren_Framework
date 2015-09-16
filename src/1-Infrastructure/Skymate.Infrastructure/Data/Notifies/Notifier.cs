// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Notifier.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Data.Notifies
{
    using System.Collections.Generic;

    using Skymate.Localization;

    /// <summary>
    /// The notifier.
    /// </summary>
    public class Notifier : INotifier
    {
        /// <summary>
        /// The entries.
        /// </summary>
        private readonly HashSet<NotifyEntry> entries = new HashSet<NotifyEntry>();

        /// <summary>
        /// Gets the entries.
        /// </summary>
        public ICollection<NotifyEntry> Entries
        {
            get { return this.entries; }
        }

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
        public void Add(NotifyType type, LocalizedString message, bool durable = true)
        {
            this.entries.Add(new NotifyEntry { Type = type, Message = message, Durable = durable });
        }
    }
}