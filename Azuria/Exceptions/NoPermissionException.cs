using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a request was made to which the <see cref="IProxerClient" />
    /// has no access to.
    /// </summary>
    public class NoPermissionException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NoPermissionException" /> class.
        /// </summary>
        public NoPermissionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoPermissionException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public NoPermissionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoPermissionException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public NoPermissionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}