// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormValueAbsentAttribute.cs" company="zhaord">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Web.Attributes.FormValues
{
    using System;

    /// <summary>
    /// The form value absent attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class FormValueAbsentAttribute : FormValueRequiredAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueAbsentAttribute"/> class.
        /// </summary>
        /// <param name="submitButtonNames">
        /// The submit button names.
        /// </param>
        public FormValueAbsentAttribute(params string[] submitButtonNames) :
            base(FormValueRequirement.Equal, FormValueRequirementRule.MatchAny, true, submitButtonNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueAbsentAttribute"/> class.
        /// </summary>
        /// <param name="requirement">
        /// The requirement.
        /// </param>
        /// <param name="submitButtonNames">
        /// The submit button names.
        /// </param>
        public FormValueAbsentAttribute(FormValueRequirement requirement, params string[] submitButtonNames)
            : base(requirement, FormValueRequirementRule.MatchAny, true, submitButtonNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueAbsentAttribute"/> class.
        /// </summary>
        /// <param name="rule">
        /// The rule.
        /// </param>
        /// <param name="submitButtonNames">
        /// The submit button names.
        /// </param>
        public FormValueAbsentAttribute(FormValueRequirementRule rule, params string[] submitButtonNames)
            : base(FormValueRequirement.Equal, rule, true, submitButtonNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormValueAbsentAttribute"/> class.
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
        public FormValueAbsentAttribute(FormValueRequirement requirement, FormValueRequirementRule rule, params string[] submitButtonNames)
            : base(requirement, rule, true, submitButtonNames)
        {
        }
    }
}