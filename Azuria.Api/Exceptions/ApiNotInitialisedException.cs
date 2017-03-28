using System;

namespace Azuria.Api.Exceptions
{
    /// <summary>
    /// </summary>
    public class ApiNotInitialisedException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ApiNotInitialisedException" /> class.
        /// </summary>
        public ApiNotInitialisedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNotInitialisedException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public ApiNotInitialisedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNotInitialisedException" /> class with a specified error message
        /// and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public ApiNotInitialisedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}