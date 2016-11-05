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
    }
}