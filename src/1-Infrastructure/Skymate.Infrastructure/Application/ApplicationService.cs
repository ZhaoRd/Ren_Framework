using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate.Application
{
    public  abstract class ApplicationService : SkymateServiceBase, IApplicationService
    {
        /// <summary>
        /// The work context.
        /// </summary>
        private readonly IWorkContext workContext;

        protected ApplicationService(IWorkContext workContext)
        {
            this.workContext = workContext;
        }

    }
}
