// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticCache.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义静态缓存
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    /// <summary>
    /// 静态缓存.
    /// </summary>
    public class StaticCache : ICache
    {
        #region 字段

        /// <summary>
        /// 内存缓存.
        /// </summary>
        private ObjectCache cache;

        #endregion

        #region 属性

        #region ICache 接口

        /// <summary>
        /// 获取缓存中的所有条目
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> Entries
        {
            get
            {
                return this.Cache;
            }
        }

        /// <summary>
        /// 获取一个值,该值指示读写是否这个缓存应该是线程安全的
        /// </summary>
        public bool IsSingleton
        {
            get
            {
                return true;
            }
        }

        #endregion

        /// <summary>
        /// 对象缓存
        /// </summary>
        protected ObjectCache Cache
        {
            get
            {
                return this.cache ?? (this.cache = new MemoryCache("Skymate"));
            }
        }

        #endregion

        #region ICache 接口

        /// <summary>
        /// 缓存项与指定的键相关联的
        /// </summary>
        /// <param name="key">
        /// 缓存项关键字
        /// </param>
        /// <returns>
        /// 缓存项的值
        /// </returns>
        public object Get(string key)
        {
            return this.Cache.Get(key);
        }

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
        public void Set(string key, object value, int? cacheTime)
        {
            var cacheItem = new CacheItem(key, value);
            CacheItemPolicy policy = null;
            if (cacheTime != null && cacheTime.GetValueOrDefault() > 0)
            {
                policy = new CacheItemPolicy
                             {
                                 AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime.Value)
                             };
            }

            this.Cache.Add(cacheItem, policy);
        }

        /// <summary>
        /// 获取一个值,该值指示一个项目是否与指定的键存在于缓存中
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        /// <returns>
        /// 结果
        /// </returns>
        public bool Contains(string key)
        {
            return this.Cache.Contains(key);
        }

        /// <summary>
        /// 从缓存中删除值与指定键
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        public void Remove(string key)
        {
            this.Cache.Remove(key);
        }

        #endregion
    }
}