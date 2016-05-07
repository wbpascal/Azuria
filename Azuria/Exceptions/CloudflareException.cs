using System;

namespace Azuria.Exceptions
{
    /// <summary>
    ///     Represents an exception that is thrown when a request encounters cloudflares anti-DDoS page that it can not solve
    ///     automaticly.
    /// </summary>
    public class CloudflareException : Exception
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="CloudflareException" /> class.
        /// </summary>
        public CloudflareException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloudflareException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public CloudflareException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloudflareException" /> class with a specified error message and a
        ///     reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public CloudflareException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}