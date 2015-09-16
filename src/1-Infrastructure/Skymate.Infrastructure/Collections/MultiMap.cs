// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiMap.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义Multimap.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Skymate;
    using Skymate.Extensions;

    /// <summary>
    /// 每个关键的数据结构,其中包含多个值.
    /// </summary>
    /// <typeparam name="TKey">类型的关键字.</typeparam>
    /// <typeparam name="TValue">类型的值.</typeparam>
    public class Multimap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>
    {
        #region 字段

        /// <summary>
        /// 项.
        /// </summary>
        private readonly IDictionary<TKey, ICollection<TValue>> items;

        /// <summary>
        /// 创建器.
        /// </summary>
        private readonly Func<ICollection<TValue>> listCreator;

        /// <summary>
        /// 是否只读.
        /// </summary>
        private readonly bool isReadonly;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建一个 <see cref="Multimap{TKey,TValue}"/> 的实例.
        /// </summary>
        public Multimap()
            : this(false)
        {
        }

        /// <summary>
        /// 创建一个 <see cref="Multimap{TKey,TValue}"/> 的实例.
        /// </summary>
        /// <param name="listCreator">
        /// 创建器列表.
        /// </param>
        public Multimap(Func<ICollection<TValue>> listCreator)
            : this(new Dictionary<TKey, ICollection<TValue>>(), listCreator)
        {
        }

        /// <summary>
        /// 创建一个 <see cref="Multimap{TKey,TValue}"/> 的实例
        /// </summary>
        /// <param name="dictionary">
        /// 关键字字典.
        /// </param>
        /// <param name="listCreator">
        /// 创建器.
        /// </param>
        internal Multimap(IDictionary<TKey, ICollection<TValue>> dictionary, Func<ICollection<TValue>> listCreator)
        {
            this.items = dictionary;
            this.listCreator = listCreator;
        }

        /// <summary>
        /// 创建一个 <see cref="Multimap{TKey,TValue}"/> 的实例.
        /// </summary>
        /// <param name="threadSafe">
        /// 指定线程安全.
        /// </param>
        internal Multimap(bool threadSafe)
        {
            if (threadSafe)
            {
                this.items = new ConcurrentDictionary<TKey, ICollection<TValue>>();
                /*this.listCreator = () => new SynchronizedCollection<TValue>();*/
            }
            else
            {
                this.items = new Dictionary<TKey, ICollection<TValue>>();
                this.listCreator = () => new List<TValue>();
            }
        }

        /// <summary>
        /// 创建一个 <see cref="Multimap{TKey,TValue}"/> 的实例
        /// </summary>
        /// <param name="dictionary">
        /// 关键字字典.
        /// </param>
        /// <param name="isReadonly">
        /// 是否只读.
        /// </param>
        protected Multimap(IDictionary<TKey, ICollection<TValue>> dictionary, bool isReadonly)
        {
            Guard.ArgumentNotNull(() => dictionary);

            this.items = dictionary;

            if (isReadonly && dictionary != null)
            {
                foreach (var kvp in dictionary)
                {
                    dictionary[kvp.Key] = kvp.Value.AsReadOnly();
                }
            }

            this.isReadonly = isReadonly;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 得到的计数组/钥匙.
        /// </summary>
        public int Count
        {
            get
            {
                return this.items.Keys.Count;
            }
        }

        /// <summary>
        /// 在所有组中总项数.
        /// </summary>
        public int TotalValueCount
        {
            get
            {
                return this.items.Values.Sum(x => x.Count);
            }
        }

        /// <summary>
        /// 键的集合.
        /// </summary>
        public virtual ICollection<TKey> Keys
        {
            get { return this.items.Keys; }
        }

        /// <summary>
        /// 得到的值的集合的集合.
        /// </summary>
        public virtual ICollection<ICollection<TValue>> Values
        {
            get { return this.items.Values; }
        }

        #endregion

        #region 索引

        /// <summary>
        /// 获取指定键值存储在集合.
        /// </summary>
        /// <param name="key">关键字.</param>
        /// <returns>返回值集合.</returns>
        public virtual ICollection<TValue> this[TKey key]
        {
            get
            {
                if (this.items.ContainsKey(key))
                {
                    return this.items[key];
                }

                if (!this.isReadonly)
                {
                    this.items[key] = this.listCreator();
                }
                else
                {
                    return null;
                }

                return this.items[key];
            }
        }

        #endregion

        #region 静态方法
        
        /// <summary>
        /// 创建一个线程安全的实例.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>Multimap</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static Multimap<TKey, TValue> ThreadSafe()
        {
            return new Multimap<TKey, TValue>(true);
        }

        /// <summary>
        /// 创建从查找.
        /// </summary>
        /// <param name="source">
        /// 源.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Multimap</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static Multimap<TKey, TValue> CreateFromLookup(ILookup<TKey, TValue> source)
        {
            Guard.ArgumentNotNull(() => source);

            var map = new Multimap<TKey, TValue>();

            foreach (IGrouping<TKey, TValue> group in source)
            {
                map.AddRange(group.Key, group);
            }

            return map;
        }

        #endregion

        #region public 方法

        /// <summary>
        /// 根据关键字查找.
        /// </summary>
        /// <param name="key">
        /// 关键字.
        /// </param>
        /// <param name="predicate">
        /// 表达式.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<TValue> Find(TKey key, Expression<Func<TValue, bool>> predicate)
        {
            Guard.ArgumentNotNull(() => key);
            Guard.ArgumentNotNull(() => predicate);

            if (this.items.ContainsKey(key))
            {
                return this.items[key].Where(predicate.Compile());
            }

            return Enumerable.Empty<TValue>();
        }

        /// <summary>
        /// 添加指定的值指定的关键.
        /// </summary>
        /// <param name="key">关键字.</param>
        /// <param name="value">值.</param>
        public virtual void Add(TKey key, TValue value)
        {
            this.CheckNotReadonly();

            this[key].Add(value);
        }

        /// <summary>
        /// 将指定的值添加到指定的关键.
        /// </summary>
        /// <param name="key">关键字.</param>
        /// <param name="values">值.</param>
        public virtual void AddRange(TKey key, IEnumerable<TValue> values)
        {
            this.CheckNotReadonly();

            this[key].AddRange(values);
        }

        /// <summary>
        /// 删除指定的值为指定的键.
        /// </summary>
        /// <param name="key">关键字.</param>
        /// <param name="value">值.</param>
        /// <returns>
        /// <c>True</c> 
        /// 值已经被删除; 
        /// 否则 <c>false</c>.</returns>
        public virtual bool Remove(TKey key, TValue value)
        {
            this.CheckNotReadonly();

            if (!this.items.ContainsKey(key))
            {
                return false;
            }

            bool result = this.items[key].Remove(value);
            if (this.items[key].Count == 0)
            {
                this.items.Remove(key);
            }

            return result;
        }

        /// <summary>
        /// 删除所有值为指定的关键.
        /// </summary>
        /// <param name="key">关键字.</param>
        /// <returns><c>True</c> 
        /// 如果关键字存在; 
        /// 否则 <c>false</c>.</returns>
        public virtual bool RemoveAll(TKey key)
        {
            this.CheckNotReadonly();
            return this.items.Remove(key);
        }

        /// <summary>
        /// 删除所有的值.
        /// </summary>
        public virtual void Clear()
        {
            this.CheckNotReadonly();
            this.items.Clear();
        }

        /// <summary>
        /// 决定了多重映射为指定的键包含任何值.
        /// </summary>
        /// <param name="key">关键字.</param>
        /// <returns><c>True</c> 
        /// 如果多重映射为指定的键有一个或多个值, 
        /// 否则 <c>false</c>.</returns>
        public virtual bool ContainsKey(TKey key)
        {
            return this.items.ContainsKey(key);
        }

        /// <summary>
        /// 决定是否包含指定值的多重映射为指定的关键.
        /// </summary>
        /// <param name="key">关键字.</param>
        /// <param name="value">值.</param>
        /// <returns><c>True</c> 
        /// 如果多重映射包含这样一个值; 
        /// 否则, <c>false</c>.</returns>
        public virtual bool ContainsValue(TKey key, TValue value)
        {
            return this.items.ContainsKey(key) && this.items[key].Contains(value);
        }

        /// <summary>
        /// 返回一个枚举器,通过多重映射迭代.
        /// </summary>
        /// <returns>一个 <see cref="IEnumerator"/> 
        /// 对象,该对象可以用来遍历多重映射.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// 返回一个枚举器,通过多重映射迭代.
        /// </summary>
        /// <returns>一个<see cref="IEnumerator"/> 
        /// 对象,该对象可以用来遍历多重映射.</returns>
        public virtual IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        #endregion

        #region private 方法

        /// <summary>
        /// The check not readonly.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// 不支持异常
        /// </exception>
        private void CheckNotReadonly()
        {
            if (this.isReadonly)
            {
                throw new NotSupportedException("Multimap is read-only.");
            }
        }

        #endregion
    }
}