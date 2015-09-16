// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspNetCache.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义AspNetCache类型
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Caching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;

    using Skymate.Extensions;

    /// <summary>
    /// asp net 缓存.
    /// </summary>
    public class AspNetCache : ICache
    {
        #region 字段

        /// <summary>
        ///  全局名称.
        /// </summary>
        private const string RegionName = "$$SkymateNET$$";

        /// <summary>
        /// 空.
        /// </summary>
        private const string FakeNull = "__[NULL]__";

        #endregion

        #region 属性

        /// <summary>
        /// 获取一个值,该值指示是是否独立.
        /// </summary>
        public bool IsSingleton
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 得到的条目.
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> Entries
        {
            get
            {
                if (HttpRuntime.Cache == null)
                {
                    return Enumerable.Empty<KeyValuePair<string, object>>();
                }

                return from entry in HttpRuntime.Cache.Cast<DictionaryEntry>()
                       let key = entry.Key.ToString()
                       where key.StartsWith(RegionName)
                       select new KeyValuePair<string, object>(key.Substring(RegionName.Length), entry.Value);
            }
        }

        #endregion

        #region public 方法

        /// <summary>
        /// 构建关键字.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string BuildKey(string key)
        {
            return key.HasValue() ? RegionName + key : null;
        }

        /// <summary>
        /// 根据关键字获取值.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Get(string key)
        {
            if (HttpRuntime.Cache == null)
            {
                return null;
            }

            var value = HttpRuntime.Cache.Get(BuildKey(key));

            return value.Equals(FakeNull) ? null : value;
        }

        /// <summary>
        /// 设置缓存内容.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <param name="value">
        /// 值.
        /// </param>
        /// <param name="cacheTime">
        /// 超时时间.
        /// </param>
        public void Set(string key, object value, int? cacheTime)
        {
            if (HttpRuntime.Cache == null)
            {
                return;
            }

            key = BuildKey(key);

            var absoluteExpiration = Cache.NoAbsoluteExpiration;
            if (cacheTime.GetValueOrDefault() > 0)
            {
                if (cacheTime != null)
                {
                    absoluteExpiration = DateTime.UtcNow + TimeSpan.FromMinutes(cacheTime.Value);
                }
            }

            HttpRuntime.Cache.Insert(key, value ?? FakeNull, null, absoluteExpiration, Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 判断缓存中是否存在关键字.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Contains(string key)
        {
            if (HttpRuntime.Cache == null)
            {
                return false;
            }

            return HttpRuntime.Cache.Get(BuildKey(key)) != null;
        }

        /// <summary>
        /// 移除缓存.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public void Remove(string key)
        {
            if (HttpRuntime.Cache == null)
            {
                return;
            }

            HttpRuntime.Cache.Remove(BuildKey(key));
        }

        #endregion
    }
}