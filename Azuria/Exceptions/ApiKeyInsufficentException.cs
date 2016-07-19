using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// </summary>
    public class ApiKeyInsufficentException : Exception
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="ApiKeyInsufficentException" /> class.
        /// </summary>
        public ApiKeyInsufficentException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiKeyInsufficentException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public ApiKeyInsufficentException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiKeyInsufficentException" /> class with a specified error message
        ///     and a
        ///     reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public ApiKeyInsufficentException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}