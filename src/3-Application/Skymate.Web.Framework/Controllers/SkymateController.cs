// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkymateController.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   基础控制器.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using Data.Notifies;
    using Engines;
    using Extensions;
    using Attributes;

    using Castle.Core.Logging;

    /// <summary>
    /// The base controller.
    /// 基础控制器
    /// </summary>
    [Notify]
    public abstract class SkymateController : Controller
    {
        private ILogger logger;

        /// <summary>
        /// 属性注入日志
        /// </summary>
        public ILogger Logger
        {
            get
            {
                if (logger==null)
                {
                    logger = EngineContext.Current.Resolve<ILogger>();
                }
                return logger;
            }
        }

        /// <summary>
        /// The notifier.
        /// </summary>
        private readonly Lazy<INotifier> notifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateController"/> class.
        /// </summary>
        protected SkymateController()
        {
            this.notifier = EngineContext.Current.Resolve<Lazy<INotifier>>();
        }

        /// <summary>
        /// Pushes an info message to the notification queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
        protected virtual void NotifyInfo(string message, bool durable = true)
        {
            this.notifier.Value.Information(message, durable);
        }

        /// <summary>
        /// Pushes a warning message to the notification queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
        protected virtual void NotifyWarning(string message, bool durable = true)
        {
            this.notifier.Value.Warning(message, durable);
        }

        /// <summary>
        /// Pushes a success message to the notification queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
        protected virtual void NotifySuccess(string message, bool durable = true)
        {
            this.notifier.Value.Success(message, durable);
        }

        /// <summary>
        /// Pushes an error message to the notification queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
        protected virtual void NotifyError(string message, bool durable = true)
        {
            this.notifier.Value.Error(message, durable);
        }

        /// <summary>
        /// Pushes an error message to the notification queue
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="durable">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether the exception should be logged</param>
        protected virtual void NotifyError(Exception exception, bool durable = true, bool logException = true)
        {
            if (logException)
            {
                this.LogException(exception);
            }

            this.notifier.Value.Error(exception.Message, durable);
        }

        /// <summary>
        /// The redirect to referrer.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        protected virtual ActionResult RedirectToReferrer()
        {
            if (Request.UrlReferrer != null && Request.UrlReferrer.ToString().HasValue())
            {
                return this.Redirect(Request.UrlReferrer.ToString());
            }

            return this.RedirectToRoute("HomePage");
        }

        /// <summary>
        /// On exception
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                this.LogException(filterContext.Exception);
            }

            base.OnException(filterContext);
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exc">Exception</param>
        private void LogException(Exception exc)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var customer = workContext.CurrentCustomerId;
            this.Logger.Error(exc.Message, exc);
        }
        
    }
}