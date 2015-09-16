// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义缓存管理器
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Caching
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;

    using Skymate.Utilities;

    using Skymate.Utilities.Threading;

    /// <summary>
    /// 缓存管理器.
    /// </summary>
    /// <typeparam name="TCache">
    /// 泛型
    /// </typeparam>
    public class CacheManager<TCache> : ICacheManager where TCache : ICache
    {
        #region 字段

        /// <summary>
        /// 读写锁.
        /// </summary>
        private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 缓存.
        /// </summary>
        private readonly ICache cache;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个关于 <see cref="CacheManager{TCache}"/> 类的实例.
        /// </summary>
        /// <param name="fn">
        /// 值.
        /// </param>
        public CacheManager(Func<Type, ICache> fn)
        {
            this.cache = fn(typeof(TCache));
        }

        #endregion

        #region 共有方法

        /// <summary>
        /// 根据关键字获取缓存的值.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <param name="acquirer">
        /// 提供缓存包保存的值.
        /// </param>
        /// <param name="cacheTime">
        /// 缓存超时时间.
        /// </param>
        /// <typeparam name="T">
        /// 获取对象的类型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Get<T>(string key, Func<T> acquirer, int? cacheTime = null)
        {
            Guard.ArgumentNotEmpty(() => key);

            if (this.cache.Contains(key))
            {
                return (T)this.cache.Get(key);
            }

            using (this.EnterReadLock())
            {
                if (this.cache.Contains(key))
                {
                    return (T)this.cache.Get(key);
                }

                var value = acquirer();
                this.Set(key, value, cacheTime);

                return value;
            }
        }

        /// <summary>
        /// 设置缓存的值.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <param name="value">
        /// 值.
        /// </param>
        /// <param name="cacheTime">
        /// 缓存超时时间.
        /// </param>
        public void Set(string key, object value, int? cacheTime = null)
        {
            Guard.ArgumentNotEmpty(() => key);

            if (value == null)
            {
                return;
            }

            using (this.EnterWriteLock())
            {
                this.cache.Set(key, value, cacheTime);
            }
        }

        /// <summary>
        /// 判断缓存中是否含有关键字.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Contains(string key)
        {
            Guard.ArgumentNotEmpty(() => key);

            return this.cache.Contains(key);
        }

        /// <summary>
        /// 移除缓存.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        public void Remove(string key)
        {
            Guard.ArgumentNotEmpty(() => key);

            using (this.EnterWriteLock())
            {
                this.cache.Remove(key);
            }
        }

        /// <summary>
        /// 根据正则表达式移除缓存.
        /// </summary>
        /// <param name="pattern">
        /// 正则表达式.
        /// </param>
        public void RemoveByPattern(string pattern)
        {
            Guard.ArgumentNotEmpty(() => pattern);

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = this.cache.Entries.Where(m => regex.IsMatch(m.Key)).Select(m => m.Key);

            using (this.EnterWriteLock())
            {
                foreach (var key in keysToRemove)
                {
                    this.cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// 清空缓存.
        /// </summary>
        public void Clear()
        {
            var keysToRemove = this.cache.Entries.Select(item => item.Key).ToList();

            using (this.EnterWriteLock())
            {
                foreach (string key in keysToRemove)
                {
                    this.cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// 进入写锁.
        /// </summary>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        public IDisposable EnterWriteLock()
        {
            return this.cache.IsSingleton ? this.readerWriterLock.GetWriteLock() : ActionDisposable.Empty;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 进入读锁.
        /// </summary>
        /// <returns>
        /// The <see cref="IDisposable"/>.
        /// </returns>
        private IDisposable EnterReadLock()
        {
            return this.cache.IsSingleton ? this.readerWriterLock.GetUpgradeableReadLock() : ActionDisposable.Empty;
        }

        #endregion
    }
}