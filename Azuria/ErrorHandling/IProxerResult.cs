using System;
using System.Collections.Generic;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// </summary>
    public interface IProxerResult<T> : IProxerResult
    {
        #region Properties

        /// <summary>
        /// </summary>
        T Result { get; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="success"></param>
        /// <param name="exceptions"></param>
        /// <param name="result"></param>
        void Deconstruct(out bool success, out IEnumerable<Exception> exceptions, out T result);

        #endregion
    }

    /// <summary>
    /// </summary>
    public interface IProxerResult
    {
        #region Properties

        /// <summary>
        /// Gets the exceptions that were thrown during method execution.
        /// </summary>
        IEnumerable<Exception> Exceptions { get; }

        /// <summary>
        /// Gets a value that indicates whether the method executed successfully.
        /// </summary>
        bool Success { get; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="success"></param>
        /// <param name="exceptions"></param>
        void Deconstruct(out bool success, out IEnumerable<Exception> exceptions);

        #endregion
    }
}