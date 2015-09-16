// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestCache.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义请求缓存
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Caching
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;

    using Skymate.Extensions;

    /// <summary>
    /// 请求缓存.
    /// </summary>
    public class RequestCache : ICache
    {
        #region 字段

        /// <summary>
        /// 全名名称.
        /// </summary>
        private const string RegionName = "$$SkymateNET$$";

        /// <summary>
        /// http上下文.
        /// </summary>
        private readonly HttpContextBase context;

        #endregion

        #region 构造方法

        /// <summary>
        /// 创建一个关于 <see cref="RequestCache"/> 的新实例.
        /// </summary>
        /// <param name="context">
        /// http上下文.
        /// </param>
        public RequestCache(HttpContextBase context)
        {
            this.context = context;
        }

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
                var items = this.GetItems();
                if (items == null)
                {
                    yield break;
                }

                var enumerator = items.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var key = enumerator.Key as string;
                    if (key == null)
                    {
                        continue;
                    }

                    if (key.StartsWith(RegionName))
                    {
                        yield return
                            new KeyValuePair<string, object>(key.Substring(RegionName.Length), enumerator.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 获取一个值,该值指示读写是否这个缓存应该是线程安全的
        /// </summary>
        public bool IsSingleton
        {
            get
            {
                return false;
            }
        }

        #endregion

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
            var items = this.GetItems();
            if (items == null)
            {
                return null;
            }

            return items[this.BuildKey(key)];
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
            var items = this.GetItems();
            if (items == null)
            {
                return;
            }

            key = this.BuildKey(key);

            if (value == null)
            {
                return;
            }

            if (items.Contains(key))
            {
                items[key] = value;
            }
            else
            {
                items.Add(key, value);
            }
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
            var items = this.GetItems();
            if (items == null)
            {
                return false;
            }

            return items.Contains(this.BuildKey(key));
        }

        /// <summary>
        /// 从缓存中删除值与指定键
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        public void Remove(string key)
        {
            var items = this.GetItems();
            if (items == null)
            {
                return;
            }

            items.Remove(this.BuildKey(key));
        }

        #endregion

        #region protected 方法

        /// <summary>
        /// 获取上下文中的条目.
        /// </summary>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        protected IDictionary GetItems()
        {
            if (this.context != null)
            {
                return this.context.Items;
            }

            return null;
        }

        #endregion

        #region private 方法

        /// <summary>
        /// 创建关键字.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string BuildKey(string key)
        {
            return key.HasValue() ? RegionName + key : null;
        }

        #endregion
    }
}