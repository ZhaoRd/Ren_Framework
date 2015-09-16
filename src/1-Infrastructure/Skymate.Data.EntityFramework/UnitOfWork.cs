using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate.Data.EntityFramework
{
    using Apworks;
    using Apworks.Repositories;

    using Skymate.Entities;

    /// <summary>
    /// The unit of work.
    /// </summary>
    public class UnitOfWork : ISkymateUnitOfWork
    {
        /// <summary>
        /// The repository context.
        /// </summary>
        private readonly IRepositoryContext repositoryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="repositoryContext">
        /// The repository context.
        /// </param>
        public UnitOfWork(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public void Commit()
        {
            this.repositoryContext.Commit();
            this.Committed = true;
        }

        public void Rollback()
        {
            this.repositoryContext.Rollback();
        }

        /// <summary>
        /// 是否支持远程事务
        /// </summary>
        public bool DistributedTransactionSupported { get; private set; }

        public bool Committed { get; private set; }

        public void Dispose()
        {
            this.repositoryContext.Dispose();
        }
    }
}
