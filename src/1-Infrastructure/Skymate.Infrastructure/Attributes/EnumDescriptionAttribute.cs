// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumDescriptionAttribute.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//  定义枚举描述的特质
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Attributes
{
    using System;

    /// <summary>
    /// 提供了一个描述枚举类型.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        #region 构造方法

        /// <summary>
        /// 创建一个 <see cref="EnumDescriptionAttribute"/> 类的实例. 
        /// </summary>
        /// <param name="description">
        /// 描述存储在这个属性.
        /// </param>
        public EnumDescriptionAttribute(string description)
        {
            this.Description = description;
        }

        #endregion


        #region 属性

        /// <summary>
        /// 获取描述属性.
        /// </summary>
        /// <value>描述.</value>
        public string Description { get; private set; }

        #endregion
    }
}