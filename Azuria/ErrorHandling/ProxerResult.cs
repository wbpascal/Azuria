using System;
using System.Collections.Generic;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// Represents the result of a method that returns an object.
    /// </summary>
    /// <typeparam name="T">The type of the returned object.</typeparam>
    public class ProxerResult<T> : ProxerResultBase, IProxerResult<T>
    {
        /// <summary>
        /// Initializes a new instance indicating the successful execution of the method.
        /// </summary>
        /// <param name="result">The returned object of the method.</param>
        public ProxerResult(T result)
        {
            this.Success = true;
            this.Result = result;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        /// Initializes a new instance with the exceptions that were thrown during method execution
        /// (indicating that the method did not execute successfully).
        /// </summary>
        /// <param name="exceptions">
        /// An enumeration of the exceptions that were thrown during method execution.
        /// </param>
        public ProxerResult(IEnumerable<Exception> exceptions) : base(exceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance with a single exception that was thrown during method execution
        /// (indicating that the method did not execute successfully).
        /// </summary>
        /// <param name="exception">A single exception that was thrown during method execution.</param>
        public ProxerResult(Exception exception) : base(exception)
        {
        }

        /// <inheritdoc />
        public T Result { get; set; }

        /// <inheritdoc />
        public void Deconstruct(out bool success, out IEnumerable<Exception> exceptions, out T result)
        {
            success = this.Success;
            exceptions = this.Exceptions;
            result = this.Result;
        }
    }

    /// <summary>
    /// Represents the result of a method.
    /// </summary>
    public class ProxerResult : ProxerResultBase, IProxerResult
    {
        /// <summary>
        /// Initializes a new instance indicating the successful execution of the method.
        /// </summary>
        public ProxerResult()
        {
        }

        /// <summary>
        /// Initializes a new instance with the exceptions that were thrown during method execution
        /// (indicating that the method did not execute successfully).
        /// </summary>
        /// <param name="exceptions">
        /// An enumeration of the exceptions that were thrown during method execution.
        /// </param>
        public ProxerResult(IEnumerable<Exception> exceptions) : base(exceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance with a single exception that was thrown during method execution
        /// (indicating that the method did not execute successfully).
        /// </summary>
        /// <param name="exception">A single exception that was thrown during method execution.</param>
        public ProxerResult(Exception exception) : base(exception)
        {
        }

        /// <inheritdoc />
        public void Deconstruct(out bool success, out IEnumerable<Exception> exceptions)
        {
            success = this.Success;
            exceptions = this.Exceptions;
        }
    }
}