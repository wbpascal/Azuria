using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// </summary>
    public class AlreadyLoggedInException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AlreadyLoggedInException" /> class.
        /// </summary>
        public AlreadyLoggedInException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlreadyLoggedInException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public AlreadyLoggedInException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlreadyLoggedInException" /> class with a specified error message
        /// and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public AlreadyLoggedInException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}