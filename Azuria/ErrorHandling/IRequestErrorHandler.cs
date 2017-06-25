using System;
using Azuria.Enums;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// </summary>
    public interface IRequestErrorHandler
    {
        /// <summary>
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Exception HandleError(ErrorCode code);
    }
}