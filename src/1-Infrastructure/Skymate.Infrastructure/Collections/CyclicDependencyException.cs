// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CyclicDependencyException.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   循环依赖异常.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Collections
{
    using System;

    /// <summary>
    /// 循环依赖异常.
    /// </summary>
    public class CyclicDependencyException : Exception
    {
        /// <summary>
        /// 创建 <see cref="CyclicDependencyException"/> 实例.
        /// </summary>
        public CyclicDependencyException()
            : base("Cyclic dependency detected")
        {
        }

        /// <summary>
        /// 创建 <see cref="CyclicDependencyException"/> 实例.
        /// </summary>
        /// <param name="message">
        /// 异常消息.
        /// </param>
        public CyclicDependencyException(string message)
            : base(message)
        {
        }
    }
}