using System;

namespace Proxer.API.Exceptions
{
    /// <summary>
    /// </summary>
    public class CaptchaException : Exception
    {
        /// <summary>
        /// </summary>
        public CaptchaException()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public CaptchaException(string message) : base(message)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CaptchaException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}