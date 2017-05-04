using System;
using System.Collections.Generic;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// An interface that is used to represent the result of a method.
    /// </summary>
    public interface IProxerResultBase
    {
        #region Properties

        /// <summary>
        /// Gets the enumeration of exceptions that were thrown during method execution.
        /// </summary>
        IEnumerable<Exception> Exceptions { get; }

        /// <summary>
        /// Gets a value indicating, whether or not the method executed successfully.
        /// </summary>
        bool Success { get; }

        #endregion
    }
}