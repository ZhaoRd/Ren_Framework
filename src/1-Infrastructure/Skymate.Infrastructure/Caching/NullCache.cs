// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullCache.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义空缓存
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Caching
{
    using System;

    using Skymate.Utilities;

    /// <summary>
    /// 代表一个空缓存
    /// </summary>
    public class NullCache : ICacheManager
    {
        #region 字段

        /// <summary>
        /// 单例.
        /// </summary>
        private static readonly ICacheManager SInstance = new NullCache();

        #endregion

        #region 属性

        /// <summary>
        /// 单例.
        /// </summary>
        public static ICacheManager Instance
        {
            get
            {
                return SInstance;
            }
        }

        #endregion

        #region ICacheManager 接口

        /// <summary>
        /// 如果缓存中不存在该项
        /// 则获得与指定键或关联缓存项添加项
        /// </summary>
        /// <param name="key">
        /// 缓存项关键字.
        /// </param>
        /// <param name="acquirer">
        /// 函数返回值被添加到缓存中.
        /// </param>
        /// <param name="cacheTime">
        /// 过期时间.
        /// </param>
        /// <typeparam name="T">
        /// 项目的类型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Get<T>(string key, Func<T> acquirer, int? cacheTime = null)
        {
            if (acquirer == null)
            {
                return default(T);
            }

            return acquirer();
        }

        /// <summary>
        /// 添加一个缓存项指定的关键字
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <param name="value">
        /// 值.
        /// </param>
        /// <param name="cacheTime">
        /// 过期时间.
        /// </param>
        public void Set(string key, object value, int? cacheTime = null)
        {
        }

        /// <summary>
        /// 获取一个值,该值指示值是否与指定的键相关联的是缓存
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        /// <returns>
        /// 结果
        /// </returns>
        public bool Contains(string key)
        {
            return false;
        }

        /// <summary>
        /// 从缓存中删除值与指定键
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        public void Remove(string key)
        {
        }

        /// <summary>
        /// 删除物品的模式
        /// </summary>
        /// <param name="pattern">
        /// 正则表达式
        /// </param>
        public void RemoveByPattern(string pattern)
        {
        }

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// 返回一个包装同步锁底层 <c>ICache</c>实现
        /// </summary>
        /// <returns>一次性同步锁</returns>
        public IDisposable EnterWriteLock()
        {
            return ActionDisposable.Empty;
        }

        #endregion
    }
}