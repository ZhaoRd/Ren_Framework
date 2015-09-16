// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyAttribute.cs" company="zhaord">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Skymate.Data.Notifies;

    using Skymate.Extensions;

    /// <summary>
    /// The notify attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NotifyAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The notifications key.
        /// </summary>
        internal const string NotificationsKey = "zrd.notifications.all";

        /// <summary>
        /// Gets or sets the notifier.
        /// </summary>
        public INotifier Notifier { get; set; }

        /// <summary>
        /// The on action executed.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (this.Notifier == null || !this.Notifier.Entries.Any())
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                this.HandleAjaxRequest(this.Notifier.Entries.FirstOrDefault(), filterContext.HttpContext.Response);
                return;
            }

            this.Persist(filterContext.Controller.ViewData, this.Notifier.Entries.Where(x => x.Durable == false));
            this.Persist(filterContext.Controller.TempData, this.Notifier.Entries.Where(x => x.Durable));
        }

        /// <summary>
        /// The persist.
        /// </summary>
        /// <param name="bag">
        /// The bag.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        private void Persist(IDictionary<string, object> bag, IEnumerable<NotifyEntry> source)
        {
            var notifyEntries = source as NotifyEntry[] ?? source.ToArray();
            if (!notifyEntries.Any())
            {
                return;
            }

            var existing = (bag[NotificationsKey] ?? new List<NotifyEntry>()) as List<NotifyEntry>;

            notifyEntries.Each(x =>
            {
                if (existing != null && (x.Message.Text.HasValue() && !existing.Contains(x)))
                {
                    existing.Add(x);
                }
            });

            bag[NotificationsKey] = existing;
        }

        /// <summary>
        /// The handle ajax request.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        private void HandleAjaxRequest(NotifyEntry entry, HttpResponseBase response)
        {
            if (entry == null)
            {
                return;
            }

            response.AddHeader("X-Message-Type", entry.Type.ToString().ToLower());
            response.AddHeader("X-Message", entry.Message.Text);
        }
    }
}