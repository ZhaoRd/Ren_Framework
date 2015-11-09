// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPassivable.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义启用禁用接口.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Domain.Entities
{
    /// <summary>
    /// 用来标识视图是否启用的接口.
    /// </summary>
    public interface IPassivable
    {
        /// <summary>
        /// 获取或设置是否启用.
        /// True:启用实体
        /// False:禁用实体
        /// </summary>
        bool IsActive { get; set; }
    }
}
