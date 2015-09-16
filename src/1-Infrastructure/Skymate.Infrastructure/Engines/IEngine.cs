// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEngine.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summery>
// IOC引擎接口
// </summery>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Engines
{
    using System;

    using Skymate.DependencyManagement;

    /// <summary>
    /// The Engine interface.
    /// IOC引擎接口
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Gets the container manager.
        /// 获取 container manager
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// The initialize.
        /// 初始化
        /// </summary>
        void Initialize();

        /// <summary>
        /// The resolve.
        /// 解析
        /// </summary>
        /// <param name="name">
        /// The name.
        /// 名称
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Resolve<T>(string name = null) where T : class;

        /// <summary>
        /// The resolve.
        /// 解析
        /// </summary>
        /// <param name="type">
        /// The type.
        /// 类型
        /// </param>
        /// <param name="name">
        /// The name.
        /// 名称
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object Resolve(Type type, string name = null);

        /// <summary>
        /// The resolve all.
        /// 解析所有
        /// </summary>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>T[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        T[] ResolveAll<T>();
    }
}
