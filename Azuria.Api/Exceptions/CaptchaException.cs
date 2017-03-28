using System;

namespace Azuria.Api.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a request encounters a captcha.
    /// </summary>
    public class CaptchaException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CaptchaException" /> class.
        /// </summary>
        public CaptchaException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public CaptchaException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public CaptchaException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}