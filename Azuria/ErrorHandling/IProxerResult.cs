using System;
using System.Collections.Generic;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// An interface that is used to represent the result of a method which returns an object.
    /// </summary>
    /// <typeparam name="T">The type of the returned object.</typeparam>
    public interface IProxerResult<T> : IProxerResultBase
    {
        #region Properties

        /// <summary>
        /// Gets the returned object if the method executed successfully.
        /// </summary>
        T Result { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Used to deconstruct the object to a tupple.
        /// </summary>
        /// <param name="success">Whether the method ran successfully.</param>
        /// <param name="exceptions">The exceptions that were thrown during execution if any.</param>
        /// <param name="result">The returned object of the executed method.</param>
        void Deconstruct(out bool success, out IEnumerable<Exception> exceptions, out T result);

        #endregion
    }

    /// <summary>
    /// An interface that is used to represent the result of a method.
    /// </summary>
    public interface IProxerResult : IProxerResultBase
    {
        #region Methods

        /// <summary>
        /// Used to deconstruct the object to a tupple.
        /// </summary>
        /// <param name="success">Whether the method ran successfully.</param>
        /// <param name="exceptions">The exceptions that were thrown during execution if any.</param>
        void Deconstruct(out bool success, out IEnumerable<Exception> exceptions);

        #endregion
    }
}