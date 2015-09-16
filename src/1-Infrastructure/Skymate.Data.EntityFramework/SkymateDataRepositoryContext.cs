// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkymateDataRepositoryContext.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   The skymate data repository context.
//   自定义的仓储向下文
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Data.EntityFramework
{
    using System;
    using System.Data.Entity;

    using Apworks.Repositories.EntityFramework;

    using Skymate.Engines;
    using Skymate.Entities.Auditing;

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
            var toAddObject = obj;
            if (obj is IHasCreationTime)
            {
                var newObject = (IHasCreationTime)toAddObject;
                newObject.CreationTime = DateTime.Now;
                toAddObject = newObject;
            }

            if (obj is ICreationAudited)
            {
                if (this.workContext != null && !string.IsNullOrEmpty(this.workContext.CurrentCustomerId))
                {
                    var newObject = (ICreationAudited)toAddObject;
                    newObject.CreatorUserId = Guid.Parse(this.workContext.CurrentCustomerId);
                    toAddObject = newObject;
                }
            }

            base.RegisterNew(toAddObject);
        }

        /// <summary>
        /// The register modified.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public override void RegisterModified(object obj)
        {
            var newObject = obj as IModificationAudited;
            if (newObject == null)
            {
                base.RegisterModified(obj);
                return;
            }
            
            newObject.LastModificationTime = DateTime.Now;
            if (this.workContext != null && !string.IsNullOrEmpty(this.workContext.CurrentCustomerId))
            {
                newObject.LastModifierUserId = Guid.Parse(this.workContext.CurrentCustomerId);
            }

            base.RegisterModified(newObject);
        }

        /// <summary>
        /// 注册删除.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public override void RegisterDeleted(object obj)
        {
            var newObject = obj as IDeletionAudited;
            if (newObject == null)
            {
                base.RegisterDeleted(obj);
                return;
            }

            newObject.DeletionTime = DateTime.Now;
            if (this.workContext != null && !string.IsNullOrEmpty(this.workContext.CurrentCustomerId))
            {
                newObject.DeleterUserId = Guid.Parse(this.workContext.CurrentCustomerId);
            }

            base.RegisterDeleted(newObject);
        }
    }
}
