// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeletionAudited.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved. 
// </copyright>
// <summary>
//   定义删除审计接口.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Domain.Entities.Auditing
{
    using System;

    using Skymate.Domain.Entities;

    /// <summary>
    /// 删除审计接口.
    /// </summary>
    public interface IDeletionAudited : ISoftDelete
    {
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        Guid? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}
