// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITopologicSortable.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   TopologicSortable接口.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Collections
{
    /// <summary>
    /// TopologicSortable接口.
    /// </summary>
    /// <typeparam name="TKey">
    /// 类型
    /// </typeparam>
    public interface ITopologicSortable<TKey>
    {
        /// <summary>
        /// 关键字.
        /// </summary>
        TKey Key { get; }

        /// <summary>
        /// 获取依赖.
        /// </summary>
        TKey[] DependsOn { get; }
    }
}