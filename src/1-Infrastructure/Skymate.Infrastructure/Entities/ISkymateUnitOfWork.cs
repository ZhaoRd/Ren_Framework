using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate.Entities
{
    public interface ISkymateUnitOfWork:IDisposable
    {
        void Commit();

        void Rollback();

        bool DistributedTransactionSupported { get; }

        bool Committed { get; }
    }
}
