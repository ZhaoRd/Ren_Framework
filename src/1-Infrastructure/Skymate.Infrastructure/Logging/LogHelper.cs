// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="zsharp">
//   copyright (c) zsharp 2015 all right
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
        /// A reference to the logger.
        /// </summary>
        public static ILogger Logger { get; private set; }

        static LogHelper()
        {
            var container = EngineContext.Current.Resolve<IWindsorContainer>();
            Logger =  container.Resolve<ILogger>();
        }

        public static void LogException(Exception ex)
        {
            LogException(Logger, ex);
        }

        public static void LogException(ILogger logger, Exception ex)
        {
            logger.Error(ex.ToString(), ex);
            LogValidationErrors(ex);
        }

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
                var memberNames = "";
                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
                }

                Logger.Warn(validationResult.ErrorMessage + memberNames);
            }
        }
    }
}
