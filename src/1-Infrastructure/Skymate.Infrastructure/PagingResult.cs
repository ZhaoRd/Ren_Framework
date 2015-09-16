// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagingResult.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义分页结果.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System.Collections.Generic;

    /// <summary>
    /// 分页结果.
    /// </summary>
    /// <typeparam name="T">
    /// 对象类型
    /// </typeparam>
    public class PagingResult<T>
        where T : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagingResult{T}"/> class.
        /// </summary>
        /// <param name="total">
        /// 总数.
        /// </param>
        /// <param name="source">
        /// 数据源.
        /// </param>
        /// <exception cref="SkymateException">
        /// SkymateException
        /// </exception>
        public PagingResult(int total, IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new SkymateException("paging result source not allow null");
            }

            if (total < 0)
            {
                throw new SkymateException("paging result total not allow < 0");
            }

            this.Total = total;
            this.List = source;
        }

        /// <summary>
        /// Gets the total.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        public IEnumerable<T> List { get; set; }
    }
}
