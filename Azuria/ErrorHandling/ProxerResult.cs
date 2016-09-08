using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1;
using Azuria.Exceptions;

namespace Azuria.ErrorHandling
{
    /// <summary>
    ///     Represents a result of a method.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class ProxerResult<T> : ProxerResult
    {
        /// <summary>
        ///     Initialises a new instance with a specified result and indicates that the method was successful.
        /// </summary>
        /// <param name="result">The result.</param>
        public ProxerResult(T result)
        {
            this.Success = true;
            this.Result = result;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        ///     Initialises a new instance with the exceptions that were thrown during method execution and indicates that the
        ///     method failed to execute.
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
        ///     Gets the result of the method if the method was successful.
        /// </summary>
        public T Result { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns the result of the method if the method was successful. Otherwise returns the value of
        ///     <paramref name="returnObject" />.
        /// </summary>
        /// <param name="returnObject">
        ///     The value that will be returned if the method was not executed successfully.
        /// </param>
        /// <returns>An object of type <typeparamref name="T" />.</returns>
        public T OnError(T returnObject)
        {
            return this.Success && (this.Result != null) ? this.Result : returnObject;
        }

        #endregion
    }

    /// <summary>
    ///     Represents a result of a method.
    /// </summary>
    public class ProxerResult
    {
        /// <summary>
        ///     Initialises a new instance and indicates that the method executed successfully.
        /// </summary>
        public ProxerResult()
        {
            this.Success = true;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        ///     Initialises a new instance with the exceptions that were thrown during method execution and indicates that the
        ///     method failed to execute.
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
        ///     Gets the exceptions that were thrown during method execution.
        /// </summary>
        public IEnumerable<Exception> Exceptions { get; set; }

        /// <summary>
        ///     Gets a value that indicates whether the method executed successfully.
        /// </summary>
        public bool Success { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds an exception to the collection that were thrown during method execution.
        /// </summary>
        /// <param name="exception">The exception that is added to the collection.</param>
        public void AddException(Exception exception)
        {
            List<Exception> lExceptions = this.Exceptions.ToList();
            lExceptions.Add(exception);
            this.Exceptions = lExceptions.ToArray();

            this.Success = false;
        }

        /// <summary>
        ///     Adds multiple exceptions to the collection that were thrown during method execution.
        /// </summary>
        /// <param name="exception">The exception that are added to the collection.</param>
        public void AddExceptions(IEnumerable<Exception> exception)
        {
            List<Exception> lExceptions = this.Exceptions.ToList();
            lExceptions.AddRange(exception);
            this.Exceptions = lExceptions.ToArray();

            this.Success = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public bool ContainsApiError(ErrorCode errorCode)
        {
            return
                this.Exceptions.Any(
                    exception =>
                            exception is ProxerApiException && (((ProxerApiException) exception).ErrorCode == errorCode));
        }

        #endregion
    }
}