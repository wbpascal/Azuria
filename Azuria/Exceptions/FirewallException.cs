using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a request encounters Proxer's internal firewall.
    /// </summary>
    public class FirewallException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FirewallException" /> class.
        /// </summary>
        public FirewallException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirewallException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message string.</param>
        public FirewallException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirewallException" /> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public FirewallException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}