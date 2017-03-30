using System;
using Azuria.Enums;

namespace Azuria.Exceptions
{
    /// <summary>
    /// </summary>
    public class ProxerApiException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ProxerApiException" /> class.
        /// </summary>
        public ProxerApiException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ProxerApiException" /> class.
        /// </summary>
        /// <param name="errorCode"></param>
        public ProxerApiException(ErrorCode errorCode)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxerApiException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public ProxerApiException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxerApiException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public ProxerApiException(string message, Exception inner) : base(message, inner)
        {
        }

        #region Properties

        /// <summary>
        /// </summary>
        public ErrorCode ErrorCode { get; }

        #endregion
    }
}