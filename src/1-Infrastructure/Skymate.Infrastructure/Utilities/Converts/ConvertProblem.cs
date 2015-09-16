// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertProblem.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   转换问题实体类
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities.Converts
{
    using System;
    using System.Reflection;

    using Skymate.Extensions;

    /// <summary>
    /// The convert problem.
    /// 转换问题
    /// </summary>
    [Serializable]
    public class ConvertProblem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the item.
        /// 获取或设置项目
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets or sets the property.
        /// 获取或设置属性信息
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// Gets or sets the attempted value.
        /// 获取或设置附加值
        /// </summary>
        public object AttemptedValue { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// 获取或设置异常
        /// </summary>
        public Exception Exception { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The to string.
        /// 转换为字符串
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>string</cref>
        ///     </see>
        ///     .
        /// </returns>
        public override string ToString()
        {
            return @"Item type:     {0}
                Property:        {1}
                Property Type:   {2}
                Attempted Value: {3}
                Exception:
                {4}.".FormatCurrent(
                (this.Item != null) ? this.Item.GetType().FullName : "(null)",
                this.Property.Name,
                this.Property.PropertyType,
                this.AttemptedValue,
                this.Exception);
        }

        #endregion
    }
}