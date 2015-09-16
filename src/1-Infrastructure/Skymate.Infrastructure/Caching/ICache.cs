// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICache.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义缓存接口
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Caching
{
    using System.Collections.Generic;

    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        #region 属性

        /// <summary>
        /// 获取缓存中的所有条目
        /// </summary>
        IEnumerable<KeyValuePair<string, object>> Entries { get; }

        /// <summary>
        /// 获取一个值,该值指示读写是否这个缓存应该是线程安全的
        /// </summary>
        bool IsSingleton { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 缓存项与指定的键相关联的
        /// </summary>
        /// <param name="key">
        /// 缓存项关键字
        /// </param>
        /// <returns>
        /// 缓存项的值
        /// </returns>
        object Get(string key);

        /// <summary>
        /// 添加缓存条目指定的关键
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        /// <param name="value">
        /// 缓存值.
        /// </param>
        /// <param name="cacheTime">
        /// 缓存时间在几分钟内
        /// </param>
        void Set(string key, object value, int? cacheTime);

        /// <summary>
        /// 获取一个值,该值指示一个项目是否与指定的键存在于缓存中
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        /// <returns>
        /// 结果
        /// </returns>
        bool Contains(string key);

        /// <summary>
        /// 从缓存中删除值与指定键
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        void Remove(string key);

        #endregion
    }
}