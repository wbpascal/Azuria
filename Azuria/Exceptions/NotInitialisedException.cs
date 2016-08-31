using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuria.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class NotInitialisedException : Exception
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="NotInitialisedException" /> class.
        /// </summary>
        public NotInitialisedException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotInitialisedException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public NotInitialisedException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotInitialisedException" /> class with a specified error message
        ///     and a
        ///     reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public NotInitialisedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}