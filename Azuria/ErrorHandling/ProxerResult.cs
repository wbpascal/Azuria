using System;
using System.Collections.Generic;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// Represents a result of a method.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class ProxerResult<T> : ProxerResult, IProxerResult<T>
    {
        /// <summary>
        /// Initialises a new instance with a specified result and indicates that the method was successful.
        /// </summary>
        /// <param name="result">The result.</param>
        public ProxerResult(T result)
        {
            this.Success = true;
            this.Result = result;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        /// Initialises a new instance with the exceptions that were thrown during method execution and indicates that the
        /// method failed to execute.
        /// </summary>
        /// <param name="exceptions">The exception that were thrown during method execution.</param>
        public ProxerResult(IEnumerable<Exception> exceptions) : base(exceptions)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="exception"></param>
        public ProxerResult(Exception exception) : base(exception)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the result of the method if the method was successful.
        /// </summary>
        public T Result { get; set; }

        /// <inheritdoc />
        public void Deconstruct(out bool sucess, out IEnumerable<Exception> exceptions, out T result)
        {
            sucess = this.Success;
            exceptions = this.Exceptions;
            result = this.Result;
        }

        #endregion
    }

    /// <summary>
    /// Represents a result of a method.
    /// </summary>
    public class ProxerResult : IProxerResult
    {
        /// <summary>
        /// Initialises a new instance and indicates that the method executed successfully.
        /// </summary>
        public ProxerResult()
        {
            this.Success = true;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        /// Initialises a new instance with the exceptions that were thrown during method execution and indicates that the
        /// method failed to execute.
        /// </summary>
        /// <param name="exceptions">The exception that were thrown during method execution.</param>
        public ProxerResult(IEnumerable<Exception> exceptions)
        {
            this.Success = false;
            this.Exceptions = exceptions;
        }

        /// <summary>
        /// </summary>
        /// <param name="exception"></param>
        public ProxerResult(Exception exception) : this(new[] {exception})
        {
        }

        #region Properties

        /// <summary>
        /// Gets the exceptions that were thrown during method execution.
        /// </summary>
        public IEnumerable<Exception> Exceptions { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the method executed successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <inheritdoc />
        public void Deconstruct(out bool sucess, out IEnumerable<Exception> exceptions)
        {
            sucess = this.Success;
            exceptions = this.Exceptions;
        }

        #endregion
    }
}