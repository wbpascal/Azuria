using System;
using System.Runtime.Serialization;

namespace Proxer.API.Exceptions
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class NotLoggedInException : Exception
    {
        /// <summary>
        /// </summary>
        public NotLoggedInException()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public NotLoggedInException(string message) : base(message)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public NotLoggedInException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NotLoggedInException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}