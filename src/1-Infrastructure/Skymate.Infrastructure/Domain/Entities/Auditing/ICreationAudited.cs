// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICreationAudited.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved. 
// </copyright>
// <summary>
//   定义创建审计接口.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Domain.Entities.Auditing
{
    using System;

    /// <summary>
    /// 审计创建者接口.
    /// </summary>
    public interface ICreationAudited : IHasCreationTime
    {
        /// <summary>
        /// 获取或设置实体创建者ID.
        /// </summary>
        Guid? CreatorUserId { get; set; }
    }

    /// <summary>
    /// The CreationAudited interface.
    /// </summary>
    /// <typeparam name="TUser">
    /// 用户
    /// </typeparam>
    public interface ICreationAudited<TUser> : ICreationAudited
    {
        /// <summary>
        /// 获取或设置创建者.
        /// </summary>
        TUser CreatorUser { get; set; }
    }
}
