// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationResuleGeneric.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   The operation resule generic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    /// <summary>
    /// The operation resule generic.
    /// </summary>
    public class OperationResult<TAppendData>
    {
        #region Ctro

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// 初始化操作结果实例
        /// </summary>
        /// <param name="resultType">
        /// The result type.
        /// 操作结果类型
        /// </param>
        public OperationResult(OperationResultType resultType)
        {
            this.ResultType = resultType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// 初始化操作结果实例
        /// </summary>
        /// <param name="resultType">
        /// The result type.
        /// 操作结果类型
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        public OperationResult(OperationResultType resultType, string message)
            : this(resultType)
        {
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// 初始化操作结果实例
        /// </summary>
        /// <param name="resultType">
        /// The result type.
        /// 操作结果类型
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="appendData">
        /// The append data.
        /// 附加数据
        /// </param>
        public OperationResult(OperationResultType resultType, string message, TAppendData appendData)
            : this(resultType, message)
        {
            this.AppendData = appendData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// 初始化操作结果实例
        /// </summary>
        /// <param name="resultType">
        /// The result type.
        /// 操作结果类型
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="logMessage">
        /// The log message.
        /// 日志消息
        /// </param>
        public OperationResult(OperationResultType resultType, string message, string logMessage)
            : this(resultType, message)
        {
            this.LogMessage = logMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// 初始化操作结果实例
        /// </summary>
        /// <param name="resultType">
        /// The result type.
        /// 操作结果类型
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="logMessage">
        /// The log message.
        /// 日志消息
        /// </param>
        /// <param name="appendData">
        /// The append data.
        /// 附加数据
        /// </param>
        public OperationResult(OperationResultType resultType, string message, string logMessage, TAppendData appendData)
            : this(resultType, message, logMessage)
        {
            this.AppendData = appendData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the result type.
        /// 获取操作结果类型
        /// </summary>
        public OperationResultType ResultType { get; private set; }

        /// <summary>
        /// Gets the message.
        /// 获取消息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the log message.
        /// 获取日志消息
        /// </summary>
        public string LogMessage { get; private set; }

        /// <summary>
        /// Gets the append data.
        /// 获取附加数据
        /// </summary>
        public TAppendData AppendData { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// The success.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="logMessage">
        /// The log message.
        /// </param>
        /// <param name="appendData">
        /// The append data.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResult"/>.
        /// </returns>
        public static OperationResult Success(string message = null, string logMessage = null, TAppendData appendData = default(TAppendData))
        {
            return new OperationResult(OperationResultType.Success, message, logMessage, appendData);
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="logMessage">
        /// The log message.
        /// </param>
        /// <param name="appendData">
        /// The append data.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResult"/>.
        /// </returns>
        public static OperationResult Error(string message = null, string logMessage = null, TAppendData appendData = default(TAppendData))
        {
            return new OperationResult(OperationResultType.Error, message, logMessage, appendData);
        }

        #endregion

    }
}