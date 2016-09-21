using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a request was made to which the <see cref="Senpai" /> has no access to.
    /// </summary>
    public class NoAccessException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NoAccessException" /> class.
        /// </summary>
        public NoAccessException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="NotLoggedInException" /> class.
        /// </summary>
        /// <param name="senpai">The user that has no access to the action.</param>
        public NoAccessException(Senpai senpai)
        {
            this.Senpai = senpai;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoAccessException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public NoAccessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoAccessException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public NoAccessException(string message, Exception inner) : base(message, inner)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the <see cref="Senpai" /> object which is connected to this exception.
        /// </summary>
        public Senpai Senpai { get; set; }

        #endregion
    }
}