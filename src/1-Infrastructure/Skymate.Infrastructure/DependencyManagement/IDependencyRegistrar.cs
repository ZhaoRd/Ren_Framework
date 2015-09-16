// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDependencyRegistrar.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义依赖注册接口.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.DependencyManagement
{
    using Autofac;

    using Skymate.TypeFinders;

    /// <summary>
    /// 依赖注册接口.
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// 获取一个值，指定注册顺序.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 允许实现者在未来全球注册依赖项容器的依赖
        /// </summary>
        /// <param name="builder">容器实例</param>
        /// <param name="typeFinder">查找器实例可以反映所有应用程序类型</param>
        /// <param name="isActiveModule">
        /// Indicates, whether the assembly containing this registrar instance is an active (installed) plugin assembly.
        /// The value is always <c>true</c>, if the containing assembly is not a plugin type.
        /// </param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, bool isActiveModule);
    }
}