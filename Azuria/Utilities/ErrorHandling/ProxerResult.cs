using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Azuria.Utilities.ErrorHandling
{
    /// <summary>
    ///     Represents a result of a method.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class ProxerResult<T> : ProxerResult
    {
        [UsedImplicitly]
        private ProxerResult()
        {
        }

        /// <summary>
        ///     Initialises a new instance with a specified result and indicates that the method was successful.
        /// </summary>
        /// <param name="result">The result.</param>
        public ProxerResult([NotNull] T result)
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
        public ProxerResult([NotNull] IEnumerable<Exception> exceptions) : base(exceptions)
        {
        }

        #region Properties

        /// <summary>
        ///     Gets the result of the method if the method was successful.
        /// </summary>
        [CanBeNull]
        public T Result { get; set; }

        #endregion

        #region

        /// <summary>
        ///     Returns the result of the method if the method was successful. Otherwise returns the value of
        ///     <paramref name="returnObject" />.
        /// </summary>
        /// <param name="returnObject">
        ///     The value that will be returned if the method was not executed successfully.
        /// </param>
        /// <returns>An object of type <typeparamref name="T" />.</returns>
        [CanBeNull]
        public T OnError([CanBeNull] T returnObject)
        {
            return this.Success && this.Result != null ? this.Result : returnObject;
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
        public ProxerResult([NotNull] IEnumerable<Exception> exceptions)
        {
            this.Success = false;
            this.Exceptions = exceptions;
        }

        #region Properties

        /// <summary>
        ///     Gibt die Fehler zurück, die während der Ausführung aufgetreten sind, oder legt diese fest.
        /// </summary>
        /// <value>Ist null, wenn <see cref="Success" /> == true</value>
        [NotNull]
        public IEnumerable<Exception> Exceptions { get; set; }

        /// <summary>
        ///     Gibt zurück, ob die Methode erfolg hatte, oder legt dieses fest.
        /// </summary>
        public bool Success { get; set; }

        #endregion

        #region

        /// <summary>
        ///     Adds an exception to the collection that were thrown during method execution.
        /// </summary>
        /// <param name="exception">The exception that is added to the collection.</param>
        public void AddException([NotNull] Exception exception)
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
        public void AddExceptions([NotNull] IEnumerable<Exception> exception)
        {
            List<Exception> lExceptions = this.Exceptions.ToList();
            lExceptions.AddRange(exception);
            this.Exceptions = lExceptions.ToArray();

            this.Success = false;
        }

        #endregion
    }
}