// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinqExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   LINQ扩展
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// The linq extensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// The extract property info.
        /// </summary>
        /// <param name="propertyAccessor">
        /// The property accessor.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyInfo"/>.
        /// </returns>
        public static PropertyInfo ExtractPropertyInfo(this LambdaExpression propertyAccessor)
        {
            return propertyAccessor.ExtractMemberInfo() as PropertyInfo;
        }

        /// <summary>
        /// The extract field info.
        /// </summary>
        /// <param name="propertyAccessor">
        /// The property accessor.
        /// </param>
        /// <returns>
        /// The <see cref="FieldInfo"/>.
        /// </returns>
        public static FieldInfo ExtractFieldInfo(this LambdaExpression propertyAccessor)
        {
            return propertyAccessor.ExtractMemberInfo() as FieldInfo;
        }

        /// <summary>
        /// The extract member info.
        /// </summary>
        /// <param name="propertyAccessor">
        /// The property accessor.
        /// </param>
        /// <returns>
        /// The <see cref="MemberInfo"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// 参数异常
        /// </exception>
        public static MemberInfo ExtractMemberInfo(this LambdaExpression propertyAccessor)
        {
            Guard.ArgumentNotNull(() => propertyAccessor);

            MemberInfo info;
            try
            {
                MemberExpression operand;

                var expression = propertyAccessor;

                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    var body = unaryExpression;
                    operand = (MemberExpression)body.Operand;
                }
                else
                {
                    // o.PropertyOrField
                    operand = (MemberExpression)expression.Body;
                }

                // Member
                MemberInfo member = operand.Member;
                info = member;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The property or field accessor expression is not in the expected format 'o => o.PropertyOrField'.", e);
            }

            return info;
        }
    }
}