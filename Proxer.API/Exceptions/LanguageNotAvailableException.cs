using System;
using System.Runtime.Serialization;

namespace Proxer.API.Exceptions
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class LanguageNotAvailableException : Exception
    {
        /// <summary>
        /// </summary>
        public LanguageNotAvailableException()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public LanguageNotAvailableException(string message) : base(message)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public LanguageNotAvailableException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected LanguageNotAvailableException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}