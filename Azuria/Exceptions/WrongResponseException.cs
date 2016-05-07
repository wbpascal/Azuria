using System;

namespace Azuria.Exceptions
{
    /// <summary>
    ///     Represents an exception that is thrown when a respone of the server does not match the expected one.
    /// </summary>
    public class WrongResponseException : Exception
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="WrongResponseException" /> class.
        /// </summary>
        public WrongResponseException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WrongResponseException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public WrongResponseException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WrongResponseException" /> class with a specified error message and a
        ///     reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public WrongResponseException(string message, Exception inner) : base(message, inner)
        {
        }

        #region Properties

        /// <summary>
        ///     Gibt die Antwort des Servers zurück, mit der diese Ausnahme zusammenhängt oder legt diese fest.
        /// </summary>
        public string Response { get; set; }

        #endregion
    }
}