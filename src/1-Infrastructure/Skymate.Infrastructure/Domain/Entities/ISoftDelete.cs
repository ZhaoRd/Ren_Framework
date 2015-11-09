// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISoftDelete.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义软删除接口
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Domain.Entities
{
    /// <summary>
    /// 软删除接口.
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 标志实体已删除 
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
