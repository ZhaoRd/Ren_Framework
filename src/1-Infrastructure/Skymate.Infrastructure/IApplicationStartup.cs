using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate
{
    public interface IApplicationStartup
    {
        void Startup();

        int Order { get; }
    }
}
