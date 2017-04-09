using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an unauthenticated <see cref="IProxerClient" /> performed
    /// a authenticated request.
    /// </summary>
    public class NotAuthenticatedException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NotAuthenticatedException" /> class.
        /// </summary>
        public NotAuthenticatedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotAuthenticatedException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public NotAuthenticatedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotAuthenticatedException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public NotAuthenticatedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}