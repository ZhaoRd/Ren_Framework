// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifierExtension.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Data.Notifies
{
    using Skymate.Localization;

    /// <summary>
    /// The notifier extension.
    /// </summary>
    public static class NotifierExtension
    {
        /// <summary>
        /// The information.
        /// </summary>
        /// <param name="notifier">
        /// The notifier.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="durable">
        /// The durable.
        /// </param>
        public static void Information(this INotifier notifier, LocalizedString message, bool durable = true)
        {
            notifier.Add(NotifyType.Info, message, durable);
        }

        /// <summary>
        /// The success.
        /// </summary>
        /// <param name="notifier">
        /// The notifier.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="durable">
        /// The durable.
        /// </param>
        public static void Success(this INotifier notifier, LocalizedString message, bool durable = true)
        {
            notifier.Add(NotifyType.Success, message, durable);
        }

        /// <summary>
        /// The warning.
        /// </summary>
        /// <param name="notifier">
        /// The notifier.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="durable">
        /// The durable.
        /// </param>
        public static void Warning(this INotifier notifier, LocalizedString message, bool durable = true)
        {
            notifier.Add(NotifyType.Warning, message, durable);
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="notifier">
        /// The notifier.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="durable">
        /// The durable.
        /// </param>
        public static void Error(this INotifier notifier, LocalizedString message, bool durable = true)
        {
            notifier.Add(NotifyType.Error, message, durable);
        }
    }
}