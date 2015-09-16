// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFriendlyNameAttribute.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//  定义枚举类型的友好名称
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Attributes
{
    using System;

    /// <summary>
    /// 提供了一个友好的显示名称的枚举类型的值.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumFriendlyNameAttribute : Attribute
    {
        #region 构造函数

        /// <summary>
        /// 创建一个关于 <see cref="EnumFriendlyNameAttribute"/> 的实例.
        /// </summary>
        /// <param name="friendlyName">
        /// 友好名称.
        /// </param>
        public EnumFriendlyNameAttribute(string friendlyName)
        {
            this.FriendlyName = friendlyName;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取友好的名称.
        /// </summary>
        public string FriendlyName { get; private set; }

        #endregion
    }
}
