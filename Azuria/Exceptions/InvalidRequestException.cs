using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// </summary>
    public class InvalidRequestException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidRequestException" /> class.
        /// </summary>
        public InvalidRequestException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public InvalidRequestException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public InvalidRequestException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}