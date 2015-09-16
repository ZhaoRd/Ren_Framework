// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChannelFactoryManager.cs" company="">
//   Copyright © 2015 Skymate. All rights reserved
// </copyright>
// <summary>
//   表示Channel Factory管理器。
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Communication
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    using Skymate;

    /// <summary>
    /// 表示Channel Factory管理器。
    /// </summary>
    internal sealed class ChannelFactoryManager : DisposableObject
    {
        #region Private Fields

        /// <summary>
        /// The factories.
        /// </summary>
        private static readonly Dictionary<Type, ChannelFactory> Factories = new Dictionary<Type, ChannelFactory>();

        /// <summary>
        /// The sync.
        /// </summary>
        private static readonly object Sync = new object();

        /// <summary>
        /// The instance.
        /// </summary>
        private static readonly ChannelFactoryManager instance = new ChannelFactoryManager();
        #endregion

        #region Ctor
        static ChannelFactoryManager() { }
        private ChannelFactoryManager() { }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取<c>ChannelFactoryManager</c>的单件（Singleton）实例。
        /// </summary>
        public static ChannelFactoryManager Instance
        {
            get { return instance; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 获取与指定服务契约类型相关的Channel Factory实例。
        /// </summary>
        /// <typeparam name="T">服务契约的类型。</typeparam>
        /// <returns>与指定服务契约类型相关的Channel Factory实例。</returns>
        public ChannelFactory<T> GetFactory<T>()
            where T : class//, IApplicationServiceContract
        {
            lock (Sync)
            {
                ChannelFactory factory = null;
                if (!Factories.TryGetValue(typeof(T), out factory))
                {
                    factory = new ChannelFactory<T>(typeof(T).Name);
                    factory.Open();
                    Factories.Add(typeof(T), factory);
                }
                return factory as ChannelFactory<T>;
            }
        }
        #endregion

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                lock (Sync)
                {
                    foreach (Type type in Factories.Keys)
                    {
                        ChannelFactory factory = Factories[type];
                        try
                        {
                            factory.Close();
                            continue;
                        }
                        catch
                        {
                            factory.Abort();
                            continue;
                        }
                    }
                    Factories.Clear();
                }
            }
        }
    }
}
