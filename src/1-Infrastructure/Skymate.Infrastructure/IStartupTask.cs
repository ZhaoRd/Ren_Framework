// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStartupTask.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    /// <summary>
    /// 启动任务接口
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// Gets the order.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// The execute.
        /// </summary>
        void Execute();
    }
}