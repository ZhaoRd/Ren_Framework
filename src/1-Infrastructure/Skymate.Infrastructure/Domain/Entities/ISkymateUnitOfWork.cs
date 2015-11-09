namespace Skymate.Domain.Entities
{
    using System;

    public interface ISkymateUnitOfWork:IDisposable
    {
        void Commit();

        void Rollback();

        bool DistributedTransactionSupported { get; }

        bool Committed { get; }
    }
}
