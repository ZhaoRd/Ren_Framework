// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Error.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   错误类
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using Skymate.Extensions;

    /// <summary>
    /// The error.
    /// 错误类
    /// </summary>
    public static class Error
    {
        /// <summary>
        /// The application.
        /// 应用程序异常
        /// </summary>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="args">
        /// The args.
        /// 参数
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception Application(string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args));
        }

        /// <summary>
        /// The application.
        /// 应用程序异常
        /// </summary>
        /// <param name="innerException">
        /// The inner exception.
        /// 内部异常
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="args">
        /// The args.
        /// 参数
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception Application(Exception innerException, string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args), innerException);
        }

        /// <summary>
        /// The argument null or empty.
        /// 参数非空异常
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// 参数
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception ArgumentNullOrEmpty(Func<string> arg)
        {
            var argName = arg.Method.Name;
            return new ArgumentException("String parameter '{0}' cannot be null or all whitespace.", argName);
        }

        /// <summary>
        /// The argument null.
        /// 参数非空异常
        /// </summary>
        /// <param name="argName">
        /// The arg name.
        /// 参数名称
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception ArgumentNull(string argName)
        {
            return new ArgumentNullException(argName);
        }

        /// <summary>
        /// The argument null.
        /// 参数非空异常
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// 参数
        /// </param>
        /// <typeparam name="T">
        /// 类型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception ArgumentNull<T>(Func<T> arg)
        {
            var message = "Argument of type '{0}' cannot be null".FormatInvariant(typeof(T));
            var argName = arg.Method.Name;
            return new ArgumentNullException(argName, message);
        }

        /// <summary>
        /// The argument out of range.
        /// 超出范围异常
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// 参数
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange<T>(Func<T> arg)
        {
            var argName = arg.Method.Name;
            return new ArgumentOutOfRangeException(argName);
        }

        /// <summary>
        /// The argument out of range.
        /// 超出范围异常
        /// </summary>
        /// <param name="argName">
        /// The arg name.
        /// 参数名称
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName)
        {
            return new ArgumentOutOfRangeException(argName);
        }

        /// <summary>
        /// The argument out of range.
        /// 超出范围异常
        /// </summary>
        /// <param name="argName">
        /// The arg name.
        /// 参数名称
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName, string message, params object[] args)
        {
            return new ArgumentOutOfRangeException(argName, string.Format(CultureInfo.CurrentCulture, message, args));
        }

        /// <summary>
        /// The argument.
        /// 参数异常
        /// </summary>
        /// <param name="argName">
        /// The arg name.
        /// 参数名称
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception Argument(string argName, string message, params object[] args)
        {
            return new ArgumentException(string.Format(CultureInfo.CurrentCulture, message, args), argName);
        }

        /// <summary>
        /// The argument.
        /// 参数异常
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// 参数
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception Argument<T>(Func<T> arg, string message, params object[] args)
        {
            var argName = arg.Method.Name;
            return new ArgumentException(message.FormatCurrent(args), argName);
        }

        /// <summary>
        /// The invalid operation.
        /// 无效操作异常
        /// </summary>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="args">
        /// The args.
        /// 参数
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, params object[] args)
        {
            return InvalidOperation(message, null, args);
        }

        /// <summary>
        /// The invalid operation.
        /// 无效操作异常
        /// </summary>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// 内部异常
        /// </param>
        /// <param name="args">
        /// The args.
        /// 参数
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, Exception innerException, params object[] args)
        {
            return new InvalidOperationException(message.FormatCurrent(args), innerException);
        }

        /// <summary>
        /// The invalid operation.
        /// 无效操作异常
        /// </summary>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="member">
        /// The member.
        /// 成员
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Func<T> member)
        {
            return InvalidOperation(message, null, member);
        }

        /// <summary>
        /// The invalid operation.
        /// 无效操作异常
        /// </summary>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// 内部异常
        /// </param>
        /// <param name="member">
        /// The member.
        /// 成员
        /// </param>
        /// <typeparam name="T">
        /// 泛型
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Exception innerException, Func<T> member)
        {
            Guard.ArgumentNotNull(message, "message");
            Guard.ArgumentNotNull(member, "member");

            return new InvalidOperationException(message.FormatCurrent(member.Method.Name), innerException);
        }

        /// <summary>
        /// The invalid cast.
        /// 类型转换异常
        /// </summary>
        /// <param name="fromType">
        /// The from type.
        /// 来源类型
        /// </param>
        /// <param name="toType">
        /// The to type.
        /// 转换类型
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType)
        {
            return InvalidCast(fromType, toType, null);
        }

        /// <summary>
        /// The invalid cast.
        /// 类型转换异常
        /// </summary>
        /// <param name="fromType">
        /// The from type.
        /// 来源类型
        /// </param>
        /// <param name="toType">
        /// The to type.
        /// 目标类型
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// 内部异常
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType, Exception innerException)
        {
            return new InvalidCastException("Cannot convert from type '{0}' to '{1}'.".FormatCurrent(fromType.FullName, toType.FullName), innerException);
        }

        /// <summary>
        /// The not supported.
        /// 不支持异常
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        /// <summary>
        /// The not implemented.
        /// 未实现异常
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        /// <summary>
        /// The object disposed.
        /// 对象已注销异常
        /// </summary>
        /// <param name="objectName">
        /// The object name.
        /// 对象名称
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName)
        {
            return new ObjectDisposedException(objectName);
        }

        /// <summary>
        /// The object disposed.
        /// 对象已注销异常
        /// </summary>
        /// <param name="objectName">
        /// The object name.
        /// 对象名称
        /// </param>
        /// <param name="message">
        /// The message.
        /// 消息
        /// </param>
        /// <param name="args">
        /// The args.
        /// 参数
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName, string message, params object[] args)
        {
            return new ObjectDisposedException(objectName, string.Format(CultureInfo.CurrentCulture, message, args));
        }

        /// <summary>
        /// The no elements.
        /// 无元素异常
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception NoElements()
        {
            return new InvalidOperationException("Sequence contains no elements.");
        }

        /// <summary>
        /// The more than one element.
        /// 超过一个异常
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>Exception</cref>
        ///     </see>
        ///     .
        /// </returns>
        [DebuggerStepThrough]
        public static Exception MoreThanOneElement()
        {
            return new InvalidOperationException("Sequence contains more than one element.");
        }
    }
}
