// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectSignatureAttribute.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义ObjectSignatureAttribute类型
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Attributes
{
    using System;

    /// <summary>
    /// 对象签名属性.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)] 
    public sealed class ObjectSignatureAttribute : Attribute
    {
    }
}
