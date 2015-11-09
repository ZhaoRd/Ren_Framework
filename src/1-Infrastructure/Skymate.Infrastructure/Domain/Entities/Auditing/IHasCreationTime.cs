// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHasCreationTime.cs" company="Skymate">
//  Copyright © 2015 Skymate. All rights reserved. 
// </copyright>
// <summary>
//   定义拥有创建时间的接口.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Domain.Entities.Auditing
{
    using System;

    /// <summary>
    /// 创建时间的接口.
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// 获取或设置实体的创建时间.
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}
