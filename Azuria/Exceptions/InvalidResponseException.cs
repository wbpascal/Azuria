using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a response of the server does not match the expected one.
    /// </summary>
    public class InvalidResponseException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidResponseException" /> class.
        /// </summary>
        public InvalidResponseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResponseException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public InvalidResponseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResponseException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public InvalidResponseException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Gibt die Antwort des Servers zurück, mit der diese Ausnahme zusammenhängt oder legt diese fest.
        /// </summary>
        public string Response { get; set; }
    }
}