// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkymateDataRepositoryContext.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   The skymate data repository context.
//   自定义的仓储向下文
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Apworks.Repositories.EntityFramework;

    using Engines;
    
    using Extensions;

    using Skymate.Domain.Entities;
    using Skymate.Domain.Entities.Auditing;

    /// <summary>
    /// The skymate data repository context.
    /// 自定义的仓储向下文
    /// </summary>
    public class SkymateDataRepositoryContext : EntityFrameworkRepositoryContext
    {
        /// <summary>
        /// The work context.
        /// </summary>
        private readonly IWorkContext workContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateDataRepositoryContext"/> class.
        /// </summary>
        /// <param name="context">
        /// The ef context.
        /// </param>
        public SkymateDataRepositoryContext(DbContext context)
            : base(context)
        {
            // 解析当前工作环境
            this.workContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// The register new.
        /// 注册新实体
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public override void RegisterNew(object obj)
        {
            var entry = this.Context.Entry(obj);
            base.RegisterNew(obj);

            if (entry.Entity is IHasCreationTime)
            {
                entry.Cast<IHasCreationTime>().Entity.CreationTime = DateTime.Now;
            }

            if (!(entry.Entity is ICreationAudited))
            {
                return;
            }

            if (this.workContext != null && !string.IsNullOrEmpty(this.workContext.CurrentCustomerId))
            {
                entry.Cast<ICreationAudited>().Entity.CreatorUserId = Guid.Parse(
                    this.workContext.CurrentCustomerId);
            }
        }

        /// <summary>
        /// The register modified.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public override void RegisterModified(object obj)
        {
            var entry = this.Context.Entry(obj);
            base.RegisterModified(obj);

            this.SetModificationAuditProperties(entry);

            if (!(entry.Entity is ISoftDelete) || !entry.Entity.As<ISoftDelete>().IsDeleted)
            {
                return;
            }

            if (entry.Entity is IDeletionAudited)
            {
                this.SetDeletionAuditProperties(entry.Entity.As<IDeletionAudited>());
            }
        }

        /// <summary>
        /// The set modification audit properties.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        protected virtual void SetModificationAuditProperties(DbEntityEntry entry)
        {
            if (!(entry.Entity is IModificationAudited))
            {
                return;
            }

            entry.Cast<IModificationAudited>().Entity.LastModificationTime = DateTime.Now;
            if (this.workContext != null && !string.IsNullOrEmpty(this.workContext.CurrentCustomerId))
            {
                entry.Cast<IModificationAudited>().Entity.LastModifierUserId = Guid.Parse(this.workContext.CurrentCustomerId);
            }
        }

        /// <summary>
        /// 注册删除.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public override void RegisterDeleted(object obj)
        {
            var entry = this.Context.Entry(obj);

            base.RegisterDeleted(entry);

            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            var softDeleteEntry = entry.Cast<ISoftDelete>();

            softDeleteEntry.State = EntityState.Unchanged;
            softDeleteEntry.Entity.IsDeleted = true;

            if (!(entry.Entity is IDeletionAudited))
            {
                return;
            }

            entry.Cast<IDeletionAudited>().Entity.DeletionTime = DateTime.Now;
            if (this.workContext != null && !string.IsNullOrEmpty(this.workContext.CurrentCustomerId))
            {
                entry.Cast<IDeletionAudited>().Entity.DeleterUserId = Guid.Parse(this.workContext.CurrentCustomerId);
            }
        }

        /// <summary>
        /// The set deletion audit properties.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected virtual void SetDeletionAuditProperties(IDeletionAudited entity)
        {
            entity.DeletionTime = DateTime.Now;
            if (this.workContext != null && !string.IsNullOrEmpty(this.workContext.CurrentCustomerId))
            {
                entity.DeleterUserId = Guid.Parse(this.workContext.CurrentCustomerId);
            }
        }
    }
}
