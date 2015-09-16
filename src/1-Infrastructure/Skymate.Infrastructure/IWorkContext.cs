// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorkContext.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    /// <summary>
    /// 工作上下文接口
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current customer
        /// 获取或设置当前用户id
        /// </summary>
        string CurrentCustomerId { get; set; }

        /// <summary>
        /// Gets or sets the original customer (in case the current one is impersonated)
        /// 获取原来的用户id
        /// </summary>
        string OriginalCustomerIdIfImpersonated { get; }

        /// <summary>
        /// Gets or sets current user working language
        /// 获取或设置当前工作语言
        /// </summary>
        string WorkingLanguageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we're in admin area
        /// 获取或设置是否为管理员
        /// </summary>
        bool IsAdmin { get; set; }

        /// <summary>
        /// Gets a value indicating whether a language exists and is published within a store's scope.
        /// </summary>
        /// <param name="seoCode">
        /// The unique seo code of the language to check for
        /// </param>
        /// <param name="storeId">
        /// The store id (will be resolved internally when 0)
        /// </param>
        /// <returns>
        /// Whether the language exists and is published
        /// </returns>
        bool IsPublishedLanguage(string seoCode, int storeId = 0);

        /// <summary>
        /// Gets the default (fallback) language for a store
        /// </summary>
        /// <param name="storeId">
        /// The store id (will be resolved internally when 0)
        /// </param>
        /// <returns>
        /// The unique seo code of the language to check for
        /// </returns>
        string GetDefaultLanguageSeoCode(int storeId = 0);
    }
}
