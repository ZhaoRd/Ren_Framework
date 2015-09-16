// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryConvertException.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   字典转换异常
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities.Converts
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// The dictionary convert exception.
    /// 字典转换异常
    /// </summary>
    [Serializable]
    public class DictionaryConvertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryConvertException"/> class.
        /// 初始化字典转换异常实例
        /// </summary>
        /// <param name="problems">
        /// The problems.
        /// 问题
        /// </param>
        public DictionaryConvertException(ICollection<ConvertProblem> problems)
            : this(CreateMessage(problems), problems)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryConvertException"/> class.
        /// 初始化字典转换异常实例
        /// </summary>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="problems">
        /// The problems.
        /// 问题
        /// </param>
        public DictionaryConvertException(string message, ICollection<ConvertProblem> problems)
            : base(message)
        {
            this.Problems = problems;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryConvertException"/> class.
        /// 初始化字典转换异常实例
        /// </summary>
        /// <param name="info">
        /// The info.
        /// 信息
        /// </param>
        /// <param name="context">
        /// The context.
        /// 上下文
        /// </param>
        public DictionaryConvertException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the problems.
        /// 获取问题集合
        /// </summary>
        public ICollection<ConvertProblem> Problems { get; private set; }

        /// <summary>
        /// The create message.
        /// 创建消息
        /// </summary>
        /// <param name="problems">
        /// The problems.
        /// 问题
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>string</cref>
        ///     </see>
        ///     .
        /// </returns>
        private static string CreateMessage(IEnumerable<ConvertProblem> problems)
        {
            var counter = 0;
            var builder = new StringBuilder();
            builder.Append("Could not convert all input values into their expected types:");
            builder.Append(Environment.NewLine);
            foreach (var prob in problems)
            {
                builder.AppendFormat("-----Problem[{0}]---------------------", counter++);
                builder.Append(Environment.NewLine);
                builder.Append(prob);
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }
    }
}