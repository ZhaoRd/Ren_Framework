// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManagerExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义缓存管理器的扩展
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Caching
{
    /// <summary>
    /// 缓存管理器的扩展.
    /// </summary>
    public static class CacheManagerExtensions
    {
        /// <summary>
        /// 获取值.
        /// </summary>
        /// <param name="cacheManager">
        /// 缓存管理器.
        /// </param>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Get<T>(this ICacheManager cacheManager, string key)
        {
            return cacheManager.Get(key, () => default(T));
        }
    }
}