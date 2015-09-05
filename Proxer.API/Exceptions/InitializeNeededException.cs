using System;
using System.Runtime.Serialization;

namespace Proxer.API.Exceptions
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class InitializeNeededException : Exception
    {
        /// <summary>
        /// </summary>
        public InitializeNeededException()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public InitializeNeededException(string message) : base(message)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InitializeNeededException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InitializeNeededException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}