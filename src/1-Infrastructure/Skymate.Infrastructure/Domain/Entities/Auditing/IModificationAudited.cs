// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModificationAudited.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved. 
// </copyright>
// <summary>
//   定义修改审计类型.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Domain.Entities.Auditing
{
    using System;

    /// <summary>
    /// 修改审计接口.
    /// </summary>
    public interface IModificationAudited
    {
        /// <summary>
        /// 最后一次修改时间.
        /// </summary>
        DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后一次修改者.
        /// </summary>
        Guid? LastModifierUserId { get; set; }
    }
}
