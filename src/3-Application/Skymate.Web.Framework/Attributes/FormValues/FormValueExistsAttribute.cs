// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormValueExistsAttribute.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   Form的值是否存在的Attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.Attributes.FormValues
{
    using System.Web.Mvc;

    /// <summary>
    /// The form value exists attribute.
    /// Form的值是否存在的Attribute
    /// </summary>
    public class FormValueExistsAttribute : FilterAttribute, IActionFilter
    {
        #region Fields

        /// <summary>
        /// The name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The value.
        /// </summary>
        private readonly string value;

        /// <summary>
        /// The action parameter name.
        /// </summary>
        private readonly string actionParameterName;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueExistsAttribute"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="actionParameterName">
        /// The action parameter name.
        /// </param>
        public FormValueExistsAttribute(string name, string value, string actionParameterName)
        {
            this.name = name;
            this.value = value;
            this.actionParameterName = actionParameterName;
        }

        #endregion

        /// <summary>
        /// The on action executed.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        /// <summary>
        /// The on action executing.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var formValue = filterContext.RequestContext.HttpContext.Request.Form[this.name];
            filterContext.ActionParameters[this.actionParameterName] = !string.IsNullOrEmpty(formValue) &&
                                                                   formValue.ToLower().Equals(this.value.ToLower());
        }
    }
}
