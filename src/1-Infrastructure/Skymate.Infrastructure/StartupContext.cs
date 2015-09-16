using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate
{
    using System.Runtime.CompilerServices;

    using Skymate.Singletons;

    public class StartupContext
    {
        
        public static void Initialize()
        {
            var application = new ApplicationStartup();
            application.Initialize();
        }

    }
}
