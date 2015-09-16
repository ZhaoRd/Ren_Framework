// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormValueRequiredAttribute.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   Form的值必须存在的Attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.Attributes.FormValues
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    using Skymate.Extensions;

    /// <summary>
    /// The form value required attribute.
    /// </summary>
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        #region Fields

        /// <summary>
        /// The _submit button names.
        /// </summary>
        private readonly string[] submitButtonNames;

        /// <summary>
        /// The _requirement.
        /// </summary>
        private readonly FormValueRequirement requirement;

        /// <summary>
        /// The _rule.
        /// </summary>
        private readonly FormValueRequirementRule rule;

        /// <summary>
        /// The _inverse.
        /// </summary>
        private readonly bool inverse;

        #endregion Fields

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueRequiredAttribute"/> class.
        /// </summary>
        /// <param name="submitButtonNames">
        /// The submit button names.
        /// </param>
        public FormValueRequiredAttribute(params string[] submitButtonNames)
            : this(FormValueRequirement.Equal, FormValueRequirementRule.MatchAny, false, submitButtonNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueRequiredAttribute"/> class.
        /// </summary>
        /// <param name="requirement">
        /// The requirement.
        /// </param>
        /// <param name="submitButtonNames">
        /// The submit button names.
        /// </param>
        public FormValueRequiredAttribute(FormValueRequirement requirement, params string[] submitButtonNames)
            : this(requirement, FormValueRequirementRule.MatchAny, false, submitButtonNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueRequiredAttribute"/> class.
        /// </summary>
        /// <param name="rule">
        /// The rule.
        /// </param>
        /// <param name="submitButtonNames">
        /// The submit button names.
        /// </param>
        public FormValueRequiredAttribute(FormValueRequirementRule rule, params string[] submitButtonNames)
            : this(FormValueRequirement.Equal, rule, false, submitButtonNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueRequiredAttribute"/> class.
        /// </summary>
        /// <param name="requirement">
        /// The requirement.
        /// </param>
        /// <param name="rule">
        /// The rule.
        /// </param>
        /// <param name="submitButtonNames">
        /// The submit button names.
        /// </param>
        public FormValueRequiredAttribute(
            FormValueRequirement requirement,
            FormValueRequirementRule rule,
            params string[] submitButtonNames)
            : this(requirement, rule, false, submitButtonNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueRequiredAttribute"/> class.
        /// </summary>
        /// <param name="requirement">
        /// The requirement.
        /// </param>
        /// <param name="rule">
        /// The rule.
        /// </param>
        /// <param name="inverse">
        /// The inverse.
        /// </param>
        /// <param name="submitButtonNames">
        /// The submit button names.
        /// </param>
        protected internal FormValueRequiredAttribute(
            FormValueRequirement requirement,
            FormValueRequirementRule rule,
            bool inverse,
            params string[] submitButtonNames)
        {
            this.submitButtonNames = submitButtonNames;
            this.requirement = requirement;
            this.rule = rule;
            this.inverse = inverse;
        }

        #endregion Ctor

        /// <summary>
        /// The is valid for request.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="methodInfo">
        /// The method info.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return this.IsValidForRequest(controllerContext.HttpContext.Request.Form);
        }

        /// <summary>
        /// The is valid for request.
        /// </summary>
        /// <param name="form">
        /// The form.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected internal virtual bool IsValidForRequest(NameValueCollection form)
        {
            try
            {
                var isMatch = this.rule == FormValueRequirementRule.MatchAny ? this.submitButtonNames.Any(x => this.IsMatch(form, x)) : this.submitButtonNames.All(x => this.IsMatch(form, x));

                return isMatch;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// The is match.
        /// </summary>
        /// <param name="form">
        /// The form.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsMatch(NameValueCollection form, string key)
        {
            var value = string.Empty;

            if (this.requirement == FormValueRequirement.Equal)
            {
                value = form[key];
            }
            else
            {
                var firstMatch = form.AllKeys.FirstOrDefault(x => x.StartsWith(key, StringComparison.InvariantCultureIgnoreCase));
                if (firstMatch != null)
                {
                    value = form[firstMatch];
                }
            }

            if (this.inverse)
            {
                return value.IsEmpty();
            }

            return value.HasValue();
        }
    }
}