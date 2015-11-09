using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate
{
    using Castle.Core.Logging;

    using Skymate.Engines;

    public abstract class SkymateServiceBase
    {
        private ILogger logger;

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

    }
}
