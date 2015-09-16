// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheManager.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义缓存管理器接口
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Caching
{
    using System;

    /// <summary>
    /// 缓存管理器接口
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 如果缓存中不存在该项
        /// 则获得与指定键或关联缓存项添加项
        /// </summary>
        /// <typeparam name="T">
        /// 项目的类型或添加
        /// </typeparam>
        /// <param name="key">
        /// 缓存项关键字
        /// </param>
        /// <param name="acquirer">
        /// 函数返回值被添加到缓存中
        /// </param>
        /// <param name="cacheTime">
        /// 过期时间
        /// </param>
        /// <returns>
        /// 缓存项的值
        /// </returns>
        T Get<T>(string key, Func<T> acquirer, int? cacheTime = null);

        /// <summary>
        /// 添加一个缓存项指定的关键字
        /// </summary>
        /// <param name="key">
        /// 关键字
        /// </param>
        /// <param name="value">
        /// 值
        /// </param>
        /// <param name="cacheTime">
        /// 过期时间
        /// </param>
        void Set(string key, object value, int? cacheTime = null);

        /// <summary>
        /// 获取一个值,该值指示值是否与指定的键相关联的是缓存
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

        /// <summary>
        /// 删除物品的模式
        /// </summary>
        /// <param name="pattern">
        /// 正则表达式
        /// </param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        void Clear();

        /// <summary>
        /// 返回一个包装同步锁底层 <c>ICache</c>实现
        /// </summary>
        /// <returns>一次性同步锁</returns>
        /// <remarks>
        /// 这个方法包裹这一个 <c>ReaderWriterLockSlim</c> 或则一个空的操作
        /// 依赖 <c>ICache</c> 实现.
        /// 静态或者单例缓存通常返回一个 <c>ReaderWriterLockSlim</c> 实例
        /// 如果你想修改缓存项的值，在更新过程中必须锁定访问缓存。
        /// </remarks>
        IDisposable EnterWriteLock();
    }
}