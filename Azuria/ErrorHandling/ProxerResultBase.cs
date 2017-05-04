using System;
using System.Collections.Generic;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// Represents the result of a method.
    /// </summary>
    public abstract class ProxerResultBase : IProxerResultBase
    {
        /// <summary>
        /// Initialises a new instance indicating the successful execution of the method.
        /// </summary>
        protected ProxerResultBase()
        {
            this.Success = true;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        /// Initialises a new instance with the exceptions that were thrown during method execution
        /// (indicating that the method did not execute successfully).
        /// </summary>
        /// <param name="exceptions">
        /// An enumeration of the exceptions that were thrown during method execution.
        /// </param>
        protected ProxerResultBase(IEnumerable<Exception> exceptions)
        {
            this.Success = false;
            this.Exceptions = exceptions;
        }

        /// <summary>
        /// Initialises a new instance with a single exception that was thrown during method execution
        /// (indicating that the method did not execute successfully).
        /// </summary>
        /// <param name="exception">A single exception that was thrown during method execution.</param>
        protected ProxerResultBase(Exception exception) : this(new[] {exception})
        {
        }

        #region Properties

        /// <inheritdoc />
        public IEnumerable<Exception> Exceptions { get; set; }

        /// <inheritdoc />
        public bool Success { get; set; }

        #endregion
    }
}