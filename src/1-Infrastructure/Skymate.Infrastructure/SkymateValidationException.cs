using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [Serializable]
    public class SkymateValidationException : SkymateException
    {
        /// <summary>
        /// Detailed list of validation errors for this exception.
        /// </summary>
        public List<ValidationResult> ValidationErrors { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SkymateValidationException()
        {
            ValidationErrors = new List<ValidationResult>();
        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public SkymateValidationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            ValidationErrors = new List<ValidationResult>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public SkymateValidationException(string message)
            : base(message)
        {
            ValidationErrors = new List<ValidationResult>();
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="validationErrors">Validation errors</param>
        public SkymateValidationException(string message, List<ValidationResult> validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public SkymateValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            ValidationErrors = new List<ValidationResult>();
        }
    }
}
