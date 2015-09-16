// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Guard.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved. 
// </copyright>
// <summary>
//   定义守护类
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    using Skymate.Extensions;

    /// <summary>
    /// 守护类，验证参数等.
    /// </summary>
    public class Guard
    {
        /// <summary>
        /// The against message.
        /// </summary>
        private const string AgainstMessage = "Assertion evaluation failed with 'false'.";

        /// <summary>
        /// The implements message.
        /// </summary>
        private const string ImplementsMessage = "Type '{0}' must implement type '{1}'.";

        /// <summary>
        /// The inherits from message.
        /// </summary>
        private const string InheritsFromMessage = "Type '{0}' must inherit from type '{1}'.";

        /// <summary>
        /// The is type of message.
        /// </summary>
        private const string IsTypeOfMessage = "Type '{0}' must be of type '{1}'.";

        /// <summary>
        /// The is equal message.
        /// </summary>
        private const string IsEqualMessage = "Compared objects must be equal.";

        /// <summary>
        /// The is positive message.
        /// </summary>
        private const string IsPositiveMessage = "Argument '{0}' must be a positive value. Value: '{1}'.";

        /// <summary>
        /// The is true message.
        /// </summary>
        private const string IsTrueMessage = "True expected for '{0}' but the condition was False.";

        /// <summary>
        /// The not negative message.
        /// </summary>
        private const string NotNegativeMessage = "Argument '{0}' cannot be a negative value. Value: '{1}'.";

        /// <summary>
        /// Prevents a default instance of the <see cref="Guard"/> class from being created.
        /// </summary>
        private Guard()
        {
        }

        /// <summary>
        /// Throws proper exception if the class reference is null.
        /// </summary>
        /// <typeparam name="TValue">
        /// 泛型
        /// </typeparam>
        /// <param name="value">
        /// Class reference to check.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// If class reference is null.
        /// </exception>
        [DebuggerStepThrough]
        public static void NotNull<TValue>(Func<TValue> value)
        {
            if (value() == null)
            {
                throw new InvalidOperationException("'{0}' cannot be null.".FormatInvariant(value));
            }
        }

        /// <summary>
        /// The argument not null.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotNull(object arg, string argName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }
        }

        /// <summary>
        /// The argument not null.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(Func<T> arg)
        {
            if (arg() == null)
            {
                throw new ArgumentNullException(GetParamName(arg));
            }
        }

        /// <summary>
        /// The arguments.
        /// </summary>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        /// <typeparam name="T1">
        /// </typeparam>
        /// <typeparam name="T2">
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        [DebuggerStepThrough]
        public static void Arguments<T1, T2>(Func<T1> arg1, Func<T2> arg2)
        {
            ArgumentNotNull(arg1);
            ArgumentNotNull(arg2);
        }

        /// <summary>
        /// The arguments.
        /// </summary>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        /// <param name="arg3">
        /// The arg 3.
        /// </param>
        /// <typeparam name="T1">
        /// </typeparam>
        /// <typeparam name="T2">
        /// </typeparam>
        /// <typeparam name="T3">
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3)
        {
            ArgumentNotNull(arg1);
            ArgumentNotNull(arg2);
            ArgumentNotNull(arg3);
        }

        /// <summary>
        /// The arguments.
        /// </summary>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        /// <param name="arg3">
        /// The arg 3.
        /// </param>
        /// <param name="arg4">
        /// The arg 4.
        /// </param>
        /// <typeparam name="T1">
        /// </typeparam>
        /// <typeparam name="T2">
        /// </typeparam>
        /// <typeparam name="T3">
        /// </typeparam>
        /// <typeparam name="T4">
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3, T4>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4)
        {
            ArgumentNotNull(arg1);
            ArgumentNotNull(arg2);
            ArgumentNotNull(arg3);
            ArgumentNotNull(arg4);
        }

        /// <summary>
        /// The arguments.
        /// </summary>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        /// <param name="arg3">
        /// The arg 3.
        /// </param>
        /// <param name="arg4">
        /// The arg 4.
        /// </param>
        /// <param name="arg5">
        /// The arg 5.
        /// </param>
        /// <typeparam name="T1">
        /// </typeparam>
        /// <typeparam name="T2">
        /// </typeparam>
        /// <typeparam name="T3">
        /// </typeparam>
        /// <typeparam name="T4">
        /// </typeparam>
        /// <typeparam name="T5">
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3, T4, T5>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4, Func<T5> arg5)
        {
            ArgumentNotNull(arg1);
            ArgumentNotNull(arg2);
            ArgumentNotNull(arg3);
            ArgumentNotNull(arg4);
            ArgumentNotNull(arg5);
        }

        /// <summary>
        /// The argument not empty.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <exception>
        ///     <cref>???</cref>
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotEmpty(Func<string> arg)
        {
            if (arg().IsEmpty())
            {
                string argName = GetParamName(arg);
                throw Error.Argument(argName, "String parameter '{0}' cannot be null or all whitespace.", argName);
            }
        }

        /// <summary>
        /// The argument not empty.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <exception>
        ///     <cref>???</cref>
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotEmpty(Func<Guid> arg)
        {
            if (arg() == Guid.Empty)
            {
                string argName = GetParamName(arg);
                throw Error.Argument(argName, "Argument '{0}' cannot be an empty guid.", argName);
            }
        }

        /// <summary>
        /// The argument not empty.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotEmpty(string arg, string argName)
        {
            if (arg.IsEmpty())
                throw Error.Argument(argName, "String parameter '{0}' cannot be null or all whitespace.", argName);
        }

        /// <summary>
        /// The argument not empty.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="Exception">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotEmpty<T>(ICollection<T> arg, string argName)
        {
            if (arg != null && !arg.Any())
                throw Error.Argument(argName, "Collection cannot be null and must have at least one item.");
        }

        /// <summary>
        /// The argument not empty.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotEmpty(Guid arg, string argName)
        {
            if (arg == Guid.Empty)
                throw Error.Argument(argName, "Argument '{0}' cannot be an empty guid.", argName);
        }

        /// <summary>
        /// The argument in range.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="Exception">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentInRange<T>(T arg, T min, T max, string argName) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(min) < 0 || arg.CompareTo(max) > 0)
                throw Error.ArgumentOutOfRange(argName, "The argument '{0}' must be between '{1}' and '{2}'.", argName, min, max);
        }

        /// <summary>
        /// The argument not out of length.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="maxLength">
        /// The max length.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotOutOfLength(string arg, int maxLength, string argName)
        {
            if (arg.Trim().Length > maxLength)
            {
                throw Error.Argument(argName, "Argument '{0}' cannot be more than {1} characters long.", argName, maxLength);
            }
        }

        /// <summary>
        /// The argument not negative.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="Exception">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotNegative<T>(T arg, string argName, string message = NotNegativeMessage) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) < 0)
                throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName, arg));
        }

        /// <summary>
        /// The argument not zero.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="???">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotZero<T>(T arg, string argName) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) == 0)
                throw Error.ArgumentOutOfRange(argName, "Argument '{0}' must be greater or less than zero. Value: '{1}'.", argName, arg);
        }

        /// <summary>
        /// The against.
        /// </summary>
        /// <param name="assertion">
        /// The assertion.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="TException">
        /// </typeparam>
        /// <exception cref="TException">
        /// </exception>
        [DebuggerStepThrough]
        public static void Against<TException>(bool assertion, string message = AgainstMessage) where TException : Exception
        {
            if (assertion)
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        /// <summary>
        /// The against.
        /// </summary>
        /// <param name="assertion">
        /// The assertion.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="TException">
        /// </typeparam>
        /// <exception cref="TException">
        /// </exception>
        [DebuggerStepThrough]
        public static void Against<TException>(Func<bool> assertion, string message = AgainstMessage) where TException : Exception
        {
            // Execute the lambda and if it evaluates to true then throw the exception.
            if (assertion())
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        /// <summary>
        /// The inherits from.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <typeparam name="TBase">
        /// </typeparam>
        [DebuggerStepThrough]
        public static void InheritsFrom<TBase>(Type type)
        {
            InheritsFrom<TBase>(type, InheritsFromMessage.FormatInvariant(type.FullName, typeof(TBase).FullName));
        }

        /// <summary>
        /// The inherits from.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="TBase">
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        [DebuggerStepThrough]
        public static void InheritsFrom<TBase>(Type type, string message)
        {
            if (type.BaseType != typeof(TBase))
                throw new InvalidOperationException(message);
        }

        /// <summary>
        /// The implements.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="TInterface">
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        [DebuggerStepThrough]
        public static void Implements<TInterface>(Type type, string message = ImplementsMessage)
        {
            if (!typeof(TInterface).IsAssignableFrom(type))
                throw new InvalidOperationException(message.FormatInvariant(type.FullName, typeof(TInterface).FullName));
        }

        /// <summary>
        /// The is subclass of.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <typeparam name="TBase">
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        [DebuggerStepThrough]
        public static void IsSubclassOf<TBase>(Type type)
        {
            var baseType = typeof(TBase);
            if (!baseType.IsSubClass(type))
            {
                throw new InvalidOperationException("Type '{0}' must be a subclass of type '{1}'.".FormatInvariant(type.FullName, baseType.FullName));
            }
        }

        /// <summary>
        /// The is type of.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <typeparam name="TType">
        /// </typeparam>
        [DebuggerStepThrough]
        public static void IsTypeOf<TType>(object instance)
        {
            IsTypeOf<TType>(instance, IsTypeOfMessage.FormatInvariant(instance.GetType().Name, typeof(TType).FullName));
        }

        /// <summary>
        /// The is type of.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="TType">
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        [DebuggerStepThrough]
        public static void IsTypeOf<TType>(object instance, string message)
        {
            if (!(instance is TType))
                throw new InvalidOperationException(message);
        }

        /// <summary>
        /// The is equal.
        /// </summary>
        /// <param name="compare">
        /// The compare.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="TException">
        /// </typeparam>
        /// <exception cref="TException">
        /// </exception>
        [DebuggerStepThrough]
        public static void IsEqual<TException>(object compare, object instance, string message = IsEqualMessage) where TException : Exception
        {
            if (!compare.Equals(instance))
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        /// <summary>
        /// The has default constructor.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        [DebuggerStepThrough]
        public static void HasDefaultConstructor<T>()
        {
            HasDefaultConstructor(typeof(T));
        }

        /// <summary>
        /// The has default constructor.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <exception cref="???">
        /// </exception>
        [DebuggerStepThrough]
        public static void HasDefaultConstructor(Type t)
        {
            if (!t.HasDefaultConstructor())
                throw Error.InvalidOperation("The type '{0}' must have a default parameterless constructor.", t.FullName);
        }

        /// <summary>
        /// The argument is positive.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="???">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentIsPositive<T>(T arg, string argName, string message = IsPositiveMessage) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) < 1)
                throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName));
        }

        /// <summary>
        /// The argument is true.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <exception cref="???">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentIsTrue(bool arg, string argName, string message = IsTrueMessage)
        {
            if (!arg)
                throw Error.Argument(argName, message.FormatInvariant(argName));
        }

        /// <summary>
        /// The argument is enum type.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <exception cref="???">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentIsEnumType(Type arg, string argName)
        {
            ArgumentNotNull(arg, argName);
            if (!arg.IsEnum)
                throw Error.Argument(argName, "Type '{0}' must be a valid Enum type.", arg.FullName);
        }

        /// <summary>
        /// The argument is enum type.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <exception cref="???">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentIsEnumType(Type enumType, object arg, string argName)
        {
            ArgumentNotNull(arg, argName);
            if (!Enum.IsDefined(enumType, arg))
            {
                throw Error.ArgumentOutOfRange(argName, "The value of the argument '{0}' provided for the enumeration '{1}' is invalid.", argName, enumType.FullName);
            }
        }

        /// <summary>
        /// The argument not disposed.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <exception cref="???">
        /// </exception>
        [DebuggerStepThrough]
        public static void ArgumentNotDisposed(DisposableObject arg, string argName)
        {
            ArgumentNotNull(arg, argName);
            if (arg.IsDisposed)
                throw Error.ObjectDisposed(argName);
        }

        /// <summary>
        /// The paging args valid.
        /// </summary>
        /// <param name="indexArg">
        /// The index arg.
        /// </param>
        /// <param name="sizeArg">
        /// The size arg.
        /// </param>
        /// <param name="indexArgName">
        /// The index arg name.
        /// </param>
        /// <param name="sizeArgName">
        /// The size arg name.
        /// </param>
        [DebuggerStepThrough]
        public static void PagingArgsValid(int indexArg, int sizeArg, string indexArgName, string sizeArgName)
        {
            ArgumentNotNegative<int>(indexArg, indexArgName, "PageIndex cannot be below 0");
            if (indexArg > 0)
            {
                // if pageIndex is specified (> 0), PageSize CANNOT be 0
                ArgumentIsPositive<int>(sizeArg, sizeArgName, "PageSize cannot be below 1 if a PageIndex greater 0 was provided.");
            }
            else
            {
                // pageIndex 0 actually means: take all!
                ArgumentNotNegative(sizeArg, sizeArgName);
            }
        }

        /// <summary>
        /// The get param name.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        private static string GetParamName<T>(Expression<Func<T>> expression)
        {
            string name = string.Empty;
            MemberExpression body = expression.Body as MemberExpression;

            if (body != null)
            {
                name = body.Member.Name;
            }

            return name;
        }

        /// <summary>
        /// The get param name.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [DebuggerStepThrough]
        private static string GetParamName<T>(Func<T> expression)
        {
            return expression.Method.Name;
        }
    }
}