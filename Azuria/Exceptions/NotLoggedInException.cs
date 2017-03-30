using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an unauthenticated <see cref="IProxerClient" /> performed
    /// a authenticated request.
    /// </summary>
    public class NotLoggedInException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NotLoggedInException" /> class.
        /// </summary>
        public NotLoggedInException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotLoggedInException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public NotLoggedInException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotLoggedInException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public NotLoggedInException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}