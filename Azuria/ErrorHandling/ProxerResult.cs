using System;
using System.Collections.Generic;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// Represents the result of a method including a returned object.
    /// </summary>
    /// <typeparam name="T">The type of the returned object.</typeparam>
    /// <seealso cref="Azuria.ErrorHandling.ProxerResult" />
    /// <seealso cref="Azuria.ErrorHandling.IProxerResult{T}" />
    public class ProxerResult<T> : ProxerResult, IProxerResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxerResult{T}" /> class.
        /// </summary>
        /// <param name="result">The returned object of the method.</param>
        public ProxerResult(T result)
        {
            this.Success = true;
            this.Result = result;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxerResult{T}" /> class and sets <see cref="ProxerResult.Success" /> to
        /// false.
        /// </summary>
        /// <param name="exceptions">An enumeration of the exceptions that were thrown during method execution.</param>
        public ProxerResult(IEnumerable<Exception> exceptions) : base(exceptions)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ProxerResult{T}" /> class and sets <see cref="ProxerResult.Success" /> to
        /// false.
        /// </summary>
        /// <param name="exception">A single exception that was thrown during method execution.</param>
        public ProxerResult(Exception exception) : base(exception)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the returned object if the method executed successfully. If the method did not execute successfully the default
        /// value of the type <typeparamref name="T" /> will be returned.
        /// </summary>
        public T Result { get; set; }

        #endregion

        #region Methods

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
    /// Represents the result of a method without a returned object.
    /// </summary>
    /// <seealso cref="Azuria.ErrorHandling.ProxerResult" />
    /// <seealso cref="Azuria.ErrorHandling.IProxerResult{T}" />
    public class ProxerResult : IProxerResult
    {
        /// <summary>
        /// Initialises a new instance and sets <see cref="Success" /> to true.
        /// </summary>
        public ProxerResult()
        {
            this.Success = true;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        /// Initialises a new instance with the exceptions that were thrown during method execution and sets <see cref="Success" />
        /// to false.
        /// </summary>
        /// <param name="exceptions">An enumeration of the exceptions that were thrown during method execution.</param>
        public ProxerResult(IEnumerable<Exception> exceptions)
        {
            this.Success = false;
            this.Exceptions = exceptions;
        }

        /// <summary>
        /// Initialises a new instance with a single exception that was thrown during method execution and sets
        /// <see cref="Success" /> to true.
        /// </summary>
        /// <param name="exception">A single exception that was thrown during method execution.</param>
        public ProxerResult(Exception exception) : this(new[] {exception})
        {
        }

        #region Properties

        /// <summary>
        /// Gets an enumeration of the exceptions that were thrown during method execution.
        /// </summary>
        public IEnumerable<Exception> Exceptions { get; set; }

        /// <summary>
        /// Gets a value indicating whether the method executed successfully.
        /// </summary>
        public bool Success { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Deconstruct(out bool sucess, out IEnumerable<Exception> exceptions)
        {
            sucess = this.Success;
            exceptions = this.Exceptions;
        }

        #endregion
    }
}