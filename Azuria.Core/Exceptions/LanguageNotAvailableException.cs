using System;

namespace Azuria.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a chapter or episode was
    /// requested in a language it is not available in.
    /// </summary>
    public class LanguageNotAvailableException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LanguageNotAvailableException" /> class.
        /// </summary>
        public LanguageNotAvailableException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageNotAvailableException" /> class with a specified error
        /// message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public LanguageNotAvailableException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageNotAvailableException" /> class with a specified error message
        /// and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public LanguageNotAvailableException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}