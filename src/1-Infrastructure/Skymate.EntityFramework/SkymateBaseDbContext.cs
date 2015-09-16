// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkymateBaseDbContext.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   Defines the SkymateBaseDbContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.EntityFramework
{
    using System;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;

    using Skymate.Engines;
    using Skymate.Entities;
    using Skymate.Entities.Auditing;
    using Skymate.Extensions;

    /// <summary>
    /// The skymate base db context.
    /// </summary>
    public class SkymateBaseDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateBaseDbContext"/> class. 
        /// Constructor.
        /// </summary>
        protected SkymateBaseDbContext()
        {
            this.WorkContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateBaseDbContext"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">
        /// The name or connection string.
        /// </param>
        protected SkymateBaseDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.WorkContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateBaseDbContext"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        protected SkymateBaseDbContext(DbCompiledModel model)
            : base(model)
        {
            this.WorkContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateBaseDbContext"/> class.
        /// </summary>
        /// <param name="existingConnection">
        /// The existing connection.
        /// </param>
        /// <param name="contextOwnsConnection">
        /// The context owns connection.
        /// </param>
        protected SkymateBaseDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            this.WorkContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateBaseDbContext"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">
        /// The name or connection string.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        protected SkymateBaseDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            this.WorkContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateBaseDbContext"/> class.
        /// </summary>
        /// <param name="objectContext">
        /// The object context.
        /// </param>
        /// <param name="dbContextOwnsObjectContext">
        /// The db context owns object context.
        /// </param>
        protected SkymateBaseDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            this.WorkContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateBaseDbContext"/> class.
        /// </summary>
        /// <param name="existingConnection">
        /// The existing connection.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="contextOwnsConnection">
        /// The context owns connection.
        /// </param>
        protected SkymateBaseDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            this.WorkContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// Gets the work context.
        /// </summary>
        public IWorkContext WorkContext { get; private set; }

        /// <summary>
        /// The save changes.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int SaveChanges()
        {
            this.ApplyAbpConcepts();
            return base.SaveChanges();
        }

        /// <summary>
        /// The save changes async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.ApplyAbpConcepts();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// The apply abp concepts.
        /// </summary>
        protected virtual void ApplyAbpConcepts()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        this.SetCreationAuditProperties(entry);

                        // CheckAndSetTenantIdProperty(entry);
                        // EntityChangedEventHelper.TriggerEntityCreatedEvent(entry.Entity);
                        break;
                    case EntityState.Modified:
                        // PreventSettingCreationAuditProperties(entry);
                        // CheckAndSetTenantIdProperty(entry);
                        this.SetModificationAuditProperties(entry);

                        if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                        {
                            if (entry.Entity is IDeletionAudited)
                            {
                                this.SetDeletionAuditProperties(entry.Entity.As<IDeletionAudited>());
                            }

                           // EntityChangedEventHelper.TriggerEntityDeletedEvent(entry.Entity);
                        }

/*
                        else
                        {
                           // EntityChangedEventHelper.TriggerEntityUpdatedEvent(entry.Entity);
                        }*/

                        break;
                    case EntityState.Deleted:
                        // PreventSettingCreationAuditProperties(entry);
                        this.HandleSoftDelete(entry);

                        // EntityChangedEventHelper.TriggerEntityDeletedEvent(entry.Entity);
                        break;
                }
            }
        }

        /// <summary>
        /// The set creation audit properties.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        protected virtual void SetCreationAuditProperties(DbEntityEntry entry)
        {
            if (entry.Entity is IHasCreationTime)
            {
                entry.Cast<IHasCreationTime>().Entity.CreationTime = DateTime.Now; // Clock.Now;
            }

            if (entry.Entity is ICreationAudited)
            {
                var userid = Guid.Parse(this.WorkContext.CurrentCustomerId);
                entry.Cast<ICreationAudited>().Entity.CreatorUserId = userid;
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
            if (entry.Entity is IModificationAudited)
            {
                var auditedEntry = entry.Cast<IModificationAudited>();

                var userid = Guid.Parse(this.WorkContext.CurrentCustomerId);
                auditedEntry.Entity.LastModificationTime = DateTime.Now;
                auditedEntry.Entity.LastModifierUserId = userid;
            }
        }

        /// <summary>
        /// The handle soft delete.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        protected virtual void HandleSoftDelete(DbEntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            var softDeleteEntry = entry.Cast<ISoftDelete>();

            softDeleteEntry.State = EntityState.Unchanged;
            softDeleteEntry.Entity.IsDeleted = true;

            if (entry.Entity is IDeletionAudited)
            {
                this.SetDeletionAuditProperties(entry.Cast<IDeletionAudited>().Entity);
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
            var userid = Guid.Parse(this.WorkContext.CurrentCustomerId);
            entity.DeletionTime = DateTime.Now;
            entity.DeleterUserId = userid;
        }
    }
}
