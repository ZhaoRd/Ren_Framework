// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkymateValidationException.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   Defines the SkymateValidationException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// The skymate validation exception.
    /// </summary>
    [Serializable]
    public class SkymateValidationException : SkymateException
    {   
        /// <summary>
        /// Constructor.
        /// </summary>
        public SkymateValidationException()
        {
            this.ValidationErrors = new List<ValidationResult>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkymateValidationException"/> class.
        /// </summary>
        /// <param name="serializationInfo">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public SkymateValidationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            this.ValidationErrors = new List<ValidationResult>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public SkymateValidationException(string message)
            : base(message)
        {
            this.ValidationErrors = new List<ValidationResult>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="validationErrors">Validation errors</param>
        public SkymateValidationException(string message, List<ValidationResult> validationErrors)
            : base(message)
        {
            this.ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public SkymateValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.ValidationErrors = new List<ValidationResult>();
        }

        /// <summary>
        /// Detailed list of validation errors for this exception.
        /// </summary>
        public List<ValidationResult> ValidationErrors { get; set; }
    }
}
