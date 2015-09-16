// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   This class can be used to write logs from somewhere where it's a little hard to get a reference to the <see cref="ILogger" />.
//   Normally, get <see cref="ILogger" /> using property injection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Logging
{
    using System;
    using System.Linq;

    using Castle.Core.Internal;
    using Castle.Core.Logging;
    using Castle.Windsor;

    using Skymate.Engines;

    /// <summary>
    /// This class can be used to write logs from somewhere where it's a little hard to get a reference to the <see cref="ILogger"/>.
    /// Normally, get <see cref="ILogger"/> using property injection.
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// Initializes static members of the <see cref="LogHelper"/> class.
        /// </summary>
        static LogHelper()
        {
            var container = EngineContext.Current.Resolve<IWindsorContainer>();
            Logger = container.Resolve<ILogger>();
        }

        /// <summary>
        /// A reference to the logger.
        /// </summary>
        public static ILogger Logger { get; private set; }

        /// <summary>
        /// The log exception.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public static void LogException(Exception ex)
        {
            LogException(Logger, ex);
        }

        /// <summary>
        /// The log exception.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public static void LogException(ILogger logger, Exception ex)
        {
            logger.Error(ex.ToString(), ex);
            LogValidationErrors(ex);
        }

        /// <summary>
        /// The log validation errors.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private static void LogValidationErrors(Exception exception)
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerException is SkymateValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (!(exception is SkymateValidationException))
            {
                return;
            }

            var validationException = exception as SkymateValidationException;
            if (validationException.ValidationErrors.IsNullOrEmpty())
            {
                return;
            }

            Logger.Warn("There are " + validationException.ValidationErrors.Count + " validation errors:");
            foreach (var validationResult in validationException.ValidationErrors)
            {
                var memberNames = string.Empty;
                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
                }

                Logger.Warn(validationResult.ErrorMessage + memberNames);
            }
        }
    }
}
